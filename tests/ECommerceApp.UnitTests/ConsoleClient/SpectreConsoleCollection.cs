using Xunit;

namespace ECommerceApp.UnitTests.ConsoleClient;

[CollectionDefinition(Name, DisableParallelization = true)]
public class SpectreConsoleCollection
{
    public const string Name = "SpectreConsole";
}
