using Dal.Abstractions;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class SubProjectRepository : ISubProjectRepository
    {
        private readonly DbManager db;

        public SubProjectRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<SubProject>> GetAllAsync(CancellationToken ct)
        {
            return await db.SubProjects.AsNoTracking().ToListAsync(ct);
        }

        public async Task<SubProject?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.SubProjects.AsNoTracking().FirstOrDefaultAsync(subProject => subProject.SubProjectCode == id, ct);
        }

        public async Task<SubProject> CreateAsync(SubProject item, CancellationToken ct)
        {
            db.SubProjects.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(SubProject item, CancellationToken ct)
        {
            SubProject? existing = await db.SubProjects.FirstOrDefaultAsync(subProject => subProject.SubProjectCode == item.SubProjectCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            SubProject? existing = await db.SubProjects.FirstOrDefaultAsync(subProject => subProject.SubProjectCode == id, ct);
            if (existing == null)
                return false;

            db.SubProjects.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
