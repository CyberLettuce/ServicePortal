namespace ServicePortal.Models;

public sealed class Microsoft365EmailOptions
{
    public bool Enabled { get; set; }
    public string TenantId { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public string SupportMailbox { get; set; } = string.Empty;
}
