using Bl.BLModels;
using Dal.Models;

namespace Bl.Mapping
{
    public static class EichudMapper
    {
        public static BlEichudModel ToModel(Eichud eichud)
        {
            return new BlEichudModel()
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

        public static Eichud ToEntity(BlEichudModel eichud)
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
