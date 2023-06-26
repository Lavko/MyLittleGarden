namespace Domain.DTOs.Measurements;

public class MeasurementDto
{
    public int Id { get; set; }
    public string Result { get; set; } = null!;
    public DateTime TimeStamp { get; set; }
}