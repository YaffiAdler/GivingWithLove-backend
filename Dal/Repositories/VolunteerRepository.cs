using Dal.Abstractions;
using Dal.Models;
using Dal.Paging;
using Dal.Queries;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class VolunteerRepository : IVolunteerRepository
    {
        private readonly DbManager db;

        public VolunteerRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<PagedRows<VolunteerRow>> GetPageAsync(PersonListQuery query, CancellationToken ct)
        {
            IQueryable<Volunteer> volunteers = db.Volunteers.AsNoTracking();

            if (query.Code is int code)
                volunteers = volunteers.Where(volunteer => volunteer.VolunteerCode == code);
            if (query.NameContains is string name)
                volunteers = volunteers.Where(volunteer =>
                    (volunteer.VolunteerCodeNavigation.FirstName != null && volunteer.VolunteerCodeNavigation.FirstName.Contains(name))
                    || (volunteer.VolunteerCodeNavigation.FamilyName != null && volunteer.VolunteerCodeNavigation.FamilyName.Contains(name)));

            return await volunteers.ToPagedRowsAsync(
                source => OrderBy(source, query.Sort),
                ToRows,
                query.Page,
                ct);
        }

        // One SQL query: Eichud and Position are joined, and the domain project codes come from a joined collection.
        public async Task<VolunteerDetailsRow?> GetDetailsByIdAsync(int id, CancellationToken ct)
        {
            return await db.Volunteers
                .AsNoTracking()
                .Where(volunteer => volunteer.VolunteerCode == id)
                .Select(volunteer => new VolunteerDetailsRow(
                    volunteer.VolunteerCode,
                    volunteer.VolunteerCodeNavigation.FirstName,
                    volunteer.VolunteerCodeNavigation.FamilyName,
                    volunteer.PositionCode,
                    volunteer.PositionCodeNavigation == null ? null : volunteer.PositionCodeNavigation.PositionName,
                    // Distinct: VolunteerDomains has duplicate (volunteer, project) rows and no unique constraint.
                    volunteer.VolunteerDomains
                        .Select(domain => domain.ProjectCode)
                        .Distinct()
                        .OrderBy(projectCode => projectCode)
                        .ToList(),
                    volunteer.VolunteerCodeNavigation.Tohar,
                    volunteer.VolunteerCodeNavigation.Ending,
                    volunteer.VolunteerCodeNavigation.FathersName,
                    volunteer.VolunteerCodeNavigation.Shtibel,
                    volunteer.VolunteerCodeNavigation.Street,
                    volunteer.VolunteerCodeNavigation.House,
                    volunteer.VolunteerCodeNavigation.City,
                    volunteer.VolunteerCodeNavigation.ZipCode,
                    volunteer.VolunteerCodeNavigation.HousePhone,
                    volunteer.VolunteerCodeNavigation.CellPhone,
                    volunteer.VolunteerCodeNavigation.Shver))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Volunteer> CreateAsync(Volunteer item, CancellationToken ct)
        {
            db.Volunteers.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        // Only PositionCode is updatable: the key is the Eichud code and the personal data lives in Eichud.
        public async Task<bool> UpdateAsync(Volunteer item, CancellationToken ct)
        {
            Volunteer? existing = await db.Volunteers.FirstOrDefaultAsync(volunteer => volunteer.VolunteerCode == item.VolunteerCode, ct);
            if (existing == null)
                return false;

            existing.PositionCode = item.PositionCode;
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            Volunteer? existing = await db.Volunteers.FirstOrDefaultAsync(volunteer => volunteer.VolunteerCode == id, ct);
            if (existing == null)
                return false;

            db.Volunteers.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> UpdateVolunteerDomainsAsync(int volunteerCode, IReadOnlyCollection<int> projectCodes, CancellationToken ct)
        {
            if (!await db.Volunteers.AnyAsync(volunteer => volunteer.VolunteerCode == volunteerCode, ct))
                return false;

            HashSet<int> requestedProjectCodes = projectCodes.ToHashSet();

            List<VolunteerDomain> existingDomains = await db.VolunteerDomains
                .Where(domain => domain.VolunteerCode == volunteerCode)
                .ToListAsync(ct);

            HashSet<int> existingProjectCodes = existingDomains
                .Select(domain => domain.ProjectCode)
                .ToHashSet();

            IEnumerable<int> projectCodesToAdd = requestedProjectCodes.Except(existingProjectCodes);
            IEnumerable<VolunteerDomain> domainsToRemove = existingDomains.Where(domain => !requestedProjectCodes.Contains(domain.ProjectCode));

            foreach (int projectCode in projectCodesToAdd)
            {
                db.VolunteerDomains.Add(new VolunteerDomain
                {
                    VolunteerCode = volunteerCode,
                    ProjectCode = projectCode
                });
            }

            db.VolunteerDomains.RemoveRange(domainsToRemove);

            await db.SaveChangesAsync(ct);
            return true;
        }

        // Every order ends with the key so that rows with equal names keep a stable position across pages.
        private static IOrderedQueryable<Volunteer> OrderBy(IQueryable<Volunteer> volunteers, PersonSort sort)
        {
            return sort switch
            {
                PersonSort.Name => volunteers
                    .OrderBy(volunteer => volunteer.VolunteerCodeNavigation.FamilyName)
                    .ThenBy(volunteer => volunteer.VolunteerCodeNavigation.FirstName)
                    .ThenBy(volunteer => volunteer.VolunteerCode),
                PersonSort.Code => volunteers.OrderBy(volunteer => volunteer.VolunteerCode),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };
        }

        private static IQueryable<VolunteerRow> ToRows(IQueryable<Volunteer> volunteers)
        {
            return volunteers.Select(volunteer => new VolunteerRow(
                volunteer.VolunteerCode,
                volunteer.VolunteerCodeNavigation.FirstName,
                volunteer.VolunteerCodeNavigation.FamilyName,
                volunteer.PositionCode,
                volunteer.PositionCodeNavigation == null ? null : volunteer.PositionCodeNavigation.PositionName,
                // Distinct: VolunteerDomains has duplicate (volunteer, project) rows and no unique constraint.
                volunteer.VolunteerDomains
                    .Select(domain => domain.ProjectCode)
                    .Distinct()
                    .OrderBy(projectCode => projectCode)
                    .ToList()));
        }
    }
}
