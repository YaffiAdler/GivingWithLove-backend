namespace Dal.Queries
{
    // A department (top-level project) with its child projects.
    public sealed record DepartmentTreeRow(
        int ProjectCode,
        string ProjectName,
        List<ProjectTreeRow> Projects);
}
