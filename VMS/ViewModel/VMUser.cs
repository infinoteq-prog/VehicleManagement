using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMUser : BaseModel
    {
        public VMUser()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public string Id { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string? UserId { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Password { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public string StartDateString { get; set; }

        public string? EndDateString { get; set; }

        //public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

        //public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

        //public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

        public virtual ICollection<TblRoleMaster> Roles { get; set; } = new List<TblRoleMaster>();

        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public String CreatedBy { get; set; }

        public String UpdateBy { get; set; }

        public String RoleName { get; set; }
    }
}
