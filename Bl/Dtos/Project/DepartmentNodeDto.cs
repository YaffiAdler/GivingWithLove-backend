namespace Bl.Dtos.Project
{
    // Top level of the project tree: a department (אגף) and its projects.
    public sealed record DepartmentNodeDto(
        int ProjectCode,
        string ProjectName,
        IReadOnlyList<ProjectNodeDto> Projects);
}
