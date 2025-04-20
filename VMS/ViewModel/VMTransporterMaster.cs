using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;
namespace VMS.ViewModel
{
    public class VMTransporterMaster : BaseModel
    {
        public VMTransporterMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public Int32 Id { get; set; }

        public string TransporterCode { get; set; }

        public string TransporterName { get; set; }

        public string OwnerName { get; set; }

        public string MobileNo { get; set; }

        public string EmailID { get; set; }

        public string Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Address3 { get; set; }

        public Int32 StateId { get; set; }

        public Int32 DistrictId { get; set; }

        public Int32 CityId { get; set; }

        public string? PinCode { get; set; }

        public string GSTINNo { get; set; }

        public string PanNo { get; set; }

        public Int32 UserId { get; set; }

        public string BillPrifix { get; set; }

        //public DateTime StartDate { get; set; }

        //public DateTime EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public String CreatedBy { get; set; }

        public String UpdateBy { get; set; }


        public string? UserName { get; set; }

        public string? StateName { get; set; }

        public string? DistrictName { get; set; }

        public string? CityName { get; set; }

        public bool? IsUnionTerritory { get; set; }

        public string? StateCode { get; set; }

        public VMGstMaster GstMaster { get; set; }

        //public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

        public virtual ICollection<TblTransporterMaster> TransporterMaster { get; set; } = new List<TblTransporterMaster>();
    }
}
