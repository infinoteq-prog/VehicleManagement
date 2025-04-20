using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMGstRateMaster : BaseModel
    {
        public VMGstRateMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }
        public int Id { get; set; }

        public decimal SgstRate { get; set; }

        public decimal CgstRate { get; set; }

        public decimal IgstRate { get; set; }

        public decimal UgstRate { get; set; }

        public DateTime EffectiveDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string EffectiveDateString { get; set; }

        public string? EndDateString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }
    }
}
