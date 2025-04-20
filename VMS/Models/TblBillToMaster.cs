using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblBillToMaster
{
    public int Id { get; set; }

    public string BillToCode { get; set; } = null!;

    public string BillToCompany { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public int CityId { get; set; }

    public string? PinCode { get; set; }

    public string GstinNo { get; set; } = null!;

    public string PanNo { get; set; } = null!;

    public string? MobileNo { get; set; }

    public string? EmailId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblCity City { get; set; } = null!;

    public virtual TblDistrict District { get; set; } = null!;

    public virtual TblState State { get; set; } = null!;
}
