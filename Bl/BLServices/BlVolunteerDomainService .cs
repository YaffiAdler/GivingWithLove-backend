using Bl.Exceptions;
using Bl.Mapping;
using Dal.Api;
using Dal.Models;

namespace Bl.BLServices
{
    public class BlVolunteerDomainService : BlVolunteerDomainInterface
    {
        private const string EntityName = nameof(VolunteerDomain);

        private readonly DalVolunteerDomainInterface dal;

        public BlVolunteerDomainService(DalVolunteerDomainInterface dal)
        {
            this.dal = dal;
        }

        public async Task<List<BlVolunteerDomainModel>> GetAllAsync(CancellationToken ct)
        {
            List<VolunteerDomain> volunteerDomains = await dal.GetAllAsync(ct);
            return VolunteerDomainMapper.ToModels(volunteerDomains);
        }

        public async Task<BlVolunteerDomainModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            VolunteerDomain? volunteerDomain = await dal.GetByIdAsync(id, ct);
            return volunteerDomain == null ? null : VolunteerDomainMapper.ToModel(volunteerDomain);
        }

        public async Task<BlVolunteerDomainModel> CreateAsync(BlVolunteerDomainModel item, CancellationToken ct)
        {
            VolunteerDomain created = await dal.CreateAsync(VolunteerDomainMapper.ToEntity(item), ct);
            return VolunteerDomainMapper.ToModel(created);
        }

        public async Task UpdateAsync(BlVolunteerDomainModel item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(VolunteerDomainMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.VolunteerDomainsCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
