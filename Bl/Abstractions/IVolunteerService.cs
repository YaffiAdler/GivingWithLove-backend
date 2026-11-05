using Bl.Dtos.Common;
using Bl.Dtos.Volunteer;

namespace Bl.Abstractions
{
    // The list returns the short VolunteerDto; a single volunteer is returned with the contact details.
    public interface IVolunteerService : ICrudService<VolunteerDetailsDto, CreateVolunteerDto, UpdateVolunteerDto>
    {
        // Throws InvalidQueryException for an unknown sort value.
        Task<PagedResult<VolunteerDto>> GetPageAsync(PageQueryDto query, CancellationToken ct);

        // Throws NotFoundException when the volunteer does not exist.
        Task UpdateVolunteerDomainsAsync(UpdateVolunteerDomainsDto domains, CancellationToken ct);
    }
}
