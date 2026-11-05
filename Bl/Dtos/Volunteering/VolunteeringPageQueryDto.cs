using Bl.Dtos.Common;

namespace Bl.Dtos.Volunteering
{
    // Paging of the volunteering list. There is no free-text search: the list is narrowed with VolunteeringFilterDto.
    public sealed class VolunteeringPageQueryDto : PagingQueryDto
    {
    }
}
