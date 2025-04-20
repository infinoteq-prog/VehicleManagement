using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;
using Microsoft.AspNetCore.Http.HttpResults;

namespace VMS.ViewModel
{
    public class VMFunctionMaster : BaseModel
    {
        public VMFunctionMaster()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public string Id { get; set; } = null!;

        public string FunctionName { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string StartDateString { get; set; }

        public string? EndDateString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public String CreatedBy { get; set; }

        public String UpdateBy { get; set; }

        //public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

        public virtual ICollection<TblUserMaster> Users { get; set; } = new List<TblUserMaster>();
    }
}