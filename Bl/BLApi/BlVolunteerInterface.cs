using Bl.BLModels;
using Dal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.BLApi
{
    public interface BlVolunteerInterface:BlICrud<BLVolunteerModel>
    {
        Task UpdateVolunteerDomains(BLVolunteerModel dto);
    }
}
