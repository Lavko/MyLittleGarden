using Domain.Entities;
using Domain.Services;

namespace Infrastructure.GPIO;

public class Bme280DevelopService : IBme280Service
{
    public EnvironmentMeasure GetMeasures()
    {
        return new EnvironmentMeasure
            {
                Temperature = GetRandomNumber(19, 26),
                Pressure = GetRandomNumber(990, 1050),
                Humidity = GetRandomNumber(60, 100),
                TimeStamp = DateTime.Now
            };
    }
    
    private static double GetRandomNumber(double minimum, double maximum)
    { 
        var random = new Random();
        return random.NextDouble() * (maximum - minimum) + minimum;
    }
}