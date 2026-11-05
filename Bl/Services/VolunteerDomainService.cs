using Bl.Abstractions;
using Bl.Dtos.VolunteerDomain;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Abstractions;
using Dal.Models;

namespace Bl.Services
{
    public class VolunteerDomainService : IVolunteerDomainService
    {
        private const string EntityName = nameof(VolunteerDomain);

        private readonly IVolunteerDomainRepository repository;

        public VolunteerDomainService(IVolunteerDomainRepository repository)
        {
            this.repository = repository;
        }

        public async Task<List<VolunteerDomainDto>> GetAllAsync(CancellationToken ct)
        {
            List<VolunteerDomain> volunteerDomains = await repository.GetAllAsync(ct);
            return volunteerDomains.Select(VolunteerDomainMapper.ToDto).ToList();
        }

        public async Task<VolunteerDomainDto?> GetByIdAsync(int id, CancellationToken ct)
        {
            VolunteerDomain? volunteerDomain = await repository.GetByIdAsync(id, ct);
            return volunteerDomain == null ? null : VolunteerDomainMapper.ToDto(volunteerDomain);
        }

        public async Task<VolunteerDomainDto> CreateAsync(CreateVolunteerDomainDto item, CancellationToken ct)
        {
            VolunteerDomain created = await repository.CreateAsync(VolunteerDomainMapper.ToEntity(item), ct);
            return VolunteerDomainMapper.ToDto(created);
        }

        public async Task UpdateAsync(UpdateVolunteerDomainDto item, CancellationToken ct)
        {
            if (!await repository.UpdateAsync(VolunteerDomainMapper.ToEntity(item), ct))
                throw new NotFoundException(EntityName, item.VolunteerDomainsCode);
        }

        public async Task DeleteAsync(int id, CancellationToken ct)
        {
            if (!await repository.DeleteAsync(id, ct))
                throw new NotFoundException(EntityName, id);
        }
    }
}
