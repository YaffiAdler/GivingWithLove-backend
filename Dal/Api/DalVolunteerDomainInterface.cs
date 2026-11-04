using Dal.Models;

namespace Dal.Api
{
    public interface DalVolunteerDomainInterface : ICrud<VolunteerDomain>
    {
        Task<List<VolunteerDomain>> GetByVolunteerAsync(int volunteerCode, CancellationToken ct);
    }
}
