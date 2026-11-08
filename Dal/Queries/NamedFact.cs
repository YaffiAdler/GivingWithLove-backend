namespace Dal.Queries
{
    // A volunteering's time and cost labelled with the code and name it is grouped under.
    // Member-initialized (not a positional record) so EF can group on its members.
    internal sealed class NamedFact
    {
        public int Code { get; init; }

        public string Name { get; init; } = "";

        public double EstimatedTime { get; init; }

        public double EstimatedCost { get; init; }
    }
}
