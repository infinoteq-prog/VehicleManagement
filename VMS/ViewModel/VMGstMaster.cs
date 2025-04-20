using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMGstMaster : BaseModel
    {
        public VMGstMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }
        public int Id { get; set; }

        public string TransporterName { get; set; }

        public string TransporterCode { get; set; } = null!;

        public int TransporterId { get; set; }

        public int UserId { get; set; }

        public DateTime EffectiveDate { get; set; }

        public string EffectiveDateString { get; set; }

        public DateTime? EndDate { get; set; }

        public string EndDateString { get; set; }

        public decimal SgstRate { get; set; }

        public decimal CgstRate { get; set; }

        public decimal IgstRate { get; set; }

        public decimal UgstRate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdatedBy { get; set; }

        public bool IsRcm { get; set; }
    }
}
