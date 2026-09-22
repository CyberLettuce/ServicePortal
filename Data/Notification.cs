namespace ServicePortal.Data;

public class Notification
{
    public int Id { get; set; }
    public string RecipientUserId { get; set; } = string.Empty;
    public int? TicketNumber { get; set; }
    public int? CustomerOnboardingId { get; set; }
    public int? ProjectId { get; set; }
    public int? ExistingCustomerId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}


