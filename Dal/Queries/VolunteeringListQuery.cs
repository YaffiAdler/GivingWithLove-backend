namespace Dal.Queries
{
    public sealed record VolunteeringListQuery(
        VolunteeringFilter Filter,
        VolunteeringSort Sort,
        SortDirection Direction,
        PageRequest Page);
}
