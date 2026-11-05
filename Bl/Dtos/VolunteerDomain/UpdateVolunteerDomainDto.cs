namespace Bl.Dtos.VolunteerDomain
{
    // VolunteerDomainsCode must equal the route id; the controller rejects a mismatch.
    public sealed record UpdateVolunteerDomainDto(int VolunteerDomainsCode, int VolunteerCode, int ProjectCode);
}
