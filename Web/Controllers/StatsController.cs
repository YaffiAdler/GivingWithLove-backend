using Bl.Abstractions;
using Bl.Dtos.Common;
using Bl.Dtos.Stats;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    // Statistics for the dashboards. Every action takes an optional date range: from, to (yyyy-MM-dd, inclusive).
    // An invalid range (from later than to, or longer than 10 years) is rejected by the service with 400.
    [Route("api/stats")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IStatsService statsService;

        public StatsController(IStatsService statsService)
        {
            this.statsService = statsService;
        }

        [HttpGet("overview")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OverviewStatsDto>> GetOverview([FromQuery] StatsFilter filter, CancellationToken ct)
        {
            return await statsService.GetOverviewAsync(filter, ct);
        }

        // Paged ranking of the volunteers active in the range. sort: count (default) | hours | lastDate; direction: desc (default) | asc.
        [HttpGet("volunteers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResult<VolunteerRankingRowDto>>> GetVolunteerRanking(
            [FromQuery] StatsFilter filter,
            [FromQuery] VolunteerRankingQueryDto query,
            CancellationToken ct)
        {
            return await statsService.GetVolunteerRankingAsync(filter, query, ct);
        }

        [HttpGet("volunteers/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VolunteerStatsDto>> GetVolunteerStats(int id, [FromQuery] StatsFilter filter, CancellationToken ct)
        {
            return await statsService.GetVolunteerStatsAsync(id, filter, ct);
        }

        // Works for a department (all its projects) and for a single project.
        [HttpGet("projects/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProjectStatsDto>> GetProjectStats(int id, [FromQuery] StatsFilter filter, CancellationToken ct)
        {
            return await statsService.GetProjectStatsAsync(id, filter, ct);
        }
    }
}
