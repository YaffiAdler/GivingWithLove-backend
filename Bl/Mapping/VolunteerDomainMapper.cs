using Bl.Dtos.VolunteerDomain;
using Dal.Models;

namespace Bl.Mapping
{
    internal static class VolunteerDomainMapper
    {
        public static VolunteerDomainDto ToDto(VolunteerDomain volunteerDomain)
        {
            return new VolunteerDomainDto(
                volunteerDomain.VolunteerDomainsCode,
                volunteerDomain.VolunteerCode,
                volunteerDomain.ProjectCode);
        }

        public static VolunteerDomain ToEntity(CreateVolunteerDomainDto volunteerDomain)
        {
            return new VolunteerDomain()
            {
                VolunteerCode = volunteerDomain.VolunteerCode,
                ProjectCode = volunteerDomain.ProjectCode
            };
        }

        public static VolunteerDomain ToEntity(UpdateVolunteerDomainDto volunteerDomain)
        {
            return new VolunteerDomain()
            {
                VolunteerDomainsCode = volunteerDomain.VolunteerDomainsCode,
                VolunteerCode = volunteerDomain.VolunteerCode,
                ProjectCode = volunteerDomain.ProjectCode
            };
        }
    }
}
