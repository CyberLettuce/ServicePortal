namespace ServicePortal.Models;

public class Ticket
{
    public int TicketNumber { get; set; }
    public string TicketReference { get; set; } = "";
    public string Description { get; set; } = "";
    public string Priority { get; set; } = "";
    public string Status { get; set; } = "Open";
    public DateTime CreatedDate { get; set; }
    public string Notes { get; set; } = "";
    public string Category { get; set; } = "General";
    public string ChamberOfCommerce { get; set; } = "";
    public string AssignedTo { get; set; } = "";
    public int? ProjectId { get; set; }
    public int? ExistingCustomerId { get; set; }
    public string MainIssue { get; set; } = "";
    public string AdditionalDetails { get; set; } = "";
    public string ReferencesLrn { get; set; } = "";
    public string MainContact { get; set; } = "";
    public string ActionsTaken { get; set; } = "";
    public string SoftwareModule { get; set; } = "";
    public string Mrn { get; set; } = "";
    public string Arc { get; set; } = "";
    public string Tsn { get; set; } = "";
    public string ShipmentReference { get; set; } = "";

    public string RequesterName { get; set; } = "";

    public string RequesterEmail { get; set; } = "";

    public string EmailSubject { get; set; } = "";

    public List<TicketEmailMessage> EmailMessages { get; set; } = new();
}
