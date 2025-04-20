using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblTransporterBill
{
    public long Id { get; set; }

    public string BillNumber { get; set; } = null!;

    public DateTime BillDate { get; set; }

    public int TransporterId { get; set; }

    public int CompanyId { get; set; }

    public decimal? SgstAmount { get; set; }

    public decimal? CgstAmount { get; set; }

    public decimal? IgstAmount { get; set; }

    public decimal? UgstAmount { get; set; }

    public decimal TotalBillAmount { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }
}
