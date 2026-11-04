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
    public class BlEichudService : BlEichudInterface
    {
        DalEichudInterface dal;
        public BlEichudService(DalEichudInterface dal)
        {
            this.dal = dal;
        }
        private List<BlEichudModel> convert(List<Eichud> v)
        {
            List<BlEichudModel> list = new List<BlEichudModel>();
            foreach (var item in v)
            {
                list.Add(EichudMapper.ToModel(item));
            }
            return list;
        }
        public void Create(BlEichudModel item)
        {
            dal.Create(EichudMapper.ToEntity(item));
        }

        public void Delete(BlEichudModel item)
        {
            dal.Delete(EichudMapper.ToEntity(item));
        }

        public async Task<List<BlEichudModel>> Read(Func<BlEichudModel, bool> func)
        {

            List<BlEichudModel> list = convert(dal.Read((Func<Eichud, bool>)func).Result);
            return list;
        }

        public async Task<BlEichudModel> Read(int id)
        {
            try { return EichudMapper.ToModel(dal.Read(id).Result); }
            catch (ObjectNotFoundException e)
            {
                return null;
            }
        }

        public async Task<List<BlEichudModel>> ReadAll()
        {
            List<BlEichudModel> list = new List<BlEichudModel>();
            dal.ReadAll().Result.ForEach(item => { list.Add(EichudMapper.ToModel(item)); });
            return list;
        }

        public void Update(BlEichudModel item)
        {
            dal.Update(EichudMapper.ToEntity(item));
        }

       
    }
}
