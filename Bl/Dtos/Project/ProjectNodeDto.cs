using Bl.Dtos.SubProject;

namespace Bl.Dtos.Project
{
    // Second level of the project tree; SubProjects is empty when the project has none.
    public sealed record ProjectNodeDto(
        int ProjectCode,
        string ProjectName,
        IReadOnlyList<SubProjectDto> SubProjects);
}
