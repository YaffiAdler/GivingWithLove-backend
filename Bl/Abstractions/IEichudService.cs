using Bl.Dtos.Common;
using Bl.Dtos.Eichud;

namespace Bl.Abstractions
{
    public interface IEichudService : ICrudService<EichudDto, CreateEichudDto, UpdateEichudDto>
    {
        // Throws InvalidQueryException for an unknown sort value.
        Task<PagedResult<EichudDto>> GetPageAsync(PageQueryDto query, CancellationToken ct);
    }
}
