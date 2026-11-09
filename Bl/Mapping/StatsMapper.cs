using Bl.Dtos.Stats;
using Bl.Exceptions;
using Dal.Queries;

namespace Bl.Mapping
{
    internal static class StatsMapper
    {
        // SubProject.EstimatedTime is stored in minutes (owner's decision). Change only this constant if the unit changes.
        public const decimal EstimatedTimeUnitsPerHour = 60m;

        // Longest date range a statistics request may cover (owner's decision); it bounds the monthly series.
        public const int MaxRangeYears = 10;
        public const int MaxRangeMonths = MaxRangeYears * 12;

        private const int MoneyAndHoursDecimals = 2;

        public const string SortByCount = "count";
        public const string SortByHours = "hours";
        public const string SortByLastDate = "lastdate";

        public static decimal ToHours(double estimatedTimeSum)
        {
            return Round((decimal)estimatedTimeSum / EstimatedTimeUnitsPerHour);
        }

        // EstimatedCost is the value in NIS of one volunteering, so the sum is already the value.
        public static decimal ToValue(double estimatedCostSum)
        {
            return Round((decimal)estimatedCostSum);
        }

        // Fixed-length char columns arrive padded with spaces.
        public static string? TrimName(string? name)
        {
            return name?.Trim();
        }

        public static NamedCountDto ToDto(NamedCountRow row)
        {
            return new NamedCountDto(row.Code, row.Name.Trim(), row.Count, ToHours(row.EstimatedTimeSum), ToValue(row.EstimatedCostSum));
        }

        public static VolunteerRankingRowDto ToDto(VolunteerRankingRow row)
        {
            return new VolunteerRankingRowDto(
                row.VolunteerCode,
                TrimName(row.FirstName),
                TrimName(row.FamilyName),
                row.Count,
                ToHours(row.EstimatedTimeSum),
                ToValue(row.EstimatedCostSum),
                row.LastDate);
        }

        // Number of calendar months touched by the range, both ends included (Jan 15 .. Feb 1 is 2 months).
        public static int CountMonths(DateOnly from, DateOnly to)
        {
            return ToMonthIndex(to) - ToMonthIndex(from) + 1;
        }

        // One row per month from the first month to the last month, inclusive; months without volunteerings get zeros.
        // Without a bound in the filter, the range starts or ends at the first or last month that has data.
        // A range taken partly from the data is clamped to its last MaxRangeMonths months; a range given fully
        // by the caller has already been limited to MaxRangeMonths by the service.
        public static List<MonthlyCountDto> ToMonthSeries(IReadOnlyList<MonthlyCountRow> rows, StatsFilter filter)
        {
            DateOnly? first = filter.From ?? (rows.Count == 0 ? null : new DateOnly(rows[0].Year, rows[0].Month, 1));
            DateOnly? last = filter.To ?? (rows.Count == 0 ? null : new DateOnly(rows[^1].Year, rows[^1].Month, 1));
            if (first is not DateOnly start || last is not DateOnly end)
                return [];

            int lastIndex = ToMonthIndex(end);
            int firstIndex = Math.Max(ToMonthIndex(start), lastIndex - MaxRangeMonths + 1);

            Dictionary<(int Year, int Month), MonthlyCountRow> byMonth = rows.ToDictionary(row => (row.Year, row.Month));
            List<MonthlyCountDto> series = new List<MonthlyCountDto>(lastIndex - firstIndex + 1);
            for (int monthIndex = firstIndex; monthIndex <= lastIndex; monthIndex++)
            {
                (int year, int month) = FromMonthIndex(monthIndex);
                series.Add(byMonth.TryGetValue((year, month), out MonthlyCountRow? row)
                    ? new MonthlyCountDto(year, month, row.Count, ToHours(row.EstimatedTimeSum))
                    : new MonthlyCountDto(year, month, 0, 0m));
            }
            return series;
        }

        // Always seven rows, Sunday (0) to Saturday (6).
        public static List<WeekdayCountDto> ToWeekdays(IEnumerable<DailyCountRow> days)
        {
            int[] counts = new int[7];
            foreach (DailyCountRow day in days)
                counts[(int)day.Date.DayOfWeek] += day.Count;

            return counts.Select((count, dayOfWeek) => new WeekdayCountDto(dayOfWeek, count)).ToList();
        }

        public static StatsCriteria ToCriteria(StatsFilter filter, int? volunteerCode = null, int? matcherCode = null, int? projectCode = null)
        {
            return new StatsCriteria(filter.From, filter.To, volunteerCode, matcherCode, projectCode);
        }

        // A ranking shows the most active volunteers first unless the caller asks otherwise.
        public static SortDirection ToRankingDirection(string? direction)
        {
            return PagingMapper.ToDirection(direction, SortDirection.Descending);
        }

        public static VolunteerRankingSort ToSort(string? sort)
        {
            return sort?.Trim().ToLowerInvariant() switch
            {
                null or "" or SortByCount => VolunteerRankingSort.Count,
                SortByHours => VolunteerRankingSort.Hours,
                SortByLastDate => VolunteerRankingSort.LastDate,
                _ => throw new InvalidQueryException(
                    $"Unknown sort '{sort}'. Allowed values: count, hours, lastDate.")
            };
        }

        // Months counted from January of year 0, so month arithmetic never goes through DateOnly.AddMonths,
        // which throws past 9999-12-31.
        private static int ToMonthIndex(DateOnly date)
        {
            return date.Year * 12 + date.Month - 1;
        }

        private static (int Year, int Month) FromMonthIndex(int monthIndex)
        {
            return (monthIndex / 12, monthIndex % 12 + 1);
        }

        private static decimal Round(decimal value)
        {
            return Math.Round(value, MoneyAndHoursDecimals, MidpointRounding.AwayFromZero);
        }
    }
}
