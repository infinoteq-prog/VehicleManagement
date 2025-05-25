using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblModelMaster
{
    public int Id { get; set; }

    public int MakeId { get; set; }

    public string ModelNo { get; set; }

    public int NoOfTyres { get; set; }

    public string VehicleType { get; set; } = null!;

    public string FuleType { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblMakeMaster Make { get; set; } = null!;

    public virtual ICollection<TblAssetMaster> TblAssetMasters { get; set; } = new List<TblAssetMaster>();
}
