using System.ComponentModel.DataAnnotations;

namespace Bl.Dtos.Common
{
    // Query string of a paged list with free-text search (Eichud, Volunteer).
    public sealed class PageQueryDto : PagingQueryDto
    {
        public const int MaxSearchLength = 100;

        [StringLength(MaxSearchLength)]
        public string? Search { get; init; }
    }
}
