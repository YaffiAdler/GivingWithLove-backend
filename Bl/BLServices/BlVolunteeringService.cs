using Bl.Dtos.Volunteering;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Api;
using Dal.Models;
using Dal.Queries;

namespace Bl.BLServices
{
    public class BlVolunteeringService : BlVolunteeringInterface
    {
        private const string EntityName = nameof(Volunteering);

        private readonly DalVolunteeringInterface dal;

        public BlVolunteeringService(DalVolunteeringInterface dal)
        {
            this.dal = dal;
        }

        public async Task<List<VolunteeringDto>> GetAllAsync(VolunteeringFilterDto filter, CancellationToken ct)
        {
            List<VolunteeringRow> rows = await dal.GetRowsAsync(VolunteeringMapper.ToFilter(filter), ct);
            return rows.Select(VolunteeringMapper.ToDto).ToList();
        }

        public async Task<VolunteeringDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            VolunteeringRow? row = await dal.GetRowByIdAsync(id, ct);
            return row == null ? null : VolunteeringMapper.ToDto(row);
        }

        public async Task<VolunteeringDto> CreateAsync(CreateVolunteeringDto item, CancellationToken ct)
        {
            Volunteering created = await dal.CreateAsync(VolunteeringMapper.ToEntity(item), ct);
            // Re-read so the response carries the display names, which the request does not have.
            return await GetByIdAsync(created.VolunteeringCode, ct)
                ?? throw new NotFoundException(EntityName, created.VolunteeringCode);
        }

        public async Task UpdateAsync(UpdateVolunteeringDto item, CancellationToken ct)
        {
            if (!await dal.UpdateAsync(VolunteeringMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.VolunteeringCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await dal.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
