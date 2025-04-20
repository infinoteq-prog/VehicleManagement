using System;
using System.Collections.Generic;
using VMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using VMS.Helper;
using VMS.ViewModel;
namespace VMS.ViewModel
{
    public class VMDieselHisab : BaseModel
    {
        public VMDieselHisab()
        {
            TransactionMessage = new TransactionMessageVM();
            CreatedOn = utilityHelper.CurrentDateTime;
            UpdatedOn = utilityHelper.CurrentDateTime;
        }

        //Diesel Header Table Columns
        public int TripId { get; set; }

        public int VehicleNo { get; set; }

        public int DriverId { get; set; }

        public string LastTripRouteDescr { get; set; } = null!;

        public string TripStartDate { get; set; }

        public string TripEndDate { get; set; }

        public long StartOdometer { get; set; }

        public long EndOdometer { get; set; }

        public long OpeningDiesel { get; set; }

        public bool IsActive { get; set; }

        public DateTime DieselHeaderCreationDate { get; set; }

        public int DieselHeaderCreatedBy { get; set; }

        public DateTime DieselHeaderUpdateDate { get; set; }

        public int DieselHeaderUpdatedBy { get; set; }

        public string VehicleNumber { get; set; }

        public string DriverName { get; set; }

        public string DriverFatherName { get; set; }

        public string DieselHeaderCreatedByName { get; set; }

        public string DieselHeaderUpdatedByName { get; set; }

        //Trip Details
        public string LastTripStartDate { get; set; }

        public string LastTripEndDate { get; set; }

        public string LastTripRouteLine { get; set; }

        public string LastTripVendor{ get; set; }

        public string LastTripDriver { get; set; }

        public string LastTripDriverFatherName { get; set; }

        //Diesel Filling Table Columns

        public int DieselFillingId { get; set; }

        public int DieselHeaderForFilling { get; set; }

        public DateTime DieselFillingDate { get; set; }

        public long DieselQty { get; set; }

        public DateTime DieselFillingCreationDate { get; set; }

        public int DieselFillingCreatedBy { get; set; }

        public DateTime DieselFillingUpdateDate { get; set; }

        public int DieselFillingUpdatedBy { get; set; }

        public string DieselHeaderForFillingName { get; set; }

        public string DieselFillingCreatedByName { get; set; }

        public string DieselFillingUpdatedByName { get; set; }

        //Diesel Lines Table Columns

        public int DieselLinesId { get; set; }

        public int DieselHeaderForLines { get; set; }

        public long RouteId { get; set; }

        public string RouteDesc { get; set; } = null!;

        public string LoadType { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime UpdateDate { get; set; }

        public int UpdatedBy { get; set; }

        public string DieselHeaderForLinesName { get; set; }

        public string DieselLinesCreatedByName { get; set; }

        public string DieselLinesUpdatedByName { get; set; }

        public int? RunningKm { get; set; }

    }
}
