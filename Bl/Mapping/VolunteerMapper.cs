using Bl.Dtos.Volunteer;
using Dal.Models;
using Dal.Queries;

namespace Bl.Mapping
{
    internal static class VolunteerMapper
    {
        // The Eichud text columns are padded to a fixed length in the database, so every text value is trimmed.
        public static VolunteerDto ToDto(VolunteerRow row)
        {
            return new VolunteerDto(
                row.VolunteerCode,
                row.FirstName?.Trim(),
                row.FamilyName?.Trim(),
                row.PositionCode,
                row.PositionName,
                row.ProjectCodes);
        }

        public static VolunteerDetailsDto ToDetailsDto(VolunteerDetailsRow row)
        {
            return new VolunteerDetailsDto(
                row.VolunteerCode,
                row.FirstName?.Trim(),
                row.FamilyName?.Trim(),
                row.PositionCode,
                row.PositionName,
                row.ProjectCodes,
                row.Tohar?.Trim(),
                row.Ending?.Trim(),
                row.FathersName?.Trim(),
                row.Shtibel?.Trim(),
                row.Street?.Trim(),
                row.House?.Trim(),
                row.City?.Trim(),
                row.ZipCode,
                row.HousePhone?.Trim(),
                row.CellPhone?.Trim(),
                row.Shver?.Trim());
        }

        public static Volunteer ToEntity(CreateVolunteerDto volunteer)
        {
            return new Volunteer()
            {
                VolunteerCode = volunteer.VolunteerCode,
                PositionCode = volunteer.PositionCode,
                VolunteerDomains = volunteer.ProjectCodes
                    .Distinct()
                    .Select(projectCode => new VolunteerDomain()
                    {
                        VolunteerCode = volunteer.VolunteerCode,
                        ProjectCode = projectCode
                    })
                    .ToList()
            };
        }

        public static Volunteer ToEntity(UpdateVolunteerDto volunteer)
        {
            return new Volunteer()
            {
                VolunteerCode = volunteer.VolunteerCode,
                PositionCode = volunteer.PositionCode
            };
        }
    }
}
