using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.ViewModel;
using VMS.Helper;
using VMS.Models;
namespace VMS.ViewModel
{
    public class VMDispatchDetails : BaseModel
    {
        public VMDispatchDetails()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        public long Id { get; set; }

        public string DispatchUniqueId { get; set; } = null!;

        public string? SupplyingPlant { get; set; }

        public string? ShipmentNo { get; set; }

        public string? DeliveryNo { get; set; }

        public string? TruckNo { get; set; }

        public string? LrGrNo { get; set; }

        public string? LrGrDate { get; set; }

        public string? ShipToPartyTzone { get; set; }

        public string? DispatchQtyRoad { get; set; }

        public string? EbidNetAmt { get; set; }

        public string? EbidFrtRate { get; set; }

        public string? ForwardingAgentCode { get; set; }

        public string? ForwardingAgentName { get; set; }

        public string? RegionState { get; set; }

        public string? PgiNo { get; set; }

        public string PgiDate { get; set; }

        public string? DistributionChannel { get; set; }

        public string? Division { get; set; }

        public string? IncoTerm { get; set; }

        public string? RouteCode { get; set; }

        public string? RouteDescription { get; set; }

        public string? EpodNo { get; set; }

        public string? EpodDate { get; set; }

        public string? BillNo { get; set; }

        public string? BillDate { get; set; }

        public string? CgstRate { get; set; }

        public string? SgstRate { get; set; }

        public string? IgstRate { get; set; }

        public string? UtgstRate { get; set; }

        public string? TotalAmount { get; set; }

        public bool? IsActive { get; set; }

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }
        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }

        public VMTransporterBill TransporterBill { get; set; }

        public VMCompanyMaster CompanyMaster { get; set; }

        public VMTransporterMaster TransporterMaster { get; set; }

        public string ExceptionalEntry { get; set; }

        public Boolean isUserAdmin { get; set; }

        public string HsnCode { get; set; }

        public string? Freight_Road { get; set; }
    }
}
