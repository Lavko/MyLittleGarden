namespace Domain.DTOs.Measures;

public class MeasureDto
{
    public int Id { get; set; }
    public DateTime TimeStamp { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public double GroundHumidity { get; set; }
    public double Pressure { get; set; }
}