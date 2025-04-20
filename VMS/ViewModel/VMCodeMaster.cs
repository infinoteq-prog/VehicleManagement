using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMCodeMaster : BaseModel
    {
        public VMCodeMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }
        public int Id { get; set; }

        public string CodeType { get; set; } = null!;

        public string Code { get; set; } = null!;

        public string Description { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }
    }
}
