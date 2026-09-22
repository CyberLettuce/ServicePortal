namespace ServicePortal.Models;

public sealed class TicketEmailMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Direction { get; set; } = "Incoming";
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public string GraphMessageId { get; set; } = string.Empty;
    public bool DeliveryPending { get; set; }

    public int TicketNumber { get; set; }

    public Ticket? Ticket { get; set; }
}
