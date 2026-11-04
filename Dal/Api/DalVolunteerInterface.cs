using Dal.Models;

namespace Dal.Api
{
    public interface DalVolunteerInterface : ICrud<Volunteer>
    {
        Task UpdateVolunteerDomainsAsync(Volunteer volunteer, CancellationToken ct);
    }
}
