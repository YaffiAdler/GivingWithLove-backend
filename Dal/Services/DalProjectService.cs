using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;

namespace Dal.Services
{
    public class DalProjectService : DalProjectInterface
    {
        private readonly DbManager db;

        public DalProjectService(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<Project>> GetAllAsync(CancellationToken ct)
        {
            return await db.Projects.AsNoTracking().ToListAsync(ct);
        }

        public async Task<Project?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.Projects.AsNoTracking().FirstOrDefaultAsync(project => project.ProjectCode == id, ct);
        }

        public async Task<Project> CreateAsync(Project item, CancellationToken ct)
        {
            db.Projects.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(Project item, CancellationToken ct)
        {
            Project? existing = await db.Projects.FirstOrDefaultAsync(project => project.ProjectCode == item.ProjectCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            Project? existing = await db.Projects.FirstOrDefaultAsync(project => project.ProjectCode == id, ct);
            if (existing == null)
                return false;

            db.Projects.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }
    }
}
