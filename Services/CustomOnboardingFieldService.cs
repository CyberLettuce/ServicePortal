using Microsoft.EntityFrameworkCore;
using ServicePortal.Data;
namespace ServicePortal.Services;
public sealed class CustomOnboardingFieldService
{
 private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
 public CustomOnboardingFieldService(IDbContextFactory<ApplicationDbContext> dbFactory) => _dbFactory = dbFactory;
 public async Task<List<CustomOnboardingField>> GetFieldsAsync() { await using var db = await _dbFactory.CreateDbContextAsync(); return await db.CustomOnboardingFields.OrderBy(x => x.SortOrder).ThenBy(x => x.Label).ToListAsync(); }
 public async Task<List<CustomOnboardingFieldValue>> GetValuesAsync(int onboardingId) { await using var db = await _dbFactory.CreateDbContextAsync(); return await db.CustomOnboardingFieldValues.Where(x => x.CustomerOnboardingId == onboardingId).ToListAsync(); }
 public async Task SaveFieldAsync(CustomOnboardingField field) { await using var db = await _dbFactory.CreateDbContextAsync(); if (field.Id == 0) { field.SortOrder = await db.CustomOnboardingFields.CountAsync() + 1; db.CustomOnboardingFields.Add(field); } else { var stored = await db.CustomOnboardingFields.FindAsync(field.Id); if (stored is null) return; stored.Label = field.Label.Trim(); stored.FieldType = field.FieldType; stored.Options = field.Options.Trim(); } await db.SaveChangesAsync(); }
 public async Task SaveValuesAsync(int onboardingId, IDictionary<int,string> values) { await using var db = await _dbFactory.CreateDbContextAsync(); foreach (var pair in values) { var stored = await db.CustomOnboardingFieldValues.FirstOrDefaultAsync(x => x.CustomerOnboardingId == onboardingId && x.CustomOnboardingFieldId == pair.Key); if (stored is null) db.CustomOnboardingFieldValues.Add(new() { CustomerOnboardingId = onboardingId, CustomOnboardingFieldId = pair.Key, Value = pair.Value ?? string.Empty }); else stored.Value = pair.Value ?? string.Empty; } await db.SaveChangesAsync(); }
 public async Task MoveAsync(int fieldId, int direction) { await using var db = await _dbFactory.CreateDbContextAsync(); var fields = await db.CustomOnboardingFields.OrderBy(x => x.SortOrder).ThenBy(x => x.Id).ToListAsync(); var index = fields.FindIndex(x => x.Id == fieldId); var target = index + direction; if (index < 0 || target < 0 || target >= fields.Count) return; (fields[index].SortOrder, fields[target].SortOrder) = (fields[target].SortOrder, fields[index].SortOrder); await db.SaveChangesAsync(); }
 public async Task DeleteFieldAsync(int fieldId) { await using var db = await _dbFactory.CreateDbContextAsync(); var field = await db.CustomOnboardingFields.FindAsync(fieldId); if (field is null) return; db.CustomOnboardingFields.Remove(field); await db.SaveChangesAsync(); }
}
