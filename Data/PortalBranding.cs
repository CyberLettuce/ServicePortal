using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class PortalBranding
{
    public int Id { get; set; }
    public byte[]? LogoContent { get; set; }
    [MaxLength(100)] public string LogoContentType { get; set; } = string.Empty;
}
