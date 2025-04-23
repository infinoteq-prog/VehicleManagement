using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDieselFilling
{
    public int Id { get; set; }

    public int TripId { get; set; }
    public int VendorId { get; set; }

    public DateTime DieselFillingDate { get; set; }

    public long DieselQty { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblDieselHeader Trip { get; set; } = null!;
}
