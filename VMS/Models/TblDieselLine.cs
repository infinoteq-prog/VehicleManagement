using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblDieselLine
{
    public int Id { get; set; }

    public int TripId { get; set; }

    public long RouteId { get; set; }

    public string RouteDesc { get; set; } = null!;

    public string LoadType { get; set; } = null!;
    public decimal Average { get; set; } = 0!;
    public decimal Distance { get; set; } = 0!;
    public decimal EstimatedDiesel { get; set; } = 0;

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }

    public virtual TblDieselHeader Trip { get; set; } = null!;
}
