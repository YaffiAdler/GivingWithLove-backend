using Dal.Abstractions;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class PositionRepository : IPositionRepository
    {
        private readonly DbManager db;

        public PositionRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<Position>> GetAllAsync(CancellationToken ct)
        {
            return await db.Positions.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Position?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.Positions.AsNoTracking().FirstOrDefaultAsync(position => position.PositionCode == id, ct);
        }

        public async Task<Position> CreateAsync(Position item, CancellationToken ct)
        {
            db.Positions.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(Position item, CancellationToken ct)
        {
            Position? existing = await db.Positions.FirstOrDefaultAsync(position => position.PositionCode == item.PositionCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            Position? existing = await db.Positions.FirstOrDefaultAsync(position => position.PositionCode == id, ct);
            if (existing == null)
                return false;

            db.Positions.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
