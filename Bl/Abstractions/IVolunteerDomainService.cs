using Bl.Dtos.VolunteerDomain;

namespace Bl.Abstractions
{
    public interface IVolunteerDomainService : ICrudService<VolunteerDomainDto, CreateVolunteerDomainDto, UpdateVolunteerDomainDto>
    {
        Task<List<VolunteerDomainDto>> GetAllAsync(CancellationToken ct);
    }
}
