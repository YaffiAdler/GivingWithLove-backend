using Bl.BLModels;
using Bl.Dtos.Volunteer;

namespace Bl.BLApi
{
    public interface BlVolunteerInterface : BlICrud<BLVolunteerModel>
    {
        // Throws NotFoundException when the volunteer does not exist.
        Task UpdateVolunteerDomainsAsync(UpdateVolunteerDomainsDto domains, CancellationToken ct);
    }
}
