using System.Data.Common;

namespace Domain.DTOs;

public class OutletDto
{
    public int Id { get; set; }
    public int OutletId { get; set; }
    public int PinId { get; set; }
    public string Name { get; set; } = null!;
}