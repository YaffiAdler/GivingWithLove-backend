namespace Dal.Queries
{
    // One volunteering reduced to the values the statistics group and sum.
    // Member-initialized (not a positional record) so EF can group and filter on its members.
    internal sealed class VolunteeringFacts
    {
        public DateOnly Date { get; init; }

        public int? VolunteerCode { get; init; }

        public int? ProjectCode { get; init; }

        public string? ProjectName { get; init; }

        public int? SubProjectCode { get; init; }

        public string? SubProjectName { get; init; }

        // SubProject.EstimatedTime as stored (its unit is decided in Bl); 0 without a sub-project.
        public double EstimatedTime { get; init; }

        // SubProject.EstimatedCost of one volunteering; 0 without a sub-project.
        public double EstimatedCost { get; init; }
    }
}
