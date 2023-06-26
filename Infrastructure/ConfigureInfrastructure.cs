using Domain.Repositories;
using Domain.Repositories.Common;
using Domain.Services;
using Infrastructure.Mqtt;
using Infrastructure.Persistence.Persistence;
using Infrastructure.Persistence.Persistence.Repositories;
using Infrastructure.Persistence.Persistence.Repositories.Common;
using Infrastructure.RaspberryPi.Sensors;
using Infrastructure.RaspberryPi.Sensors.Develop;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class ConfigureInfrastructure
{
    public static IServiceCollection RegisterPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<AppDbContext>(
            options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("db"),
                    builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
                )
        );
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddScoped<AppDbContextInitialiser>();

        services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        services.AddTransient<IDeviceRepository, DeviceRepository>();
        services.AddTransient<ISensorRepository, SensorRepository>();
        services.AddTransient<IMeasurementRepository, MeasurementRepository>();

        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddScoped<MqttSubscriber>();

        return services;
    }

    public static IServiceCollection RegisterHardware(
        this IServiceCollection services,
        bool isDevelopment
    )
    {
        if (isDevelopment)
        {
            services.AddSingleton<ISensor, Bme280DevelopSensor>();
        }
        else
        {
            services.AddSingleton<ISensor, Bme280Sensor>();
        }

        return services;
    }
}
