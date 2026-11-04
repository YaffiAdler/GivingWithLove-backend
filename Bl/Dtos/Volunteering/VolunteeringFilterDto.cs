namespace Bl.Dtos.Volunteering
{
    // Optional criteria for the volunteering list; From and To are inclusive.
    public sealed class VolunteeringFilterDto
    {
        public int? VolunteerCode { get; init; }

        public int? ProjectCode { get; init; }

        public DateOnly? From { get; init; }

        public DateOnly? To { get; init; }
    }
}
