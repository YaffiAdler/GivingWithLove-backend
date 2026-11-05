namespace Dal.Abstractions
{
    // Writes are the same for every table; reads differ per entity, so each repository declares its own.
    public interface IWriteRepository<T>
    {
        Task<T> CreateAsync(T item, CancellationToken ct);

        // Returns false when no row with the item's key exists; the caller decides what that means.
        Task<bool> UpdateAsync(T item, CancellationToken ct);

        // Returns false when no row with this key exists; the caller decides what that means.
        Task<bool> DeleteAsync(int id, CancellationToken ct);
    }
}
