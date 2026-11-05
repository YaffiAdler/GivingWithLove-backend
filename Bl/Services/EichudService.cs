using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Eichud;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;
using Dal.Queries;

namespace Bl.Services
{
    public class EichudService : IEichudService
    {
        private const string EntityName = nameof(Eichud);

        private readonly IEichudRepository repository;

        public EichudService(IEichudRepository repository)
        {
            this.repository = repository;
        }

        public async Task<PagedResult<EichudDto>> GetPageAsync(PageQueryDto query, CancellationToken ct)
        {
            PagedRows<Eichud> rows = await repository.GetPageAsync(PersonQueryMapper.ToQuery(query), ct);
            return PagingMapper.ToPagedResult(rows, query, EichudMapper.ToDto);
        }

        public async Task<EichudDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            Eichud? eichud = await repository.GetByIdAsync(id, ct);
            return eichud == null ? null : EichudMapper.ToDto(eichud);
        }

        public async Task<EichudDto> CreateAsync(CreateEichudDto item, CancellationToken ct)
        {
            Eichud created = await repository.CreateAsync(EichudMapper.ToEntity(item), ct);
            return EichudMapper.ToDto(created);
        }

        public async Task UpdateAsync(UpdateEichudDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(EichudMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.EichudCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
