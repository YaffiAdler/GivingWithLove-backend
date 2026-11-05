using Bl.Abstractions;
using Bl.Dtos.SubProject;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;

namespace Bl.Services
{
    public class SubProjectService : ISubProjectService
    {
        private const string EntityName = nameof(SubProject);

        private readonly ISubProjectRepository repository;

        public SubProjectService(ISubProjectRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<SubProjectDto>> GetAllAsync(CancellationToken ct)
        {
            List<SubProject> subProjects = await repository.GetAllAsync(ct);
            return subProjects.Select(SubProjectMapper.ToDto).ToList();
        }

        public async Task<SubProjectDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            SubProject? subProject = await repository.GetByIdAsync(id, ct);
            return subProject == null ? null : SubProjectMapper.ToDto(subProject);
        }

        public async Task<SubProjectDto> CreateAsync(CreateSubProjectDto item, CancellationToken ct)
        {
            SubProject created = await repository.CreateAsync(SubProjectMapper.ToEntity(item), ct);
            return SubProjectMapper.ToDto(created);
        }

        public async Task UpdateAsync(UpdateSubProjectDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(SubProjectMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.SubProjectCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
