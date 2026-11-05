using Dal.Models;
using Dal.Queries;

namespace Dal.Abstractions
{
    public interface IEichudRepository : IWriteRepository<Eichud>
    {
        Task<PagedRows<Eichud>> GetPageAsync(PersonListQuery query, CancellationToken ct);

        Task<Eichud?> GetByIdAsync(int id, CancellationToken ct);
    }
}
