using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblState
{
    public int Id { get; set; }

    public int? CountryId { get; set; }

    public string StateName { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreationDate { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public int? UpdatedBy { get; set; }

    public bool? IsUnionTerritory { get; set; }

    public string? StateCode { get; set; }

    public virtual TblCountryMaster? Country { get; set; }

    public virtual ICollection<TblBillToMaster> TblBillToMasters { get; set; } = new List<TblBillToMaster>();

    public virtual ICollection<TblCity> TblCities { get; set; } = new List<TblCity>();

    public virtual ICollection<TblDistrict> TblDistricts { get; set; } = new List<TblDistrict>();

    public virtual ICollection<TblDriverMaster> TblDriverMasters { get; set; } = new List<TblDriverMaster>();

    public virtual ICollection<TblTransporterMaster> TblTransporterMasters { get; set; } = new List<TblTransporterMaster>();
}
