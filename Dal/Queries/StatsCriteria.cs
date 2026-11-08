namespace Dal.Queries
{
    // Which volunteerings a statistics query covers. All parts are optional and combine with AND.
    // From and To are inclusive. VolunteerCode matches the volunteer (not the matcher), MatcherCode the matcher.
    // ProjectCode matches a project, or a department together with all its child projects.
    public sealed record StatsCriteria(
        DateOnly? From,
        DateOnly? To,
        int? VolunteerCode = null,
        int? MatcherCode = null,
        int? ProjectCode = null);
}
