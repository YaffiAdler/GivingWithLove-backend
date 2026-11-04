using Bl.Dtos.Volunteering;

namespace Dal.Api
{
    // Read and write models differ, so this interface does not extend BlICrud<T>.
    public interface BlVolunteeringInterface
    {
        Task<List<VolunteeringDto>> GetAllAsync(VolunteeringFilterDto filter, CancellationToken ct);

        Task<VolunteeringDto?> GetByIdAsync(int id, CancellationToken ct);

        Task<VolunteeringDto> CreateAsync(CreateVolunteeringDto item, CancellationToken ct);

        // Throws NotFoundException when the item does not exist.
        Task UpdateAsync(UpdateVolunteeringDto item, CancellationToken ct);

        // Throws NotFoundException when the item does not exist.
        Task DeleteAsync(int id, CancellationToken ct);
    }
}
