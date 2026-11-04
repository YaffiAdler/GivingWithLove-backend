namespace Bl.Dtos.Volunteer
{
    // Replaces the full set of project domains of one volunteer; an empty list removes them all.
    public sealed class UpdateVolunteerDomainsDto
    {
        public required int VolunteerCode { get; init; }

        // Non-nullable and required: a missing or null value is rejected with 400 by model validation.
        public required List<int> ProjectCodes { get; init; }
    }
}
