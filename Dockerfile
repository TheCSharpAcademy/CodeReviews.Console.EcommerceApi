# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy project files

# Restore dependencies

# Copy source code

# Build the application

# Publish stage
FROM build AS publish
RUN dotnet publish "ECommerceApp.RyanW84.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage

# Create logs directory

# Expose ports

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ECommerceApp.RyanW84.csproj", "."]
RUN dotnet restore "ECommerceApp.RyanW84.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ECommerceApp.RyanW84.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ECommerceApp.RyanW84.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables for ASP.NET Core to use user secrets
ENV ASPNETCORE_ENVIRONMENT=Development

# Entrypoint
ENTRYPOINT ["dotnet", "ECommerceApp.RyanW84.dll"]
