using Dal.Models;

namespace Dal.Abstractions
{
    public interface ISubProjectRepository : IWriteRepository<SubProject>
    {
        Task<List<SubProject>> GetAllAsync(CancellationToken ct);

        Task<SubProject?> GetByIdAsync(int id, CancellationToken ct);
    }
}
