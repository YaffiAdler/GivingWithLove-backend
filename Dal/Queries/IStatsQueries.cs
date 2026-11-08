namespace Dal.Queries
{
    // Read-only aggregates over volunteerings for the dashboards. Every method runs a fixed number of SQL queries
    // (one, or two for a page), whatever the number of rows; grouping, sums and ordering happen in the database.
    public interface IStatsQueries
    {
        Task<TotalsRow> GetTotalsAsync(StatsCriteria criteria, CancellationToken ct);

        // Ordered by year and month.
        Task<List<MonthlyCountRow>> GetByMonthAsync(StatsCriteria criteria, CancellationToken ct);

        Task<List<DailyCountRow>> GetByDayAsync(StatsCriteria criteria, CancellationToken ct);

        // Grouped by the department of the volunteering's project; a volunteering pointing to a department itself counts there.
        // The named lists are ordered by Count descending, then by Code.
        Task<List<NamedCountRow>> GetByDepartmentAsync(StatsCriteria criteria, CancellationToken ct);

        Task<List<NamedCountRow>> GetByProjectAsync(StatsCriteria criteria, CancellationToken ct);

        Task<List<NamedCountRow>> GetBySubProjectAsync(StatsCriteria criteria, CancellationToken ct);

        // Volunteerings without a volunteer are not ranked.
        Task<PagedRows<VolunteerRankingRow>> GetVolunteerRankingPageAsync(
            StatsCriteria criteria, VolunteerRankingSort sort, PageRequest page, CancellationToken ct);

        Task<List<VolunteerRankingRow>> GetTopVolunteersAsync(
            StatsCriteria criteria, VolunteerRankingSort sort, int count, CancellationToken ct);

        Task<VolunteerIdentityRow?> GetVolunteerAsync(int volunteerCode, CancellationToken ct);

        Task<ProjectIdentityRow?> GetProjectAsync(int projectCode, CancellationToken ct);
    }
}
