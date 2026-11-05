namespace Bl.Dtos.Volunteer
{
    // VolunteerCode is part of the request: it is the code of an existing Eichud person, not generated.
    public sealed class CreateVolunteerDto
    {
        public required int VolunteerCode { get; init; }

        public int? PositionCode { get; init; }

        // The projects of the domains of the new volunteer; may be omitted when there are none yet.
        public IReadOnlyList<int> ProjectCodes { get; init; } = [];
    }
}
