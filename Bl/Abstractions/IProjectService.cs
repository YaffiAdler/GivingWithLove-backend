using Bl.Dtos.Project;

namespace Bl.Abstractions
{
    public interface IProjectService : ICrudService<ProjectDto, CreateProjectDto, UpdateProjectDto>
    {
        Task<List<ProjectDto>> GetAllAsync(CancellationToken ct);

        // Departments, their projects and the sub-projects of each project.
        Task<List<DepartmentNodeDto>> GetTreeAsync(CancellationToken ct);
    }
}
