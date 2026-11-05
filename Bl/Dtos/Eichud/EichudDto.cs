namespace Bl.Dtos.Eichud
{
    // Text fields are trimmed: the database pads them to a fixed length.
    public sealed record EichudDto(
        int EichudCode,
        string? Tohar,
        string? FamilyName,
        string? FirstName,
        string? Ending,
        string? FathersName,
        string? Shtibel,
        string? Street,
        string? House,
        string? City,
        int? ZipCode,
        string? HousePhone,
        string? CellPhone,
        string? Shver);
}
