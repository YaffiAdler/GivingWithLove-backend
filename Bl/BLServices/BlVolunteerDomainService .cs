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
    public class BlVolunteerDomainService:BlVolunteerDomainInterface
    {
        DalVolunteerDomainInterface dal;
        public BlVolunteerDomainService(DalVolunteerDomainInterface dal)
        {
            this.dal = dal;
        }

        public void Create(BlVolunteerDomainModel item)
        {
            dal.Create(VolunteerDomainMapper.ToEntity(item));
        }

        public void Delete(BlVolunteerDomainModel item)
        {
            dal.Delete(VolunteerDomainMapper.ToEntity(item));
        }


        public async Task<List<BlVolunteerDomainModel>> ReadAll()
        {
            List<BlVolunteerDomainModel> list = new List<BlVolunteerDomainModel>();

            dal.ReadAll().Result.ForEach(item => { list.Add(VolunteerDomainMapper.ToModel(item)); });

            return list;
        }

        public void Update(BlVolunteerDomainModel item)
        {
            dal.Update(VolunteerDomainMapper.ToEntity(item));

        }
        public async Task<List<BlVolunteerDomainModel>> Read(Func<BlVolunteerDomainModel, bool> func)
        {
            List<BlVolunteerDomainModel> list = VolunteerDomainMapper.ToModels(dal.Read((Func<VolunteerDomain, bool>)func).Result);
            return list;

        }

        public async Task<BlVolunteerDomainModel> Read(int vl)
        {
           return VolunteerDomainMapper.ToModel(dal.Read(vl).Result);
        }

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }
}

