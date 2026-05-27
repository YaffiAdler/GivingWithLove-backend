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
    public class DalVolunteerDomainService : DalVolunteerDomainInterface
    {
        DbManager mydb;
        public DalVolunteerDomainService(DbManager mydb)
        {
            this.mydb = mydb;
        }

        public void Create(VolunteerDomain volunteerDomain)
        {
            if (volunteerDomain == null)
                throw new ArgumentNullException("volunteerDomain is null");
            else
                try
                {
                    mydb.VolunteerDomains.Add(volunteerDomain);
                    try {mydb.SaveChanges();}
                    catch(Exception ex)
                    {
                        mydb.VolunteerDomains.Remove(volunteerDomain);
                        Console.WriteLine(ex.InnerException?.Message);
                        throw new Exception(ex.Message);
                    }
                }
                catch(Exception ex)
                {
                    throw new Exception(ex.Message);
                }
        }

       
        public void Delete(VolunteerDomain volunteerDomain)
        {
            if (volunteerDomain == null)
                throw new ArgumentNullException("item");
            mydb.VolunteerDomains.Remove(volunteerDomain);
            mydb.SaveChanges();
        }

       public async Task<List<VolunteerDomain>> Read(Func<VolunteerDomain, bool> func)
        {
            List<VolunteerDomain> list = new();
            foreach (VolunteerDomain item in mydb.VolunteerDomains)
                if (func(item))
                    list.Add(item);
              return list;
        }

        public async Task<VolunteerDomain> Read(int id)
        {
            
             VolunteerDomain vm =  mydb.VolunteerDomains.ToList().Find(v => v.VolunteerCode == id)?? throw new ObjectNotFoundException();
            return vm;
        }
        public async Task<List<VolunteerDomain>> ReadAll()
        {
            return mydb.VolunteerDomains.ToList();
        }

        

        public void Update(VolunteerDomain volunteerDomain)
        {
            try
            {
                var i = ReadAll().Result.FindIndex(v => v.VolunteerDomainsCode == volunteerDomain.VolunteerDomainsCode);
                if (i >= 0)
                {
                    mydb.VolunteerDomains.ToList<VolunteerDomain>()[i] = volunteerDomain;
                   mydb.SaveChanges();
                }

            }
            catch { Console.WriteLine("VolunteerDomain not found"); }
           
        }

       
    }
}
  
