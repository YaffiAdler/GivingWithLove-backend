namespace Bl.Dtos.Position
{
    // PositionCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdatePositionDto(int PositionCode, string PositionName);
}
