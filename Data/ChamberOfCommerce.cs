using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class ChamberOfCommerce
{
    public int Id { get; set; }
    [Required, MaxLength(250)] public string Name { get; set; } = string.Empty;
}
