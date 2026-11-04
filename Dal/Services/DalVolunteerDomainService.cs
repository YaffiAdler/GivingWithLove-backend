using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalVolunteerDomainService : DalVolunteerDomainInterface
    {
        private readonly DbManager db;

        public DalVolunteerDomainService(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<VolunteerDomain>> GetAllAsync(CancellationToken ct)
        {
            return await db.VolunteerDomains.AsNoTracking().ToListAsync(ct);
        }

        public async Task<VolunteerDomain?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.VolunteerDomains.AsNoTracking().FirstOrDefaultAsync(volunteerDomain => volunteerDomain.VolunteerDomainsCode == id, ct);
        }

        public async Task<VolunteerDomain> CreateAsync(VolunteerDomain item, CancellationToken ct)
        {
            db.VolunteerDomains.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(VolunteerDomain item, CancellationToken ct)
        {
            VolunteerDomain? existing = await db.VolunteerDomains.FirstOrDefaultAsync(volunteerDomain => volunteerDomain.VolunteerDomainsCode == item.VolunteerDomainsCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            VolunteerDomain? existing = await db.VolunteerDomains.FirstOrDefaultAsync(volunteerDomain => volunteerDomain.VolunteerDomainsCode == id, ct);
            if (existing == null)
                return false;

            db.VolunteerDomains.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<List<VolunteerDomain>> GetByVolunteerAsync(int volunteerCode, CancellationToken ct)
        {
            return await db.VolunteerDomains
                .AsNoTracking()
                .Where(volunteerDomain => volunteerDomain.VolunteerCode == volunteerCode)
                .ToListAsync(ct);
        }
    }
}
