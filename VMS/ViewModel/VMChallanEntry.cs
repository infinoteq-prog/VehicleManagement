using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMChallanEntry : BaseModel
    {
        public int VehicleId { get; set; }
        public string VehicleNo { get; set; } = null!;
        public DateTime? ChallanEntryDate { get; set; }
        public DateTime? ChallanDate { get; set; }
        public string ChallanNo { get; set; } = null!;
        public int DriverId { get; set; }
        public decimal Amount { get; set; }
        public string Remarks { get; set; }
        public string GrievancesTicket { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}
