namespace Dal.Queries
{
    // Every order is descending and ends with VolunteerCode ascending, so pages are stable.
    public enum VolunteerRankingSort
    {
        Count,
        Hours,
        LastDate
    }
}
