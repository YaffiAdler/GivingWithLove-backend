using Bl.Dtos.Common;
using Bl.Dtos.Eichud;
using Bl.Exceptions;
using Bl.Services;
using Bl.Tests.Stats;
using Dal.Models;
using Dal.Repositories;

namespace Bl.Tests.Eichuds
{
    // The Volunteer list shares the sort and direction mapping with this list. It is not tested here because its
    // domain-codes subquery needs SQL APPLY, which SQLite does not support.
    public sealed class EichudServiceTests : IDisposable
    {
        private readonly StatsTestDatabase database = new StatsTestDatabase();
        private readonly DbManager db;
        private readonly EichudService service;

        public EichudServiceTests()
        {
            db = database.CreateContext();
            service = new EichudService(new EichudRepository(db));
        }

        public void Dispose()
        {
            db.Dispose();
            database.Dispose();
        }

        [Fact]
        public async Task GetPage_DefaultsToFamilyNameAscending()
        {
            PagedResult<EichudDto> page = await service.GetPageAsync(new PageQueryDto(), CancellationToken.None);

            Assert.Equal(["כהן", "לוי", "מזרחי", "פרץ"], page.Items.Select(eichud => eichud.FamilyName));
        }

        [Fact]
        public async Task GetPage_NameDescending()
        {
            PageQueryDto query = new PageQueryDto { Sort = "name", Direction = "desc" };

            PagedResult<EichudDto> page = await service.GetPageAsync(query, CancellationToken.None);

            Assert.Equal(["פרץ", "מזרחי", "לוי", "כהן"], page.Items.Select(eichud => eichud.FamilyName));
        }

        [Fact]
        public async Task GetPage_CodeDescending()
        {
            PageQueryDto query = new PageQueryDto { Sort = "code", Direction = "desc" };

            PagedResult<EichudDto> page = await service.GetPageAsync(query, CancellationToken.None);

            Assert.Equal([4, 3, 2, 1], page.Items.Select(eichud => eichud.EichudCode));
        }

        [Fact]
        public async Task GetPage_UnknownDirection_Throws()
        {
            PageQueryDto query = new PageQueryDto { Direction = "sideways" };

            await Assert.ThrowsAsync<InvalidQueryException>(() => service.GetPageAsync(query, CancellationToken.None));
        }
    }
}
