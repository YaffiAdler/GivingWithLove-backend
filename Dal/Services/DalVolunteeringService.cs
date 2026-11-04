using Dal.Api;
using Dal.Models;
using Dal.Queries;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalVolunteeringService : DalVolunteeringInterface
    {
        private readonly DbManager db;

        public DalVolunteeringService(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<Volunteering>> GetAllAsync(CancellationToken ct)
        {
            return await db.Volunteerings.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Volunteering?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.Volunteerings.AsNoTracking().FirstOrDefaultAsync(volunteering => volunteering.VolunteeringCode == id, ct);
        }

        public async Task<List<VolunteeringRow>> GetRowsAsync(VolunteeringFilter filter, CancellationToken ct)
        {
            IQueryable<Volunteering> volunteerings = db.Volunteerings.AsNoTracking();

            if (filter.VolunteerCode is int volunteerCode)
                volunteerings = volunteerings.Where(volunteering => volunteering.VolunteerCode == volunteerCode);
            if (filter.ProjectCode is int projectCode)
                volunteerings = volunteerings.Where(volunteering => volunteering.ProjectCode == projectCode);
            if (filter.From is DateOnly from)
                volunteerings = volunteerings.Where(volunteering => volunteering.DateOfVolunteering >= from);
            if (filter.To is DateOnly to)
                volunteerings = volunteerings.Where(volunteering => volunteering.DateOfVolunteering <= to);

            // Ordered before the projection: EF cannot translate ordering by a member of the projected record.
            return await ToRows(volunteerings.OrderBy(volunteering => volunteering.VolunteeringCode))
                .ToListAsync(ct);
        }

        public async Task<VolunteeringRow?> GetRowByIdAsync(int id, CancellationToken ct)
        {
            IQueryable<Volunteering> volunteerings = db.Volunteerings
                .AsNoTracking()
                .Where(volunteering => volunteering.VolunteeringCode == id);

            return await ToRows(volunteerings).FirstOrDefaultAsync(ct);
        }

        // One SQL query with LEFT JOINs: a missing navigation yields a null name instead of an exception.
        // Eichud names are fixed-length char columns, so they are trimmed in SQL.
        // The name expression is repeated because EF cannot translate a shared C# helper inside the projection.
        private static IQueryable<VolunteeringRow> ToRows(IQueryable<Volunteering> volunteerings)
        {
            return volunteerings.Select(volunteering => new VolunteeringRow(
                volunteering.VolunteeringCode,
                volunteering.DateOfVolunteering,
                volunteering.VolunteerCode,
                volunteering.VolunteerCodeNavigation == null
                    ? null
                    : ((volunteering.VolunteerCodeNavigation.VolunteerCodeNavigation.FirstName ?? "").Trim()
                        + " " + (volunteering.VolunteerCodeNavigation.VolunteerCodeNavigation.FamilyName ?? "").Trim()).Trim(),
                volunteering.MatcherCode,
                volunteering.MatcherCodeNavigation == null
                    ? null
                    : ((volunteering.MatcherCodeNavigation.VolunteerCodeNavigation.FirstName ?? "").Trim()
                        + " " + (volunteering.MatcherCodeNavigation.VolunteerCodeNavigation.FamilyName ?? "").Trim()).Trim(),
                volunteering.PoorManCode,
                volunteering.PoorManCodeNavigation == null
                    ? null
                    : ((volunteering.PoorManCodeNavigation.FirstName ?? "").Trim()
                        + " " + (volunteering.PoorManCodeNavigation.FamilyName ?? "").Trim()).Trim(),
                volunteering.ProjectCode,
                volunteering.ProjectCodeNavigation == null ? null : volunteering.ProjectCodeNavigation.ProjectName,
                volunteering.SubProjectCode,
                volunteering.SubProjectCodeNavigation == null ? null : volunteering.SubProjectCodeNavigation.SubProjectName));
        }

        public async Task<Volunteering> CreateAsync(Volunteering item, CancellationToken ct)
        {
            db.Volunteerings.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(Volunteering item, CancellationToken ct)
        {
            Volunteering? existing = await db.Volunteerings.FirstOrDefaultAsync(volunteering => volunteering.VolunteeringCode == item.VolunteeringCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            Volunteering? existing = await db.Volunteerings.FirstOrDefaultAsync(volunteering => volunteering.VolunteeringCode == id, ct);
            if (existing == null)
                return false;

            db.Volunteerings.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
