using Bl.Dtos.SubProject;

namespace Bl.Abstractions
{
    public interface ISubProjectService : ICrudService<SubProjectDto, CreateSubProjectDto, UpdateSubProjectDto>
    {
        Task<List<SubProjectDto>> GetAllAsync(CancellationToken ct);
    }
}
