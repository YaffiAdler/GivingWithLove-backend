namespace Bl.Abstractions
{
    // Single-item operations shared by every entity service; lists differ per entity, so each service declares its own.
    public interface ICrudService<TDto, TCreateDto, TUpdateDto>
    {
        Task<TDto?> GetByIdAsync(int id, CancellationToken ct);

        Task<TDto> CreateAsync(TCreateDto item, CancellationToken ct);

        // Throws NotFoundException when the item does not exist.
        Task UpdateAsync(TUpdateDto item, CancellationToken ct);

        // Throws NotFoundException when the item does not exist.
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
