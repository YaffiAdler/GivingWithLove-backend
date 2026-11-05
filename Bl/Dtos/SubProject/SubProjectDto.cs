namespace Bl.Dtos.SubProject
{
    public sealed record SubProjectDto(
        int SubProjectCode,
        int ProjectCode,
        string SubProjectName,
        double? EstimatedTime,
        double? EstimatedCost);
}
