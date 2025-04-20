using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblMakeMaster
{
    public int Id { get; set; }

    public string Make { get; set; } = null!;

    public string MakeName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual ICollection<TblAssetMaster> TblAssetMasters { get; set; } = new List<TblAssetMaster>();

    public virtual ICollection<TblModelMaster> TblModelMasters { get; set; } = new List<TblModelMaster>();
}
