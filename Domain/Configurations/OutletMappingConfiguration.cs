namespace Domain.Configurations;

public class OutletMappingConfiguration
{
    public static string SectionName = "OutletMappings";

    public string Hello { get; set; } = null!;
    public Dictionary<int, int> Outlets { get; set; } = new();
}