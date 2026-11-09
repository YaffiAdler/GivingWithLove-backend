using Dal.Abstractions;
using Dal.Models;
using Dal.Paging;
using Dal.Queries;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class VolunteeringRepository : IVolunteeringRepository
    {
        private readonly DbManager db;

        public VolunteeringRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<PagedRows<VolunteeringRow>> GetPageAsync(VolunteeringListQuery query, CancellationToken ct)
        {
            return await Filter(db.Volunteerings.AsNoTracking(), query.Filter).ToPagedRowsAsync(
                source => OrderBy(source, query.Sort, query.Direction),
                ToRows,
                query.Page,
                ct);
        }

        public async Task<VolunteeringRow?> GetRowByIdAsync(int id, CancellationToken ct)
        {
            IQueryable<Volunteering> volunteerings = db.Volunteerings
                .AsNoTracking()
                .Where(volunteering => volunteering.VolunteeringCode == id);

            return await ToRows(volunteerings).FirstOrDefaultAsync(ct);
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

        private static IQueryable<Volunteering> Filter(IQueryable<Volunteering> volunteerings, VolunteeringFilter filter)
        {
            if (filter.VolunteerCode is int volunteerCode)
                volunteerings = volunteerings.Where(volunteering => volunteering.VolunteerCode == volunteerCode);
            if (filter.ProjectCode is int projectCode)
                volunteerings = volunteerings.Where(volunteering => volunteering.ProjectCode == projectCode);
            if (filter.From is DateOnly from)
                volunteerings = volunteerings.Where(volunteering => volunteering.DateOfVolunteering >= from);
            if (filter.To is DateOnly to)
                volunteerings = volunteerings.Where(volunteering => volunteering.DateOfVolunteering <= to);
            return volunteerings;
        }

        // Ordered on the entity, before the projection: EF cannot translate ordering by a member of the projected record.
        // Every order ends with the key so that rows with equal values keep a stable position across pages.
        // The direction applies to the sorted column only; the newest-first tie-breakers keep their direction.
        private static IOrderedQueryable<Volunteering> OrderBy(IQueryable<Volunteering> volunteerings, VolunteeringSort sort, SortDirection direction)
        {
            return sort switch
            {
                VolunteeringSort.Date => volunteerings
                    .OrderByDirection(volunteering => volunteering.DateOfVolunteering, direction)
                    .ThenByDescending(volunteering => volunteering.VolunteeringCode),
                VolunteeringSort.Volunteer => volunteerings
                    .OrderByDirection(volunteering => volunteering.VolunteerCodeNavigation == null
                        ? null
                        : volunteering.VolunteerCodeNavigation.VolunteerCodeNavigation.FamilyName, direction)
                    .ThenByDirection(volunteering => volunteering.VolunteerCodeNavigation == null
                        ? null
                        : volunteering.VolunteerCodeNavigation.VolunteerCodeNavigation.FirstName, direction)
                    .ThenByDescending(volunteering => volunteering.DateOfVolunteering)
                    .ThenByDescending(volunteering => volunteering.VolunteeringCode),
                VolunteeringSort.Project => volunteerings
                    .OrderByDirection(volunteering => volunteering.ProjectCodeNavigation == null
                        ? null
                        : volunteering.ProjectCodeNavigation.ProjectName, direction)
                    .ThenByDescending(volunteering => volunteering.DateOfVolunteering)
                    .ThenByDescending(volunteering => volunteering.VolunteeringCode),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };
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
    }
}
