using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SDR.Models.Configurations;
using SDR.Models.Interfaces;
using SDR.Services;
using SDR.ViewModels;

namespace SDR.Extensions;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        => services.AddTransient<ISignalDataProvider, RandomSignalDataProvider>(p =>
        {
            var dataProviderConfiguration = configuration.GetRequiredSection("DataProviders:Random").Get<RandomSignalDataProviderConfiguration>();
            return new RandomSignalDataProvider(
                (dataProviderConfiguration.FrequencyMin, dataProviderConfiguration.FrequencyMax), 
                (dataProviderConfiguration.StrengthMin, dataProviderConfiguration.StrengthMax),
                dataProviderConfiguration.Count,
                dataProviderConfiguration.Frequency);
        });

    public static IServiceCollection AddViewModels(this IServiceCollection serviceCollection, IConfiguration configuration)
        => serviceCollection.AddSingleton(p => new DataViewModel(p.GetRequiredService<ISignalDataProvider>(), configuration.GetValue<int>("DataProviders:Random:Count")));
}