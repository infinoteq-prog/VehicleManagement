using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.ViewModel
{
    public class VMDispatchEntry : BaseModel
    {
        public VMDispatchEntry()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;

            VehicleList = new List<SelectListItem>();
            CustomerList = new List<SelectListItem>();
            DriverList = new List<SelectListItem>();
            StationFromList = new List<SelectListItem>();
            StationToList = new List<SelectListItem>();
            MaterialList = new List<SelectListItem>();
            BillTypeList = new List<SelectListItem>();
            OwnOtherList = new List<SelectListItem>();
            OwnOtherGTAList = new List<SelectListItem>();
        }

        public int Id { get; set; }

        /* ---------- Basic Details ---------- */

        public int VehicleId { get; set; }
        public string VehicleNo { get; set; } = null!;

        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = null!;

        public DateTime? LoadingDate { get; set; }
        public string LRNo { get; set; }

        public string @OwnOtherType { get; set; }
        public int OwnOtherGTAId { get; set; }

        public int BillTypeId { get; set; }
        public string BillType { get; set; }
        public string BillNo { get; set; }

        public DateTime? BillDate { get; set; }

        public int DriverId { get; set; }

        /* ---------- Station & Material ---------- */

        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public string Material { get; set; }

        /* ---------- Weight Details ---------- */

        public decimal LoadWeight { get; set; }
        public decimal UnloadWeight { get; set; }
        public decimal Shortage { get; set; }

        /* ---------- Freight Details ---------- */

        public decimal FreightRate { get; set; }
        public decimal TotalFreight { get; set; }
        public decimal Deduction { get; set; }
        public decimal FreightForTally { get; set; }

        /* ---------- Eway Bill ---------- */

        public string EwayBillNo { get; set; }
        public string EwayBillExpiry { get; set; }

        public string Status { get; set; }
        /* ---------- Dropdown Lists ---------- */

        public List<SelectListItem> VehicleList { get; set; }
        public List<SelectListItem> CustomerList { get; set; }
        public List<SelectListItem> DriverList { get; set; }
        public List<SelectListItem> StationFromList { get; set; }
        public List<SelectListItem> StationToList { get; set; }
        public List<SelectListItem> MaterialList { get; set; }
        public List<SelectListItem> BillTypeList { get; set; }
        public List<SelectListItem> OwnOtherList { get; set; }
        public List<SelectListItem> OwnOtherGTAList { get; set; }

        /* ---------- Audit ---------- */

        public bool IsActive { get; set; }

        public DateTime CreationDate { get; set; }
        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }
        public int UpdatedBy { get; set; }

        public string CreatedByName { get; set; }
        public string UpdatedByName { get; set; }
        public bool IsInlineUpdate { get; set; }
        public string Remarks { get; set; }
        /* Added below columns on 22-Feb-2026 by SKG */
        public string InvoiceNo { get; set; }
        public int ShipmentNo { get; set; }
        public int DeliveryNo { get; set; }
        public string TradeNT { get; set; }
        public string Other1 { get; set; }
        public string Other2 { get; set; }


    }

    public class VMDispatchEntryList
    {
        public int ID { get; set; }
        public string LRNo { get; set; }
        //public DateTime? LoadingDate { get; set; }
        public string LoadingDate { get; set; } // yyyy-MM-dd format
        public string VehicleNo { get; set; }
        public string Material { get; set; }
        public string FromStation { get; set; }
        public string ToStation { get; set; }
        public decimal? UnloadWeight { get; set; }
        public decimal? FreightRate { get; set; }
        public decimal? TotalFreight { get; set; }
        public decimal? FreightForTally { get; set; }
         public decimal? LoadWeight { get; set; }
    }

    public class CityDropdownVM
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
    public class BillTypeDropdownVM
    {
        public int BillTypeId { get; set; }
        public string BillTypeName { get; set; }
    }
    public class MaterailTypeDropdownVM
    {
        public int MateriaId { get; set; }
        public string MaterialName { get; set; }
    }

    public class VMDispatchReport
    {
        public string BillNo { get; set; }
        public string BillDate { get; set; }
        public string VehicleNo { get; set; }
        public string GRNo { get; set; }
        public string PartyName { get; set; }
        public string Material { get; set; }
        public string FromLocation { get; set; }
        public string ToLocation { get; set; }
        public decimal LoadWeight { get; set; }
        public decimal Freight { get; set; }
        public decimal Shortage { get; set; }
        public decimal TotalFreight { get; set; }
        public decimal Deduction { get; set; }
        public decimal Balance { get; set; }
        public decimal FreightForTally { get; set; }
        public decimal TDS { get; set; }
        public decimal Advance { get; set; }
        public DateTime AdvanceDate { get; set; }
        public DateTime BalanceDate { get; set; }
        public DateTime Date { get; set; }
        public string BiltyCopy { get; set; }

    }
}
