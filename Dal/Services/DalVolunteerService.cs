using Dal.Api;
using Dal.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class DalVolunteerService : DalVolunteerInterface
    {

        private DbManager db;

        public DalVolunteerService(DbManager dbm)
        {
            db = dbm;
        }

        public void Create(Volunteer item)
        {
            db.Volunteers.Add(item);
            db.SaveChanges();
            
        }

        

        public void Delete(Volunteer item)
        {
            if (item == null)
                throw new ArgumentNullException("item");
            // שאילתא ששולפת את הפריט ואז לעשות רימוב

            db.Volunteers.Remove
                (db.Volunteers.Where(x => x.VolunteerCode == item.VolunteerCode).FirstOrDefault()
                            ?? throw new Exception("לא נמצא הפריט למחיקה"));
            db.SaveChanges();
            //throw new NotImplementedException();
        }

       
        public async Task<List<Volunteer>> Read(Func<Volunteer, bool> func)
        {
            List<Volunteer> list = new();
            foreach (Volunteer item in db.Volunteers)
                if (func(item))
                    list.Add(item);
            return list;
        }
        public async Task<Volunteer> Read(int id)
        {
            Volunteer v = ReadAll().Result.Find(v => v.VolunteerCode == id)??throw new ObjectNotFoundException();
            return v;
        }


        public async Task<List<Volunteer>> ReadAll() => db.Volunteers.Include(x=> x.VolunteerCodeNavigation).Include(y=> y.VolunteerDomains).ToList();
        //{
        //    throw new NotImplementedException();
        //}

        public void Update(Volunteer item)
        {
            try
            {
                var volunteer = db.Volunteers
                    .FirstOrDefault(v => v.VolunteerCode == item.VolunteerCode);

                if (volunteer != null)
                {
                    volunteer.PositionCode = item.PositionCode;
                    
                    //volunteer.VolunteerDomains = item.VolunteerDomains;
                    //volunteer. = item.Address;

                    // לעדכן את כל השדות הדרושים

                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
