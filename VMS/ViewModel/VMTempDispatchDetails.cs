using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;
namespace VMS.ViewModel
{
    public class VMTempDispatchDetails : BaseModel
    {
        public VMTempDispatchDetails()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public long Id { get; set; }

        public string SupplyingPlant { get; set; } = null!;

        public long ShipmentNo { get; set; }

        public long DeliveryNo { get; set; }

        public string TruckNo { get; set; } = null!;

        public long? LrGrNo { get; set; }

        public DateTime? LrGrDate { get; set; }

        public string? ShipToPartyTzone { get; set; }

        public long? DispatchQtyRoad { get; set; }

        public long? EbidNetAmt { get; set; }

        public long? EbidFrtRate { get; set; }

        public string? ForwardingAgentCode { get; set; }

        public string? ForwardingAgentName { get; set; }

        public string? RegionState { get; set; }

        public string? PgiNo { get; set; }

        public DateTime? PgiDate { get; set; }

        public string? DistributionChannel { get; set; }

        public string? Division { get; set; }

        public string? IncoTerm { get; set; }

        public long RouteCode { get; set; }

        public long RouteDescription { get; set; }

        public string? EpodNo { get; set; }

        public DateTime EpodDate { get; set; }

        public string BillNo { get; set; } = null!;

        public DateTime BillDate { get; set; }

        public double? CgstRate { get; set; }

        public double? SgstRate { get; set; }

        public double? IgstRate { get; set; }

        public double? UtgstRate { get; set; }

        public double TotalAmount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }
    }
}
