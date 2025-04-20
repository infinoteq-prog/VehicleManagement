using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMCompanyMaster : BaseModel
    {
        public VMCompanyMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public Int32 Id { get; set; }

        public string BillToCode { get; set; }

        public string BillToCompanyName { get; set; }

        public string Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? Address3 { get; set; }

        public Int32 StateId { get; set; }

        public Int32 DistrictId { get; set; }

        public Int32 CityId { get; set; }

        public string? PinCode { get; set; }

        public string GSTINNo { get; set; }

        public string PanNo { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public String CreatedBy { get; set; }

        public String UpdateBy { get; set; }

        public string? StateName { get; set; }

        public string? DistrictName { get; set; }

        public string? CityName { get; set; }

        public string? StateCode { get; set; }

        //public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

        public virtual ICollection<TblBillToMaster> BillToMaster { get; set; } = new List<TblBillToMaster>();
    }
}
