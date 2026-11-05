using Dal.Models;
using Dal.Queries;

namespace Dal.Abstractions
{
    public interface IProjectRepository : IWriteRepository<Project>
    {
        Task<List<Project>> GetAllAsync(CancellationToken ct);

        Task<Project?> GetByIdAsync(int id, CancellationToken ct);

        // Departments, their projects and the sub-projects of each project.
        Task<List<DepartmentTreeRow>> GetTreeAsync(CancellationToken ct);
    }
}
