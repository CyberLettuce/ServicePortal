using ServicePortal.Data;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ServicePortal.Services;

public sealed record TestingWindow(
    string Product,
    string ReleaseNumber,
    DateTime Month,
    string Status,
    DateTime? LiveDate,
    int PointCount);

public sealed class TestingService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
    private readonly AuditLogService _auditLogService;

    public TestingService(IDbContextFactory<ApplicationDbContext> dbFactory, AuditLogService auditLogService)
    {
        _dbFactory = dbFactory;
        _auditLogService = auditLogService;
    }

    public async Task<List<TestPlan>> GetPlansAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.TestPlans.AsNoTracking().Where(plan => plan.IsActive)
            .OrderBy(plan => plan.Product).ThenBy(plan => plan.Scope).ThenBy(plan => plan.TestCaseName)
            .ToListAsync();
    }

    public async Task<List<TestRun>> GetRunsAsync(DateTime month)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        var end = start.AddMonths(1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.TestRuns.AsNoTracking()
            .Where(run => run.ScheduledDate >= start && run.ScheduledDate < end)
            .OrderBy(run => run.ScheduledDate).ThenBy(run => run.Product).ThenBy(run => run.Scope)
            .ToListAsync();
    }

    public async Task<TestRun?> GetRunAsync(int runId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.TestRuns.AsNoTracking().SingleOrDefaultAsync(run => run.Id == runId);
    }

    public async Task<List<TestingWindow>> GetWindowsAsync(bool previous)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var runs = await db.TestRuns.AsNoTracking()
            .Where(run => !string.IsNullOrWhiteSpace(run.ReleaseNumber))
            .ToListAsync();
        var storedWindows = await db.ReleaseWindows.AsNoTracking().ToListAsync();
        var runWindows = runs
            .GroupBy(run => new { run.Product, run.ReleaseNumber, run.ScheduledDate.Year, run.ScheduledDate.Month })
            .Select(group => new TestingWindow(
                group.Key.Product, group.Key.ReleaseNumber,
                new DateTime(group.Key.Year, group.Key.Month, 1),
                group.First().ReleaseStatus,
                group.First().ReleaseLiveDate,
                group.Count())).ToList();
        var windows = storedWindows.Select(window =>
        {
            var matching = runWindows.FirstOrDefault(run => run.Product == window.Product && run.ReleaseNumber == window.ReleaseNumber && run.Month == window.Month);
            return new TestingWindow(window.Product, window.ReleaseNumber, window.Month, window.Status, window.LiveDate, matching?.PointCount ?? 0);
        }).Concat(runWindows.Where(run => !storedWindows.Any(window => window.Product == run.Product && window.ReleaseNumber == run.ReleaseNumber && window.Month == run.Month)));
        return windows
            .Where(window => previous
                ? window.LiveDate.HasValue && window.LiveDate.Value.Date < DateTime.Today
                : !window.LiveDate.HasValue || window.LiveDate.Value.Date >= DateTime.Today)
            .OrderByDescending(window => window.Month)
            .ThenBy(window => window.Product)
            .ToList();
    }

    public async Task CreateWindowAsync(DateTime month, string product, string releaseNumber)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        var exists = await db.ReleaseWindows.AnyAsync(window => window.Product == product && window.ReleaseNumber == releaseNumber && window.Month == start);
        if (exists) return;
        db.ReleaseWindows.Add(new ReleaseWindow { Product = product.Trim(), ReleaseNumber = releaseNumber.Trim(), Month = start, CreatedAt = DateTime.UtcNow });
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Testing window created", null, product, "Release " + releaseNumber + " for " + start.ToString("MMMM yyyy") + ".");
    }

    public async Task<List<string>> GetTestersAsync()
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        return await db.Users.AsNoTracking()
            .Select(user => user.DisplayName).Where(name => !string.IsNullOrWhiteSpace(name))
            .Select(name => name!).Distinct().OrderBy(name => name).ToListAsync();
    }

    public async Task CreatePlanAsync(TestPlan plan)
    {
        plan.Product = plan.Product.Trim();
        plan.Scope = plan.Scope.Trim();
        plan.TestCaseName = plan.TestCaseName.Trim();
        plan.Frequency = plan.Product == "Brokerage Portal" ? "Random" : "Monthly";
        plan.CreatedAt = DateTime.UtcNow;
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.TestPlans.Add(plan);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Test plan created", plan.Id.ToString(), plan.Product,
            plan.Scope + ": " + plan.TestCaseName + " (" + plan.Frequency + ").");
    }

    public async Task<bool> UpdatePlanAsync(TestPlan updated)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var plan = await db.TestPlans.FindAsync(updated.Id);
        if (plan is null) return false;
        plan.Product = updated.Product.Trim();
        plan.Scope = updated.Scope.Trim();
        plan.Reference = updated.Reference.Trim();
        plan.TestCaseName = updated.TestCaseName.Trim();
        plan.Instructions = updated.Instructions.Trim();
        plan.ExpectedResult = updated.ExpectedResult.Trim();
        plan.Frequency = "Monthly";
        plan.RequiresSecondTester = true;
        plan.IsActive = true;
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Regression check updated", plan.Id.ToString(), plan.Product,
            plan.Scope + ": " + plan.TestCaseName + ".");
        return true;
    }

    public async Task<int> EnsureEcertRegressionTemplateAsync()
    {
        var groups = new[]
        {
            ("Test Environment", "UK Chamber", new[]
            {
                "New Applications – Create application with single certificate",
                "New Applications – Create single application with multiple certificates",
                "New Applications – Create single application with certificate and eCERT invoice",
                "New Applications – Access and function",
                "Test Application – UK Certificate of Origin",
                "Test Application – EUR 1",
                "Test Application – ATA Carnet printed and eCarnet",
                "Test Application – International Import Certificate",
                "Test Application – Arab Certificate of Origin",
                "Test Application – Invoice",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "Certificate Verification Tool – Access, check certificate and view PDF",
                "General CRUD – Copy, replicate, update and delete",
                "Live Chat – Access, receive, answer and send messages",
                "All Points Tested – Test all points",
                "Browsers and Devices – Edge, Chrome, Firefox, iPad Air, iPad Pro and Surface Pro",
                "SSO Log-On – Ensure SSO works for all applications"
            }),
            ("Test Environment", "MT Chamber", new[]
            {
                "Test Application – EU Certificate of Origin",
                "Test Application – Uploaded document stamped",
                "Test Application – EU Certificate of Origin with uploaded document not stamped",
                "Test Application – EU Certificate of Origin with uploaded document stamped",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "General CRUD – Copy, replicate, update and delete",
                "Live Chat – Access, receive, answer and send messages",
                "All Points Tested – Test all points",
                "Browsers and Devices – Edge, Chrome, Firefox, iPad Air, iPad Pro and Surface Pro",
                "SSO Log-On – Ensure SSO works for all applications"
            }),
            ("Test Environment", "CA Chamber", new[]
            {
                "Test Application – Certificate of Origin",
                "Test Application – Uploaded document stamped",
                "Test Application – Certificate of Origin with uploaded document not stamped",
                "Test Application – Certificate of Origin with uploaded document stamped",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "General CRUD – Copy, replicate, update and delete",
                "Live Chat – Access, receive, answer and send messages",
                "All Points Tested – Test all points",
                "Browsers and Devices – Edge, Chrome, Firefox, iPad Air, iPad Pro and Surface Pro",
                "SSO Log-On – Ensure SSO works for all applications"
            }),
            ("Test Environment", "IE Chamber", new[]
            {
                "Test Application – ATA Carnet print and digital",
                "Test Application – Uploaded document stamped",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "General CRUD – Copy, replicate, update and delete",
                "Live Chat – Access, receive, answer and send messages",
                "All Points Tested – Test all points",
                "Browsers and Devices – Edge, Chrome, Firefox, iPad Air, iPad Pro and Surface Pro",
                "SSO Log-On – Ensure SSO works for all applications"
            }),
            ("Live Environment", "UK Chamber", new[]
            {
                "All Points Tested – Test all points as per Test Environment",
                "Test Application – UK Certificate of Origin with stamped document",
                "Test Application – EUR 1 with stamped document",
                "Test Application – ATA Carnet printed and eCarnet",
                "Test Application – International Import Certificate",
                "Test Application – Arab Certificate of Origin with stamped document",
                "Test Application – Uploaded document stamped",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "Stored Data – Access, update and apply",
                "Certificate Verification Tool – Access, check certificate and view PDF",
                "General CRUD – Copy, replicate, update and delete"
            }),
            ("Live Environment", "MT Chamber", new[]
            {
                "Test Application – EU Certificate of Origin",
                "Test Application – Uploaded document stamped",
                "Test Application – EU Certificate of Origin with uploaded document not stamped",
                "Test Application – EU Certificate of Origin with uploaded document stamped",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "General CRUD – Copy, replicate, update and delete"
            }),
            ("Live Environment", "CA Chamber", new[]
            {
                "Test Application – Certificate of Origin",
                "Test Application – Uploaded document stamped",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "General CRUD – Copy, replicate, update and delete"
            }),
            ("Live Environment", "IE Chamber", new[]
            {
                "Test Application – ATA Carnet print and digital",
                "Test Application – Uploaded document stamped",
                "Stored Data – Access, update and apply",
                "Applications – Access, function, accept, reject, convert to STD, stamp and print",
                "General CRUD – Copy, replicate, update and delete"
            }),
            ("Live Environment", "General", new[]
            {
                "Live Chat – Access, receive, answer and send messages",
                "Browsers and Devices – Edge, Chrome, Firefox, iPad Air, iPad Pro and Surface Pro"
            })
        };

        await using var db = await _dbFactory.CreateDbContextAsync();
        var retiredPlans = await db.TestPlans
            .Where(plan => plan.Product == "eCERT" && plan.Reference.StartsWith("ECERT-REG-") && !plan.Reference.StartsWith("ECERT-REG-V3-"))
            .ToListAsync();
        foreach (var plan in retiredPlans) plan.IsActive = false;
        if (retiredPlans.Count > 0) await db.SaveChangesAsync();
        var existingReferences = await db.TestPlans
            .Where(plan => plan.Product == "eCERT" && plan.IsActive && plan.Reference.StartsWith("ECERT-REG-V3-"))
            .Select(plan => plan.Reference)
            .ToHashSetAsync();
        var plans = new List<TestPlan>();
        for (var groupIndex = 0; groupIndex < groups.Length; groupIndex++)
        {
            var group = groups[groupIndex];
            for (var checkIndex = 0; checkIndex < group.Item3.Length; checkIndex++)
            {
                var reference = $"ECERT-REG-V3-{groupIndex + 1:D2}-{checkIndex + 1:D2}";
                if (existingReferences.Contains(reference)) continue;
                var scope = group.Item1 + " · " + group.Item2;
                plans.Add(new TestPlan
                {
                    Product = "eCERT",
                    Scope = scope,
                    Reference = reference,
                    TestCaseName = group.Item3[checkIndex],
                    Instructions = "Complete the listed regression check for " + scope + ".",
                    ExpectedResult = "The listed operation completes successfully.",
                    Frequency = "Monthly",
                    RequiresSecondTester = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                });
            }
        }
        if (plans.Count == 0) return 0;
        db.TestPlans.AddRange(plans);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("eCERT regression template created", null, "eCERT", plans.Count + " regression test case(s) added.");
        return plans.Count;
    }

    public async Task<int> GenerateMonthlyRunsAsync(DateTime month, string product, string releaseNumber)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        var end = start.AddMonths(1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        var plans = await db.TestPlans.Where(plan => plan.IsActive && plan.Frequency == "Monthly" && plan.Product == product).ToListAsync();
        var existingPlanIds = await db.TestRuns
            .Where(run => run.ScheduledDate >= start && run.ScheduledDate < end && run.TestPlanId.HasValue)
            .Where(run => run.Product == product && run.ReleaseNumber == releaseNumber)
            .Select(run => run.TestPlanId!.Value).ToHashSetAsync();
        var daysInMonth = DateTime.DaysInMonth(start.Year, start.Month);
        var now = DateTime.UtcNow;
        var newRuns = plans.Where(plan => !existingPlanIds.Contains(plan.Id)).Select(plan => new TestRun
        {
            TestPlanId = plan.Id, Product = plan.Product, ReleaseNumber = releaseNumber.Trim(), ReleaseStatus = "In Testing", Scope = plan.Scope, TestCaseName = plan.TestCaseName,
            Reference = plan.Reference, Instructions = plan.Instructions, ExpectedResult = plan.ExpectedResult,
            ScheduledDate = start.AddDays((plan.Id * 3) % daysInMonth), RequiresSecondTester = true,
            CreatedAt = now, UpdatedAt = now
        }).ToList();
        if (newRuns.Count > 0)
        {
            db.TestRuns.AddRange(newRuns);
            await db.SaveChangesAsync();
            await _auditLogService.RecordAsync("Monthly test schedule generated", null, string.Empty,
                newRuns.Count + " test case(s) scheduled for " + start.ToString("MMMM yyyy") + ".");
        }
        return newRuns.Count;
    }

    public async Task<int> GenerateRegressionRunAsync(DateTime month, string product, string releaseNumber)
    {
        if (product == "eCERT") await EnsureEcertRegressionTemplateAsync();

        var start = new DateTime(month.Year, month.Month, 1);
        var end = start.AddMonths(1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existing = await db.TestRuns.AnyAsync(run =>
            run.Product == product &&
            run.ReleaseNumber == releaseNumber &&
            run.ScheduledDate >= start && run.ScheduledDate < end &&
            run.Scope == "Regression Testing");
        if (existing) return 0;

        var tests = await db.TestPlans
            .Where(plan => plan.IsActive && plan.Frequency == "Monthly" && plan.Product == product)
            .OrderBy(plan => plan.Scope).ThenBy(plan => plan.Reference).ThenBy(plan => plan.TestCaseName)
            .Select(plan => new RegressionTestItem
            {
                Reference = plan.Reference,
                Scope = plan.Scope,
                TestCaseName = plan.TestCaseName
            })
            .ToListAsync();
        if (tests.Count == 0) return -1;

        var run = new TestRun
        {
            Product = product,
            ReleaseNumber = releaseNumber.Trim(),
            Scope = "Regression Testing",
            TestCaseName = product + " Regression Testing",
            RequiresSecondTester = true,
            ScheduledDate = start,
            RegressionTestsJson = JsonSerializer.Serialize(tests),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        db.TestRuns.Add(run);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Regression test generated", run.Id.ToString(), product,
            tests.Count + " regression check(s) added to release " + releaseNumber + ".");
        return tests.Count;
    }

    public List<RegressionTestItem> GetRegressionTests(TestRun run)
    {
        if (string.IsNullOrWhiteSpace(run.RegressionTestsJson)) return [];
        try
        {
            return (JsonSerializer.Deserialize<List<RegressionTestItem>>(run.RegressionTestsJson) ?? [])
                .Where(test => !test.Scope.Contains("SI Chamber", StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
        catch (JsonException)
        {
            return [];
        }
    }

    public async Task<bool> UpdateRegressionTestsAsync(int runId, List<RegressionTestItem> tests)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var run = await db.TestRuns.FindAsync(runId);
        if (run is null || string.IsNullOrWhiteSpace(run.RegressionTestsJson)) return false;
        run.RegressionTestsJson = JsonSerializer.Serialize(tests);
        run.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Regression test updated", run.Id.ToString(), run.Product,
            "Regression test results updated for release " + run.ReleaseNumber + ".");
        return true;
    }

    public async Task CreateReleasePointAsync(TestRun run)
    {
        run.Product = run.Product.Trim();
        run.ReleaseNumber = run.ReleaseNumber.Trim();
        run.Scope = run.Scope.Trim();
        run.TestCaseName = run.TestCaseName.Trim();
        run.ScheduledDate = run.ScheduledDate.Date;
        run.RequiresSecondTester = true;
        run.CreatedAt = DateTime.UtcNow;
        run.UpdatedAt = DateTime.UtcNow;
        await using var db = await _dbFactory.CreateDbContextAsync();
        db.TestRuns.Add(run);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Release test point added", run.Id.ToString(), run.Product,
            run.ReleaseNumber + ": " + run.TestCaseName + ".");
    }

    public async Task<int> CreateAxosoftPointsAsync(DateTime month, string product, string releaseNumber, IEnumerable<AxosoftItem> items)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existingIds = await db.TestRuns.Where(run => run.Product == product && run.ReleaseNumber == releaseNumber && run.ScheduledDate >= start && run.ScheduledDate < start.AddMonths(1)).Select(run => run.Scope).ToHashSetAsync();
        var runs = items.Where(item => !existingIds.Contains(item.Id)).Select(item => new TestRun
        {
            Product = product,
            ReleaseNumber = releaseNumber,
            Scope = item.Id,
            TestCaseName = item.Title,
            AxosoftNotes = FormatImportedText(item.Notes),
            ReleaseNotes = FormatImportedText(item.ReleaseNotes),
            Notes = FormatImportedText(item.TestNotes),
            RequiresSecondTester = true,
            ScheduledDate = start,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        }).ToList();
        if (runs.Count == 0) return 0;
        db.TestRuns.AddRange(runs); await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Axosoft test points imported", null, product, runs.Count + " test point(s) imported for release " + releaseNumber + ".");
        return runs.Count;
    }

    public async Task<bool> CreateRandomBrokerageRunAsync(DateTime month)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        var plans = await db.TestPlans.Where(plan => plan.IsActive && plan.Product == "Brokerage Portal").ToListAsync();
        if (plans.Count == 0) return false;
        var plan = plans[Random.Shared.Next(plans.Count)];
        var now = DateTime.UtcNow;
        var run = new TestRun
        {
            TestPlanId = plan.Id, Product = plan.Product, Scope = plan.Scope, TestCaseName = plan.TestCaseName,
            ScheduledDate = start.AddDays(Random.Shared.Next(DateTime.DaysInMonth(start.Year, start.Month))),
            RequiresSecondTester = true, CreatedAt = now, UpdatedAt = now
        };
        db.TestRuns.Add(run);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Random Brokerage Portal test scheduled", run.Id.ToString(), plan.Product,
            plan.Scope + ": " + plan.TestCaseName + " for " + run.ScheduledDate.ToString("dd/MM/yyyy") + ".");
        return true;
    }

    public async Task<int> ImportCdsTestKitAsync()
    {
        var cases = new[] {
            ("CDS-CTRL-A", "Non-blocking documentary control", "Copy an IMA shipment, enter ttimportdocnonblocking in D.E.6/8, then send.", "DMSACC, DMSTAX, DMSDOC and DMSCLE notifications are received."),
            ("CDS-CTRL-B", "Blocking documentary control", "Copy an IMA shipment, enter ttimportdocblocking in D.E.6/8, then send.", "DMSACC, DMSTAX, DMSDOC and DMSCLE notifications are received."),
            ("CDS-CTRL-C", "Physical control", "Copy an IMA shipment, enter ttimportphysical in D.E.6/8, then send.", "DMSACC, DMSTAX, DMSCTL and DMSCLE notifications are received."),
            ("CDS-DUTY-001", "Duty calculation", "Copy a duty-calculation shipment and send it. Compare GTA duty with the HMRC DMSTAX duty.", "The GTA duty calculation matches the HMRC duty calculation."),
            ("CDS-AMEND-001", "Amendment popup", "Copy and send a declaration. Change the required data, send an amendment, and review the amendment popup.", "The popup shows the edited fields and the amendment is processed."),
            ("CDS-LICENCE-001", "Licence write-off", "Copy a shipment with a licence and send it twice.", "The expected licence write-off result is shown."),
            ("CDS-CSP-001", "CSP communication", "Copy and send a known-good declaration through each required CSP route.", "The expected acceptance or received notification is shown."),
            ("CDS-READY-001", "Ready and not-ready", "Save the nominated CDS import test shipments for each state.", "Each shipment displays the expected Ready or Not-ready status."),
            ("CDS-DMS-001", "DMS rejection description", "Copy and send a DMS rejection sample.", "The textual statement description is displayed."),
            ("CDS-POSTMAN-001", "DMS notification payload", "Send a sample DMS notification through the approved Postman collection.", "The notification is processed against the declaration."),
            ("CDS-GVMS-001", "GVMS notification", "Use the approved test notification payload in the GVMS Postman collection.", "The notification is processed against the selected GMR."),
            ("CDS-ENS-001", "ENS response headers", "Submit an ENS in sandbox through the submit option with headers.", "The expected accept, reject, or intervention response is returned.")
        };
        await using var db = await _dbFactory.CreateDbContextAsync();
        var existing = await db.TestPlans.Where(plan => plan.Product == "eGTA" && plan.Scope == "CDS").Select(plan => plan.Reference).ToHashSetAsync();
        var plans = cases.Where(item => !existing.Contains(item.Item1)).Select(item => new TestPlan { Product = "eGTA", Scope = "CDS", Reference = item.Item1, TestCaseName = item.Item2, Instructions = item.Item3, ExpectedResult = item.Item4, Frequency = "Monthly", IsActive = true, CreatedAt = DateTime.UtcNow }).ToList();
        if (plans.Count == 0) return 0;
        db.TestPlans.AddRange(plans); await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("CDS test kit imported", null, "eGTA", plans.Count + " safe test case(s) added from SGS CDS Test Kit v1.7.");
        return plans.Count;
    }

    public async Task<int> FinishTestingAsync(
        DateTime month,
        string product,
        string releaseNumber,
        DateTime liveDate)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        var end = start.AddMonths(1);
        await using var db = await _dbFactory.CreateDbContextAsync();
        var runs = await db.TestRuns
            .Where(run => run.Product == product &&
                          run.ReleaseNumber == releaseNumber &&
                          run.ScheduledDate >= start &&
                          run.ScheduledDate < end)
            .ToListAsync();
        var window = await db.ReleaseWindows.SingleOrDefaultAsync(item => item.Product == product && item.ReleaseNumber == releaseNumber && item.Month == start);
        if (window is null)
        {
            window = new ReleaseWindow { Product = product, ReleaseNumber = releaseNumber, Month = start, CreatedAt = DateTime.UtcNow };
            db.ReleaseWindows.Add(window);
        }
        window.Status = "Ready for live";
        window.LiveDate = liveDate.Date;
        foreach (var run in runs)
        {
            run.ReleaseStatus = "Ready for live";
            run.ReleaseLiveDate = liveDate.Date;
            run.UpdatedAt = DateTime.UtcNow;
        }
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync(
            "Testing finished", null, product,
            "Release " + releaseNumber + " is ready for live on " +
            liveDate.ToString("dd/MM/yyyy") + ".");
        return runs.Count;
    }

    public async Task<bool> UpdateRunAsync(TestRun updated)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var run = await db.TestRuns.FindAsync(updated.Id);
        if (run is null) return false;
        run.Status = updated.Status;
        run.TestedBy = updated.TestedBy.Trim();
        run.SecondTestedBy = updated.SecondTestedBy.Trim();
        run.Notes = updated.Notes.Trim();
        run.AxosoftNotes = updated.AxosoftNotes.Trim();
        run.ReleaseNotes = updated.ReleaseNotes.Trim();
        run.PointTest = updated.PointTest.Trim();
        run.UatTest = updated.UatTest.Trim();
        run.CompletedAt = updated.Status is "Pass" or "Fail" ? DateTime.UtcNow : null;
        run.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Test run updated", run.Id.ToString(), run.Product,
            run.Scope + ": " + run.TestCaseName + "; status " + run.Status + "; tested by " + run.TestedBy + ".");
        return true;
    }

    private static string FormatImportedText(string value)
    {
        if (string.IsNullOrWhiteSpace(value)) return string.Empty;

        return System.Net.WebUtility.HtmlEncode(value.Trim())
            .Replace("\r\n", "\n")
            .Replace("\r", "\n")
            .Replace("\n", "<br />");
    }

    public async Task<bool> DeleteRunAsync(int runId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var run = await db.TestRuns.FindAsync(runId);
        if (run is null) return false;
        var month = new DateTime(run.ScheduledDate.Year, run.ScheduledDate.Month, 1);
        var windowExists = await db.ReleaseWindows.AnyAsync(window =>
            window.Product == run.Product &&
            window.ReleaseNumber == run.ReleaseNumber &&
            window.Month == month);
        if (!windowExists && !string.IsNullOrWhiteSpace(run.ReleaseNumber))
        {
            db.ReleaseWindows.Add(new ReleaseWindow
            {
                Product = run.Product,
                ReleaseNumber = run.ReleaseNumber,
                Month = month,
                Status = run.ReleaseStatus,
                LiveDate = run.ReleaseLiveDate,
                CreatedAt = DateTime.UtcNow
            });
        }
        db.TestRuns.Remove(run);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Release test point deleted", runId.ToString(), run.Product,
            run.ReleaseNumber + ": " + run.TestCaseName + ".");
        return true;
    }

    public async Task<bool> DeleteReleaseWindowAsync(string product, string releaseNumber, DateTime month)
    {
        var start = new DateTime(month.Year, month.Month, 1);
        var end = start.AddMonths(1);
        await using var db = await _dbFactory.CreateDbContextAsync();

        var runs = await db.TestRuns
            .Where(run => run.Product == product &&
                          run.ReleaseNumber == releaseNumber &&
                          run.ScheduledDate >= start &&
                          run.ScheduledDate < end)
            .ToListAsync();
        var windows = await db.ReleaseWindows
            .Where(window => window.Product == product &&
                             window.ReleaseNumber == releaseNumber &&
                             window.Month == start)
            .ToListAsync();

        if (runs.Count == 0 && windows.Count == 0) return false;

        db.TestRuns.RemoveRange(runs);
        db.ReleaseWindows.RemoveRange(windows);
        await db.SaveChangesAsync();
        await _auditLogService.RecordAsync("Testing release deleted", null, product,
            "Release " + releaseNumber + " for " + start.ToString("MMMM yyyy") + " was deleted with " + runs.Count + " test point(s).");
        return true;
    }

    public async Task<bool> DeletePlanAsync(int planId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();
        var plan = await db.TestPlans.FindAsync(planId);
        if (plan is null) return false;

        var pendingRuns = await db.TestRuns
            .Where(run => run.TestPlanId == planId &&
                          (run.Status == "Not Started" || run.Status == "In Progress"))
            .ToListAsync();

        db.TestRuns.RemoveRange(pendingRuns);
        db.TestPlans.Remove(plan);
        await db.SaveChangesAsync();

        await _auditLogService.RecordAsync(
            "Test plan deleted", planId.ToString(), plan.Product,
            plan.Scope + ": " + plan.TestCaseName + ". Removed " +
            pendingRuns.Count + " unfinished scheduled test(s).");
        return true;
    }
}

