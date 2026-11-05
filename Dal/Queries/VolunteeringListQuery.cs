namespace Dal.Queries
{
    public sealed record VolunteeringListQuery(
        VolunteeringFilter Filter,
        VolunteeringSort Sort,
        PageRequest Page);
}
