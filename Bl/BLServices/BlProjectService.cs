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
    public class BlProjectService : BlProjectInterface
    {
        private DalProjectInterface Project;
        public BlProjectService(DalProjectInterface dal)
        {
            this.Project = dal;
        }


        private Project Convert(BlProjectModel s)
        {
            return new Project()
            {
                ProjectCode = s.ProjectCode,
                ProjectName = s.ProjectName,
                ProjectManagerCode = s.ProjectManagerCode,
                DomainCode = s.DomainCode
             
    };
        }
        private BlProjectModel Convert(Project s)
        {
            return new BlProjectModel()
            {

                ProjectCode = s.ProjectCode,
                ProjectName = s.ProjectName,
                ProjectManagerCode = s.ProjectManagerCode,
                DomainCode = s.DomainCode                
            };
        }
        private List<BlProjectModel> Convert(List<Project> c)
        {
            List<BlProjectModel> list = new List<BlProjectModel>();
            foreach (var item in c)
            {
                list.Add(Convert(item));
            }
            return list;
        }

        public void Create(BlProjectModel item)
        {
            Project.Create(Convert(item));
        }

        public void Delete(BlProjectModel item)
        {
            Project.Delete(Convert(item));
        }
        public void Update(BlProjectModel item)
        {
            Project.Update(Convert(item));
        }

        public async Task<BlProjectModel> Read(int id)
        {
            try { return Convert(Project.Read(id).Result); }
            catch (ObjectNotFoundException e)
            {
                return null;
            }
        }

        public async Task<List<BlProjectModel>> Read(Func<BlProjectModel, bool> func)
        {
            List<BlProjectModel> list = Convert(Project.Read((Func<Project, bool>)func).Result);
            return list;
        }

        public async Task<List<BlProjectModel>> ReadAll()
        {
            List<BlProjectModel> list = new List<BlProjectModel>();

            Project.ReadAll().Result.ForEach(item => { list.Add(Convert(item)); });

            return list;
        }

       
    }
}
