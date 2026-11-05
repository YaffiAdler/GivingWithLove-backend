using System.ComponentModel.DataAnnotations;

namespace Bl.Dtos.Common
{
    // Paging part of a list query string. Out-of-range values are rejected with 400 by model validation.
    // The allowed Sort values depend on the list.
    public abstract class PagingQueryDto
    {
        public const int DefaultPageSize = 25;
        public const int MaxPageSize = 100;

        // Keeps (Page - 1) * MaxPageSize inside int, so the row offset can never overflow.
        public const int MaxPage = int.MaxValue / MaxPageSize;

        [Range(1, MaxPage)]
        public int Page { get; init; } = 1;

        [Range(1, MaxPageSize)]
        public int PageSize { get; init; } = DefaultPageSize;

        public string? Sort { get; init; }
    }
}
