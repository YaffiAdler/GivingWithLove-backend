namespace Bl.Dtos.Eichud
{
    // EichudCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdateEichudDto(
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
