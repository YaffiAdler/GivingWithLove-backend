using Bl.Dtos.Volunteering;
using Dal.Models;
using Dal.Queries;

namespace Bl.Mapping
{
    public static class VolunteeringMapper
    {
        public static VolunteeringDto ToDto(VolunteeringRow row)
        {
            return new VolunteeringDto(
                row.VolunteeringCode,
                row.DateOfVolunteering,
                row.VolunteerCode,
                row.VolunteerName,
                row.MatcherCode,
                row.MatcherName,
                row.PoorManCode,
                row.PoorManName,
                row.ProjectCode,
                row.ProjectName,
                row.SubProjectCode,
                row.SubProjectName);
        }

        public static Volunteering ToEntity(CreateVolunteeringDto volunteering)
        {
            return new Volunteering()
            {
                DateOfVolunteering = volunteering.DateOfVolunteering,
                VolunteerCode = volunteering.VolunteerCode,
                MatcherCode = volunteering.MatcherCode,
                PoorManCode = volunteering.PoorManCode,
                ProjectCode = volunteering.ProjectCode,
                SubProjectCode = volunteering.SubProjectCode
            };
        }

        public static Volunteering ToEntity(UpdateVolunteeringDto volunteering)
        {
            return new Volunteering()
            {
                VolunteeringCode = volunteering.VolunteeringCode,
                DateOfVolunteering = volunteering.DateOfVolunteering,
                VolunteerCode = volunteering.VolunteerCode,
                MatcherCode = volunteering.MatcherCode,
                PoorManCode = volunteering.PoorManCode,
                ProjectCode = volunteering.ProjectCode,
                SubProjectCode = volunteering.SubProjectCode
            };
        }

        public static VolunteeringFilter ToFilter(VolunteeringFilterDto filter)
        {
            return new VolunteeringFilter(filter.VolunteerCode, filter.ProjectCode, filter.From, filter.To);
        }
    }
}
