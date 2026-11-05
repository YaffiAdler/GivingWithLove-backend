using Bl.Dtos.Position;

namespace Bl.Abstractions
{
    public interface IPositionService : ICrudService<PositionDto, CreatePositionDto, UpdatePositionDto>
    {
        Task<List<PositionDto>> GetAllAsync(CancellationToken ct);
    }
}
