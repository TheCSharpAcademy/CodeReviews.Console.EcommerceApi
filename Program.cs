using ECommerceApp.RyanW84.Interfaces;
using ECommerceApp.RyanW84.Middleware;
using ECommerceApp.RyanW84.Services;
using ECommerceApp.RyanW84.Validators;
using ECommerceApp.RyanW84.Interfaces.Helpers;
using ECommerceApp.RyanW84.Services.Helpers;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using Serilog;
using ECommerceApp.RyanW84.Options;

namespace ECommerceApp.RyanW84;

public class Program
{
    // Entry point of the application
    public static void Main(string[] args)
    {
        // Configure Serilog before building the app
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File(
                Path.Combine(AppContext.BaseDirectory, "logs", "ecommerce-.log"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .Enrich.WithProperty("Application", "ECommerceAPI")
            .CreateLogger();

        try
        {
            Log.Information("Starting ECommerceAPI application");
            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog();

            ConfigureServices(builder);
            var app = builder.Build();
            ConfigureMiddlewareAndRoutes(app);
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        // Load user secrets only in Development to source connection strings safely
        if (builder.Environment.IsDevelopment())
        {
            builder.Configuration.AddUserSecrets<Program>();
        }

        builder
            .Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.WriteIndented = false; // Reduce payload size in production
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                // Performance: Handle circular references efficiently
                options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

        builder
            .Services.AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters();

        builder.Services.AddValidatorsFromAssemblyContaining<ProductValidator>();

        // Performance: Response caching and output caching
        builder.Services.AddResponseCaching();
        builder.Services.AddOutputCache(options =>
        {
            options.AddBasePolicy(builder => builder.Cache());
            options.AddPolicy("Products", builder => builder.Expire(TimeSpan.FromMinutes(2)).Tag("products"));
            options.AddPolicy("Categories", builder => builder.Expire(TimeSpan.FromMinutes(5)).Tag("categories"));
            options.AddPolicy("Sales", builder => builder.Expire(TimeSpan.FromMinutes(1)).Tag("sales"));
        });

        // Performance: Memory cache for frequently accessed data
        builder.Services.AddMemoryCache(options =>
        {
            options.SizeLimit = 1024; // Limit cache size to prevent memory bloat
            options.CompactionPercentage = 0.25; // Remove 25% when size limit reached
        });

        // Performance: HTTP Response compression
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProvider>();
            options.Providers.Add<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProvider>();
        });

        builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.BrotliCompressionProviderOptions>(options =>
        {
            options.Level = System.IO.Compression.CompressionLevel.Fastest;
        });

        builder.Services.Configure<Microsoft.AspNetCore.ResponseCompression.GzipCompressionProviderOptions>(options =>
        {
            options.Level = System.IO.Compression.CompressionLevel.Fastest;
        });

        builder.Services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            options.KnownIPNetworks.Clear();
            options.KnownProxies.Clear();
        });

        builder.Services.AddOpenApi();

        string connectionString = GetConnectionString(builder.Configuration);
        builder.Services.AddDbContextPool<Data.ECommerceDbContext>(options =>
        {
            ConfigureDbContext(options, connectionString);
            options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
            options.EnableDetailedErrors(builder.Environment.IsDevelopment());
            options.ConfigureWarnings(w => w.Throw(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.MultipleCollectionIncludeWarning));

            // Performance: Enable query caching and compiled model
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); // Default to no-tracking for read operations
        }, poolSize: 128);

        // Performance: Configure Kestrel server limits
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.Limits.MaxConcurrentConnections = 1000;
            options.Limits.MaxConcurrentUpgradedConnections = 1000;
            options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; // 10 MB
            options.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(2);
            options.Limits.RequestHeadersTimeout = TimeSpan.FromSeconds(30);
        });

        // Options
        builder.Services.AddOptions();
        builder.Services.Configure<ScalarUiOptions>(
            builder.Configuration.GetSection("ScalarUi")
        );

        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<ISaleService, SaleService>();
        builder.Services.AddScoped<ISaleProcessingHelper, SaleProcessingHelper>();
        builder.Services.AddScoped<ISaleQueryHelper, SaleQueryHelper>();
        builder.Services.AddScoped<IProductProcessingHelper, ProductProcessingHelper>();
        builder.Services.AddScoped<IProductQueryHelper, ProductQueryHelper>();
        builder.Services.AddScoped<ICategoryProcessingHelper, CategoryProcessingHelper>();
        builder.Services.AddScoped<ISalesSummaryService, SalesSummaryService>();
        builder.Services.AddScoped<IProductRepository, Repositories.ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, Repositories.CategoryRepository>();
        builder.Services.AddScoped<ISaleRepository, Repositories.SaleRepository>();

        // Hosted services
        builder.Services.AddHostedService<Hosting.DatabaseInitializerHostedService>();
    }

    private static void ConfigureMiddlewareAndRoutes(WebApplication app)
    {
        app.UseGlobalExceptionHandler();

        if (app.Environment.IsDevelopment())
        {
            ConfigureDevelopmentPipeline(app);
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseForwardedHeaders();
        app.UseHttpsRedirection();

        // Performance: Response compression before routing
        app.UseResponseCompression();

        app.UseRouting();
        app.UseResponseCaching();
        app.UseOutputCache(); // Output cache after response cache
        app.UseAuthorization();
        app.MapControllers();
        app.MapGet("/", () => Results.Redirect("/scalar/v1"));
    }

    private static void ConfigureDevelopmentPipeline(WebApplication app)
    {
        app.MapOpenApi();
        var scalarOpts = app.Services.GetService<IOptions<ScalarUiOptions>>()?.Value
                         ?? new ScalarUiOptions();
        app.MapScalarApiReference(options =>
        {
            options.Title = string.IsNullOrWhiteSpace(scalarOpts.Title)
                ? "ECommerceApp.RyanW84 API Documentation"
                : scalarOpts.Title;

            options.Theme = scalarOpts.Theme?.ToLowerInvariant() switch
            {
                "blueplanet" => ScalarTheme.BluePlanet,
                "purple" => ScalarTheme.Purple,
                "default" => ScalarTheme.Default,
                _ => ScalarTheme.BluePlanet
            };

            options.Layout = scalarOpts.Layout?.ToLowerInvariant() switch
            {
                "modern" => ScalarLayout.Modern,
                "classic" => ScalarLayout.Classic,
                _ => ScalarLayout.Modern
            };

            options.DarkMode = scalarOpts.DarkMode;
            options.WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            options.HideModels = scalarOpts.HideModels;
            options.ShowSidebar = scalarOpts.ShowSidebar;
            options.DefaultOpenAllTags = scalarOpts.DefaultOpenAllTags;
            if (!string.IsNullOrWhiteSpace(scalarOpts.SearchHotKey))
            {
                options.SearchHotKey = scalarOpts.SearchHotKey!;
            }
            if (!string.IsNullOrWhiteSpace(scalarOpts.CustomCss))
            {
                options.CustomCss = scalarOpts.CustomCss;
            }
        });

        TryOpenBrowser(app);
    }

    private static void TryOpenBrowser(WebApplication app)
    {
        var urls = app.Urls;
        if (urls.Count == 0)
            return;

        var url = urls.First().Replace("http://", "https://") + "/scalar/v1";
        try
        {
            System.Diagnostics.Process.Start(
                new System.Diagnostics.ProcessStartInfo { FileName = url, UseShellExecute = true }
            );
            if (app.Logger.IsEnabled(LogLevel.Information))
                app.Logger.LogInformation("Browser opened to: {Url}", url);
        }
        catch (Exception ex)
        {
            app.Logger.LogWarning(ex, "Failed to open browser automatically");
        }
    }

    private static string GetConnectionString(IConfiguration configuration)
    {
        // Try common locations for the connection string
        // 1) appsettings/UserSecrets ConnectionStrings section
        var cs = configuration.GetConnectionString("DatabaseConnection")
                 ?? configuration.GetConnectionString("DefaultConnection")
                 // 2) flat environment variables (common in containers / CI)
                 ?? configuration["DATABASE_CONNECTION_STRING"]
                 ?? configuration["DB_CONNECTION_STRING"]
                 // 3) Azure-style env vars
                 ?? Environment.GetEnvironmentVariable("SQLCONNSTR_DatabaseConnection")
                 ?? Environment.GetEnvironmentVariable("SQLCONNSTR_DefaultConnection");

        if (!string.IsNullOrWhiteSpace(cs))
            return cs;

        // As a development fallback, allow SQLite so the app can boot without secrets
        // This does NOT ship any credentials and only creates a local file DB.
        var devSqlitePath = Path.Combine(AppContext.BaseDirectory, "Data", "ecommerce.dev.db");
        Directory.CreateDirectory(Path.GetDirectoryName(devSqlitePath)!);
        var sqliteCs = $"Data Source={devSqlitePath}";
        return sqliteCs;
    }

    private static void ConfigureDbContext(DbContextOptionsBuilder options, string connectionString)
    {
        // Choose a provider by inspecting the connection string
        // If it looks like SQLite (Data Source=...), use SQLite; otherwise use SQL Server
        if (connectionString.Contains("Data Source=", StringComparison.OrdinalIgnoreCase)
            && !connectionString.Contains("Server=", StringComparison.OrdinalIgnoreCase)
            && !connectionString.Contains("Data Source=(localdb)", StringComparison.OrdinalIgnoreCase))
        {
            options.UseSqlite(connectionString, sqliteOptions =>
            {
                sqliteOptions.CommandTimeout(30);
            });
        }
        else
        {
            options.UseSqlServer(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.CommandTimeout(30);
                sqlServerOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            });
        }
    }
}
