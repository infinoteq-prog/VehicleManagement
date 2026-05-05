using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMDriverProfile : BaseModel
    {
            public int ProfileID { get; set; }

            public int DriverID { get; set; }
           public string DriverName { get; set; }

           public int VehicleID { get; set; }

            public string? VehicleNo { get; set; }
        public int OffenceId { get; set; }

        public string? Offence { get; set; }

            public decimal? OffencePoint { get; set; }

            public decimal? HoldAmt { get; set; }

            public string? CreatedBy { get; set; }

            public DateTime? CreationDate { get; set; }

            public string? UpdatedBy { get; set; }

            public DateTime? UpdateDate { get; set; }
            public string? Remarks { get; set; }
            public DateTime? ProfileDate { get; set; }
    }
    public class OffenceDropdownVM
    {
        public int OffenceId { get; set; }
        public string OffenceName { get; set; }
        public string OffencePoint { get; set; }
    }
}
