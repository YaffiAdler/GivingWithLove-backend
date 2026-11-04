using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalEichudService : DalEichudInterface
    {
        private readonly DbManager db;

        public DalEichudService(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<Eichud>> GetAllAsync(CancellationToken ct)
        {
            return await db.Eichuds.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Eichud?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.Eichuds.AsNoTracking().FirstOrDefaultAsync(eichud => eichud.EichudCode == id, ct);
        }

        public async Task<Eichud> CreateAsync(Eichud item, CancellationToken ct)
        {
            db.Eichuds.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(Eichud item, CancellationToken ct)
        {
            Eichud? existing = await db.Eichuds.FirstOrDefaultAsync(eichud => eichud.EichudCode == item.EichudCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            Eichud? existing = await db.Eichuds.FirstOrDefaultAsync(eichud => eichud.EichudCode == id, ct);
            if (existing == null)
                return false;

            db.Eichuds.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
