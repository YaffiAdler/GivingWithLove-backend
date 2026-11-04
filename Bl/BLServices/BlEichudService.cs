using Bl.BLApi;
using Bl.BLModels;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Api;
using Dal.Models;

namespace Bl.BLServices
{
    public class BlEichudService : BlEichudInterface
    {
        private const string EntityName = nameof(Eichud);

        private readonly DalEichudInterface dal;

        public BlEichudService(DalEichudInterface dal)
        {
            this.dal = dal;
        }

        public async Task<List<BlEichudModel>> GetAllAsync(CancellationToken ct)
        {
            List<Eichud> eichuds = await dal.GetAllAsync(ct);
            return eichuds.Select(EichudMapper.ToModel).ToList();
        }

        public async Task<BlEichudModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            Eichud? eichud = await dal.GetByIdAsync(id, ct);
            return eichud == null ? null : EichudMapper.ToModel(eichud);
        }

        public async Task<BlEichudModel> CreateAsync(BlEichudModel item, CancellationToken ct)
        {
            Eichud created = await dal.CreateAsync(EichudMapper.ToEntity(item), ct);
            return EichudMapper.ToModel(created);
        }

        public async Task UpdateAsync(BlEichudModel item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(EichudMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.EichudCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
