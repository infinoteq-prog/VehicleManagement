using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblTransporterMaster
{
    public int Id { get; set; }

    public string TransporterCode { get; set; } = null!;

    public string TransporterName { get; set; } = null!;

    public string OwnerName { get; set; } = null!;

    public string MobileNumber { get; set; } = null!;

    public string EmailId { get; set; } = null!;

    public string Address1 { get; set; } = null!;

    public string? Address2 { get; set; }

    public string? Address3 { get; set; }

    public int StateId { get; set; }

    public int DistrictId { get; set; }

    public int CityId { get; set; }

    public string? PinCode { get; set; }

    public string GstinNo { get; set; } = null!;

    public string PanNo { get; set; } = null!;

    public string? PanNoImage { get; set; }

    public int UserId { get; set; }

    public string BillPrefix { get; set; } = null!;

    public string? BillStartNo { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblCity City { get; set; } = null!;

    public virtual TblDistrict District { get; set; } = null!;

    public virtual TblState State { get; set; } = null!;

    public virtual TblUserMaster User { get; set; } = null!;
}
