using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServicePortal.Models;

namespace ServicePortal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

        public DbSet<UserModule> UserModules => Set<UserModule>();

        public DbSet<CustomerOnboarding> CustomerOnboardings => Set<CustomerOnboarding>();
        public DbSet<ExistingCustomer> ExistingCustomers => Set<ExistingCustomer>();
        public DbSet<CustomOnboardingField> CustomOnboardingFields => Set<CustomOnboardingField>();
        public DbSet<CustomOnboardingFieldValue> CustomOnboardingFieldValues => Set<CustomOnboardingFieldValue>();

        public DbSet<OnboardingChecklistItem> OnboardingChecklistItems => Set<OnboardingChecklistItem>();

        public DbSet<TestPlan> TestPlans => Set<TestPlan>();

        public DbSet<TestRun> TestRuns => Set<TestRun>();
        public DbSet<Project> Projects => Set<Project>();
        public DbSet<ProjectUpdate> ProjectUpdates => Set<ProjectUpdate>();
        public DbSet<ProjectFollower> ProjectFollowers => Set<ProjectFollower>();
        public DbSet<ProjectPost> ProjectPosts => Set<ProjectPost>();
        public DbSet<ProjectPostComment> ProjectPostComments => Set<ProjectPostComment>();
        public DbSet<ProjectPostReaction> ProjectPostReactions => Set<ProjectPostReaction>();
        public DbSet<ProjectRole> ProjectRoles => Set<ProjectRole>();
        public DbSet<TestKitDocument> TestKitDocuments => Set<TestKitDocument>();
        public DbSet<ReleaseWindow> ReleaseWindows => Set<ReleaseWindow>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<TicketTemplate> TicketTemplates => Set<TicketTemplate>();
        public DbSet<TicketEmailMessage> TicketEmailMessages => Set<TicketEmailMessage>();
        public DbSet<AutomationTestResult> AutomationTestResults => Set<AutomationTestResult>();
        public DbSet<BrowserTest> BrowserTests => Set<BrowserTest>();
        public DbSet<BrowserTestStep> BrowserTestSteps => Set<BrowserTestStep>();
        public DbSet<Notification> Notifications => Set<Notification>();
        public DbSet<ChamberOfCommerce> ChambersOfCommerce => Set<ChamberOfCommerce>();
        public DbSet<PortalBranding> PortalBranding => Set<PortalBranding>();
        public DbSet<KnowledgeBaseCategory> KnowledgeBaseCategories => Set<KnowledgeBaseCategory>();
        public DbSet<KnowledgeArticle> KnowledgeBaseArticles => Set<KnowledgeArticle>();
        public DbSet<KnowledgeBaseSoftwareOption> KnowledgeBaseSoftwareOptions => Set<KnowledgeBaseSoftwareOption>();

        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserModule>()
                .HasKey(assignment => new
                {
                    assignment.UserId,
                    assignment.ModuleKey
                });

            builder.Entity<CustomerOnboarding>()
                .HasMany(onboarding => onboarding.ChecklistItems)
                .WithOne(item => item.CustomerOnboarding)
                .HasForeignKey(item => item.CustomerOnboardingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OnboardingChecklistItem>()
                .HasIndex(item => new
                {
                    item.CustomerOnboardingId,
                    item.SortOrder
                });

            builder.Entity<CustomOnboardingFieldValue>()
                .HasIndex(value => new { value.CustomerOnboardingId, value.CustomOnboardingFieldId })
                .IsUnique();

            builder.Entity<TestPlan>()
                .HasMany(plan => plan.TestRuns)
                .WithOne(run => run.TestPlan)
                .HasForeignKey(run => run.TestPlanId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<TestRun>()
                .HasIndex(run => new { run.ScheduledDate, run.Status });

            builder.Entity<BrowserTest>()
                .HasMany(test => test.Steps)
                .WithOne()
                .HasForeignKey(step => step.BrowserTestId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Project>()
                .HasMany(project => project.Updates)
                .WithOne(update => update.Project)
                .HasForeignKey(update => update.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProjectFollower>().HasKey(follower => new { follower.ProjectId, follower.UserId });
            builder.Entity<ProjectFollower>().HasOne<Project>().WithMany(project => project.Followers).HasForeignKey(follower => follower.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Project>().HasMany(project => project.Posts).WithOne().HasForeignKey(post => post.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Project>().HasMany(project => project.Roles).WithOne().HasForeignKey(role => role.ProjectId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProjectPost>().HasMany(post => post.Comments).WithOne().HasForeignKey(comment => comment.ProjectPostId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProjectPost>().HasMany(post => post.Reactions).WithOne().HasForeignKey(reaction => reaction.ProjectPostId).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ticket>()
                .HasKey(ticket => ticket.TicketNumber);

            builder.Entity<Ticket>()
                .Property(ticket => ticket.TicketNumber)
                .ValueGeneratedNever();

            builder.Entity<Ticket>()
                .HasMany(ticket => ticket.EmailMessages)
                .WithOne(message => message.Ticket)
                .HasForeignKey(message => message.TicketNumber)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TicketEmailMessage>()
                .HasIndex(message => message.TicketNumber);

            builder.Entity<Notification>()
                .HasIndex(notification => new
                {
                    notification.RecipientUserId,
                    notification.IsRead,
                    notification.CreatedAt
                });

            builder.Entity<Notification>()
                .HasOne<CustomerOnboarding>()
                .WithMany()
                .HasForeignKey(notification => notification.CustomerOnboardingId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
