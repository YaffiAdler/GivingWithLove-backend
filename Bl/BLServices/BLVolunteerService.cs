using Bl.BLApi;
using Bl.BLModels;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Api;
using Dal.Models;

namespace Bl.BLServices
{
    public class BLVolunteerService : BlVolunteerInterface
    {
        private const string EntityName = nameof(Volunteer);

        private readonly DalVolunteerInterface dal;
        private readonly BlPositionInterface positionService;

        public BLVolunteerService(DalVolunteerInterface dal, BlPositionInterface positionService)
        {
            this.dal = dal;
            this.positionService = positionService;
        }

        public async Task<List<BLVolunteerModel>> GetAllAsync(CancellationToken ct)
        {
            List<Volunteer> volunteers = await dal.GetAllAsync(ct);
            List<BLVolunteerModel> models = new List<BLVolunteerModel>();
            // Sequential on purpose: the scoped DbContext does not support parallel queries.
            foreach (Volunteer volunteer in volunteers)
            {
                models.Add(await ToModelAsync(volunteer, ct));
            }
            return models;
        }

        public async Task<BLVolunteerModel?> GetByIdAsync(int id, CancellationToken ct)
        {
            Volunteer? volunteer = await dal.GetByIdAsync(id, ct);
            return volunteer == null ? null : await ToModelAsync(volunteer, ct);
        }

        public async Task<BLVolunteerModel> CreateAsync(BLVolunteerModel item, CancellationToken ct)
        {
            Volunteer created = await dal.CreateAsync(ToEntity(item), ct);
            // Re-read so the returned model includes the Eichud data, which the request does not carry.
            return await GetByIdAsync(created.VolunteerCode, ct)
                ?? throw new NotFoundException(EntityName, created.VolunteerCode);
        }

        public async Task UpdateAsync(BLVolunteerModel item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.VolunteerCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }

        public async Task UpdateVolunteerDomainsAsync(BLVolunteerModel volunteer, CancellationToken ct)
        {
            await dal.UpdateVolunteerDomainsAsync(ToEntity(volunteer), ct);
        }

        private static Volunteer ToEntity(BLVolunteerModel volunteer)
        {
            return new Volunteer()
            {
                VolunteerCode = volunteer.VolunteerCode,
                PositionCode = volunteer.PositionCode,
                VolunteerDomains = volunteer.VolunteerDomains.Select(VolunteerDomainMapper.ToEntity).ToList()
            };
        }

        private async Task<BLVolunteerModel> ToModelAsync(Volunteer volunteer, CancellationToken ct)
        {
            BLVolunteerModel model = new BLVolunteerModel()
            {
                VolunteerCode = volunteer.VolunteerCode,
                PositionCode = volunteer.PositionCode,
                VolunteerCodeNavigation = EichudMapper.ToModel(volunteer.VolunteerCodeNavigation),
                VolunteerDomains = volunteer.VolunteerDomains.Select(VolunteerDomainMapper.ToModel).ToList()
            };
            if (volunteer.PositionCode is int positionCode)
            {
                BlPositionModel position = await positionService.GetByIdAsync(positionCode, ct)
                    ?? throw new NotFoundException(nameof(Position), positionCode);
                model.PositionName = position.positionName;
            }
            return model;
        }
    }
}
