using Bl.Dtos.Common;
using Bl.Dtos.Volunteering;
using Bl.Exceptions;
using Dal.Models;
using Dal.Queries;

namespace Bl.Mapping
{
    internal static class VolunteeringMapper
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

        public const string SortByDate = "date";
        public const string SortByVolunteer = "volunteer";
        public const string SortByProject = "project";

        public static VolunteeringListQuery ToQuery(VolunteeringPageQueryDto query, VolunteeringFilterDto filter)
        {
            return new VolunteeringListQuery(
                new VolunteeringFilter(filter.VolunteerCode, filter.ProjectCode, filter.From, filter.To),
                ToSort(query.Sort),
                PagingMapper.ToPageRequest(query));
        }

        private static VolunteeringSort ToSort(string? sort)
        {
            return sort?.Trim().ToLowerInvariant() switch
            {
                null or "" or SortByDate => VolunteeringSort.Date,
                SortByVolunteer => VolunteeringSort.Volunteer,
                SortByProject => VolunteeringSort.Project,
                _ => throw new InvalidQueryException(
                    $"Unknown sort '{sort}'. Allowed values: {SortByDate}, {SortByVolunteer}, {SortByProject}.")
            };
        }
    }
}
