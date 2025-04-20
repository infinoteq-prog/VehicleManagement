using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMUserSubscription : BaseModel
    {
        public VMUserSubscription()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }

        public string FinYear { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidDate { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string PaidDateString { get; set; }

        public string StartDateString { get; set; }

        public string? EndDateString { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public String CreatedBy { get; set; }

        public String UpdateBy { get; set; }

        public string UserName { get; set; }

        //public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

        public virtual ICollection<TblUserMaster> Users { get; set; } = new List<TblUserMaster>();
    }
}
