using Bl.Dtos.Common;
using Bl.Dtos.Stats;

namespace Bl.Abstractions
{
    // Statistics for the dashboards and the per-volunteer and per-project charts. Read-only.
    // Every method throws InvalidQueryException when filter.From is later than filter.To
    // or when the range is longer than StatsMapper.MaxRangeYears.
    public interface IStatsService
    {
        Task<OverviewStatsDto> GetOverviewAsync(StatsFilter filter, CancellationToken ct);

        // Throws InvalidQueryException for an unknown sort value.
        Task<PagedResult<VolunteerRankingRowDto>> GetVolunteerRankingAsync(StatsFilter filter, VolunteerRankingQueryDto query, CancellationToken ct);

        // Throws NotFoundException for an unknown volunteer.
        Task<VolunteerStatsDto> GetVolunteerStatsAsync(int volunteerCode, StatsFilter filter, CancellationToken ct);

        // Throws NotFoundException for an unknown project or department.
        Task<ProjectStatsDto> GetProjectStatsAsync(int projectCode, StatsFilter filter, CancellationToken ct);
    }
}
