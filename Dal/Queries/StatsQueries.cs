using Dal.Models;
using Dal.Paging;
using Microsoft.EntityFrameworkCore;

namespace Dal.Queries
{
    public class StatsQueries : IStatsQueries
    {
        private readonly DbManager db;

        public StatsQueries(DbManager db)
        {
            this.db = db;
        }

        public async Task<TotalsRow> GetTotalsAsync(StatsCriteria criteria, CancellationToken ct)
        {
            // Grouping by a constant gives one row of aggregates in one query, and no row at all when nothing matches.
            TotalsRow? totals = await Facts(criteria)
                .GroupBy(fact => 1)
                .Select(group => new TotalsRow(
                    group.Count(),
                    group.Where(fact => fact.VolunteerCode != null).Select(fact => fact.VolunteerCode).Distinct().Count(),
                    group.Sum(fact => fact.EstimatedTime),
                    group.Sum(fact => fact.EstimatedCost),
                    group.Min(fact => (DateOnly?)fact.Date),
                    group.Max(fact => (DateOnly?)fact.Date)))
                .FirstOrDefaultAsync(ct);

            return totals ?? new TotalsRow(0, 0, 0, 0, null, null);
        }

        public async Task<List<MonthlyCountRow>> GetByMonthAsync(StatsCriteria criteria, CancellationToken ct)
        {
            return await Facts(criteria)
                .GroupBy(fact => new { fact.Date.Year, fact.Date.Month })
                .OrderBy(group => group.Key.Year)
                .ThenBy(group => group.Key.Month)
                .Select(group => new MonthlyCountRow(
                    group.Key.Year,
                    group.Key.Month,
                    group.Count(),
                    group.Sum(fact => fact.EstimatedTime)))
                .ToListAsync(ct);
        }

        public async Task<List<DailyCountRow>> GetByDayAsync(StatsCriteria criteria, CancellationToken ct)
        {
            return await Facts(criteria)
                .GroupBy(fact => fact.Date)
                .OrderBy(group => group.Key)
                .Select(group => new DailyCountRow(group.Key, group.Count()))
                .ToListAsync(ct);
        }

        public async Task<List<NamedCountRow>> GetByDepartmentAsync(StatsCriteria criteria, CancellationToken ct)
        {
            IQueryable<ProjectDepartmentLink> links = ProjectHierarchy.DepartmentLinks(db.Projects.AsNoTracking());

            return await Facts(criteria)
                .Join(links,
                    fact => fact.ProjectCode,
                    link => (int?)link.ProjectCode,
                    (fact, link) => new NamedFact
                    {
                        Code = link.DepartmentCode,
                        Name = link.DepartmentName,
                        EstimatedTime = fact.EstimatedTime,
                        EstimatedCost = fact.EstimatedCost
                    })
                .GroupBy(fact => new { fact.Code, fact.Name })
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key.Code)
                .Select(group => new NamedCountRow(
                    group.Key.Code,
                    group.Key.Name,
                    group.Count(),
                    group.Sum(fact => fact.EstimatedTime),
                    group.Sum(fact => fact.EstimatedCost)))
                .ToListAsync(ct);
        }

        public async Task<List<NamedCountRow>> GetByProjectAsync(StatsCriteria criteria, CancellationToken ct)
        {
            return await Facts(criteria)
                .Where(fact => fact.ProjectCode != null)
                .GroupBy(fact => new { Code = fact.ProjectCode ?? 0, Name = fact.ProjectName ?? "" })
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key.Code)
                .Select(group => new NamedCountRow(
                    group.Key.Code,
                    group.Key.Name,
                    group.Count(),
                    group.Sum(fact => fact.EstimatedTime),
                    group.Sum(fact => fact.EstimatedCost)))
                .ToListAsync(ct);
        }

        public async Task<List<NamedCountRow>> GetBySubProjectAsync(StatsCriteria criteria, CancellationToken ct)
        {
            return await Facts(criteria)
                .Where(fact => fact.SubProjectCode != null)
                .GroupBy(fact => new { Code = fact.SubProjectCode ?? 0, Name = fact.SubProjectName ?? "" })
                .OrderByDescending(group => group.Count())
                .ThenBy(group => group.Key.Code)
                .Select(group => new NamedCountRow(
                    group.Key.Code,
                    group.Key.Name,
                    group.Count(),
                    group.Sum(fact => fact.EstimatedTime),
                    group.Sum(fact => fact.EstimatedCost)))
                .ToListAsync(ct);
        }

        public async Task<PagedRows<VolunteerRankingRow>> GetVolunteerRankingPageAsync(
            StatsCriteria criteria, VolunteerRankingSort sort, PageRequest page, CancellationToken ct)
        {
            return await VolunteerTotals(criteria).ToPagedRowsAsync(
                totals => OrderBy(totals, sort),
                WithNames,
                page,
                ct);
        }

        public async Task<List<VolunteerRankingRow>> GetTopVolunteersAsync(
            StatsCriteria criteria, VolunteerRankingSort sort, int count, CancellationToken ct)
        {
            IQueryable<VolunteerTotals> top = OrderBy(VolunteerTotals(criteria), sort).Take(count);
            return await WithNames(top).ToListAsync(ct);
        }

        public async Task<VolunteerIdentityRow?> GetVolunteerAsync(int volunteerCode, CancellationToken ct)
        {
            return await db.Volunteers
                .AsNoTracking()
                .Where(volunteer => volunteer.VolunteerCode == volunteerCode)
                .Select(volunteer => new VolunteerIdentityRow(
                    volunteer.VolunteerCode,
                    volunteer.VolunteerCodeNavigation.FirstName,
                    volunteer.VolunteerCodeNavigation.FamilyName))
                .FirstOrDefaultAsync(ct);
        }

        public async Task<ProjectIdentityRow?> GetProjectAsync(int projectCode, CancellationToken ct)
        {
            IQueryable<Project> departments = db.Projects.AsNoTracking().Where(ProjectHierarchy.IsDepartment);

            return await db.Projects
                .AsNoTracking()
                .Where(project => project.ProjectCode == projectCode)
                .Select(project => new ProjectIdentityRow(
                    project.ProjectCode,
                    project.ProjectName,
                    departments.Any(department => department.ProjectCode == project.ProjectCode)))
                .FirstOrDefaultAsync(ct);
        }

        // The matching volunteerings with the time and cost of their sub-project. LEFT JOINs: a volunteering
        // without a sub-project or project keeps its row, with 0 time and cost and a null name.
        private IQueryable<VolunteeringFacts> Facts(StatsCriteria criteria)
        {
            IQueryable<Volunteering> volunteerings = db.Volunteerings.AsNoTracking();

            if (criteria.From is DateOnly from)
                volunteerings = volunteerings.Where(volunteering => volunteering.DateOfVolunteering >= from);
            if (criteria.To is DateOnly to)
                volunteerings = volunteerings.Where(volunteering => volunteering.DateOfVolunteering <= to);
            if (criteria.VolunteerCode is int volunteerCode)
                volunteerings = volunteerings.Where(volunteering => volunteering.VolunteerCode == volunteerCode);
            if (criteria.MatcherCode is int matcherCode)
                volunteerings = volunteerings.Where(volunteering => volunteering.MatcherCode == matcherCode);
            if (criteria.ProjectCode is int projectCode)
            {
                // A single IN (subquery) rather than "= code OR EXISTS (...)", so SQL Server can seek the ProjectCode index.
                // A project's own link matches on ProjectCode; a department's links (itself included) match on DepartmentCode.
                IQueryable<int?> coveredProjectCodes = ProjectHierarchy.DepartmentLinks(db.Projects.AsNoTracking())
                    .Where(link => link.ProjectCode == projectCode || link.DepartmentCode == projectCode)
                    .Select(link => (int?)link.ProjectCode);
                volunteerings = volunteerings.Where(volunteering => coveredProjectCodes.Contains(volunteering.ProjectCode));
            }

            // Explicit joins instead of navigations: with navigations EF 8 defers the joins past GroupBy and sums
            // each group in a correlated subquery that reads the table again; explicit joins keep one GROUP BY pass.
            return from volunteering in volunteerings
                   join subProject in db.SubProjects.AsNoTracking()
                       on volunteering.SubProjectCode equals (int?)subProject.SubProjectCode into subProjects
                   from subProject in subProjects.DefaultIfEmpty()
                   join project in db.Projects.AsNoTracking()
                       on volunteering.ProjectCode equals (int?)project.ProjectCode into projects
                   from project in projects.DefaultIfEmpty()
                   select new VolunteeringFacts
                   {
                       Date = volunteering.DateOfVolunteering,
                       VolunteerCode = volunteering.VolunteerCode,
                       ProjectCode = volunteering.ProjectCode,
                       ProjectName = project == null ? null : project.ProjectName,
                       SubProjectCode = volunteering.SubProjectCode,
                       SubProjectName = subProject == null ? null : subProject.SubProjectName,
                       EstimatedTime = subProject == null ? 0 : subProject.EstimatedTime ?? 0,
                       EstimatedCost = subProject == null ? 0 : subProject.EstimatedCost ?? 0
                   };
        }

        // Grouped before the names are joined, so the join touches only the volunteers that are returned.
        private IQueryable<VolunteerTotals> VolunteerTotals(StatsCriteria criteria)
        {
            return Facts(criteria)
                .Where(fact => fact.VolunteerCode != null)
                .GroupBy(fact => fact.VolunteerCode ?? 0)
                .Select(group => new VolunteerTotals
                {
                    VolunteerCode = group.Key,
                    Count = group.Count(),
                    EstimatedTimeSum = group.Sum(fact => fact.EstimatedTime),
                    EstimatedCostSum = group.Sum(fact => fact.EstimatedCost),
                    LastDate = group.Max(fact => fact.Date)
                });
        }

        private static IOrderedQueryable<VolunteerTotals> OrderBy(IQueryable<VolunteerTotals> totals, VolunteerRankingSort sort)
        {
            return sort switch
            {
                VolunteerRankingSort.Count => totals
                    .OrderByDescending(volunteer => volunteer.Count)
                    .ThenBy(volunteer => volunteer.VolunteerCode),
                VolunteerRankingSort.Hours => totals
                    .OrderByDescending(volunteer => volunteer.EstimatedTimeSum)
                    .ThenBy(volunteer => volunteer.VolunteerCode),
                VolunteerRankingSort.LastDate => totals
                    .OrderByDescending(volunteer => volunteer.LastDate)
                    .ThenBy(volunteer => volunteer.VolunteerCode),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };
        }

        // Volunteer.VolunteerCode equals the EichudCode of the same person, so the names come straight from Eichud.
        private IQueryable<VolunteerRankingRow> WithNames(IQueryable<VolunteerTotals> totals)
        {
            return from volunteer in totals
                   join eichud in db.Eichuds.AsNoTracking() on volunteer.VolunteerCode equals eichud.EichudCode into people
                   from person in people.DefaultIfEmpty()
                   select new VolunteerRankingRow(
                       volunteer.VolunteerCode,
                       person == null ? null : person.FirstName,
                       person == null ? null : person.FamilyName,
                       volunteer.Count,
                       volunteer.EstimatedTimeSum,
                       volunteer.EstimatedCostSum,
                       volunteer.LastDate);
        }
    }
}
