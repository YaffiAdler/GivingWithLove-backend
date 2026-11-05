namespace Bl.Dtos.Project
{
    // ProjectCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdateProjectDto(
        int ProjectCode,
        string ProjectName,
        int? ProjectManagerCode,
        int? DomainCode);
}
