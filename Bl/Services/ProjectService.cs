using Bl.Abstractions;
using Bl.Dtos.Project;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;
using Dal.Queries;

namespace Bl.Services
{
    public class ProjectService : IProjectService
    {
        private const string EntityName = nameof(Project);

        private readonly IProjectRepository repository;

        public ProjectService(IProjectRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<ProjectDto>> GetAllAsync(CancellationToken ct)
        {
            List<Project> projects = await repository.GetAllAsync(ct);
            return projects.Select(ProjectMapper.ToDto).ToList();
        }

        public async Task<List<DepartmentNodeDto>> GetTreeAsync(CancellationToken ct)
        {
            List<DepartmentTreeRow> departments = await repository.GetTreeAsync(ct);
            return departments.Select(ProjectMapper.ToNode).ToList();
        }

        public async Task<ProjectDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            Project? project = await repository.GetByIdAsync(id, ct);
            return project == null ? null : ProjectMapper.ToDto(project);
        }

        public async Task<ProjectDto> CreateAsync(CreateProjectDto item, CancellationToken ct)
        {
            Project created = await repository.CreateAsync(ProjectMapper.ToEntity(item), ct);
            return ProjectMapper.ToDto(created);
        }

        public async Task UpdateAsync(UpdateProjectDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(ProjectMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.ProjectCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
