namespace Dal.Queries
{
    // A class with init members rather than a positional record: EF can compose further operators
    // (joins, filters) on members of a member-initialized projection, but not on constructor arguments.
    internal sealed class ProjectDepartmentLink
    {
        public int ProjectCode { get; init; }

        public int DepartmentCode { get; init; }

        public string DepartmentName { get; init; } = "";
    }
}
