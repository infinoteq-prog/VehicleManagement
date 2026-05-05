using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;
namespace VMS.ViewModel
{
    public class VMDriverHisab : BaseModel
    {
        public VMDriverHisab()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        //Driver HIsab Header Table Columns
        public int SettlementNo { get; set; }

        public int DriverId { get; set; }

        public int VehicleNo { get; set; }

        public DateTime SettlementDate { get; set; }

        public string RouteDescription { get; set; } = null!;

        public DateTime TripStartDate { get; set; }

        public DateTime TripEndDate { get; set; }

        public decimal OpeningBalance { get; set; }

        public decimal Weight { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

      //  public int TripDays { get; set; } // added on 3-Feb-2026
       // public decimal PnL { get; set; } // added on 3-Feb-2026

        public virtual TblDriverMaster Driver { get; set; } = null!;

        public virtual ICollection<TblDriverHisabLine> TblDriverHisabLines { get; set; } = new List<TblDriverHisabLine>();

        public virtual TblVehicleMaster VehicleNoNavigation { get; set; } = null!;
    }
}
