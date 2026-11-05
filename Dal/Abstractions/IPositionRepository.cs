using Dal.Models;

namespace Dal.Abstractions
{
    public interface IPositionRepository : IWriteRepository<Position>
    {
        Task<List<Position>> GetAllAsync(CancellationToken ct);

        Task<Position?> GetByIdAsync(int id, CancellationToken ct);
    }
}
