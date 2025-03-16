using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDR.Models.Interfaces;
using SDR.Models.Settings;
using SDR.Services;
using SDR.ViewModels;

namespace SDR.Extensions;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddTransient<ISignalDataProvider, RandomSignalDataProvider>();

    public static IServiceCollection AddViewModels(this IServiceCollection services)
        => services.AddSingleton<DataViewModel>();

    public static IServiceCollection ConfigureSettings(this IServiceCollection services, IConfiguration configuration)
        => services.Configure<SignalSettings>(configuration.GetSection("DataProviders:Random"));
}