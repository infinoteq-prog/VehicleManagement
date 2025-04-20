using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDriverHisabLine
{
    public int Sno { get; set; }

    public int SettlementNo { get; set; }

    public int DriverId { get; set; }

    public string ExpenseCode { get; set; } = null!;

    public string ExpenseType { get; set; } = null!;

    public decimal DrAmt { get; set; }

    public decimal CrAmt { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblDriverMaster Driver { get; set; } = null!;

    public virtual TblDriverHisabHeader SettlementNoNavigation { get; set; } = null!;
}
