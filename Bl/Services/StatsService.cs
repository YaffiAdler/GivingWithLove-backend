using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Stats;
using Bl.Exceptions;
using Bl.Mapping;
using Dal.Queries;

namespace Bl.Services
{
    // The queries of one request run one after another: they share the scoped DbManager, which allows one query at a time.
    public class StatsService : IStatsService
    {
        private const string VolunteerEntityName = "Volunteer";
        private const string ProjectEntityName = "Project";
        private const int TopVolunteersCount = 10;

        private readonly IStatsQueries queries;

        public StatsService(IStatsQueries queries)
        {
            this.queries = queries;
        }

        public async Task<OverviewStatsDto> GetOverviewAsync(StatsFilter filter, CancellationToken ct)
        {
            ValidateRange(filter);
            StatsCriteria criteria = StatsMapper.ToCriteria(filter);

            TotalsRow totals = await queries.GetTotalsAsync(criteria, ct);
            List<MonthlyCountRow> byMonth = await queries.GetByMonthAsync(criteria, ct);
            List<NamedCountRow> byDepartment = await queries.GetByDepartmentAsync(criteria, ct);
            List<NamedCountRow> byProject = await queries.GetByProjectAsync(criteria, ct);
            List<NamedCountRow> bySubProject = await queries.GetBySubProjectAsync(criteria, ct);

            return new OverviewStatsDto(
                totals.Count,
                totals.DistinctVolunteers,
                StatsMapper.ToHours(totals.EstimatedTimeSum),
                StatsMapper.ToValue(totals.EstimatedCostSum),
                StatsMapper.ToMonthSeries(byMonth, filter),
                byDepartment.Select(StatsMapper.ToDto).ToList(),
                byProject.Select(StatsMapper.ToDto).ToList(),
                bySubProject.Select(StatsMapper.ToDto).ToList());
        }

        public async Task<PagedResult<VolunteerRankingRowDto>> GetVolunteerRankingAsync(
            StatsFilter filter, VolunteerRankingQueryDto query, CancellationToken ct)
        {
            ValidateRange(filter);
            PagedRows<VolunteerRankingRow> rows = await queries.GetVolunteerRankingPageAsync(
                StatsMapper.ToCriteria(filter),
                StatsMapper.ToSort(query.Sort),
                StatsMapper.ToRankingDirection(query.Direction),
                PagingMapper.ToPageRequest(query),
                ct);
            return PagingMapper.ToPagedResult(rows, query, StatsMapper.ToDto);
        }

        public async Task<VolunteerStatsDto> GetVolunteerStatsAsync(int volunteerCode, StatsFilter filter, CancellationToken ct)
        {
            ValidateRange(filter);
            VolunteerIdentityRow volunteer = await queries.GetVolunteerAsync(volunteerCode, ct)
                ?? throw new NotFoundException(VolunteerEntityName, volunteerCode);

            StatsCriteria asVolunteer = StatsMapper.ToCriteria(filter, volunteerCode: volunteerCode);
            TotalsRow totals = await queries.GetTotalsAsync(asVolunteer, ct);
            TotalsRow asMatcher = await queries.GetTotalsAsync(StatsMapper.ToCriteria(filter, matcherCode: volunteerCode), ct);
            List<MonthlyCountRow> byMonth = await queries.GetByMonthAsync(asVolunteer, ct);
            List<DailyCountRow> byDay = await queries.GetByDayAsync(asVolunteer, ct);
            List<NamedCountRow> byProject = await queries.GetByProjectAsync(asVolunteer, ct);
            List<NamedCountRow> bySubProject = await queries.GetBySubProjectAsync(asVolunteer, ct);

            return new VolunteerStatsDto(
                volunteer.VolunteerCode,
                StatsMapper.TrimName(volunteer.FirstName),
                StatsMapper.TrimName(volunteer.FamilyName),
                totals.Count,
                asMatcher.Count,
                StatsMapper.ToHours(totals.EstimatedTimeSum),
                StatsMapper.ToValue(totals.EstimatedCostSum),
                totals.FirstDate,
                totals.LastDate,
                StatsMapper.ToMonthSeries(byMonth, filter),
                StatsMapper.ToWeekdays(byDay),
                byProject.Select(StatsMapper.ToDto).ToList(),
                bySubProject.Select(StatsMapper.ToDto).ToList());
        }

        public async Task<ProjectStatsDto> GetProjectStatsAsync(int projectCode, StatsFilter filter, CancellationToken ct)
        {
            ValidateRange(filter);
            ProjectIdentityRow project = await queries.GetProjectAsync(projectCode, ct)
                ?? throw new NotFoundException(ProjectEntityName, projectCode);

            StatsCriteria criteria = StatsMapper.ToCriteria(filter, projectCode: projectCode);
            TotalsRow totals = await queries.GetTotalsAsync(criteria, ct);
            List<MonthlyCountRow> byMonth = await queries.GetByMonthAsync(criteria, ct);
            // A department's children are its projects; a project's children are its sub-projects.
            List<NamedCountRow> byChild = project.IsDepartment
                ? await queries.GetByProjectAsync(criteria, ct)
                : await queries.GetBySubProjectAsync(criteria, ct);
            List<VolunteerRankingRow> topVolunteers = await queries.GetTopVolunteersAsync(
                criteria, VolunteerRankingSort.Count, TopVolunteersCount, ct);

            return new ProjectStatsDto(
                project.ProjectCode,
                project.ProjectName.Trim(),
                project.IsDepartment,
                totals.Count,
                totals.DistinctVolunteers,
                StatsMapper.ToHours(totals.EstimatedTimeSum),
                StatsMapper.ToValue(totals.EstimatedCostSum),
                StatsMapper.ToMonthSeries(byMonth, filter),
                byChild.Select(StatsMapper.ToDto).ToList(),
                topVolunteers.Select(StatsMapper.ToDto).ToList());
        }

        // The only range check; InvalidQueryException is mapped to 400 ProblemDetails by the global exception handler.
        private static void ValidateRange(StatsFilter filter)
        {
            if (filter.From is not DateOnly from || filter.To is not DateOnly to)
                return;

            if (from > to)
                throw new InvalidQueryException($"'from' ({from:yyyy-MM-dd}) is later than 'to' ({to:yyyy-MM-dd}).");
            if (StatsMapper.CountMonths(from, to) > StatsMapper.MaxRangeMonths)
                throw new InvalidQueryException(
                    $"The range from {from:yyyy-MM-dd} to {to:yyyy-MM-dd} is longer than {StatsMapper.MaxRangeYears} years. Choose a shorter range.");
        }
    }
}
