using Bl.Dtos.Common;
using Bl.Dtos.Volunteering;
using Bl.Exceptions;
using Bl.Services;
using Bl.Tests.Stats;
using Dal.Models;
using Dal.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Bl.Tests.Volunteerings
{
    public sealed class VolunteeringServiceTests : IDisposable
    {
        private readonly StatsTestDatabase database = new StatsTestDatabase();
        private readonly DbManager db;
        private readonly VolunteeringService service;

        public VolunteeringServiceTests()
        {
            db = database.CreateContext();
            service = new VolunteeringService(new VolunteeringRepository(db));
        }

        public void Dispose()
        {
            db.Dispose();
            database.Dispose();
        }

        [Fact]
        public async Task GetPage_DefaultsToNewestFirst()
        {
            PagedResult<VolunteeringDto> page = await service.GetPageAsync(
                new VolunteeringPageQueryDto(), new VolunteeringFilterDto(), CancellationToken.None);

            Assert.Equal([6, 5, 4, 3, 2, 1], page.Items.Select(volunteering => volunteering.VolunteeringCode));
        }

        [Fact]
        public async Task GetPage_DateAscending()
        {
            VolunteeringPageQueryDto query = new VolunteeringPageQueryDto { Direction = "asc" };

            PagedResult<VolunteeringDto> page = await service.GetPageAsync(query, new VolunteeringFilterDto(), CancellationToken.None);

            Assert.Equal([1, 2, 3, 4, 5, 6], page.Items.Select(volunteering => volunteering.VolunteeringCode));
        }

        [Fact]
        public async Task GetPage_UnknownDirection_Throws()
        {
            VolunteeringPageQueryDto query = new VolunteeringPageQueryDto { Direction = "sideways" };

            await Assert.ThrowsAsync<InvalidQueryException>(
                () => service.GetPageAsync(query, new VolunteeringFilterDto(), CancellationToken.None));
        }

        // A missing person must be stored as NULL, not as 0: code 0 is a real Eichud person.
        [Fact]
        public async Task Create_StoresMissingPeopleAsNull()
        {
            CreateVolunteeringDto request = new CreateVolunteeringDto(
                StatsTestDatabase.April2, VolunteerCode: null, MatcherCode: null, PoorManCode: null, ProjectCode: 21, SubProjectCode: null);

            VolunteeringDto created = await service.CreateAsync(request, CancellationToken.None);

            Assert.Null(created.VolunteerCode);
            Assert.Null(created.MatcherCode);
            Assert.Null(created.PoorManCode);

            using DbManager freshContext = database.CreateContext();
            Volunteering stored = await freshContext.Volunteerings
                .AsNoTracking()
                .SingleAsync(volunteering => volunteering.VolunteeringCode == created.VolunteeringCode);
            Assert.Null(stored.VolunteerCode);
            Assert.Null(stored.MatcherCode);
            Assert.Null(stored.PoorManCode);
        }
    }
}
