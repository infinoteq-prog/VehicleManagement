using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDistrict
{
    public int Id { get; set; }

    public string DistrictName { get; set; } = null!;

    public int? StateId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblState? State { get; set; }

    public virtual ICollection<TblBillToMaster> TblBillToMasters { get; set; } = new List<TblBillToMaster>();

    public virtual ICollection<TblCity> TblCities { get; set; } = new List<TblCity>();

    public virtual ICollection<TblDriverMaster> TblDriverMasters { get; set; } = new List<TblDriverMaster>();

    public virtual ICollection<TblTransporterMaster> TblTransporterMasters { get; set; } = new List<TblTransporterMaster>();
}
