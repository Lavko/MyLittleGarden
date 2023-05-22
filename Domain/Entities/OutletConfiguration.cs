using Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Index(nameof(OutletConfiguration.OutletId), IsUnique = true)]
public class OutletConfiguration : BaseEntity
{
    public int OutletId { get; set; }
    public int PinId { get; set; }
    public string? Name { get; set; }
}