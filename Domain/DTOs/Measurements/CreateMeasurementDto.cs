namespace Domain.DTOs.Measurements;

public class CreateMeasurementDto
{
    public int SensorId { get; set; }
    public string Result { get; set; } = null!;
}