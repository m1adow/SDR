using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDR.Extensions;
using System;
using Windows.ApplicationModel;

namespace SDR;

public partial class App
{
    public IServiceProvider Services { get; }
    public IConfiguration Configuration { get; }

    private static IServiceProvider ConfigureServices(IConfiguration configuration)
    {
        Ioc.Default.ConfigureServices(new ServiceCollection()
            .AddServices(configuration)
            .AddViewModels()
            .BuildServiceProvider());
        return Ioc.Default;
    }

    private static IConfiguration BuildConfiguration()
        => new ConfigurationBuilder()
        .SetBasePath(Package.Current.InstalledLocation.Path)
        .AddJsonFile("appsettings.json", false, true)
#if DEBUG
        .AddJsonFile($"appsettings.Development.json", false, true)
#endif
        .Build();
}