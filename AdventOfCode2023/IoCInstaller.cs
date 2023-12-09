using Microsoft.Extensions.DependencyInjection;
using Repository;
using Solutions;

namespace Installer
{
    internal static class IoCInstaller
    {
        public static IServiceProvider GetService()
        {
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IDataRetriever, DataRetriever>()
                .AddSingleton<IAdventSolution, Day3>()
                .BuildServiceProvider(validateScopes: true);
            return serviceProvider;
        }
    }
}
