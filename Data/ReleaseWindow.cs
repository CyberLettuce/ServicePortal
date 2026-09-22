using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class ReleaseWindow
{
    public int Id { get; set; }
    [Required, MaxLength(50)] public string Product { get; set; } = string.Empty;
    [Required, MaxLength(100)] public string ReleaseNumber { get; set; } = string.Empty;
    public DateTime Month { get; set; }
    [Required, MaxLength(30)] public string Status { get; set; } = "In Testing";
    public DateTime? LiveDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
