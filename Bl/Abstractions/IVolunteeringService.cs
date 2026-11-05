using Bl.Dtos.Common;
using Bl.Dtos.Volunteering;

namespace Bl.Abstractions
{
    public interface IVolunteeringService : ICrudService<VolunteeringDto, CreateVolunteeringDto, UpdateVolunteeringDto>
    {
        // Throws InvalidQueryException for an unknown sort value.
        Task<PagedResult<VolunteeringDto>> GetPageAsync(VolunteeringPageQueryDto query, VolunteeringFilterDto filter, CancellationToken ct);
    }
}
