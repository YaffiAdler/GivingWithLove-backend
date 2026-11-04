using Dal.Models;

namespace Bl.Mapping
{
    public static class VolunteerDomainMapper
    {
        public static BlVolunteerDomainModel ToModel(VolunteerDomain volunteerDomain)
        {
            return new BlVolunteerDomainModel()
            {
                VolunteerDomainsCode = volunteerDomain.VolunteerDomainsCode,
                VolunteerCode = volunteerDomain.VolunteerCode,
                ProjectCode = volunteerDomain.ProjectCode
            };
        }

        public static List<BlVolunteerDomainModel> ToModels(List<VolunteerDomain> volunteerDomains)
        {
            return volunteerDomains.Select(ToModel).ToList();
        }

        public static VolunteerDomain ToEntity(BlVolunteerDomainModel volunteerDomain)
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
