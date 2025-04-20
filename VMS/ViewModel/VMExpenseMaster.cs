using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMExpenseMaster : BaseModel
    {
        public VMExpenseMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public int Id { get; set; }

        public int ExpType { get; set; }

        public string ExpCode { get; set; } = null!;

        public string? ExpOther { get; set; }

        public string ExpDescription { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }

        public string ExpTypeName { get; set; }

    }
}
