namespace Bl.Dtos.Project
{
    // DomainCode is the parent department of a project.
    public sealed record ProjectDto(
        int ProjectCode,
        string ProjectName,
        int? ProjectManagerCode,
        int? DomainCode);
}
