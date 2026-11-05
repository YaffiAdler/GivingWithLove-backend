using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Volunteering;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;
using Dal.Queries;

namespace Bl.Services
{
    public class VolunteeringService : IVolunteeringService
    {
        private const string EntityName = nameof(Volunteering);

        private readonly IVolunteeringRepository repository;

        public VolunteeringService(IVolunteeringRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PagedResult<VolunteeringDto>> GetPageAsync(VolunteeringPageQueryDto query, VolunteeringFilterDto filter, CancellationToken ct)
        {
            PagedRows<VolunteeringRow> rows = await repository.GetPageAsync(VolunteeringMapper.ToQuery(query, filter), ct);
            return PagingMapper.ToPagedResult(rows, query, VolunteeringMapper.ToDto);
        }

        public async Task<VolunteeringDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            VolunteeringRow? row = await repository.GetRowByIdAsync(id, ct);
            return row == null ? null : VolunteeringMapper.ToDto(row);
        }

        public async Task<VolunteeringDto> CreateAsync(CreateVolunteeringDto item, CancellationToken ct)
        {
            Volunteering created = await repository.CreateAsync(VolunteeringMapper.ToEntity(item), ct);
            // Re-read so the response carries the display names, which the request does not have.
            return await GetByIdAsync(created.VolunteeringCode, ct)
                ?? throw new NotFoundException(EntityName, created.VolunteeringCode);
        }

        public async Task UpdateAsync(UpdateVolunteeringDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(VolunteeringMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.VolunteeringCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
