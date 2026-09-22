namespace ServicePortal.Models
{
    public class AppSettings
    {
        // Appearance
        public bool DarkMode { get; set; } = false;
        public string Theme { get; set; } = "SgsLight";
        public string TicketLayout { get; set; } = "Comfortable";

        // Ticket Defaults
        public string DefaultPriority { get; set; } = "Medium";
        public string DefaultCategory { get; set; } = "General";
        public int TicketsPerPage { get; set; } = 25;

        // Ticket Behaviour
        public bool ConfirmDelete { get; set; } = true;
        public bool ConfirmClose { get; set; } = true;
        public bool AutoRefresh { get; set; } = false;
        public bool RememberSettings { get; set; } = true;

        // Notifications
        public bool NotificationsEnabled { get; set; } = true;
        public bool NewTicketNotifications { get; set; } = true;
        public bool AssignmentNotifications { get; set; } = true;

        // User Preferences
        public string DisplayName { get; set; } = "";
        public string DefaultLandingPage { get; set; } = "Dashboard";

        // Dashboard
        public bool ShowRecentTickets { get; set; } = true;

        // Customer Onboarding
        public int OnboardingDefaultGoLiveDays { get; set; } = 30;
        public bool OnboardingShowCompleted { get; set; } = true;

        // Testing
        public string TestingDefaultSoftware { get; set; } = "eCERT";
        public int TestingDefaultLiveDateDays { get; set; } = 7;
        public bool TestingSecondTesterByDefault { get; set; } = false;
    }
}
