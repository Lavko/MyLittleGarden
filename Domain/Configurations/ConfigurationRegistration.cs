using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.Configurations;

public static class ConfigurationRegistration
{
    public static IServiceCollection RegisterConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var scheduledServicesConfiguration = new ScheduledServicesConfiguration();
        configuration.Bind(ScheduledServicesConfiguration.SectionName, scheduledServicesConfiguration);
        services.AddSingleton(scheduledServicesConfiguration);

        return services;
    }

    public static IServiceCollection RegisterDomain(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}