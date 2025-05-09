using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblAssetMaster
{
    public int Id { get; set; }

    public string VehicleNo { get; set; } = null!;

    public string VehicleClassification { get; set; } = null!;

    public string VehicleOwner { get; set; } = null!;

    public DateTime? PurchaseDate { get; set; }

    public string? PanNo { get; set; }

    public string? PanNoImage { get; set; }

    public string? Address { get; set; }

    public string? MfgYear { get; set; }

    public int? MakeId { get; set; }

    public int? ModelId { get; set; }

    public int? NoOfTyres { get; set; }

    public string? EngineNo { get; set; }

    public string? ChasisNo { get; set; }

    public string? VehicleType { get; set; }

    public string? BodyType { get; set; }

    public string? FinancerName { get; set; }

    public string? BodyManufacturer { get; set; }

    public int? RunningKm { get; set; }

    public DateTime? InsuranceDue { get; set; }

    public string? InsuranceDoc { get; set; }

    public DateTime? NationalPermitDue { get; set; }

    public string? NationalPermitDoc { get; set; }

    public DateTime? LocalPermitDue { get; set; }

    public string? LocalPermitDoc { get; set; }

    public DateTime? RcValidityDue { get; set; }

    public string? RcDoc { get; set; }

    public DateTime? RtoDue { get; set; }

    public string? RtoDoc { get; set; }

    public DateTime? PollutionDue { get; set; }

    public string? PollutionDoc { get; set; }

    public DateTime? FitnessDue { get; set; }

    public string? FitnessDoc { get; set; }

    public string? Amc { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblMakeMaster? Make { get; set; }

    public virtual TblModelMaster? Model { get; set; }
}
