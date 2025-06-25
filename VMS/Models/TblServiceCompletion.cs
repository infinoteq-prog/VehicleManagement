using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblServiceCompletion
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime ServiceDate { get; set; }
    public int KmReadingOnService { get; set; }
    public int IntervalKm { get; set; }
    public int IntervalMonth { get; set; }
    public DateTime DueDate { get; set; }
    public decimal PartCost { get; set; }
    public decimal LabourCost { get; set; }
    public decimal TotalCost { get; set; }
    public string Workshop { get; set; }
    public string Remarks { get; set; }
    public bool IsActive { get; set; }
    public DateTime? CreationDate { get; set; }
    public int? CreatedBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public int? UpdatedBy { get; set; }
}
