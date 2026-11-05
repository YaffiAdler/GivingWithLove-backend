namespace Bl.Dtos.Volunteer
{
    // Only the position is updatable here: personal data lives in Eichud and domains go through PUT api/Volunteer/domains.
    // VolunteerCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdateVolunteerDto(int VolunteerCode, int? PositionCode);
}
