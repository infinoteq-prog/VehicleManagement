using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblCity
{
    public int Id { get; set; }

    public string CityName { get; set; } = null!;

    public int? DistrictId { get; set; }

    public int? StateId { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual TblDistrict? District { get; set; }

    public virtual TblState? State { get; set; }

    public virtual ICollection<TblBillToMaster> TblBillToMasters { get; set; } = new List<TblBillToMaster>();

    public virtual ICollection<TblDriverMaster> TblDriverMasters { get; set; } = new List<TblDriverMaster>();

    public virtual ICollection<TblTransporterMaster> TblTransporterMasters { get; set; } = new List<TblTransporterMaster>();
}
