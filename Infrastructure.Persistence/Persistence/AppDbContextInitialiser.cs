using Domain.Entities;
using Domain.Repositories.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Persistence;

public class AppDbContextInitialiser
{
    private readonly ILogger<AppDbContextInitialiser> _logger;
    private readonly AppDbContext _context;
    private readonly IUnitOfWork _unitOfWork;

    public AppDbContextInitialiser(
        ILogger<AppDbContextInitialiser> logger,
        AppDbContext context,
        IUnitOfWork unitOfWork
    )
    {
        _logger = logger;
        _context = context;
        _unitOfWork = unitOfWork;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            if (_context.Database.IsSqlServer())
            {
                _logger.LogDebug("Migrating DB");
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            _logger.LogDebug("Starting seed DB task");
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    private async Task TrySeedAsync()
    {
        await SeedAppConfiguration();
        await SeedDevices();

        await _unitOfWork.CompleteAsync();
    }

    private async Task SeedAppConfiguration()
    {
        if (await _unitOfWork.AppConfiguration.GetCurrent() is null)
        {
            var appConfig = new AppConfiguration
            {
                MqttConfiguration = new MqttConfiguration
                {
                    IsClientEnabled = false,
                    ClientId = string.Empty,
                    TcpServer = string.Empty,
                    Login = string.Empty,
                    Password = string.Empty,
                    TimeStamp = DateTime.Now
                },
                TimeStamp = DateTime.Now
            };

            _unitOfWork.AppConfiguration.Add(appConfig);
        }
    }

    private async Task SeedDevices()
    {
        var savedDevice = await _unitOfWork.Devices.FindAsync(d => d.Name == "TestDevice");

        if (!savedDevice.Any())
        {
            var testDevice = new Device
            {
                Name = "TestDevice",
                Path = "/testDevice",
                TimeStamp = DateTime.Now,
                Sensors = new List<Sensor>
                {
                    new()
                    {
                        Name = "TestSensor",
                        MqttPath = "/testSensor",
                        TimeStamp = DateTime.Now,
                        Measurements = new List<Measurement>
                        {
                            new() { Result = "0", TimeStamp = DateTime.Now },
                            new() { Result = "1", TimeStamp = DateTime.Now },
                            new() { Result = "2", TimeStamp = DateTime.Now },
                            new() { Result = "3", TimeStamp = DateTime.Now },
                        }
                    },
                    new()
                    {
                        Name = "AnotherTestSensor",
                        MqttPath = "/anotherTestSensor",
                        TimeStamp = DateTime.Now,
                        Measurements = new List<Measurement>
                        {
                            new() { Result = "100", TimeStamp = DateTime.Now },
                            new() { Result = "50", TimeStamp = DateTime.Now },
                            new() { Result = "135", TimeStamp = DateTime.Now },
                            new() { Result = "3", TimeStamp = DateTime.Now },
                        }
                    }
                }
            };
            _unitOfWork.Devices.Add(testDevice);
        }
    }
}
