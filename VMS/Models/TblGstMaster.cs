using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblGstMaster
{
    public int Id { get; set; }

    public string TransporterCode { get; set; } = null!;

    public DateTime EffectiveDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal SgstRate { get; set; }

    public decimal CgstRate { get; set; }

    public decimal IgstRate { get; set; }

    public decimal UtgstRate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public bool IsRcm { get; set; }
}
