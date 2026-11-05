using Bl.Dtos.Common;
using Bl.Exceptions;
using Dal.Queries;

namespace Bl.Mapping
{
    // Builds the list query shared by the Eichud and Volunteer lists.
    internal static class PersonQueryMapper
    {
        public const string SortByName = "name";
        public const string SortByCode = "code";

        // A numeric search text is a code and must match exactly; any other text is searched inside the first and family names.
        public static PersonListQuery ToQuery(PageQueryDto query)
        {
            string? search = string.IsNullOrWhiteSpace(query.Search) ? null : query.Search.Trim();
            int? code = int.TryParse(search, out int parsedCode) ? parsedCode : null;
            string? nameContains = code == null ? search : null;

            return new PersonListQuery(code, nameContains, ToSort(query.Sort), PagingMapper.ToPageRequest(query));
        }

        private static PersonSort ToSort(string? sort)
        {
            return sort?.Trim().ToLowerInvariant() switch
            {
                null or "" or SortByName => PersonSort.Name,
                SortByCode => PersonSort.Code,
                _ => throw new InvalidQueryException($"Unknown sort '{sort}'. Allowed values: {SortByName}, {SortByCode}.")
            };
        }
    }
}
