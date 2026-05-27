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
    public class BlVolunteeringService:BlVolunteeringInterface
    {
        DalVolunteeringInterface dal;
        BlVolunteerInterface volunteerInterface;
        BlProjectInterface projectInterface;
        BlSubProjectInterface subProjectInterface;
        public BlVolunteeringService(IDal dal,BlVolunteerInterface volunteerInterface,BlProjectInterface projectInterface,BlSubProjectInterface subProjectInterface )
        {
            this.dal = dal.Volunteerings;
            this.volunteerInterface = volunteerInterface;
            this.subProjectInterface = subProjectInterface;
            this.projectInterface = projectInterface;
        }
        private BlVolunteeringModel Convert(Volunteering v)
        {
           BlVolunteeringModel blV=  new BlVolunteeringModel()
            {
                VolunteeringCode = v.VolunteeringCode,
                DateOfVolunteering = v.DateOfVolunteering,
                MatcherCode = v.MatcherCode,
                //MatcherName = ((BLVolunteerService)volunteerInterface).Read(v.MatcherCode ?? 0).Result.VolunteerCodeNavigation.FirstName + " " + ((BLVolunteerService)volunteerInterface).Read(v.MatcherCode ?? 0).Result.VolunteerCodeNavigation.FamilyName,
                VolunteerCode = v.VolunteerCode,
                //VolunteerName = ((BLVolunteerService)volunteerInterface).Read(v.VolunteerCode ?? 0).Result.VolunteerCodeNavigation.FirstName + " " + ((BLVolunteerService)volunteerInterface).Read(v.VolunteerCode ?? 0).Result.VolunteerCodeNavigation.FamilyName,
                PoorManCode = v.PoorManCode,
                //PoorManName = ((BLVolunteerService)volunteerInterface).Read(v.PoorManCode ?? 0).Result.VolunteerCodeNavigation.FirstName + " " + ((BLVolunteerService)volunteerInterface).Read(v.PoorManCode ?? 0).Result.VolunteerCodeNavigation.FamilyName,
                ProjectCode = v.ProjectCode,
                SubProjectCode = v.SubProjectCode,
                
            };
           if( v.MatcherCode != null)
            {


              blV.MatcherName=  ((BLVolunteerService)volunteerInterface).Read(v.MatcherCode ?? 0).Result.VolunteerCodeNavigation.FirstName + " " + ((BLVolunteerService)volunteerInterface).Read(v.MatcherCode ?? 0).Result.VolunteerCodeNavigation.FamilyName;
              blV.VolunteerName =  ((BLVolunteerService)volunteerInterface).Read(v.VolunteerCode ?? 0).Result.VolunteerCodeNavigation.FirstName + " " + ((BLVolunteerService)volunteerInterface).Read(v.VolunteerCode ?? 0).Result.VolunteerCodeNavigation.FamilyName;
              blV.PoorManName = ((BLVolunteerService)volunteerInterface).Read(v.PoorManCode ?? 0).Result.VolunteerCodeNavigation.FirstName + " " + ((BLVolunteerService)volunteerInterface).Read(v.PoorManCode ?? 0).Result.VolunteerCodeNavigation.FamilyName;
              
            }
           if(v.ProjectCode != null)
                blV.ProjectName = ((BlProjectService)projectInterface).Read(v.ProjectCode ?? 0).Result.ProjectName;
           if(v.SubProjectCode != null)
                
                blV.SubProjectName = ((BlSubProjectService)subProjectInterface).Read(v.SubProjectCode ?? 0).Result.SubProjectName;
            return blV;
            
              

             
        }
        private List<BlVolunteeringModel> Convert(List<Volunteering> v)
        {
            List<BlVolunteeringModel> list = new List<BlVolunteeringModel>();
            foreach (var item in v)
            {
                list.Add(Convert(item));
            }
            return list;
        }
        private Volunteering Convert(BlVolunteeringModel v)
        {
            return new Volunteering() {
                DateOfVolunteering = v.DateOfVolunteering,
                MatcherCode = v.MatcherCode ,
                VolunteerCode = v.VolunteerCode,
                PoorManCode=v.PoorManCode,
                ProjectCode=v.ProjectCode,
                SubProjectCode=v.SubProjectCode,
                
                  };
        }

        public void Create(BlVolunteeringModel item)
        {
            dal.Create(Convert(item));
        }

        public void Delete(BlVolunteeringModel item)
        {
            dal.Delete(Convert(item));
        }


        public async Task<List<BlVolunteeringModel>> ReadAll()
        {
            List<BlVolunteeringModel> list = new List<BlVolunteeringModel>();

            dal.ReadAll().Result.ForEach(item => { list.Add(Convert(item)); });

            return list;
        }

        public void Update(BlVolunteeringModel item)
        {
            dal.Update(Convert(item));

        }
        public async Task<List<BlVolunteeringModel>> Read(Func<BlVolunteeringModel, bool> func)
        {
            List<BlVolunteeringModel> list = Convert(dal.Read((Func<Volunteering, bool>)func).Result);
            return list;

        }

        public async Task<BlVolunteeringModel> Read(int id)
        {
           //BLVolunteeringModel model=Convert(dal.ReadAll().Result.Find(v=>v.VolunteerCode==id));
           //return Convert(dal.Read(id).Result);

            try { return Convert(dal.Read(id).Result); }
            catch (ObjectNotFoundException e)
            {
                return null;
            }
        }

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        
    }
}

