using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDriverHisabHeader
{
    public int SettlementNo { get; set; }
    public int LastSettlementId { get; set; }

    public int DriverId { get; set; }

    public int VehicleNo { get; set; }

    public DateTime SettlementDate { get; set; }

    public string RouteDescription { get; set; } = null!;

    public DateTime TripStartDate { get; set; }

    public DateTime TripEndDate { get; set; }

    public decimal OpeningBalance { get; set; }
    public decimal ClosingBalance { get; set; }

    public decimal Weight { get; set; }
    public string Remarks { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblDriverMaster Driver { get; set; } = null!;

    public virtual ICollection<TblDriverHisabLine> TblDriverHisabLines { get; set; } = new List<TblDriverHisabLine>();

    public virtual TblVehicleMaster VehicleNoNavigation { get; set; } = null!;
}
