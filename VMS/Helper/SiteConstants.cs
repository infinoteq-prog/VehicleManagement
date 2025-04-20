namespace VMS.Helper
{
    public class SiteConstants
    {
        public const string Backslash = "\\";
        public const string Forwardslash = "/";
        public const string Underscore = "_";
        public const string Dot = ".";
        public const string Comma = ",";
        public const string NA = "N/A";
        public const string Dash = "-";

        //User Roles Constants
        //User Constants
        //public const int SuperAdmin = 5;
        //public const int Admin = 1;
        //public const int User = 2;
        public const string Admin = "Admin";
        public const string User = "User";
        public const string SuperAdmin = "SuperAdmin";


        public const string ddMMyyyyHHss = "ddMMyyyyHHss";
        public const string excelFileExtension = "xlsx";
        public const string pdfFileExtension = "pdf";
        public const string TransporterBillInitials = "TRANSBILLNO";
        public const string TransporterBillPrefix = "TransporterBill";
        public const int min = 300000;
        public const int max = 90000000;
        public const double shortagePercentage = .05;
        public const decimal shortageRate = 7000;
        public const string inward = "INWARD";
        public const string outward = "OUTWARD";


        public const string companyPanNoFolder = "CompanyPan";
        public const string insurancePhotoFolder = "InsurancePhoto";
        public const string nationalPermitPhotoFolder = "NationalPermitPhoto";
        public const string localPermitPhotoFolder = "LocalPermitPhoto";
        public const string rcDocPhotoFolder = "RcDocPhotoFolder";
        public const string rtoDocPhotoFolder = "RtoDocPhotoFolder";
        public const string pollutionDocPhotoFolder = "PollutionDocPhotoFolder";
        public const string fitnessPhotoFolder = "FitnessPhotoFolder";

        public const string driverPhotoFolder = "DriverPhoto";
        public const string driverAadharFolder = "DriverAadhar";
        public const string driverPanNoFolder = "DriverPan";
        public const string driverLicenceFolder = "DriverLicence";
        public const string dispatchFileFolder = "DispatchFiles";
        public const string dispatchErrorFileFolder = "DispatchErrorFiles";
        public const string dispatchErrorFileName = "DispatchFileUplodError";
        public const string transporterBillsFolder = "TransporterBills";
        public const string lrGrNoUpdateFolder = "LRGRNOUpdateFiles";
        public const string lrGrNoUpdateErrorFileName = "LRGRNOUpdateErrorFiles";

        public const string SGST_CGST_Tax = "SGST-IGST";
        public const string IGST_Tax = "IGST";
        public const string UGST_Tax = "UGST";

        public const string CodeType_Category = "CATEGORY";
        public const string CodeType_Incoterm = "INCOTERM";
        public const string CodeType_Division = "DIVISION";
        public const string CodeType_Hsncode = "HSNCODE";

        public const string CodeType_Make = "MAKE";
        public const string CodeType_Manufacturer = "MANUFACTURER";
        public const string CodeType_VehicleType = "VEHICLETYPE";
        public const string CodeType_Financer = "FINANCER";
        public const string CodeType_Vendor = "VENDOR";
        public const string CodeType_BodyType = "BODYTYPE";
        public const string CodeType_ExpenseType = "EXPTYPE";


        //Dispatch Table Constants
        public const int Batch_Size = 100;
        public const string Serial_No = "Serial_No";
        public const string tbl_Dispatch_Details = "tbl_Dispatch_Details";
        public const string Dispatch_Unique_Id = "Dispatch_Unique_ID";
        public const string Supplying_Plant = "Supplying_Plant";
        public const string Shipment_Doc = "Shipment_Doc";
        public const string Delivery_No = "Delivery_No";
        public const string Dispatch_Category = "Dispatch_Category";
        public const string Truck_No = "Truck_No";
        public const string LR_GR_No = "LR_GR_No";
        public const string Lr_Gr_No_Unique_Id = "LR_GR_NO_Unique_Id";
        public const string LR_GR_date = "LR_GR_Date";
        public const string Ship_To_Party_TZone = "Ship_To_Party_Tzone";
        public const string Dispatch_Qty_Road = "Dispatch_Qty_Road";
        public const string Ebid_net_amt = "E-Bidding_Net_Price";
        public const string Ebid_frt_rate = "E-Bidding_Rate";
        public const string Forwarding_Agent_Code = "Forwarding_Agent_Code";
        public const string Forwarding_Agent_Name = "Forwarding_Agent_Name";
        public const string Region_State = "Region_State";
        public const string RR_LR_No = "RR_LR_No";
        public const string Freight_Road = "Freight_Road";
        public const string Pgi_No = "Pgi_No";
        public const string Pgi_Date = "Pgi_Date";
        public const string Distribution_Channel = "Distribution_Channel";
        public const string Division = "Division";
        public const string Inco_Term = "Incoterm";
        public const string Route_Code = "Route_Code";
        public const string Route_Description = "Route_Description";
        public const string Total_Amount = "Total_Amount";
        public const string Creation_Date = "Creation_Date";
        public const string Update_Date = "Update_Date";
        public const string Created_By = "Created_By";
        public const string Updated_By = "Updated_By";


        public const string BillTo_C001 = "C001";
        public const string BillTo_C002 = "C002";

        public const string ReqFieldMessage = "Note: All fields marked with * are mandatory fields";
        public const string GstNoMessage = "Note : Please enter GSTNOTAVAILABLE in case gst no not available!";
    }
}
