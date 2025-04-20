using System;
using System.Collections.Generic;

namespace VMS.Models;

public partial class TblTempDispatchDetail
{
    public int Id { get; set; }

    public string DispatchUniqueId { get; set; } = null!;

    public string? SupplyingPlant { get; set; }

    public string? ShipmentNo { get; set; }

    public string? DeliveryNo { get; set; }

    public string? TruckNo { get; set; }

    public string? LrGrNo { get; set; }

    public DateTime? LrGrDate { get; set; }

    public string? ShipToPartyTzone { get; set; }

    public long? DispatchQtyRoad { get; set; }

    public decimal? EbidNetAmt { get; set; }

    public decimal? EbidFrtRate { get; set; }

    public string? ForwardingAgentCode { get; set; }

    public string? ForwardingAgentName { get; set; }

    public string? RegionState { get; set; }

    public string? PgiNo { get; set; }

    public DateTime? PgiDate { get; set; }

    public string? DistributionChannel { get; set; }

    public string? Division { get; set; }

    public string? IncoTerm { get; set; }

    public string? RouteCode { get; set; }

    public string? RouteDescription { get; set; }

    public string? EpodNo { get; set; }

    public DateTime? EpodDate { get; set; }

    public string? BillNo { get; set; }

    public DateTime? BillDate { get; set; }

    public decimal? CgstRate { get; set; }

    public decimal? SgstRate { get; set; }

    public decimal? IgstRate { get; set; }

    public decimal? UtgstRate { get; set; }

    public decimal? TotalAmount { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreationDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime UpdateDate { get; set; }

    public int UpdatedBy { get; set; }
}
