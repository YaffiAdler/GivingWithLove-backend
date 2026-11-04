using Bl.BLApi;
using Bl.BLModels;
using Bl.Mapping;
using Dal.Api;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.BLServices
{
    public class BLVolunteerService : BlVolunteerInterface
    {
        DalVolunteerInterface Volunteer;
        BlPositionInterface BlPosition;

        public BLVolunteerService(DalVolunteerInterface volunteer, BlPositionInterface blPosition)
        {
            this.Volunteer = volunteer;
            this.BlPosition = blPosition;
        }
        private Volunteer Convert(BLVolunteerModel v)
        {
            return new Volunteer() { VolunteerCode = v.VolunteerCode, PositionCode = v.PositionCode,
            VolunteerDomains = v.VolunteerDomains.Select(VolunteerDomainMapper.ToEntity).ToList()
            };
        }
        private BLVolunteerModel Convert(Volunteer v)
        {
             BLVolunteerModel blv =  new BLVolunteerModel() { VolunteerCode = v.VolunteerCode, PositionCode = v.PositionCode ,VolunteerCodeNavigation=EichudMapper.ToModel( v.VolunteerCodeNavigation),
                VolunteerDomains = v.VolunteerDomains.Select(VolunteerDomainMapper.ToModel).ToList()
            };
            if (blv.PositionCode != null)
            {
                blv.PositionName = BlPosition.Read(v.PositionCode ?? 0).Result.positionName;
            }
            return blv;
        }
        private List<BLVolunteerModel> Convert(List<Volunteer> c)
        {
            List<BLVolunteerModel> list = new List<BLVolunteerModel>();
            foreach (var item in c)
            {
                list.Add(Convert(item));
            }
            return list;
        }

        public void Create(BLVolunteerModel item)

        {
 
            Volunteer.Create(Convert(item));
            
        }

        public void Delete(BLVolunteerModel item)
        {
            Volunteer.Delete(Convert(item));
        }

        public async Task<BLVolunteerModel> Read(int id)
        {
            
            try { return Convert(Volunteer.Read(id).Result ); }
            catch (ObjectNotFoundException e) 
            {
                return null;
            }
        }

        public async Task<List<BLVolunteerModel>> Read(Func<BLVolunteerModel, bool> func)
        {
            List<BLVolunteerModel> list = Convert(Volunteer.Read((Func<Volunteer, bool>)func).Result);
            return list;
        }

        public async Task<List<BLVolunteerModel>> ReadAll()
        {
            List<BLVolunteerModel> list = new List<BLVolunteerModel>();

            Volunteer.ReadAll().Result.ForEach(item => { list.Add(Convert(item)); });

            return list;
        }

        public void Update(BLVolunteerModel item)
        {   
            var tt = Convert(item);

            Volunteer.Update(tt);
        }
        public async Task UpdateVolunteerDomains(BLVolunteerModel dto)
        {
            await Volunteer.UpdateVolunteerDomains(Convert(dto));
        }
    }
}
