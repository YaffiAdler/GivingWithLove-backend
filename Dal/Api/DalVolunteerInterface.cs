using Dal.Models;

namespace Dal.Api
{
    public interface DalVolunteerInterface : ICrud<Volunteer>
    {
        // Makes the volunteer's domains exactly the given projects; returns false when the volunteer does not exist.
        Task<bool> UpdateVolunteerDomainsAsync(int volunteerCode, IReadOnlyCollection<int> projectCodes, CancellationToken ct);
    }
}
