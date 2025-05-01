using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDieselHeader
{
    public int TripId { get; set; }

    public int VehicleNo { get; set; }

    public int DriverId { get; set; }

    public string LastTripRouteDescr { get; set; } = null!;

    public DateTime TripStartDate { get; set; }

    public DateTime TripEndDate { get; set; }

    public long StartOdometer { get; set; }

    public long EndOdometer { get; set; }

    public long OpeningDiesel { get; set; }
    public long ClosingDiesel { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public int? RunningKm { get; set; }
    public int? ApprovedBy { get; set; }
    public DateTime? ApprovedDate { get; set; }

    public virtual TblDriverMaster Driver { get; set; } = null!;

    public virtual ICollection<TblDieselFilling> TblDieselFillings { get; set; } = new List<TblDieselFilling>();

    public virtual ICollection<TblDieselLine> TblDieselLines { get; set; } = new List<TblDieselLine>();

    public virtual TblVehicleMaster VehicleNoNavigation { get; set; } = null!;
}
