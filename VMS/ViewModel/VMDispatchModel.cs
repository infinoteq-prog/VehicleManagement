using System;
using System.Collections.Generic;
using VMS.Models;

namespace VMS.ViewModel
{
    public class VMDispatchModel
    {
        public List<VMDispatchErrorModel>? DispatchErrorModel { get; set; }

        public string? UniqueDispatchId { get; set; }

        public string? DisptachErrorFilePath { get; set; }

        public string? LrGrNoUniqueId { get; set; }
        
    }
}
