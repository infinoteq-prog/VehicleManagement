using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VMS.Models;

public partial class TblServiceDueMaster
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public DateTime PurchaseDate { get; set; } 
    public string ServiceCode { get; set; }
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
