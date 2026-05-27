using Bl.BLApi;
using Bl.BLModels;
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
        BlEichudInterface BlEichud;
        BlVolunteerDomainInterface BlDomain;
        BlPositionInterface BlPosition;

        public BLVolunteerService(IDal dal,BlEichudInterface blEichud,BlVolunteerDomainInterface _blDomain ,BlPositionInterface blPosition)
        {
            this.Volunteer = dal.Volunteer;
            this.BlEichud = blEichud;
            this.BlDomain = _blDomain;
            this.BlPosition = blPosition;
        }
        private Volunteer Convert(BLVolunteerModel v)
        {
            return new Volunteer() { VolunteerCode = v.VolunteerCode, PositionCode = v.PositionCode,
            VolunteerDomains = v.VolunteerDomains.Select(x => ((BlVolunteerDomainService)BlDomain).Convert(x)).ToList()
            };
        }
        private BLVolunteerModel Convert(Volunteer v)
        {
             BLVolunteerModel blv =  new BLVolunteerModel() { VolunteerCode = v.VolunteerCode, PositionCode = v.PositionCode ,VolunteerCodeNavigation=((BlEichudService) BlEichud).convert( v.VolunteerCodeNavigation),
                VolunteerDomains = v.VolunteerDomains.Select(x => ((BlVolunteerDomainService)BlDomain).Convert(x)).ToList()
            };
            if (blv.PositionCode != null)
            {
                blv.PositionName = ((BlPositionService)BlPosition).Read(v.PositionCode ?? 0).Result.positionName;
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
            
            //item.VolunteerDomains.ToList().ForEach(x => BlDomain.Create(x));
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
            //item.VolunteerDomains.ToList().ForEach(x => BlDomain.Create(x));
            var tt = Convert(item);

            Volunteer.Update(tt);
        }
        public async Task UpdateVolunteerDomains(BLVolunteerModel dto)
        {
            await Volunteer.UpdateVolunteerDomains(Convert(dto));
        }
    }
}
