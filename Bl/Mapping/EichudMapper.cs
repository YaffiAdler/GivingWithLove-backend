using Bl.Dtos.Eichud;
using Dal.Models;

namespace Bl.Mapping
{
    internal static class EichudMapper
    {
        // The text columns are padded to a fixed length in the database, so every text value is trimmed.
        public static EichudDto ToDto(Eichud eichud)
        {
            return new EichudDto(
                eichud.EichudCode,
                eichud.Tohar?.Trim(),
                eichud.FamilyName?.Trim(),
                eichud.FirstName?.Trim(),
                eichud.Ending?.Trim(),
                eichud.FathersName?.Trim(),
                eichud.Shtibel?.Trim(),
                eichud.Street?.Trim(),
                eichud.House?.Trim(),
                eichud.City?.Trim(),
                eichud.ZipCode,
                eichud.HousePhone?.Trim(),
                eichud.CellPhone?.Trim(),
                eichud.Shver?.Trim());
        }

        public static Eichud ToEntity(CreateEichudDto eichud)
        {
            return new Eichud()
            {
                EichudCode = eichud.EichudCode,
                Tohar = eichud.Tohar,
                FamilyName = eichud.FamilyName,
                FirstName = eichud.FirstName,
                Ending = eichud.Ending,
                FathersName = eichud.FathersName,
                Shtibel = eichud.Shtibel,
                Street = eichud.Street,
                House = eichud.House,
                City = eichud.City,
                ZipCode = eichud.ZipCode,
                HousePhone = eichud.HousePhone,
                CellPhone = eichud.CellPhone,
                Shver = eichud.Shver
            };
        }

        public static Eichud ToEntity(UpdateEichudDto eichud)
        {
            return new Eichud()
            {
                EichudCode = eichud.EichudCode,
                Tohar = eichud.Tohar,
                FamilyName = eichud.FamilyName,
                FirstName = eichud.FirstName,
                Ending = eichud.Ending,
                FathersName = eichud.FathersName,
                Shtibel = eichud.Shtibel,
                Street = eichud.Street,
                House = eichud.House,
                City = eichud.City,
                ZipCode = eichud.ZipCode,
                HousePhone = eichud.HousePhone,
                CellPhone = eichud.CellPhone,
                Shver = eichud.Shver
            };
        }
    }
}
