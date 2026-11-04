namespace Bl.BLApi
{
    public interface BlICrud<T>
    {
        Task<List<T>> GetAllAsync(CancellationToken ct);

        Task<T?> GetByIdAsync(int id, CancellationToken ct);

        Task<T> CreateAsync(T item, CancellationToken ct);

        // Throws NotFoundException when the item does not exist.
        Task UpdateAsync(T item, CancellationToken ct);

        // Throws NotFoundException when the item does not exist.
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
