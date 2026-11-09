using Dal.Abstractions;
using Dal.Models;
using Dal.Paging;
using Dal.Queries;
using Microsoft.EntityFrameworkCore;

namespace Dal.Repositories
{
    public class EichudRepository : IEichudRepository
    {
        private readonly DbManager db;

        public EichudRepository(DbManager db)
        {
            this.db = db;
        }

        public async Task<PagedRows<Eichud>> GetPageAsync(PersonListQuery query, CancellationToken ct)
        {
            IQueryable<Eichud> eichuds = db.Eichuds.AsNoTracking();

            if (query.Code is int code)
                eichuds = eichuds.Where(eichud => eichud.EichudCode == code);
            if (query.NameContains is string name)
                eichuds = eichuds.Where(eichud =>
                    (eichud.FirstName != null && eichud.FirstName.Contains(name))
                    || (eichud.FamilyName != null && eichud.FamilyName.Contains(name)));

            return await eichuds.ToPagedRowsAsync(
                source => OrderBy(source, query.Sort, query.Direction),
                page => page,
                query.Page,
                ct);
        }

        public async Task<Eichud?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await db.Eichuds.AsNoTracking().FirstOrDefaultAsync(eichud => eichud.EichudCode == id, ct);
        }

        public async Task<Eichud> CreateAsync(Eichud item, CancellationToken ct)
        {
            db.Eichuds.Add(item);
            await db.SaveChangesAsync(ct);
            return item;
        }

        public async Task<bool> UpdateAsync(Eichud item, CancellationToken ct)
        {
            Eichud? existing = await db.Eichuds.FirstOrDefaultAsync(eichud => eichud.EichudCode == item.EichudCode, ct);
            if (existing == null)
                return false;

            db.Entry(existing).CurrentValues.SetValues(item);
            await db.SaveChangesAsync(ct);
            return true;
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct)
        {
            Eichud? existing = await db.Eichuds.FirstOrDefaultAsync(eichud => eichud.EichudCode == id, ct);
            if (existing == null)
                return false;

            db.Eichuds.Remove(existing);
            await db.SaveChangesAsync(ct);
            return true;
        }

        // Every order ends with the key so that rows with equal names keep a stable position across pages.
        private static IOrderedQueryable<Eichud> OrderBy(IQueryable<Eichud> eichuds, PersonSort sort, SortDirection direction)
        {
            return sort switch
            {
                PersonSort.Name => eichuds
                    .OrderByDirection(eichud => eichud.FamilyName, direction)
                    .ThenByDirection(eichud => eichud.FirstName, direction)
                    .ThenBy(eichud => eichud.EichudCode),
                PersonSort.Code => eichuds.OrderByDirection(eichud => eichud.EichudCode, direction),
                _ => throw new ArgumentOutOfRangeException(nameof(sort), sort, null)
            };
        }
    }
}
