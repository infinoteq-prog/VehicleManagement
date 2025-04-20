using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblModelAverageMaster
{
    public int Id { get; set; }

    public int MakeId { get; set; }

    public int ModelNo { get; set; }

    public decimal UlAvg { get; set; }

    public decimal MegaHw { get; set; }

    public decimal Khali { get; set; }

    public decimal Nh { get; set; }

    public decimal OffRoad { get; set; }

    public decimal OverLoad { get; set; }

    public decimal? Other { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblCodeMaster Make { get; set; } = null!;

    public virtual ICollection<TblVehicleMaster> TblVehicleMasters { get; set; } = new List<TblVehicleMaster>();
}
