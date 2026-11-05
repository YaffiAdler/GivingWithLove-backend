using System;
using System.Collections.Generic;

namespace Dal.Models;

public partial class Position
{
    public int PositionCode { get; set; }

    public string PositionName { get; set; } = null!;

    public virtual ICollection<Volunteer> Volunteers { get; set; } = new List<Volunteer>();
}
