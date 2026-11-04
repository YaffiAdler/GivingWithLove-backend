namespace Dal.Api
{
    public interface ICrud<T>
    {
        Task<List<T>> GetAllAsync(CancellationToken ct);

        Task<T?> GetByIdAsync(int id, CancellationToken ct);

        Task<T> CreateAsync(T item, CancellationToken ct);

        // Returns false when no row with the item's key exists; the caller decides what that means.
        Task<bool> UpdateAsync(T item, CancellationToken ct);

        // Returns false when no row with this key exists; the caller decides what that means.
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
