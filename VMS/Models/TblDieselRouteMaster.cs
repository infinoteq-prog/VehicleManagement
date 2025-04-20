using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDieselRouteMaster
{
    public int Id { get; set; }

    public string RouteDescription { get; set; } = null!;

    public int Distance { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }
}
