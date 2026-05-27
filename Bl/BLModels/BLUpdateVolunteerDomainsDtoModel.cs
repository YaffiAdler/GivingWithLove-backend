using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bl.BLModels
{
    
        public class UpdateVolunteerDomainsDto
        {
            public int VolunteerCode { get; set; }
            public List<int> ProjectCodes { get; set; } = new();
        }
    
}
