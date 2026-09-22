using System.ComponentModel.DataAnnotations;

namespace ServicePortal.Data;

public sealed class ExistingCustomer
{
    public int Id { get; set; }
    [Required, MaxLength(200)] public string CustomerName { get; set; } = string.Empty;
    [MaxLength(200)] public string ContactName { get; set; } = string.Empty;
    [EmailAddress, MaxLength(256)] public string ContactEmail { get; set; } = string.Empty;
    [MaxLength(500)] public string ProductsPurchased { get; set; } = string.Empty;
    [MaxLength(500)] public string SoftwareDetails { get; set; } = string.Empty;
    [MaxLength(150)] public string ImplementationOwner { get; set; } = string.Empty;
    [MaxLength(100)] public string ClientNumber { get; set; } = string.Empty;
    [MaxLength(250)] public string ChamberOfCommerce { get; set; } = string.Empty;
    [MaxLength(1000)] public string Documents { get; set; } = string.Empty;
    [MaxLength(1000)] public string LanguagesRequired { get; set; } = string.Empty;
    [MaxLength(100)] public string ProjectNumber { get; set; } = string.Empty;
    [MaxLength(150)] public string SalesManager { get; set; } = string.Empty;
    public DateTime? GoLiveDate { get; set; }
    public DateTime? RenewalDate { get; set; }
    public bool VocCompleted { get; set; }
    [MaxLength(4000)] public string Feedback { get; set; } = string.Empty;
    [MaxLength(200)] public string VocContactName { get; set; } = string.Empty;
    public DateTime? VocCompletedDate { get; set; }
    [MaxLength(50)] public string Status { get; set; } = "Active";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}





