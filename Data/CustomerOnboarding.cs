using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServicePortal.Data;

public sealed class CustomerOnboarding
{
    public int Id { get; set; }

    [MaxLength(100)]
    public string OnboardingReference { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string CustomerName { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string ContactName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(256)]
    public string ContactEmail { get; set; } = string.Empty;

    [Required, MaxLength(500)]
    public string ProductsPurchased { get; set; } = string.Empty;

    [MaxLength(150)]
    public string ImplementationOwner { get; set; } = string.Empty;

    [MaxLength(100)]
    public string ClientNumber { get; set; } = string.Empty;

    [MaxLength(250)]
    public string EgtaSubscription { get; set; } = string.Empty;

    [MaxLength(100)]
    public string AxosoftId { get; set; } = string.Empty;

    public DateTime? OrderDateAccepted { get; set; }

    public DateTime? PaymentReceivedDate { get; set; }

    [MaxLength(100)]
    public string SubscriptionInvoiceNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string SystemBuildStatus { get; set; } = string.Empty;

    [MaxLength(50)]
    public string SendToBrokerage { get; set; } = string.Empty;

    [MaxLength(50)]
    public string TrainingStatus { get; set; } = string.Empty;

    [MaxLength(150)]
    public string SalesOwner { get; set; } = string.Empty;

    [MaxLength(500)]
    public string SoftwareOwners { get; set; } = string.Empty;

    public bool FinanceNotified { get; set; }

    public DateTime? FinanceNotifiedDate { get; set; }

    public bool BackOfficeNotified { get; set; }

    public DateTime? BackOfficeNotifiedDate { get; set; }

    [MaxLength(200)]
    public string ChamberContact { get; set; } = string.Empty;

    [MaxLength(250)]
    public string ChamberOfCommerce { get; set; } = string.Empty;

    public bool DDConfirmed { get; set; }

    public DateTime? DDConfirmedDate { get; set; }

    [MaxLength(1000)]
    public string LanguagesRequired { get; set; } = string.Empty;

    public bool OnboardingQuestionnaireReceived { get; set; }

    public DateTime? OnboardingQuestionnaireReceivedDate { get; set; }

    public bool Paid { get; set; }

    public bool PracticeSiteAccess { get; set; }

    public bool PricingSetupWithIT { get; set; }

    [MaxLength(100)]
    public string ProjectNumber { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string SharePointUrl { get; set; } = string.Empty;

    public bool SignedDocumentsReceived { get; set; }

    public DateTime? SignedDocumentsReceivedDate { get; set; }

    public DateTime? OrderDate { get; set; }

    public DateTime? AdminToolAndPriceGroupDate { get; set; }

    public DateTime? PricingSetupDate { get; set; }

    public DateTime? SubscriptionStartDate { get; set; }

    [MaxLength(500)]
    public string SystemBuild { get; set; } = string.Empty;

    public DateTime? SystemBuildDate { get; set; }

    [MaxLength(250)]
    public string TNetSubscription { get; set; } = string.Empty;

    public bool TrainingGiven { get; set; }

    [MaxLength(50)]
    public string TrainingGivenStatus { get; set; } = string.Empty;

    public bool TrainingRequired { get; set; }

    public bool WelcomeEmailSent { get; set; }

    public DateTime? WelcomeEmailSentDate { get; set; }

    [MaxLength(500)]
    public string PracticeSiteAccessDetails { get; set; } = string.Empty;

    [MaxLength(150)]
    public string SalesManager { get; set; } = string.Empty;

    public DateTime StartDate { get; set; }

    public DateTime TargetGoLiveDate { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Not Started";

    [MaxLength(2000)]
    public string Notes { get; set; } = string.Empty;

    [MaxLength(260)]
    public string ImageFileName { get; set; } = string.Empty;

    public byte[]? ImageContent { get; set; }

    [MaxLength(100)]
    public string ImageContentType { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<OnboardingChecklistItem> ChecklistItems { get; set; } = new();

    [NotMapped]
    public int CompletedTaskCount => ChecklistItems.Count(item => item.IsComplete);

    [NotMapped]
    public int ProgressPercent => ChecklistItems.Count == 0
        ? 0
        : (int)Math.Round(CompletedTaskCount * 100d / ChecklistItems.Count);
}
