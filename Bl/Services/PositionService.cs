using Bl.Abstractions;
using Bl.Dtos.Position;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;

namespace Bl.Services
{
    public class PositionService : IPositionService
    {
        private const string EntityName = nameof(Position);

        private readonly IPositionRepository repository;

        public PositionService(IPositionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<PositionDto>> GetAllAsync(CancellationToken ct)
        {
            List<Position> positions = await repository.GetAllAsync(ct);
            return positions.Select(PositionMapper.ToDto).ToList();
        }

        public async Task<PositionDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            Position? position = await repository.GetByIdAsync(id, ct);
            return position == null ? null : PositionMapper.ToDto(position);
        }

        public async Task<PositionDto> CreateAsync(CreatePositionDto item, CancellationToken ct)
        {
            Position created = await repository.CreateAsync(PositionMapper.ToEntity(item), ct);
            return PositionMapper.ToDto(created);
        }

        public async Task UpdateAsync(UpdatePositionDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(PositionMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.PositionCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
