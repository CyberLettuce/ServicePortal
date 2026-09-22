using ServicePortal.Components;
using ServicePortal.Data;
using ServicePortal.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
var ukCulture = CultureInfo.GetCultureInfo("en-GB");
CultureInfo.DefaultThreadCurrentCulture = ukCulture;
CultureInfo.DefaultThreadCurrentUICulture = ukCulture;

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection must be configured for SQL Server.");

builder.Services.AddAuthorization(options =>
{
    // Everything requires a signed-in user unless explicitly [AllowAnonymous].
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    // Administrator-only areas.
    options.AddPolicy("AdministratorOnly", policy =>
        policy.RequireRole("Administrator"));

    // Administrator or Support.
    options.AddPolicy("SupportOrAdministrator", policy =>
        policy.RequireRole("Administrator", "Support"));

    // Any recognised helpdesk user.
    options.AddPolicy("HelpdeskUser", policy =>
        policy.RequireRole(
            "Administrator",
            "Support",
            "ReadOnly"));

    // Read-only users can access enabled modules but cannot change records.
    options.AddPolicy("CanEdit", policy =>
        policy.RequireRole("Administrator", "Support"));

    options.AddPolicy("TicketsModule", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(
            new ModuleAccessRequirement(ModuleKeys.Tickets));
    });

    options.AddPolicy("OnboardingModule", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(
            new ModuleAccessRequirement(ModuleKeys.Onboarding));
    });

    options.AddPolicy("TestingModule", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(
            new ModuleAccessRequirement(ModuleKeys.Testing));
    });

    options.AddPolicy("ProjectsModule", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.AddRequirements(new ModuleAccessRequirement(ModuleKeys.Projects));
    });
});    

builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;

        options.User.RequireUniqueEmail = true;

        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(15);

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>,ApplicationUserClaimsPrincipalFactory>();

builder.Services.AddRazorPages();

builder.Services
    .AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<SettingsService>();
builder.Services.AddScoped<TicketService>();
builder.Services.AddScoped<AuditLogService>();
builder.Services.AddScoped<ModuleAccessService>();
builder.Services.AddScoped<OnboardingService>();
builder.Services.AddScoped<TestingService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TestKitDocumentService>();
builder.Services.AddScoped<AxosoftImportService>();
builder.Services.AddScoped<AutomationRunnerService>();
builder.Services.AddScoped<BrowserTestService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<CustomOnboardingFieldService>();
builder.Services.AddScoped<ChamberService>();
builder.Services.AddScoped<BrandingService>();
builder.Services.AddScoped<KnowledgeBaseService>();
builder.Services.AddScoped<ExistingCustomerService>();
builder.Services.AddScoped<IAuthorizationHandler, ModuleAccessHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var db = services.GetRequiredService<ApplicationDbContext>();

    await db.Database.EnsureCreatedAsync();

    if (db.Database.IsSqlServer())
    {
        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[Tickets]', N'U') IS NULL
            BEGIN
                CREATE TABLE [Tickets]
                (
                    [TicketNumber] int NOT NULL CONSTRAINT [PK_Tickets] PRIMARY KEY,
                    [TicketReference] nvarchar(30) NOT NULL CONSTRAINT [DF_Tickets_TicketReference] DEFAULT N'',
                    [Description] nvarchar(max) NOT NULL,
                    [Priority] nvarchar(max) NOT NULL,
                    [Status] nvarchar(max) NOT NULL,
                    [CreatedDate] datetime2 NOT NULL,
                    [Notes] nvarchar(max) NOT NULL,
                    [Category] nvarchar(max) NOT NULL,
                    [AssignedTo] nvarchar(max) NOT NULL,
                    [RequesterName] nvarchar(max) NOT NULL,
                    [RequesterEmail] nvarchar(max) NOT NULL,
                    [EmailSubject] nvarchar(max) NOT NULL
                );
            END

            IF OBJECT_ID(N'[TicketEmailMessages]', N'U') IS NULL
            BEGIN
                CREATE TABLE [TicketEmailMessages]
                (
                    [Id] uniqueidentifier NOT NULL CONSTRAINT [PK_TicketEmailMessages] PRIMARY KEY,
                    [Direction] nvarchar(max) NOT NULL,
                    [From] nvarchar(max) NOT NULL,
                    [To] nvarchar(max) NOT NULL,
                    [Subject] nvarchar(max) NOT NULL,
                    [Body] nvarchar(max) NOT NULL,
                    [SentAt] datetime2 NOT NULL,
                    [GraphMessageId] nvarchar(max) NOT NULL,
                    [DeliveryPending] bit NOT NULL,
                    [TicketNumber] int NOT NULL,
                    CONSTRAINT [FK_TicketEmailMessages_Tickets_TicketNumber]
                        FOREIGN KEY ([TicketNumber]) REFERENCES [Tickets]([TicketNumber]) ON DELETE CASCADE
                );
                CREATE INDEX [IX_TicketEmailMessages_TicketNumber]
                    ON [TicketEmailMessages]([TicketNumber]);
            END

            IF OBJECT_ID(N'[AutomationTestResults]', N'U') IS NULL
            BEGIN
                CREATE TABLE [AutomationTestResults]
                (
                    [Id] bigint NOT NULL IDENTITY CONSTRAINT [PK_AutomationTestResults] PRIMARY KEY,
                    [TestName] nvarchar(100) NOT NULL,
                    [Country] nvarchar(100) NOT NULL,
                    [Url] nvarchar(500) NOT NULL,
                    [Succeeded] bit NOT NULL,
                    [Summary] nvarchar(500) NOT NULL,
                    [Output] nvarchar(max) NOT NULL,
                    [StartedBy] nvarchar(256) NOT NULL,
                    [StartedAt] datetime2 NOT NULL,
                    [CompletedAt] datetime2 NOT NULL
                );
                CREATE INDEX [IX_AutomationTestResults_CompletedAt]
                    ON [AutomationTestResults]([CompletedAt] DESC);
            END

            IF OBJECT_ID(N'[BrowserTests]', N'U') IS NULL
            BEGIN
                CREATE TABLE [BrowserTests]
                (
                    [Id] int NOT NULL IDENTITY CONSTRAINT [PK_BrowserTests] PRIMARY KEY,
                    [Product] nvarchar(30) NOT NULL,
                    [Name] nvarchar(150) NOT NULL,
                    [StartUrl] nvarchar(500) NOT NULL,
                    [CreatedBy] nvarchar(256) NOT NULL,
                    [CreatedAt] datetime2 NOT NULL
                );
                CREATE TABLE [BrowserTestSteps]
                (
                    [Id] int NOT NULL IDENTITY CONSTRAINT [PK_BrowserTestSteps] PRIMARY KEY,
                    [BrowserTestId] int NOT NULL,
                    [SortOrder] int NOT NULL,
                    [Action] nvarchar(30) NOT NULL,
                    [Target] nvarchar(500) NOT NULL,
                    [Value] nvarchar(1000) NOT NULL,
                    CONSTRAINT [FK_BrowserTestSteps_BrowserTests] FOREIGN KEY ([BrowserTestId]) REFERENCES [BrowserTests]([Id]) ON DELETE CASCADE
                );
            END

            IF OBJECT_ID(N'[KnowledgeBaseCategories]', N'U') IS NULL
            BEGIN
                CREATE TABLE [KnowledgeBaseCategories] ([Id] int NOT NULL IDENTITY CONSTRAINT [PK_KnowledgeBaseCategories] PRIMARY KEY, [Name] nvarchar(100) NOT NULL, [SortOrder] int NOT NULL);
                CREATE TABLE [KnowledgeBaseArticles]
                (
                    [Id] int NOT NULL IDENTITY CONSTRAINT [PK_KnowledgeBaseArticles] PRIMARY KEY,
                    [CategoryId] int NOT NULL, [Title] nvarchar(200) NOT NULL, [Summary] nvarchar(500) NOT NULL,
                    [Symptoms] nvarchar(max) NOT NULL, [Resolution] nvarchar(max) NOT NULL, [AdditionalInformation] nvarchar(max) NOT NULL,
                    [Tags] nvarchar(500) NOT NULL, [Software] nvarchar(50) NOT NULL CONSTRAINT [DF_KnowledgeBaseArticles_Software] DEFAULT N'', [Subcategory] nvarchar(100) NOT NULL CONSTRAINT [DF_KnowledgeBaseArticles_Subcategory] DEFAULT N'', [IsPublished] bit NOT NULL, [IsArchived] bit NOT NULL,
                    [ViewCount] int NOT NULL, [UsefulCount] int NOT NULL, [NotUsefulCount] int NOT NULL, [UpdatedAt] datetime2 NOT NULL,
                    CONSTRAINT [FK_KnowledgeBaseArticles_KnowledgeBaseCategories] FOREIGN KEY ([CategoryId]) REFERENCES [KnowledgeBaseCategories]([Id])
                );
            END

            IF COL_LENGTH(N'[KnowledgeBaseArticles]', N'Software') IS NULL ALTER TABLE [KnowledgeBaseArticles] ADD [Software] nvarchar(50) NOT NULL CONSTRAINT [DF_KnowledgeBaseArticles_Software] DEFAULT N'';
            IF COL_LENGTH(N'[KnowledgeBaseArticles]', N'Subcategory') IS NULL ALTER TABLE [KnowledgeBaseArticles] ADD [Subcategory] nvarchar(100) NOT NULL CONSTRAINT [DF_KnowledgeBaseArticles_Subcategory] DEFAULT N'';

            IF OBJECT_ID(N'[KnowledgeBaseSoftwareOptions]', N'U') IS NULL
            BEGIN
                CREATE TABLE [KnowledgeBaseSoftwareOptions]
                (
                    [Id] int NOT NULL IDENTITY CONSTRAINT [PK_KnowledgeBaseSoftwareOptions] PRIMARY KEY,
                    [Software] nvarchar(50) NOT NULL,
                    [Subcategory] nvarchar(100) NOT NULL,
                    [SortOrder] int NOT NULL,
                    [ImageContent] varbinary(max) NULL,
                    [ImageContentType] nvarchar(100) NOT NULL CONSTRAINT [DF_KnowledgeBaseSoftwareOptions_ImageContentType] DEFAULT N''
                );
            END
            IF COL_LENGTH(N'[KnowledgeBaseSoftwareOptions]', N'ImageContent') IS NULL ALTER TABLE [KnowledgeBaseSoftwareOptions] ADD [ImageContent] varbinary(max) NULL;
            IF COL_LENGTH(N'[KnowledgeBaseSoftwareOptions]', N'ImageContentType') IS NULL ALTER TABLE [KnowledgeBaseSoftwareOptions] ADD [ImageContentType] nvarchar(100) NOT NULL CONSTRAINT [DF_KnowledgeBaseSoftwareOptions_ImageContentType] DEFAULT N'';

            IF OBJECT_ID(N'[Notifications]', N'U') IS NULL
            BEGIN
                CREATE TABLE [Notifications]
                (
                    [Id] int NOT NULL IDENTITY CONSTRAINT [PK_Notifications] PRIMARY KEY,
                    [RecipientUserId] nvarchar(450) NOT NULL,
                    [TicketNumber] int NULL,
                    [CustomerOnboardingId] int NULL,
                    [Message] nvarchar(500) NOT NULL,
                    [CreatedAt] datetime2 NOT NULL,
                    [IsRead] bit NOT NULL CONSTRAINT [DF_Notifications_IsRead] DEFAULT 0,
                    [ReadAt] datetime2 NULL,
                    CONSTRAINT [FK_Notifications_AspNetUsers_RecipientUserId]
                        FOREIGN KEY ([RecipientUserId]) REFERENCES [AspNetUsers]([Id]) ON DELETE CASCADE,
                    CONSTRAINT [FK_Notifications_Tickets_TicketNumber]
                        FOREIGN KEY ([TicketNumber]) REFERENCES [Tickets]([TicketNumber]) ON DELETE CASCADE,
                    CONSTRAINT [FK_Notifications_CustomerOnboardings_CustomerOnboardingId]
                        FOREIGN KEY ([CustomerOnboardingId]) REFERENCES [CustomerOnboardings]([Id]) ON DELETE CASCADE
                );
                CREATE INDEX [IX_Notifications_RecipientUserId_IsRead_CreatedAt]
                    ON [Notifications]([RecipientUserId], [IsRead], [CreatedAt] DESC);
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[Notifications]', N'U') IS NOT NULL
            BEGIN
                IF COL_LENGTH(N'[Notifications]', N'CustomerOnboardingId') IS NULL
                    ALTER TABLE [Notifications] ADD [CustomerOnboardingId] int NULL;

                IF COL_LENGTH(N'[Notifications]', N'ProjectId') IS NULL
                    ALTER TABLE [Notifications] ADD [ProjectId] int NULL;

                IF COL_LENGTH(N'[Notifications]', N'ExistingCustomerId') IS NULL
                    ALTER TABLE [Notifications] ADD [ExistingCustomerId] int NULL;

                IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'[Notifications]') AND name = N'TicketNumber' AND is_nullable = 0)
                    ALTER TABLE [Notifications] ALTER COLUMN [TicketNumber] int NULL;

                IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_Notifications_CustomerOnboardings_CustomerOnboardingId')
                    ALTER TABLE [Notifications] ADD CONSTRAINT [FK_Notifications_CustomerOnboardings_CustomerOnboardingId]
                    FOREIGN KEY ([CustomerOnboardingId]) REFERENCES [CustomerOnboardings]([Id]) ON DELETE CASCADE;
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[ProjectFollowers]', N'U') IS NULL
            BEGIN
                CREATE TABLE [ProjectFollowers]
                (
                    [ProjectId] int NOT NULL,
                    [UserId] nvarchar(450) NOT NULL,
                    CONSTRAINT [PK_ProjectFollowers] PRIMARY KEY ([ProjectId], [UserId]),
                    CONSTRAINT [FK_ProjectFollowers_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) ON DELETE CASCADE
                );
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[ProjectPosts]', N'U') IS NULL
            BEGIN
                CREATE TABLE [ProjectPosts] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectPosts] PRIMARY KEY, [ProjectId] int NOT NULL, [Message] nvarchar(3000) NOT NULL, [Author] nvarchar(150) NOT NULL, [CreatedAt] datetime2 NOT NULL, CONSTRAINT [FK_ProjectPosts_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) ON DELETE CASCADE);
                CREATE TABLE [ProjectPostComments] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectPostComments] PRIMARY KEY, [ProjectPostId] int NOT NULL, [Author] nvarchar(150) NOT NULL, [Message] nvarchar(2000) NOT NULL, [CreatedAt] datetime2 NOT NULL, CONSTRAINT [FK_ProjectPostComments_ProjectPosts] FOREIGN KEY ([ProjectPostId]) REFERENCES [ProjectPosts]([Id]) ON DELETE CASCADE);
                CREATE TABLE [ProjectPostReactions] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectPostReactions] PRIMARY KEY, [ProjectPostId] int NOT NULL, [UserId] nvarchar(450) NOT NULL, [Emoji] nvarchar(20) NOT NULL, CONSTRAINT [FK_ProjectPostReactions_ProjectPosts] FOREIGN KEY ([ProjectPostId]) REFERENCES [ProjectPosts]([Id]) ON DELETE CASCADE);
                CREATE TABLE [ProjectRoles] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectRoles] PRIMARY KEY, [ProjectId] int NOT NULL, [RoleName] nvarchar(100) NOT NULL, [PersonName] nvarchar(150) NOT NULL, CONSTRAINT [FK_ProjectRoles_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) ON DELETE CASCADE);
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[CustomerOnboardings]', N'U') IS NOT NULL
            BEGIN
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SignedDocumentsReceivedDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SignedDocumentsReceivedDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'OrderDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [OrderDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'AdminToolAndPriceGroupDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [AdminToolAndPriceGroupDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'PricingSetupDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [PricingSetupDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SystemBuildDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SystemBuildDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'TrainingGivenStatus') IS NULL ALTER TABLE [CustomerOnboardings] ADD [TrainingGivenStatus] nvarchar(50) NOT NULL CONSTRAINT [DF_CustomerOnboardings_TrainingGivenStatus] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'WelcomeEmailSentDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [WelcomeEmailSentDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'PracticeSiteAccessDetails') IS NULL ALTER TABLE [CustomerOnboardings] ADD [PracticeSiteAccessDetails] nvarchar(500) NOT NULL CONSTRAINT [DF_CustomerOnboardings_PracticeSiteAccessDetails] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SalesManager') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SalesManager] nvarchar(150) NOT NULL CONSTRAINT [DF_CustomerOnboardings_SalesManager] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'AxosoftId') IS NULL ALTER TABLE [CustomerOnboardings] ADD [AxosoftId] nvarchar(100) NOT NULL CONSTRAINT [DF_CustomerOnboardings_AxosoftId] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'OrderDateAccepted') IS NULL ALTER TABLE [CustomerOnboardings] ADD [OrderDateAccepted] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'PaymentReceivedDate') IS NULL ALTER TABLE [CustomerOnboardings] ADD [PaymentReceivedDate] datetime2 NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SubscriptionInvoiceNumber') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SubscriptionInvoiceNumber] nvarchar(100) NOT NULL CONSTRAINT [DF_CustomerOnboardings_SubscriptionInvoiceNumber] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SystemBuildStatus') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SystemBuildStatus] nvarchar(50) NOT NULL CONSTRAINT [DF_CustomerOnboardings_SystemBuildStatus] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SendToBrokerage') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SendToBrokerage] nvarchar(50) NOT NULL CONSTRAINT [DF_CustomerOnboardings_SendToBrokerage] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'TrainingStatus') IS NULL ALTER TABLE [CustomerOnboardings] ADD [TrainingStatus] nvarchar(50) NOT NULL CONSTRAINT [DF_CustomerOnboardings_TrainingStatus] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SalesOwner') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SalesOwner] nvarchar(150) NOT NULL CONSTRAINT [DF_CustomerOnboardings_SalesOwner] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'SoftwareOwners') IS NULL ALTER TABLE [CustomerOnboardings] ADD [SoftwareOwners] nvarchar(500) NOT NULL CONSTRAINT [DF_CustomerOnboardings_SoftwareOwners] DEFAULT N'';
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[CustomOnboardingFields]', N'U') IS NULL
            BEGIN
                CREATE TABLE [CustomOnboardingFields] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_CustomOnboardingFields] PRIMARY KEY, [Label] nvarchar(100) NOT NULL, [FieldType] nvarchar(20) NOT NULL, [Options] nvarchar(1000) NOT NULL, [SortOrder] int NOT NULL);
                CREATE TABLE [CustomOnboardingFieldValues] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_CustomOnboardingFieldValues] PRIMARY KEY, [CustomerOnboardingId] int NOT NULL, [CustomOnboardingFieldId] int NOT NULL, [Value] nvarchar(2000) NOT NULL, CONSTRAINT [FK_CustomOnboardingFieldValues_CustomerOnboardings] FOREIGN KEY ([CustomerOnboardingId]) REFERENCES [CustomerOnboardings]([Id]) ON DELETE CASCADE, CONSTRAINT [FK_CustomOnboardingFieldValues_CustomOnboardingFields] FOREIGN KEY ([CustomOnboardingFieldId]) REFERENCES [CustomOnboardingFields]([Id]) ON DELETE CASCADE);
                CREATE UNIQUE INDEX [IX_CustomOnboardingFieldValues_CustomerOnboardingId_CustomOnboardingFieldId] ON [CustomOnboardingFieldValues]([CustomerOnboardingId], [CustomOnboardingFieldId]);
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF COL_LENGTH(N'[Tickets]', N'ChamberOfCommerce') IS NULL
                ALTER TABLE [Tickets] ADD [ChamberOfCommerce] nvarchar(250) NOT NULL CONSTRAINT [DF_Tickets_ChamberOfCommerce] DEFAULT N'';
            IF OBJECT_ID(N'[ChambersOfCommerce]', N'U') IS NULL
                CREATE TABLE [ChambersOfCommerce] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ChambersOfCommerce] PRIMARY KEY, [Name] nvarchar(250) NOT NULL);
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF COL_LENGTH(N'[Tickets]', N'TicketReference') IS NULL
                ALTER TABLE [Tickets] ADD [TicketReference] nvarchar(30) NOT NULL CONSTRAINT [DF_Tickets_TicketReference] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'ProjectId') IS NULL
                ALTER TABLE [Tickets] ADD [ProjectId] int NULL;
            UPDATE [Tickets] SET [Category] = N'UKCS Portal' WHERE [Category] = N'ePortal';
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[TicketTemplates]', N'U') IS NULL
                CREATE TABLE [TicketTemplates] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_TicketTemplates] PRIMARY KEY, [Name] nvarchar(100) NOT NULL, [Description] nvarchar(3000) NOT NULL DEFAULT N'', [MainIssue] nvarchar(500) NOT NULL DEFAULT N'', [AdditionalDetails] nvarchar(3000) NOT NULL DEFAULT N'', [ReferencesLrn] nvarchar(500) NOT NULL DEFAULT N'', [MainContact] nvarchar(500) NOT NULL DEFAULT N'', [ActionsTaken] nvarchar(3000) NOT NULL DEFAULT N'', [SoftwareModule] nvarchar(250) NOT NULL DEFAULT N'', [Priority] nvarchar(20) NOT NULL, [Category] nvarchar(100) NOT NULL, [AssignedTo] nvarchar(150) NOT NULL, [UpdatedAt] datetime2 NOT NULL);
            IF COL_LENGTH(N'[TicketTemplates]', N'Description') IS NULL ALTER TABLE [TicketTemplates] ADD [Description] nvarchar(3000) NOT NULL CONSTRAINT [DF_TicketTemplates_Description] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'MainIssue') IS NULL ALTER TABLE [TicketTemplates] ADD [MainIssue] nvarchar(500) NOT NULL CONSTRAINT [DF_TicketTemplates_MainIssue] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'AdditionalDetails') IS NULL ALTER TABLE [TicketTemplates] ADD [AdditionalDetails] nvarchar(3000) NOT NULL CONSTRAINT [DF_TicketTemplates_AdditionalDetails] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'ReferencesLrn') IS NULL ALTER TABLE [TicketTemplates] ADD [ReferencesLrn] nvarchar(500) NOT NULL CONSTRAINT [DF_TicketTemplates_ReferencesLrn] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'MainContact') IS NULL ALTER TABLE [TicketTemplates] ADD [MainContact] nvarchar(500) NOT NULL CONSTRAINT [DF_TicketTemplates_MainContact] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'ActionsTaken') IS NULL ALTER TABLE [TicketTemplates] ADD [ActionsTaken] nvarchar(3000) NOT NULL CONSTRAINT [DF_TicketTemplates_ActionsTaken] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'SoftwareModule') IS NULL ALTER TABLE [TicketTemplates] ADD [SoftwareModule] nvarchar(250) NOT NULL CONSTRAINT [DF_TicketTemplates_SoftwareModule] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'Mrn') IS NULL ALTER TABLE [TicketTemplates] ADD [Mrn] nvarchar(250) NOT NULL CONSTRAINT [DF_TicketTemplates_Mrn] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'Arc') IS NULL ALTER TABLE [TicketTemplates] ADD [Arc] nvarchar(250) NOT NULL CONSTRAINT [DF_TicketTemplates_Arc] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'Tsn') IS NULL ALTER TABLE [TicketTemplates] ADD [Tsn] nvarchar(250) NOT NULL CONSTRAINT [DF_TicketTemplates_Tsn] DEFAULT N'';
            IF COL_LENGTH(N'[TicketTemplates]', N'ShipmentReference') IS NULL ALTER TABLE [TicketTemplates] ADD [ShipmentReference] nvarchar(250) NOT NULL CONSTRAINT [DF_TicketTemplates_ShipmentReference] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'ExistingCustomerId') IS NULL ALTER TABLE [Tickets] ADD [ExistingCustomerId] int NULL;
            IF COL_LENGTH(N'[Tickets]', N'MainIssue') IS NULL ALTER TABLE [Tickets] ADD [MainIssue] nvarchar(500) NOT NULL CONSTRAINT [DF_Tickets_MainIssue] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'AdditionalDetails') IS NULL ALTER TABLE [Tickets] ADD [AdditionalDetails] nvarchar(3000) NOT NULL CONSTRAINT [DF_Tickets_AdditionalDetails] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'ReferencesLrn') IS NULL ALTER TABLE [Tickets] ADD [ReferencesLrn] nvarchar(500) NOT NULL CONSTRAINT [DF_Tickets_ReferencesLrn] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'MainContact') IS NULL ALTER TABLE [Tickets] ADD [MainContact] nvarchar(500) NOT NULL CONSTRAINT [DF_Tickets_MainContact] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'ActionsTaken') IS NULL ALTER TABLE [Tickets] ADD [ActionsTaken] nvarchar(3000) NOT NULL CONSTRAINT [DF_Tickets_ActionsTaken] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'SoftwareModule') IS NULL ALTER TABLE [Tickets] ADD [SoftwareModule] nvarchar(250) NOT NULL CONSTRAINT [DF_Tickets_SoftwareModule] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'Mrn') IS NULL ALTER TABLE [Tickets] ADD [Mrn] nvarchar(250) NOT NULL CONSTRAINT [DF_Tickets_Mrn] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'Arc') IS NULL ALTER TABLE [Tickets] ADD [Arc] nvarchar(250) NOT NULL CONSTRAINT [DF_Tickets_Arc] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'Tsn') IS NULL ALTER TABLE [Tickets] ADD [Tsn] nvarchar(250) NOT NULL CONSTRAINT [DF_Tickets_Tsn] DEFAULT N'';
            IF COL_LENGTH(N'[Tickets]', N'ShipmentReference') IS NULL ALTER TABLE [Tickets] ADD [ShipmentReference] nvarchar(250) NOT NULL CONSTRAINT [DF_Tickets_ShipmentReference] DEFAULT N'';
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[ExistingCustomers]', N'U') IS NULL
                CREATE TABLE [ExistingCustomers] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ExistingCustomers] PRIMARY KEY, [CustomerName] nvarchar(200) NOT NULL, [ContactName] nvarchar(200) NOT NULL, [ContactEmail] nvarchar(256) NOT NULL, [ProductsPurchased] nvarchar(500) NOT NULL, [SoftwareDetails] nvarchar(500) NOT NULL, [ImplementationOwner] nvarchar(150) NOT NULL, [ClientNumber] nvarchar(100) NOT NULL, [Status] nvarchar(50) NOT NULL, [CreatedAt] datetime2 NOT NULL, [UpdatedAt] datetime2 NOT NULL);
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF COL_LENGTH(N'[ExistingCustomers]', N'ChamberOfCommerce') IS NULL ALTER TABLE [ExistingCustomers] ADD [ChamberOfCommerce] nvarchar(250) NOT NULL CONSTRAINT [DF_ExistingCustomers_ChamberOfCommerce] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'Documents') IS NULL ALTER TABLE [ExistingCustomers] ADD [Documents] nvarchar(1000) NOT NULL CONSTRAINT [DF_ExistingCustomers_Documents] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'LanguagesRequired') IS NULL ALTER TABLE [ExistingCustomers] ADD [LanguagesRequired] nvarchar(1000) NOT NULL CONSTRAINT [DF_ExistingCustomers_LanguagesRequired] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'ProjectNumber') IS NULL ALTER TABLE [ExistingCustomers] ADD [ProjectNumber] nvarchar(100) NOT NULL CONSTRAINT [DF_ExistingCustomers_ProjectNumber] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'SalesManager') IS NULL ALTER TABLE [ExistingCustomers] ADD [SalesManager] nvarchar(150) NOT NULL CONSTRAINT [DF_ExistingCustomers_SalesManager] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'GoLiveDate') IS NULL ALTER TABLE [ExistingCustomers] ADD [GoLiveDate] datetime2 NULL;
            IF COL_LENGTH(N'[ExistingCustomers]', N'RenewalDate') IS NULL ALTER TABLE [ExistingCustomers] ADD [RenewalDate] datetime2 NULL;
            IF COL_LENGTH(N'[ExistingCustomers]', N'VocCompleted') IS NULL ALTER TABLE [ExistingCustomers] ADD [VocCompleted] bit NOT NULL CONSTRAINT [DF_ExistingCustomers_VocCompleted] DEFAULT 0;
            IF COL_LENGTH(N'[ExistingCustomers]', N'Feedback') IS NULL ALTER TABLE [ExistingCustomers] ADD [Feedback] nvarchar(4000) NOT NULL CONSTRAINT [DF_ExistingCustomers_Feedback] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'VocContactName') IS NULL ALTER TABLE [ExistingCustomers] ADD [VocContactName] nvarchar(200) NOT NULL CONSTRAINT [DF_ExistingCustomers_VocContactName] DEFAULT N'';
            IF COL_LENGTH(N'[ExistingCustomers]', N'VocCompletedDate') IS NULL ALTER TABLE [ExistingCustomers] ADD [VocCompletedDate] datetime2 NULL;
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[Projects]', N'U') IS NOT NULL
            BEGIN
                IF COL_LENGTH(N'[Projects]', N'ProjectLead') IS NULL ALTER TABLE [Projects] ADD [ProjectLead] nvarchar(150) NOT NULL CONSTRAINT [DF_Projects_ProjectLead] DEFAULT N'';
                IF COL_LENGTH(N'[Projects]', N'Developer') IS NULL ALTER TABLE [Projects] ADD [Developer] nvarchar(150) NOT NULL CONSTRAINT [DF_Projects_Developer] DEFAULT N'';
                IF COL_LENGTH(N'[Projects]', N'ImageFileName') IS NULL ALTER TABLE [Projects] ADD [ImageFileName] nvarchar(260) NOT NULL CONSTRAINT [DF_Projects_ImageFileName] DEFAULT N'';
                IF COL_LENGTH(N'[Projects]', N'ImageContent') IS NULL ALTER TABLE [Projects] ADD [ImageContent] varbinary(max) NULL;
                IF COL_LENGTH(N'[Projects]', N'ImageContentType') IS NULL ALTER TABLE [Projects] ADD [ImageContentType] nvarchar(100) NOT NULL CONSTRAINT [DF_Projects_ImageContentType] DEFAULT N'';
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[CustomerOnboardings]', N'U') IS NOT NULL
            BEGIN
                IF COL_LENGTH(N'[CustomerOnboardings]', N'ImageFileName') IS NULL ALTER TABLE [CustomerOnboardings] ADD [ImageFileName] nvarchar(260) NOT NULL CONSTRAINT [DF_CustomerOnboardings_ImageFileName] DEFAULT N'';
                IF COL_LENGTH(N'[CustomerOnboardings]', N'ImageContent') IS NULL ALTER TABLE [CustomerOnboardings] ADD [ImageContent] varbinary(max) NULL;
                IF COL_LENGTH(N'[CustomerOnboardings]', N'ImageContentType') IS NULL ALTER TABLE [CustomerOnboardings] ADD [ImageContentType] nvarchar(100) NOT NULL CONSTRAINT [DF_CustomerOnboardings_ImageContentType] DEFAULT N'';
            END
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[PortalBranding]', N'U') IS NULL
                CREATE TABLE [PortalBranding] ([Id] int NOT NULL CONSTRAINT [PK_PortalBranding] PRIMARY KEY, [LogoContent] varbinary(max) NULL, [LogoContentType] nvarchar(100) NOT NULL CONSTRAINT [DF_PortalBranding_LogoContentType] DEFAULT N'');
            """);

        await db.Database.ExecuteSqlRawAsync("""
            IF OBJECT_ID(N'[ProjectFollowers]', N'U') IS NULL
                CREATE TABLE [ProjectFollowers] ([ProjectId] int NOT NULL, [UserId] nvarchar(450) NOT NULL, CONSTRAINT [PK_ProjectFollowers] PRIMARY KEY ([ProjectId], [UserId]), CONSTRAINT [FK_ProjectFollowers_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) ON DELETE CASCADE);
            IF OBJECT_ID(N'[ProjectPosts]', N'U') IS NULL
                CREATE TABLE [ProjectPosts] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectPosts] PRIMARY KEY, [ProjectId] int NOT NULL, [Message] nvarchar(3000) NOT NULL, [Author] nvarchar(150) NOT NULL, [CreatedAt] datetime2 NOT NULL, CONSTRAINT [FK_ProjectPosts_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) ON DELETE CASCADE);
            IF OBJECT_ID(N'[ProjectPostComments]', N'U') IS NULL
                CREATE TABLE [ProjectPostComments] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectPostComments] PRIMARY KEY, [ProjectPostId] int NOT NULL, [Author] nvarchar(150) NOT NULL, [Message] nvarchar(2000) NOT NULL, [CreatedAt] datetime2 NOT NULL, CONSTRAINT [FK_ProjectPostComments_ProjectPosts] FOREIGN KEY ([ProjectPostId]) REFERENCES [ProjectPosts]([Id]) ON DELETE CASCADE);
            IF OBJECT_ID(N'[ProjectPostReactions]', N'U') IS NULL
                CREATE TABLE [ProjectPostReactions] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectPostReactions] PRIMARY KEY, [ProjectPostId] int NOT NULL, [UserId] nvarchar(450) NOT NULL, [Emoji] nvarchar(20) NOT NULL, CONSTRAINT [FK_ProjectPostReactions_ProjectPosts] FOREIGN KEY ([ProjectPostId]) REFERENCES [ProjectPosts]([Id]) ON DELETE CASCADE);
            IF OBJECT_ID(N'[ProjectRoles]', N'U') IS NULL
                CREATE TABLE [ProjectRoles] ([Id] int IDENTITY NOT NULL CONSTRAINT [PK_ProjectRoles] PRIMARY KEY, [ProjectId] int NOT NULL, [RoleName] nvarchar(100) NOT NULL, [PersonName] nvarchar(150) NOT NULL, CONSTRAINT [FK_ProjectRoles_Projects] FOREIGN KEY ([ProjectId]) REFERENCES [Projects]([Id]) ON DELETE CASCADE);
            """);
    }

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles =
    {
        "Administrator",
        "Support",
        "ReadOnly"
    };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(
                new IdentityRole(role));
        }
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

// app.UseStatusCodePagesWithReExecute(
//     "/not-found",
//     createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

var pathBase = app.Configuration["PathBase"];
if (!string.IsNullOrWhiteSpace(pathBase))
{
    app.UsePathBase(pathBase);
}

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets()
    .Add(endpointBuilder =>
        endpointBuilder.Metadata.Add(
            new AllowAnonymousAttribute()));

app.MapRazorPages();

app.MapGet("/brand-image", async (IDbContextFactory<ApplicationDbContext> dbFactory, IWebHostEnvironment environment) =>
{
    await using var db = await dbFactory.CreateDbContextAsync();
    var branding = await db.Set<PortalBranding>().AsNoTracking().SingleOrDefaultAsync(item => item.Id == 1);
    if (branding?.LogoContent is { Length: > 0 })
        return Results.File(branding.LogoContent, string.IsNullOrWhiteSpace(branding.LogoContentType) ? "application/octet-stream" : branding.LogoContentType);
    return Results.File(Path.Combine(environment.WebRootPath, "SGS.png"), "image/png");
});

app.MapGet("/uploads/project-images/{fileName}", async (string fileName, IDbContextFactory<ApplicationDbContext> dbFactory) =>
{
    await using var db = await dbFactory.CreateDbContextAsync();
    var image = await db.Projects.AsNoTracking()
        .Where(project => project.ImageFileName == fileName && project.ImageContent != null)
        .Select(project => new { project.ImageContent, project.ImageContentType })
        .SingleOrDefaultAsync();

    return image?.ImageContent is { Length: > 0 }
        ? Results.File(image.ImageContent, string.IsNullOrWhiteSpace(image.ImageContentType) ? "application/octet-stream" : image.ImageContentType)
        : Results.NotFound();
});

app.MapGet("/onboarding-images/{id:int}", async (int id, IDbContextFactory<ApplicationDbContext> dbFactory) =>
{
    await using var db = await dbFactory.CreateDbContextAsync();
    var image = await db.CustomerOnboardings.AsNoTracking()
        .Where(onboarding => onboarding.Id == id && onboarding.ImageContent != null)
        .Select(onboarding => new { onboarding.ImageContent, onboarding.ImageContentType })
        .SingleOrDefaultAsync();
    return image?.ImageContent is { Length: > 0 }
        ? Results.File(image.ImageContent, string.IsNullOrWhiteSpace(image.ImageContentType) ? "application/octet-stream" : image.ImageContentType)
        : Results.NotFound();
});

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapGet("/testing/test-kits/{id:int}/download",
    [Authorize(Policy = "TestingModule")] async (
        int id,
        IDbContextFactory<ApplicationDbContext> dbFactory) =>
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var document = await db.TestKitDocuments.FindAsync(id);
        return document is null
            ? Results.NotFound()
            : Results.File(document.Content, document.ContentType, document.OriginalFileName);
    });

app.Run();





