using Dal.Api;
using Dal.Models;
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
