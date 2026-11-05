namespace Bl.Dtos.SubProject
{
    // SubProjectCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdateSubProjectDto(
        int SubProjectCode,
        int ProjectCode,
        string SubProjectName,
        double? EstimatedTime,
        double? EstimatedCost);
}
