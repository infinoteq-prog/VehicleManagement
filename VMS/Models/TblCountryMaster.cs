using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblCountryMaster
{
    public int Id { get; set; }

    public string CountryName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual ICollection<TblState> TblStates { get; set; } = new List<TblState>();
}
