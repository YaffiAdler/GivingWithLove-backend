using Dal.Models;
using Dal.Queries;

namespace Dal.Abstractions
{
    public interface IVolunteeringRepository : IWriteRepository<Volunteering>
    {
        Task<PagedRows<VolunteeringRow>> GetPageAsync(VolunteeringListQuery query, CancellationToken ct);

        Task<VolunteeringRow?> GetRowByIdAsync(int id, CancellationToken ct);
    }
}
