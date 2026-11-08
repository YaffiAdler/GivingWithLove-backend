namespace Bl.Dtos.Stats
{
    // Volunteerings of one department, project or sub-project. Value is in NIS.
    public sealed record NamedCountDto(int Code, string Name, int Count, decimal Hours, decimal Value);
}
