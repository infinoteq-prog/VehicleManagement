using System;
using System.Collections.Generic;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMDispatchErrorModel
    {
        public int? Sno { get; set; }

        public string? ErrorType { get; set; }

        public string? ErrorReason { get; set; }

        public string? ErrorDescription { get; set; }
    }
}
