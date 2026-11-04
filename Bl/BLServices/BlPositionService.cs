using Bl.BLApi;
using Bl.BLModels;
using Bl.Exceptions;
using Dal.Api;
using Dal.Models;

namespace Bl.BLServices
{
    public class BlPositionService : BlPositionInterface
    {
        private const string EntityName = nameof(Position);

        private readonly DalPositionInterface dal;

        public BlPositionService(DalPositionInterface dal)
        {
            this.dal = dal;
        }

        public async Task<List<BlPositionModel>> GetAllAsync(CancellationToken ct)
        {
            List<Position> positions = await dal.GetAllAsync(ct);
            return positions.Select(Convert).ToList();
        }

        public async Task<BlPositionModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            Position? position = await dal.GetByIdAsync(id, ct);
            return position == null ? null : Convert(position);
        }

        public async Task<BlPositionModel> CreateAsync(BlPositionModel item, CancellationToken ct)
        {
            Position created = await dal.CreateAsync(Convert(item), ct);
            return Convert(created);
        }

        public async Task UpdateAsync(BlPositionModel item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(Convert(item), ct))
                throw new NotFoundException(EntityName, item.positionCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }

        private static BlPositionModel Convert(Position position)
        {
            return new BlPositionModel()
            {
                positionCode = position.positionCode,
                positionName = position.positionName
            };
        }

        private static Position Convert(BlPositionModel position)
        {
            return new Position()
            {
                positionCode = position.positionCode,
                positionName = position.positionName
            };
        }
    }
}
