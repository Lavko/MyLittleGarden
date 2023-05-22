using Domain.Repositories;
using Domain.Repositories.Common;
using Domain.Services;
using Infrastructure.Hardware;
using Infrastructure.Persistence.Persistence;
using Infrastructure.Persistence.Persistence.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Infrastructure.RaspberryPi.Outputs;
using Infrastructure.RaspberryPi.Sensors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection RegisterPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("db"),
                builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddScoped<AppDbContextInitialiser>();
        
        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddTransient<IEnvironmentMeasureRepository, EnvironmentMeasureRepository>();
        services.AddTransient<IOutletConfigurationRepository, OutletConfigurationRepository>();
        services.AddTransient<IActionRuleRepository, ActionRuleRepository>();
        services.AddTransient<ITakenActionRepository, TakenActionRepository>();
        
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        return services;
    }

    public static IServiceCollection RegisterHardware(this IServiceCollection services, bool isDevelopment)
    {
        if (isDevelopment)
        {
            services.AddScoped<IEnvironmentMeasureService, EnvironmentMeasureMockService>();
            services.AddSingleton<IOutletService, OutletMockService>();
        }
        else
        {
            services.AddScoped<IOutletService, OutletService>();
            services.AddScoped<IEnvironmentMeasureService, Bme280Service>();
        }
        
        return services;
    }
}