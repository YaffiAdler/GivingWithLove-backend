using Dal.Models;
using Dal.Queries;

namespace Dal.Api
{
    public interface DalVolunteeringInterface : ICrud<Volunteering>
    {
        Task<List<VolunteeringRow>> GetRowsAsync(VolunteeringFilter filter, CancellationToken ct);

        Task<VolunteeringRow?> GetRowByIdAsync(int id, CancellationToken ct);
    }
}
