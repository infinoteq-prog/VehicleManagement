using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMUserRoleAccess : BaseModel
    {
        public VMUserRoleAccess()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public Int32 Id { get; set; }

        public Int32 UserId { get; set; }

        public Int32 RoleId { get; set; }

        public Int32 FunctionId { get; set; }

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

        public string UserName { get; set; }

        public string RoleName { get; set; }

        public string FunctionMasterName { get; set; }

        //public virtual ICollection<AspNetRoleClaim> AspNetRoleClaims { get; set; } = new List<AspNetRoleClaim>();

        public virtual ICollection<TblUserMaster> Users { get; set; } = new List<TblUserMaster>();

        public virtual ICollection<TblRoleMaster> Roles { get; set; } = new List<TblRoleMaster>();

        public virtual ICollection<TblFunctionMaster> Functions { get; set; } = new List<TblFunctionMaster>();
    }
}