using Dal.Abstractions;
using Dal.Models;
using Dal.Queries;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DbManager db;

        public ProjectRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<List<Project>> GetAllAsync(CancellationToken ct)
        {
            return await db.Projects.AsNoTracking().ToListAsync(ct);
        }

        // One SQL query: the departments with their child projects and the sub-projects of each child, joined in SQL.
        // A child is a project whose DomainCode points to the department; a self-parented row is excluded
        // because by ProjectHierarchy.IsDepartment it is a department itself. Projects whose parent is not a department are not reached.
        // Sub-projects attached directly to a department are not part of the tree.
        public async Task<List<DepartmentTreeRow>> GetTreeAsync(CancellationToken ct)
        {
            return await db.Projects
                .AsNoTracking()
                .Where(ProjectHierarchy.IsDepartment)
                .OrderBy(department => department.ProjectName)
                .ThenBy(department => department.ProjectCode)
                .Select(department => new DepartmentTreeRow(
                    department.ProjectCode,
                    department.ProjectName,
                    department.InverseDomainCodeNavigation
                        .Where(child => child.ProjectCode != department.ProjectCode)
                        .OrderBy(child => child.ProjectName)
                        .ThenBy(child => child.ProjectCode)
                        .Select(child => new ProjectTreeRow(
                            child.ProjectCode,
                            child.ProjectName,
                            child.SubProjects
                                .OrderBy(subProject => subProject.SubProjectName)
                                .ThenBy(subProject => subProject.SubProjectCode)
                                .ToList()))
                        .ToList()))
                .ToListAsync(ct);
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
