using Dal.Models;
using Dal.Queries;

namespace Dal.Abstractions
{
    public interface IVolunteerRepository : IWriteRepository<Volunteer>
    {
        Task<PagedRows<VolunteerRow>> GetPageAsync(PersonListQuery query, CancellationToken ct);

        Task<VolunteerRow?> GetRowByIdAsync(int id, CancellationToken ct);

        // Makes the volunteer's domains exactly the given projects; returns false when the volunteer does not exist.
        Task<bool> UpdateVolunteerDomainsAsync(int volunteerCode, IReadOnlyCollection<int> projectCodes, CancellationToken ct);
    }
}
