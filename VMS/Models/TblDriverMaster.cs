using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDriverMaster
{
    public int Id { get; set; }

    public string DriverName { get; set; } = null!;

    public string FatherName { get; set; } = null!;

    public string DriverAddress1 { get; set; } = null!;

    public string? DriverAddress2 { get; set; }

    public string? DriverAddress3 { get; set; }

    public int CityId { get; set; }

    public int DistrictId { get; set; }

    public int StateId { get; set; }

    public string PinCode { get; set; } = null!;

    public string? DriverPhoto { get; set; }

    public string AadharNo { get; set; } = null!;

    public string? AadharNoImage { get; set; }

    public string PanNo { get; set; } = null!;

    public string? PanNoImage { get; set; }

    public string BankName { get; set; } = null!;

    public string BankAccountNumber { get; set; } = null!;

    public string BankIfsccode { get; set; } = null!;

    public string DrivingLicenceNo { get; set; } = null!;

    public string DrivingLicenceIssueAuth { get; set; } = null!;

    public DateTime DrivingLicenceValidity { get; set; }

    public string? DrivingLicenceImage { get; set; }

    public string MobileNumber1 { get; set; } = null!;

    public string? MobileNumber2 { get; set; }

    public bool IsExistingReference { get; set; }

    public string? ReferenceName { get; set; }

    public string? ReferenceAddress1 { get; set; }

    public string? ReferenceAddress2 { get; set; }

    public string? ReferenceAddress3 { get; set; }

    public string? ReferenceCity { get; set; }

    public string? ReferencePin { get; set; }

    public string? ReferenceMobile { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public string? OldFirm { get; set; }
    public string? Remark { get; set; }

    public virtual TblCity City { get; set; } = null!;

    public virtual TblDistrict District { get; set; } = null!;

    public virtual TblState State { get; set; } = null!;

    public virtual ICollection<TblDieselHeader> TblDieselHeaders { get; set; } = new List<TblDieselHeader>();

    public virtual ICollection<TblDriverHisabHeader> TblDriverHisabHeaders { get; set; } = new List<TblDriverHisabHeader>();

    public virtual ICollection<TblDriverHisabLine> TblDriverHisabLines { get; set; } = new List<TblDriverHisabLine>();
}
