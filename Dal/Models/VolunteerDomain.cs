using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dal.Models;

public partial class VolunteerDomain
{
    
    public int VolunteerDomainsCode { get; set; }

    public int VolunteerCode { get; set; }

    public int ProjectCode { get; set; }

    public virtual Project ProjectCodeNavigation { get; set; } = null!;

    public virtual Volunteer VolunteerCodeNavigation { get; set; } = null!;
}
