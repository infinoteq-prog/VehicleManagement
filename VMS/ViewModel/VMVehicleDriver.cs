using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;
namespace VMS.ViewModel
{
    public class VMVehicleDriver : BaseModel
    {
        public VMVehicleDriver()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public int Id { get; set; }

        public int DriverId { get; set; }

        public int VehicleId { get; set; }

        public DateTime LinkDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public string? ReasonOfDlink { get; set; } = null!;

        public string DriverName { get; set; }

        public string VehicleName { get; set; }


        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string CreatedByName { get; set; }

        public string UpdatedByName { get; set; }
    }
}
