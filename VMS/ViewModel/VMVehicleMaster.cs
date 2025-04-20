using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMVehicleMaster : BaseModel
    {
        public VMVehicleMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public int Id { get; set; }

        public string VehicleNo { get; set; } = null!;

        public string VehicleOwner { get; set; } = null!;

        public string PurchaseDate { get; set; }

        public int? MfgYear { get; set; }

        public int MakeId { get; set; }

        public int ModelId { get; set; }

        public int? NoOfTyres { get; set; }

        public string? EngineNo { get; set; }

        public string? ChasisNo { get; set; }

        public int VehicleTypeId { get; set; }

        public int BodyTypeId { get; set; }

        public string? FinancerName { get; set; }

        public int BodyManufacturerId { get; set; }

        public int? RunningKm { get; set; }

        public string? PanNo { get; set; }

        public string? PanNoImage { get; set; }

        public string? InsuranceDue { get; set; }

        public string? InsuranceDoc { get; set; }

        public string? NationalPermitDue { get; set; }

        public string? NationalPermitDoc { get; set; }

        public string? LocalPermitDue { get; set; }

        public string? LocalPermitDoc { get; set; }

        public string? RcValidityDue { get; set; }

        public string? RcDoc { get; set; }

        public string? RtoDue { get; set; }

        public string? RtoDoc { get; set; }

        public string? PollutionDue { get; set; }

        public string? PollutionDoc { get; set; }

        public string? FitnessDue { get; set; }

        public string? FitnessDoc { get; set; }

        public string Dimension { get; set; } = null!;

        public string? BranchOffice { get; set; }

        public string? RcCapicity { get; set; }

        public string? ActualCapicity { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string MakeName { get; set; }

        public string ModelName { get; set; }

        public string VehicleTypeName { get; set; }

        public string BodyTypeName { get; set; }

        public string BodyManufacturerName { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }
    }
}
