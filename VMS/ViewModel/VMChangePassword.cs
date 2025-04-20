using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMChangePassword : BaseModel
    {
        public VMChangePassword()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public string userName { get; set; } = null!;

        public string Password { get; set; }
    }
}
