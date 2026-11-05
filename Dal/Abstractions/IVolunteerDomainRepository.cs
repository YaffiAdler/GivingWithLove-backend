using Dal.Models;

namespace Dal.Abstractions
{
    public interface IVolunteerDomainRepository : IWriteRepository<VolunteerDomain>
    {
        Task<List<VolunteerDomain>> GetAllAsync(CancellationToken ct);

        Task<VolunteerDomain?> GetByIdAsync(int id, CancellationToken ct);

        Task<List<VolunteerDomain>> GetByVolunteerAsync(int volunteerCode, CancellationToken ct);
    }
}
