using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalVolunteerService : DalVolunteerInterface
    {
        private readonly DbManager db;

        public DalVolunteerService(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<Volunteer>> GetAllAsync(CancellationToken ct)
        {
            return await VolunteersWithDetails().ToListAsync(ct);
        }

        public async Task<Volunteer?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await VolunteersWithDetails().FirstOrDefaultAsync(volunteer => volunteer.VolunteerCode == id, ct);
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

        private IQueryable<Volunteer> VolunteersWithDetails()
        {
            return db.Volunteers
                .AsNoTracking()
                .Include(volunteer => volunteer.VolunteerCodeNavigation)
                .Include(volunteer => volunteer.VolunteerDomains);
        }
    }
}
