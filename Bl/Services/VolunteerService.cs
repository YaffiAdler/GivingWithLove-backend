using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Volunteer;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;
using Dal.Queries;

namespace Bl.Services
{
    public class VolunteerService : IVolunteerService
    {
        private const string EntityName = nameof(Volunteer);

        private readonly IVolunteerRepository repository;

        public VolunteerService(IVolunteerRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PagedResult<VolunteerDto>> GetPageAsync(PageQueryDto query, CancellationToken ct)
        {
            PagedRows<VolunteerRow> rows = await repository.GetPageAsync(PersonQueryMapper.ToQuery(query), ct);
            return PagingMapper.ToPagedResult(rows, query, VolunteerMapper.ToDto);
        }

        public async Task<VolunteerDetailsDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            VolunteerRow? row = await repository.GetRowByIdAsync(id, ct);
            return row == null ? null : VolunteerMapper.ToDetailsDto(row);
        }

        public async Task<VolunteerDetailsDto> CreateAsync(CreateVolunteerDto item, CancellationToken ct)
        {
            Volunteer created = await repository.CreateAsync(VolunteerMapper.ToEntity(item), ct);
            // Re-read so the response carries the Eichud and position data, which the request does not have.
            return await GetByIdAsync(created.VolunteerCode, ct)
                ?? throw new NotFoundException(EntityName, created.VolunteerCode);
        }

        public async Task UpdateAsync(UpdateVolunteerDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(VolunteerMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.VolunteerCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }

        public async Task UpdateVolunteerDomainsAsync(UpdateVolunteerDomainsDto domains, CancellationToken ct)
        {
            if (!await repository.UpdateVolunteerDomainsAsync(domains.VolunteerCode, domains.ProjectCodes, ct))
                throw new NotFoundException(EntityName, domains.VolunteerCode);
        }
    }
}
