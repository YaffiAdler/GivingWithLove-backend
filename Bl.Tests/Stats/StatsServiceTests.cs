using Bl.Dtos.Common;
using Bl.Dtos.Stats;
using Bl.Exceptions;
using Bl.Services;
using Dal.Models;
using Dal.Queries;

namespace Bl.Tests.Stats
{
    public sealed class StatsServiceTests : IDisposable
    {
        private static readonly StatsFilter AllDates = new StatsFilter();

        private readonly StatsTestDatabase database = new StatsTestDatabase();
        private readonly DbManager db;
        private readonly StatsService service;

        public StatsServiceTests()
        {
            db = database.CreateContext();
            service = new StatsService(new StatsQueries(db));
        }

        public void Dispose()
        {
            db.Dispose();
            database.Dispose();
        }

        [Fact]
        public async Task Overview_SumsAllVolunteerings()
        {
            OverviewStatsDto overview = await service.GetOverviewAsync(AllDates, CancellationToken.None);

            Assert.Equal(6, overview.TotalVolunteerings);
            Assert.Equal(3, overview.ActiveVolunteers);
            // 90 + 30 + 60 + 60 minutes; the rows without a sub-project add nothing.
            Assert.Equal(4.00m, overview.TotalHours);
            Assert.Equal(270m, overview.TotalValue);
        }

        [Fact]
        public async Task Overview_FillsMonthsWithoutActivity()
        {
            OverviewStatsDto overview = await service.GetOverviewAsync(AllDates, CancellationToken.None);

            Assert.Equal(
                [
                    new MonthlyCountDto(2026, 1, 2, 2.00m),
                    new MonthlyCountDto(2026, 2, 0, 0m),
                    new MonthlyCountDto(2026, 3, 2, 1.00m),
                    new MonthlyCountDto(2026, 4, 2, 1.00m)
                ],
                overview.ByMonth);
        }

        [Fact]
        public async Task Overview_MonthSeriesCoversTheWholeFilterRange()
        {
            StatsFilter filter = new StatsFilter { From = new DateOnly(2025, 12, 15), To = new DateOnly(2026, 2, 10) };

            OverviewStatsDto overview = await service.GetOverviewAsync(filter, CancellationToken.None);

            Assert.Equal(
                [(2025, 12, 0), (2026, 1, 2), (2026, 2, 0)],
                overview.ByMonth.Select(month => (month.Year, month.Month, month.Count)));
        }

        [Fact]
        public async Task Overview_DateFilterIsInclusive()
        {
            StatsFilter filter = new StatsFilter { From = StatsTestDatabase.March6, To = StatsTestDatabase.March8 };

            OverviewStatsDto overview = await service.GetOverviewAsync(filter, CancellationToken.None);

            Assert.Equal(2, overview.TotalVolunteerings);
            Assert.Equal(1, overview.ActiveVolunteers);
            Assert.Equal(1.00m, overview.TotalHours);
            Assert.Equal(100m, overview.TotalValue);
            Assert.Equal([new MonthlyCountDto(2026, 3, 2, 1.00m)], overview.ByMonth);
        }

        [Fact]
        public async Task Overview_EmptyRangeReturnsZerosAndZeroMonths()
        {
            StatsFilter filter = new StatsFilter { From = new DateOnly(2027, 1, 1), To = new DateOnly(2027, 2, 28) };

            OverviewStatsDto overview = await service.GetOverviewAsync(filter, CancellationToken.None);

            Assert.Equal(0, overview.TotalVolunteerings);
            Assert.Equal(0m, overview.TotalHours);
            Assert.Equal([new MonthlyCountDto(2027, 1, 0, 0m), new MonthlyCountDto(2027, 2, 0, 0m)], overview.ByMonth);
            Assert.Empty(overview.ByDepartment);
        }

        [Fact]
        public async Task Overview_GroupsByDepartmentIncludingRowsOnTheDepartmentItself()
        {
            OverviewStatsDto overview = await service.GetOverviewAsync(AllDates, CancellationToken.None);

            Assert.Equal(
                [
                    new NamedCountDto(20, "חסד", 3, 2.00m, 200m),
                    new NamedCountDto(10, "רווחה", 2, 2.00m, 70m)
                ],
                overview.ByDepartment);
        }

        [Fact]
        public async Task Overview_ShowsRowsOnADepartmentUnderTheDepartmentInByProject()
        {
            OverviewStatsDto overview = await service.GetOverviewAsync(AllDates, CancellationToken.None);

            Assert.Equal(
                [
                    new NamedCountDto(11, "הסעות לבתי חולים", 2, 2.00m, 70m),
                    new NamedCountDto(21, "ארוחות לשבת", 2, 2.00m, 200m),
                    new NamedCountDto(20, "חסד", 1, 0m, 0m)
                ],
                overview.ByProject);
        }

        [Fact]
        public async Task Overview_RunsAConstantNumberOfQueries()
        {
            database.Commands.Reset();

            await service.GetOverviewAsync(AllDates, CancellationToken.None);

            Assert.Equal(5, database.Commands.Count);
        }

        [Fact]
        public async Task VolunteerRanking_RunsAConstantNumberOfQueries()
        {
            database.Commands.Reset();

            await service.GetVolunteerRankingAsync(AllDates, new VolunteerRankingQueryDto(), CancellationToken.None);

            Assert.Equal(2, database.Commands.Count);
        }

        [Fact]
        public async Task VolunteerStats_RunsAConstantNumberOfQueries()
        {
            database.Commands.Reset();

            await service.GetVolunteerStatsAsync(1, AllDates, CancellationToken.None);

            Assert.Equal(7, database.Commands.Count);
        }

        [Fact]
        public async Task ProjectStats_RunsAConstantNumberOfQueries()
        {
            database.Commands.Reset();

            await service.GetProjectStatsAsync(20, AllDates, CancellationToken.None);

            Assert.Equal(5, database.Commands.Count);
        }

        [Fact]
        public async Task Overview_RangeOfExactlyTenYears_IsAccepted()
        {
            StatsFilter filter = new StatsFilter { From = new DateOnly(2017, 1, 1), To = new DateOnly(2026, 12, 31) };

            OverviewStatsDto overview = await service.GetOverviewAsync(filter, CancellationToken.None);

            Assert.Equal(120, overview.ByMonth.Count);
        }

        [Theory]
        [InlineData("2016-12-31", "2026-12-01")]
        [InlineData("0001-01-01", "9999-12-31")]
        public async Task AllEndpoints_RangeLongerThanTenYears_Throws(string from, string to)
        {
            StatsFilter filter = new StatsFilter { From = DateOnly.Parse(from), To = DateOnly.Parse(to) };

            await Assert.ThrowsAsync<InvalidQueryException>(() => service.GetOverviewAsync(filter, CancellationToken.None));
            await Assert.ThrowsAsync<InvalidQueryException>(
                () => service.GetVolunteerRankingAsync(filter, new VolunteerRankingQueryDto(), CancellationToken.None));
            await Assert.ThrowsAsync<InvalidQueryException>(() => service.GetVolunteerStatsAsync(1, filter, CancellationToken.None));
            await Assert.ThrowsAsync<InvalidQueryException>(() => service.GetProjectStatsAsync(20, filter, CancellationToken.None));
        }

        [Fact]
        public async Task Overview_OpenStartWithFarEnd_ClampsMonthSeriesWithoutThrowing()
        {
            StatsFilter filter = new StatsFilter { To = DateOnly.MaxValue };

            OverviewStatsDto overview = await service.GetOverviewAsync(filter, CancellationToken.None);

            Assert.Equal(6, overview.TotalVolunteerings);
            Assert.Equal(120, overview.ByMonth.Count);
            Assert.Equal((9990, 1), (overview.ByMonth[0].Year, overview.ByMonth[0].Month));
            Assert.Equal((9999, 12), (overview.ByMonth[^1].Year, overview.ByMonth[^1].Month));
        }

        [Fact]
        public async Task Overview_FromLaterThanTo_Throws()
        {
            StatsFilter filter = new StatsFilter { From = StatsTestDatabase.March8, To = StatsTestDatabase.March6 };

            await Assert.ThrowsAsync<InvalidQueryException>(() => service.GetOverviewAsync(filter, CancellationToken.None));
        }

        [Fact]
        public async Task VolunteerRanking_ByCount_TiesOrderedByCode()
        {
            PagedResult<VolunteerRankingRowDto> page = await service.GetVolunteerRankingAsync(
                AllDates, new VolunteerRankingQueryDto(), CancellationToken.None);

            Assert.Equal(3, page.TotalCount);
            Assert.Equal([1, 2, 3], page.Items.Select(row => row.VolunteerCode));
            VolunteerRankingRowDto first = page.Items[0];
            Assert.Equal("אברהם", first.FirstName);
            Assert.Equal("כהן", first.FamilyName);
            Assert.Equal(2, first.Count);
            Assert.Equal(2.00m, first.Hours);
            Assert.Equal(70m, first.Value);
            Assert.Equal(StatsTestDatabase.January5, first.LastDate);
        }

        [Fact]
        public async Task VolunteerRanking_ByLastDate_SecondPage()
        {
            VolunteerRankingQueryDto query = new VolunteerRankingQueryDto { Sort = "lastDate", Page = 2, PageSize = 2 };

            PagedResult<VolunteerRankingRowDto> page = await service.GetVolunteerRankingAsync(AllDates, query, CancellationToken.None);

            Assert.Equal(3, page.TotalCount);
            Assert.Equal(2, page.Page);
            Assert.Equal([1], page.Items.Select(row => row.VolunteerCode));
        }

        [Fact]
        public async Task VolunteerRanking_ByHours()
        {
            VolunteerRankingQueryDto query = new VolunteerRankingQueryDto { Sort = "hours" };

            PagedResult<VolunteerRankingRowDto> page = await service.GetVolunteerRankingAsync(AllDates, query, CancellationToken.None);

            // 90 + 30 minutes, 60 minutes (+ a row without a sub-project), no sub-project at all.
            Assert.Equal([(1, 2.00m), (2, 1.00m), (3, 0m)], page.Items.Select(row => (row.VolunteerCode, row.Hours)));
        }

        [Fact]
        public async Task VolunteerRanking_UnknownSort_Throws()
        {
            VolunteerRankingQueryDto query = new VolunteerRankingQueryDto { Sort = "name" };

            await Assert.ThrowsAsync<InvalidQueryException>(
                () => service.GetVolunteerRankingAsync(AllDates, query, CancellationToken.None));
        }

        [Fact]
        public async Task VolunteerRanking_ByCountAscending_TiesStillOrderedByCode()
        {
            VolunteerRankingQueryDto query = new VolunteerRankingQueryDto { Sort = "count", Direction = "asc" };

            PagedResult<VolunteerRankingRowDto> page = await service.GetVolunteerRankingAsync(AllDates, query, CancellationToken.None);

            Assert.Equal([(3, 1), (1, 2), (2, 2)], page.Items.Select(row => (row.VolunteerCode, row.Count)));
        }

        [Fact]
        public async Task VolunteerRanking_ExplicitDescending_MatchesTheDefault()
        {
            VolunteerRankingQueryDto query = new VolunteerRankingQueryDto { Sort = "hours", Direction = "DESC" };

            PagedResult<VolunteerRankingRowDto> page = await service.GetVolunteerRankingAsync(AllDates, query, CancellationToken.None);

            Assert.Equal([1, 2, 3], page.Items.Select(row => row.VolunteerCode));
        }

        [Fact]
        public async Task VolunteerRanking_UnknownDirection_Throws()
        {
            VolunteerRankingQueryDto query = new VolunteerRankingQueryDto { Direction = "sideways" };

            await Assert.ThrowsAsync<InvalidQueryException>(
                () => service.GetVolunteerRankingAsync(AllDates, query, CancellationToken.None));
        }

        [Fact]
        public async Task VolunteerStats_CountsVolunteerAndMatcherSeparately()
        {
            VolunteerStatsDto stats = await service.GetVolunteerStatsAsync(1, AllDates, CancellationToken.None);

            Assert.Equal("אברהם", stats.FirstName);
            Assert.Equal("כהן", stats.FamilyName);
            Assert.Equal(2, stats.TotalVolunteerings);
            Assert.Equal(1, stats.TotalAsMatcher);
            Assert.Equal(2.00m, stats.TotalHours);
            Assert.Equal(70m, stats.TotalValue);
            Assert.Equal(StatsTestDatabase.January4, stats.FirstDate);
            Assert.Equal(StatsTestDatabase.January5, stats.LastDate);
            Assert.Equal([new MonthlyCountDto(2026, 1, 2, 2.00m)], stats.ByMonth);
            Assert.Equal([100, 101], stats.BySubProject.Select(subProject => subProject.Code));
        }

        [Fact]
        public async Task VolunteerStats_MapsWeekdaysFromSunday()
        {
            VolunteerStatsDto stats = await service.GetVolunteerStatsAsync(2, AllDates, CancellationToken.None);

            // March 6 2026 is a Friday, March 8 2026 a Sunday.
            Assert.Equal(
                [
                    new WeekdayCountDto(0, 1),
                    new WeekdayCountDto(1, 0),
                    new WeekdayCountDto(2, 0),
                    new WeekdayCountDto(3, 0),
                    new WeekdayCountDto(4, 0),
                    new WeekdayCountDto(5, 1),
                    new WeekdayCountDto(6, 0)
                ],
                stats.ByWeekday);
        }

        [Fact]
        public async Task VolunteerStats_NoActivityInRange_ReturnsNullDates()
        {
            StatsFilter filter = new StatsFilter { From = new DateOnly(2027, 1, 1) };

            VolunteerStatsDto stats = await service.GetVolunteerStatsAsync(3, filter, CancellationToken.None);

            Assert.Equal(0, stats.TotalVolunteerings);
            Assert.Null(stats.FirstDate);
            Assert.Null(stats.LastDate);
            Assert.Equal(7, stats.ByWeekday.Count);
        }

        [Fact]
        public async Task VolunteerStats_UnknownVolunteer_ThrowsNotFound()
        {
            // Eichud 4 exists but is not a volunteer.
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetVolunteerStatsAsync(4, AllDates, CancellationToken.None));
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetVolunteerStatsAsync(999, AllDates, CancellationToken.None));
        }

        [Fact]
        public async Task ProjectStats_DepartmentAggregatesItsProjects()
        {
            ProjectStatsDto stats = await service.GetProjectStatsAsync(20, AllDates, CancellationToken.None);

            Assert.True(stats.IsDepartment);
            Assert.Equal(3, stats.TotalVolunteerings);
            // Volunteering 5 has no volunteer.
            Assert.Equal(1, stats.DistinctVolunteers);
            Assert.Equal(2.00m, stats.TotalHours);
            Assert.Equal(200m, stats.TotalValue);
            Assert.Equal(
                [
                    new NamedCountDto(21, "ארוחות לשבת", 2, 2.00m, 200m),
                    new NamedCountDto(20, "חסד", 1, 0m, 0m)
                ],
                stats.ByChild);
            Assert.Equal([2], stats.TopVolunteers.Select(volunteer => volunteer.VolunteerCode));
        }

        [Fact]
        public async Task ProjectStats_DepartmentWithNullDomainCode()
        {
            ProjectStatsDto stats = await service.GetProjectStatsAsync(10, AllDates, CancellationToken.None);

            Assert.True(stats.IsDepartment);
            Assert.Equal(2, stats.TotalVolunteerings);
            Assert.Equal([11], stats.ByChild.Select(child => child.Code));
        }

        [Fact]
        public async Task ProjectStats_ProjectListsItsSubProjects()
        {
            ProjectStatsDto stats = await service.GetProjectStatsAsync(11, AllDates, CancellationToken.None);

            Assert.False(stats.IsDepartment);
            Assert.Equal(2, stats.TotalVolunteerings);
            Assert.Equal(
                [
                    new NamedCountDto(100, "שערי צדק", 1, 1.50m, 50m),
                    new NamedCountDto(101, "הדסה", 1, 0.50m, 20m)
                ],
                stats.ByChild);
            Assert.Equal([1], stats.TopVolunteers.Select(volunteer => volunteer.VolunteerCode));
        }

        [Fact]
        public async Task ProjectStats_UnknownProject_ThrowsNotFound()
        {
            await Assert.ThrowsAsync<NotFoundException>(() => service.GetProjectStatsAsync(999, AllDates, CancellationToken.None));
        }
    }
}
