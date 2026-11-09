namespace Dal.Queries
{
    // The requested direction applies to the ranked value; ties are always broken by VolunteerCode ascending,
    // so pages are stable.
    public enum VolunteerRankingSort
    {
        Count,
        Hours,
        LastDate
    }
}
