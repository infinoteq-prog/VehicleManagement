using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using static iTextSharp.text.pdf.PdfDiv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace VMS.Controllers
{
    public class DieselHisabController : Controller
    {
        private readonly ILogger<DieselHisabController> _logger;
        private readonly VmsDbContext _context; private readonly string _connectionString;
        private string _controllerName = "DieselHisab";
        public DieselHisabController(VmsDbContext context, IConfiguration configuration)
        {
            _context = context; 
            _connectionString = configuration.GetConnectionString("VMSContext"); 
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult DieselFilter()
        {
            return View();
        }

        public ActionResult Details(String tripid)
        {
            ViewBag.tripid = tripid;
            return View("Details");
        }

        public ActionResult Update(String tripid)
        {
            ViewBag.tripid = tripid;
            return View("Update");
        }

        public ActionResult Print(String tripid)
        {
            ViewBag.tripid = tripid;
            return View("Print");
        }

        [HttpPost]
        public async Task<JsonResult> Approve(int tripid)
        {
            VMDriverMaster model = new VMDriverMaster();

            try
            {
                int userID = 0;
                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                if (userDetails != null)
                {
                    userID = userDetails.Id;
                    // Retrieve the existing TblDieselHeader record based on TripId
                    var UpdateDieselHisab = _context.TblDieselHeaders.Where(d => d.TripId == tripid).FirstOrDefault();

                    if (UpdateDieselHisab != null)
                    {
                        // Update the properties of the existing TblDieselHeader

                        if (UpdateDieselHisab.ApprovedBy != 0)
                        {
                            UpdateDieselHisab.ApprovedBy = 0;
                            UpdateDieselHisab.ApprovedDate = null;
                        }
                        else
                        {
                            UpdateDieselHisab.ApprovedBy = userID;
                            UpdateDieselHisab.ApprovedDate = utilityHelper.CurrentDateTime;
                        }

                        _context.TblDieselHeaders.Update(UpdateDieselHisab);
                        _context.SaveChanges(); // Save changes to the header first to ensure TripId is consistent

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Diesel Hisab approved status has been changed successfully.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Diesel Hisab has not been approved. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Diesel Hisab has not been approved. Please try again.";
                }
            }
            catch(Exception ex)
            {

                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = ex.Message.ToString();
            }
            return Json(model);
        }

        public ActionResult List()
        {
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            ViewBag.roleName = userDetails.RoleName.ToString();
            return View();
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = utilityHelper.getdownloadFilePath(fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpGet]
        public JsonResult getDriverMaster()
        {
            return Json(_context.TblDriverMasters.Select(x => new
            {
                DriverId = x.Id,
                DriverName = x.DriverName,
            }).ToList());
        }

        [HttpGet]
        public async Task<JsonResult> getRouteNameMaster()
        {
            try
            {
                var model = await DieselHisabContext.getRouteMaster(_connectionString);

                if (model != null)
                {
                    return Json(model);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        [HttpGet]
        public async Task<JsonResult> getDriverScoreMaster()
        {
            try
            {
                var model = await DieselHisabContext.getDriverScore(_connectionString);

                if (model != null)
                {
                    return Json(model);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        [HttpGet]
        public JsonResult getCurrentTripNumber(int vehicleNo)
        {
            var model = _context.TblDieselHeaders
                         .Where(y => y.IsActive==true)
                         .OrderByDescending(x => x.TripId) // Ensure highest TripId is picked
                         .Select(x => new
                         {
                             TripId = x.TripId == 0 ? 1 : x.TripId + 1,
                             vehicleId = x.VehicleNo
                         })
                         .FirstOrDefault();

            
            if (model != null)
            {
                return Json(model);
            }
            else
            {
                var trip = new 
                {
                    TripId = 1,
                    vehicleId = vehicleNo
                };
                return Json(trip);
            }
        }

        [HttpGet]
        public async Task<JsonResult> getLastDieselTripHistory(int vehicleId)
        {
            try
            {
                var averageData = await DieselHisabContext.GetLastDieselTripHistory(_connectionString, vehicleId);
                return Json(averageData);
            }
            catch(Exception ex) 
            {
                return Json(null);
            }

            //var model = (from diesel in _context.TblDieselHeaders
            //             join vehicle in _context.TblVehicleMasters
            //                 on diesel.VehicleNo equals vehicle.Id
            //             where diesel.IsActive == true && diesel.VehicleNo == Convert.ToInt32(vehicleId)
            //             orderby diesel.TripId descending
            //             select new VMDieselHisab
            //             {
            //                 TripId = diesel.TripId,
            //                 LastTripStartDate = Convert.ToString(Convert.ToDateTime(diesel.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
            //                 LastTripEndDate = Convert.ToString(Convert.ToDateTime(diesel.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
            //                 LastTripRouteDescr = diesel.LastTripRouteDescr,
            //                 EndOdometer = diesel.EndOdometer == null || diesel.EndOdometer == 0
            //                                 ? 1
            //                                 : diesel.EndOdometer,
            //                 OpeningDiesel = diesel.OpeningDiesel == null || diesel.OpeningDiesel == 0
            //                                 ? 1
            //                                 : diesel.OpeningDiesel,
            //                 ClosingDiesel = diesel.ClosingDiesel == null || diesel.ClosingDiesel == 0
            //                                 ? 1
            //                                 : diesel.ClosingDiesel,
            //                 LastTripVendor = "Test Vendor",
            //                 LastTripDriver = _context.TblDriverMasters
            //                                    .Where(p => p.Id == diesel.DriverId)
            //                                    .Select(p => p.DriverName)
            //                                    .FirstOrDefault(),
            //                 LastTripDriverFatherName = _context.TblDriverMasters
            //                                            .Where(p => p.Id == diesel.DriverId)
            //                                            .Select(p => p.FatherName)
            //                                            .FirstOrDefault(),
            //                 VehicleNumber = vehicle.VehicleNo // <-- From joined VehicleMaster
            //             }).FirstOrDefault();

            //return Json(model);
        }

        [HttpGet]
        public async Task<JsonResult> getDieselHisablWithId(string tripId)
        {
            int TripId = Convert.ToInt32(tripId);
            object model = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @"  SELECT
                         dh.TripId,dh.Last_Trip_Id[LastTripId],dh.VehicleNo,vm.Vehicle_No[VehicleNumber], dh.DriverId,
                         CONVERT(VARCHAR, dh.Trip_Start_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_Start_Date, 108) AS TripStartDate,
                         CONVERT(VARCHAR, dh.Trip_End_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_End_Date, 108) AS TripEndDate,
                         dh.Last_Trip_Route_Descr[Route_Descr], dhlast.Last_Trip_Route_Descr,dh.Start_Odometer,
                         dh.End_Odometer, dh.Opening_Diesel,dh.Closing_Diesel,
                         dh.RunningKm, dh.Is_Active,dh.Is_DifferenceAdded,dh.Is_LoadingAdded,
                         CONVERT(VARCHAR(10),dh.Creation_Date, 105) AS DieselHeaderCreationDate,
                         CONVERT(VARCHAR(10),dh.Update_Date, 105) AS DieselHeaderUpdateDate,
                         dh.Created_By AS DieselHeaderCreatedBy,
                         dh.Updated_By AS DieselHeaderUpdatedBy,
                         dm.Driver_Name,dm.Father_Name AS DriverFatherName,
                         uc.User_Name AS DieselHeaderCreatedByName,
                         uu.User_Name AS DieselHeaderUpdatedByName,
                         dh.discountPer,dh.DiscountValue,dh.RouteNameId,dh.DriverScoreId,dh.DriverChnageRemarks,ISNULL(dm.Bank_AccountNumber,'')[Bank_AccountNumber]
                         ,(CASE WHEN dh.DriverScoreId=0 THEN 'Below'  
						 WHEN dh.DriverScoreId=1 THEN 'Low' 
						 WHEN dh.DriverScoreId=2 THEN 'Medium' 
						 WHEN dh.DriverScoreId=3 THEN 'Good' 
						 ELSE '' END)[ScoreCard], [dbo].[GetDieselRate](GetDATE(), 'SALE')[DieselRate]
                         FROM [dbo].[tbl_Diesel_Header] dh
                         INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.VehicleNo = vm.Id
                         LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.DriverId = dm.Id
                         LEFT JOIN [dbo].[tbl_UserMaster] uc ON dh.Created_By = uc.Id
                         LEFT JOIN [dbo].[tbl_UserMaster] uu ON dh.Updated_By = uu.Id
						 LEFT OUTER JOIN [dbo].[tbl_Diesel_Header] dhlast on dh.Last_Trip_Id=dhlast.TripId
                        WHERE dh.Is_Active = 1 AND dh.TripId = @TripId;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@TripId", TripId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                model = new
                                {
                                    TripId = reader.GetInt32("TripId"),
                                    LastTripId = reader.GetInt32("LastTripId"),
                                    VehicleNo = reader.GetInt32("VehicleNo").ToIntFromNull(),
                                    VehicleNumber = reader.GetString("VehicleNumber"),
                                    DriverId = reader.GetInt32("DriverId"),
                                    TripStartDate = reader.GetString("TripStartDate"),
                                    TripEndDate = reader.GetString("TripEndDate"),
                                    ScoreCard = reader.GetString("ScoreCard"),
                                    Route_Descr = reader.GetString("Route_Descr"),
                                    LastTripRouteDescr = reader.GetString("Last_Trip_Route_Descr"),
                                    StartOdometer = reader.GetInt64("Start_Odometer"),
                                    EndOdometer = reader.GetInt64("End_Odometer"),
                                    OpeningDiesel = reader.GetInt64("Opening_Diesel").ToIntFromNull(),
                                    ClosingDiesel = reader.GetInt64("Closing_Diesel").ToIntFromNull(),
                                    discountPer = reader.GetDecimal("discountPer").To2Decimal(),
                                    DiscountValue = reader.GetDecimal("DiscountValue").To2Decimal(),
                                    RunningKm = reader.GetInt32("RunningKm").ToIntFromNull(),
                                    IsDifferenceAdded = reader.GetBoolean("Is_DifferenceAdded"),
                                    IsLoadingAdded = reader.GetBoolean("Is_LoadingAdded"),
                                    IsActive = reader.GetBoolean("Is_Active"),
                                    DieselHeaderCreationDate = reader.GetString("DieselHeaderCreationDate"),
                                    DieselHeaderUpdateDate = reader.GetString("DieselHeaderUpdateDate"),
                                    DieselHeaderCreatedBy = reader.GetInt32("DieselHeaderCreatedBy"),
                                    DieselHeaderUpdatedBy = reader.GetInt32("DieselHeaderUpdatedBy"),
                                    DriverName = reader.GetString("Driver_Name"),
                                    DriverFatherName = reader.GetString("DriverFatherName"),
                                    DieselHeaderCreatedByName = reader.GetString("DieselHeaderCreatedByName"),
                                    DieselHeaderUpdatedByName = reader.GetString("DieselHeaderUpdatedByName"),
                                    BankAccountNumber = reader.GetString("Bank_AccountNumber"),
                                    DieselRate = reader.IsDBNull("DieselRate") ? 0 : reader.GetDecimal("DieselRate"),

                                    RouteNameId = reader.IsDBNull("RouteNameId") ? 0 : reader.GetInt32("RouteNameId"),
                                    DriverScoreId = reader.IsDBNull("DriverScoreId") ? 0 : reader.GetInt32("DriverScoreId"),
                                    DriverChnageRemarks = reader.IsDBNull("DriverChnageRemarks") ? "" : reader.GetString("DriverChnageRemarks"),
                                    LastTripHistory = await DieselHisabContext.GetLastTripHistoryByTripIdAsync(_connectionString, reader.GetInt32("LastTripId")),
                                    DieselFillingList = await DieselHisabContext.GetDieselFillingListAsync(_connectionString, TripId),
                                    stationList = await DieselHisabContext.GetStationListAsync(_connectionString, TripId, reader.GetInt32("VehicleNo"))
                                };
                            }
                        }
                    }
                }

                return Json(model);
            }
            catch(Exception ex)
            {
                return Json(null);
            }
        }       

        [HttpGet]
        public JsonResult getDieselHisablListByDriver(int driverId)
        {
            List<VMDieselHisab> model = new List<VMDieselHisab>();
            model = _context.TblDieselHeaders.Where(x => x.IsActive == true && x.DriverId.Equals(driverId)).Select(x => new VMDieselHisab
            {
                TripId = x.TripId,
                VehicleNo = x.VehicleNo,
                DriverId = x.DriverId,
                TripStartDate = Convert.ToString(Convert.ToDateTime(x.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                TripEndDate = Convert.ToString(Convert.ToDateTime(x.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                LastTripRouteDescr = x.LastTripRouteDescr,
                StartOdometer = x.StartOdometer,
                EndOdometer = x.EndOdometer,
                OpeningDiesel = x.OpeningDiesel,
                RunningKm = x.RunningKm,
                IsActive = x.IsActive,
                LastTripVendor = "Test Vendor",
                DieselHeaderCreationDate = x.CreationDate,
                DieselHeaderUpdateDate = x.UpdateDate,
                DieselHeaderCreatedBy = x.CreatedBy,
                DieselHeaderUpdatedBy = x.UpdatedBy,
                DriverName = _context.TblDriverMasters
                                .Where(p => p.Id == x.DriverId)
                                .Select(p => p.DriverName).FirstOrDefault(),
                DriverFatherName = _context.TblDriverMasters
                                           .Where(p => p.Id == x.DriverId)
                                           .Select(p => p.FatherName).FirstOrDefault(),
                DieselHeaderCreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                DieselHeaderUpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            }).ToList();

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }


        [HttpGet]
        public JsonResult getDieselHisablListByVehicle(int vehicleId)
        {
            var model = (from diesel in _context.TblDieselHeaders
                         join vehicle in _context.TblVehicleMasters
                             on diesel.VehicleNo equals vehicle.Id
                         where diesel.IsActive == true && diesel.VehicleNo == vehicleId
                         select new
                         {
                             TripId = diesel.TripId,
                             VehicleNumber = vehicle.VehicleNo,
                             DriverId = diesel.DriverId,
                             TripStartDate = Convert.ToString(Convert.ToDateTime(diesel.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             TripEndDate = Convert.ToString(Convert.ToDateTime(diesel.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             CreationDate = Convert.ToString(Convert.ToDateTime(diesel.CreationDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             UpdateDate = Convert.ToString(Convert.ToDateTime(diesel.UpdateDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             CreatedBy="",
                             UpdatedBy="",
                             LastTripRouteDescr = diesel.LastTripRouteDescr,
                             StartOdometer = diesel.StartOdometer,
                             EndOdometer = diesel.EndOdometer,
                             OpeningDiesel = diesel.OpeningDiesel,
                             RunningKm = diesel.RunningKm,
                             IsActive = diesel.IsActive,
                             LastTripVendor = "Test Vendor",
                             DieselHeaderCreationDate = diesel.CreationDate,
                             DieselHeaderUpdateDate = diesel.UpdateDate,
                             DieselHeaderCreatedBy = diesel.CreatedBy,
                             DieselHeaderUpdatedBy = diesel.UpdatedBy,
                             DriverName = _context.TblDriverMasters
                                                 .Where(p => p.Id == diesel.DriverId)
                                                 .Select(p => p.DriverName).FirstOrDefault(),
                             DriverFatherName = _context.TblDriverMasters
                                                            .Where(p => p.Id == diesel.DriverId)
                                                            .Select(p => p.FatherName).FirstOrDefault(),
                             DieselHeaderCreatedByName = _context.TblUserMasters
                                             .Where(p => p.Id == diesel.CreatedBy)
                                             .Select(p => p.UserName).FirstOrDefault(),
                             DieselHeaderUpdatedByName = _context.TblUserMasters
                                             .Where(p => p.Id == diesel.UpdatedBy).FirstOrDefault()
                         }).ToList();

            return Json(model);


            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpPost]
        public async Task<JsonResult> searchDieselHisabMaster(int id, int vehicleNo, int driverId, string tripStartDate,
            string tripEndDate, Int32 startOdometer, Int32 endOdometer, int openingDiesel)
        {
            try
            {

                if(tripStartDate.ToStringFromNull()=="" && tripEndDate.ToStringFromNull()=="")
                {
                    tripStartDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                    tripEndDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var model = await DieselHisabContext.searchDieselHisabMaster(_connectionString, id, vehicleNo, driverId, tripStartDate,
                tripEndDate, startOdometer, endOdometer, openingDiesel);


                if (model != null)
                {
                    return Json(model);
                }
                else
                {
                    return Json(null);
                }
            }
            catch(Exception ex)
            {
                return Json(null);
            }
        }

        [HttpPost]
        public async Task<JsonResult> searchDieselFilter(int id, int vehicleNo, int vendorId, string tripStartDate,
            string tripEndDate)
        {
            try
            {

                if (tripStartDate.ToStringFromNull() == "" && tripEndDate.ToStringFromNull() == "")
                {
                    tripStartDate = DateTime.Now.AddDays(-3).ToString("yyyy-MM-dd");
                    tripEndDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var model = await DieselHisabContext.searchDieselFilter(_connectionString, id, vehicleNo, vendorId, tripStartDate,
                tripEndDate);


                if (model != null)
                {
                    return Json(model);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }

        [HttpGet]
        public ActionResult DisplayImage(string folderName, string uniqueModulePrefix, string fileName)
        {
            return File(utilityHelper.getFileName(folderName, uniqueModulePrefix, fileName), "image/jpeg"); // Adjust the content type based on the image type
        }


        [HttpPost]
        public ActionResult SaveUpdate(int tripId, int vehicleNo, int driverId, int tripNo,int lastTripId, string driverName, 
                                 string driverFatherName, string tripStartDate,
                                 string tripEndDate, Int32 startOdometer, Int32 endOdometer,
                                 int openingDiesel, int closingDiesel, int runningKm,decimal DiscountPer, bool IsDifferenceAdded,bool IsLoadingAdded,
                               int RouteNameId,int DriverScoreId,   string DriverChnageRemarks,
                                 string tripRouteDescription,List<TblDieselFilling> _lstDieselFilling, List<TblDieselLine> _lstDieselLine)
        {
            VMTrip model = new VMTrip();
            DateTime dtTripStartDate = DateTime.Now;
            DateTime dtTripEndDate = DateTime.Now;
            try
            {
                Globalsettings.Log(_controllerName, string.Format("Before conversion StartDate {0}, EndDate {1}", tripStartDate, tripEndDate));

                dtTripStartDate = Convert.ToDateTime(tripStartDate);
                dtTripEndDate = Convert.ToDateTime(tripEndDate);
                Globalsettings.Log(_controllerName, string.Format("After conversion StartDate {0}, EndDate {1}", Convert.ToDateTime(dtTripStartDate), Convert.ToDateTime(dtTripEndDate)));

            }
            catch(Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("Error occured while converting date {0}", ex.Message));
                model.TransactionMessage.Status = TransactionStatus.Failed;
                model.TransactionMessage.Message = "Diesel Hisab Date Conversion Issue!";
                return Json(model);
            }
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");

            Globalsettings.Log(_controllerName, string.Format("Save update started"));
            if (userDetails != null)
            {
                userID = userDetails.Id;

                if (tripId <= 0)
                {
                    Globalsettings.Log(_controllerName, string.Format("Save started"));
                    #region Insert Section
                    var dieselHeader = _context.TblDieselHeaders.AsQueryable().Where(x => x.VehicleNo==vehicleNo &&
                     x.TripStartDate.Year == dtTripStartDate.Year &&
                     x.TripStartDate.Month == dtTripStartDate.Month &&
                     x.TripStartDate.Day == dtTripStartDate.Day &&
                     x.TripEndDate.Year == dtTripEndDate.Year &&
                     x.TripEndDate.Month == dtTripEndDate.Month &&
                     x.TripEndDate.Day == dtTripEndDate.Day).FirstOrDefault();
                    if (dieselHeader==null)
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                // Insert Vehicle Release Info
                                var dieselHisab = new TblDieselHeader
                                {
                                    //TripId = tripNo.ToIntFromNull(),
                                    LastTripId = lastTripId.ToIntFromNull(),
                                    DriverId = driverId.ToIntFromNull(),
                                    VehicleNo = vehicleNo.ToIntFromNull(),
                                    TripStartDate =Convert.ToDateTime(dtTripStartDate.ToString("yyyy-MM-dd HH:mm")),
                                    TripEndDate = Convert.ToDateTime(dtTripEndDate.ToString("yyyy-MM-dd HH:mm")),
                                    LastTripRouteDescr = tripRouteDescription,
                                    StartOdometer = startOdometer,
                                    EndOdometer = endOdometer.ToIntFromNull(),
                                    OpeningDiesel = openingDiesel.ToIntFromNull(),
                                    ClosingDiesel = closingDiesel.ToIntFromNull(),
                                    RunningKm = runningKm.ToIntFromNull(),
                                    IsDifferenceAdded = IsDifferenceAdded,
                                    IsLoadingAdded = IsLoadingAdded,
                                    IsActive = true,
                                    RouteNameId = RouteNameId,
                                    DriverScoreId = DriverScoreId,
                                    DriverChnageRemarks = DriverChnageRemarks,
                                    Profit_Loss = DieselHisabContext.calProfitLoss(openingDiesel,closingDiesel,_lstDieselFilling,_lstDieselLine),
                                    Percent_Loss = DieselHisabContext.calPercentLoss(openingDiesel, closingDiesel, _lstDieselFilling, _lstDieselLine),
                                    Bhari_Ka_Average = DieselHisabContext.calBhariKaAverage(openingDiesel, closingDiesel, _lstDieselFilling, _lstDieselLine),
                                    DiscountPer = DiscountPer,
                                    DiscountValue = DieselHisabContext.calDiscountValue(DiscountPer, _lstDieselLine),
                                    CreationDate = utilityHelper.CurrentDateTime,
                                    UpdateDate = utilityHelper.CurrentDateTime,
                                    CreatedBy = userID.ToIntFromNull(),
                                    UpdatedBy = userID.ToIntFromNull()
                                };

                                _context.TblDieselHeaders.Add(dieselHisab);
                                _context.SaveChanges();

                                // Save Diesel Filling
                                if (_lstDieselFilling != null && _lstDieselFilling.Count > 0)
                                {
                                    foreach (var item in _lstDieselFilling)
                                    {
                                        Globalsettings.Log(_controllerName, string.Format("Diesel Filling Date {0}",item.StrDieselFillingDate));
                                        DateTime dtFillingDate = DateTime.Now;
                                        try
                                        {
                                            dtFillingDate = Convert.ToDateTime(item.StrDieselFillingDate);
                                        }
                                        catch (Exception ex)
                                        {
                                            Globalsettings.Log(_controllerName, string.Format("Error occured while converting diesel filling date {0}", ex.Message));
                                            model.TransactionMessage.Status = TransactionStatus.Failed;
                                            model.TransactionMessage.Message = "Diesel Hisab DieselFillingDate Conversion Issue "+ item.StrDieselFillingDate;
                                            return Json(model);
                                        }
                                        var filling = new TblDieselFilling
                                        {
                                            TripId = dieselHisab.TripId,
                                            VendorId = item.VendorId,
                                            DieselFillingDate = Convert.ToDateTime(dtFillingDate.ToString("yyyy-MM-dd")),
                                            DieselQty = item.DieselQty,
                                            CreationDate = utilityHelper.CurrentDateTime,
                                            UpdateDate = utilityHelper.CurrentDateTime,
                                            CreatedBy = userID,
                                            UpdatedBy = userID
                                        };

                                        _context.TblDieselFillings.Add(filling);
                                    }
                                    _context.SaveChanges();
                                }

                                // Save Diesel Line
                                if (_lstDieselLine != null && _lstDieselLine.Count > 0)
                                {
                                    foreach (var item in _lstDieselLine)
                                    {
                                        var line = new TblDieselLine
                                        {
                                            TripId = dieselHisab.TripId,
                                            RouteId = item.RouteId,
                                            RouteDesc = item.RouteDesc,
                                            LoadType = item.LoadType,
                                            Distance = item.Distance,
                                            Average = item.Average,
                                            EstimatedDiesel = item.EstimatedDiesel,
                                            CreationDate = utilityHelper.CurrentDateTime,
                                            UpdateDate = utilityHelper.CurrentDateTime,
                                            CreatedBy = userID,
                                            UpdatedBy = userID
                                        };

                                        _context.TblDieselLines.Add(line);
                                    }
                                    _context.SaveChanges();
                                }

                                transaction.Commit();

                                model.TripId = dieselHisab.TripId;
                                model.TransactionMessage.Status = TransactionStatus.Success;
                                model.TransactionMessage.Message = "Diesel Hisab has been saved successfully.";
                            }
                            catch (Exception ex)
                            {
                                Globalsettings.Log(_controllerName, string.Format("Error: {0}", ex.Message));
                                Globalsettings.Log(_controllerName, string.Format("Error: {0}", ex.InnerException));
                                transaction.Rollback();
                                model.TransactionMessage.Status = TransactionStatus.Error;
                                model.TransactionMessage.Message = "Diesel Hisab has not been saved due to some technical issue. Please try again.";
                            }
                        }
                    }
                    else
                    {
                        Globalsettings.Log(_controllerName, string.Format("dieselHeader already exist at"));
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Diesel Hisab Already Exist! Please try again with diffrent username.";
                    }
                    #endregion
                }
                else
                {
                    Globalsettings.Log(_controllerName, string.Format("Update started"));

                    #region Update Section
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Retrieve the existing TblDieselHeader record based on TripId
                            var existingDieselHisab = _context.TblDieselHeaders.FirstOrDefault(d => d.TripId == tripNo);

                            if (existingDieselHisab != null)
                            {
                                // Update the properties of the existing TblDieselHeader
                                existingDieselHisab.DriverId = driverId;
                                existingDieselHisab.VehicleNo = vehicleNo;
                                existingDieselHisab.TripStartDate = dtTripStartDate;
                                existingDieselHisab.TripEndDate = dtTripEndDate;
                                existingDieselHisab.LastTripRouteDescr = tripRouteDescription;
                                existingDieselHisab.StartOdometer = startOdometer;
                                existingDieselHisab.EndOdometer = endOdometer;
                                existingDieselHisab.OpeningDiesel = openingDiesel;
                                existingDieselHisab.ClosingDiesel = closingDiesel;
                                existingDieselHisab.RunningKm = runningKm;
                                existingDieselHisab.IsActive = true; // You might want to control this based on input
                                existingDieselHisab.IsDifferenceAdded = IsDifferenceAdded;
                                existingDieselHisab.IsLoadingAdded = IsLoadingAdded;
                                existingDieselHisab.UpdateDate = utilityHelper.CurrentDateTime;
                                existingDieselHisab.UpdatedBy = userID;
                                existingDieselHisab.RouteNameId = RouteNameId;
                                existingDieselHisab.DriverScoreId = DriverScoreId;
                                existingDieselHisab.DriverChnageRemarks = DriverChnageRemarks;
                                existingDieselHisab.Profit_Loss = DieselHisabContext.calProfitLoss(openingDiesel, closingDiesel, _lstDieselFilling, _lstDieselLine);
                                existingDieselHisab.Percent_Loss = DieselHisabContext.calPercentLoss(openingDiesel, closingDiesel, _lstDieselFilling, _lstDieselLine);
                                existingDieselHisab.Bhari_Ka_Average = DieselHisabContext.calBhariKaAverage(openingDiesel, closingDiesel, _lstDieselFilling, _lstDieselLine);
                                existingDieselHisab.DiscountPer = DiscountPer;
                                existingDieselHisab.DiscountValue = DieselHisabContext.calDiscountValue(DiscountPer, _lstDieselLine);
                                _context.TblDieselHeaders.Update(existingDieselHisab);
                                _context.SaveChanges(); // Save changes to the header first to ensure TripId is consistent
                            }
                            else
                            {
                                // Handle the case where the TblDieselHeader doesn't exist (you might want to log this or throw an error)
                                model.TransactionMessage.Status = TransactionStatus.Error;
                                model.TransactionMessage.Message = $"Diesel Hisab with TripId '{tripNo}' not found for update.";
                                return Json(model);
                            }

                            // Update TblDieselFillings
                            if (_lstDieselFilling != null)
                            {
                                foreach (var j in _lstDieselFilling)
                                {
                                    Globalsettings.Log(_controllerName, string.Format("Diesel Filling Date {0}", j.StrDieselFillingDate));
                                    DateTime dtFillingDate = DateTime.Now;
                                    try
                                    {
                                        dtFillingDate = Convert.ToDateTime(j.StrDieselFillingDate);
                                    }
                                    catch (Exception ex)
                                    {
                                        Globalsettings.Log(_controllerName, string.Format("Error occured while converting diesel filling date {0}", ex.Message));
                                        model.TransactionMessage.Status = TransactionStatus.Failed;
                                        model.TransactionMessage.Message = "Diesel Hisab DieselFillingDate Conversion Issue " + j.StrDieselFillingDate;
                                        return Json(model);
                                    }
                                }
                                // Get existing fillings for the current TripId
                                var existingFillings = _context.TblDieselFillings.Where(f => f.TripId == tripNo).ToList();

                                // Identify fillings to add (New Rows)
                                var fillingsToAdd = _lstDieselFilling.Where(item => !existingFillings.Any(e =>
                                    e.VendorId == item.VendorId)).Select(item => new TblDieselFilling
                                    {
                                        TripId = existingDieselHisab.TripId,
                                        VendorId = item.VendorId,
                                        DieselFillingDate = Convert.ToDateTime(item.StrDieselFillingDate),
                                        DieselQty = item.DieselQty,
                                        CreationDate = utilityHelper.CurrentDateTime,
                                        UpdateDate = utilityHelper.CurrentDateTime,
                                        CreatedBy = userID,
                                        UpdatedBy = userID
                                    }).ToList();
                                _context.TblDieselFillings.AddRange(fillingsToAdd);

                                // Identify fillings to remove
                                var fillingsToRemove = existingFillings.Where(existing => !_lstDieselFilling.Any(item =>
                                    existing.VendorId == item.VendorId &&
                                    existing.DieselFillingDate == Convert.ToDateTime(item.StrDieselFillingDate) &&
                                    existing.DieselQty == item.DieselQty)).ToList();
                                _context.TblDieselFillings.RemoveRange(fillingsToRemove);

                                // Check for Existing Row Update
                                // Identify fillings to update and remove
                                foreach (var existingFilling in existingFillings)
                                {
                                    var matchingItem = _lstDieselFilling.FirstOrDefault(item =>
                                        existingFilling.VendorId == item.VendorId);

                                    if (matchingItem != null)
                                    {
                                        existingFilling.DieselFillingDate =Convert.ToDateTime(matchingItem.StrDieselFillingDate);
                                        existingFilling.DieselQty = matchingItem.DieselQty;
                                        existingFilling.UpdateDate = utilityHelper.CurrentDateTime;
                                        existingFilling.UpdatedBy = userID;
                                        _context.TblDieselFillings.Update(existingFilling);
                                    }
                                }

                                _context.SaveChanges();
                            }
                            else
                            {
                                // If _lstDieselFilling is null, you might want to remove all existing fillings for this TripId
                                var existingFillings = _context.TblDieselFillings.Where(f => f.TripId == tripNo).ToList();
                                _context.TblDieselFillings.RemoveRange(existingFillings);
                                _context.SaveChanges();
                            }

                            // Update TblDieselLines
                            if (_lstDieselLine != null)
                            {
                                // Get existing lines for the current TripId
                                var existingLines = _context.TblDieselLines.Where(l => l.TripId == tripNo).ToList();

                                // Remove all existing lines for this TripId
                                _context.TblDieselLines.RemoveRange(existingLines);

                                // Create new TblDieselLine entries from _lstDieselLine
                                var newLines = _lstDieselLine.Select(item => new TblDieselLine
                                {
                                    TripId = existingDieselHisab.TripId, // Assuming existingDieselHisab.TripId is the correct TripId
                                    RouteId = item.RouteId,
                                    RouteDesc = item.RouteDesc,
                                    LoadType = item.LoadType,
                                    Distance = item.Distance,
                                    Average = item.Average,
                                    EstimatedDiesel = item.EstimatedDiesel,
                                    CreationDate = utilityHelper.CurrentDateTime,
                                    UpdateDate = utilityHelper.CurrentDateTime, // Set update date as well for new entries
                                    CreatedBy = userID,
                                    UpdatedBy = userID
                                }).ToList();

                                // Add all the newly created lines
                                _context.TblDieselLines.AddRange(newLines);

                                // Save all changes to the database
                                _context.SaveChanges();
                            }
                            else
                            {
                                // If _lstDieselLine is null, you might want to remove all existing lines for this TripId
                                var existingLines = _context.TblDieselLines.Where(l => l.TripId == tripNo).ToList();
                                _context.TblDieselLines.RemoveRange(existingLines);
                                _context.SaveChanges();
                            }

                            transaction.Commit();

                            model.TripId = existingDieselHisab.TripId;
                            model.TransactionMessage.Status = TransactionStatus.Success;
                            model.TransactionMessage.Message = "Diesel Hisab has been updated successfully.";
                        }
                        catch (Exception ex)
                        {
                            Globalsettings.Log(_controllerName, string.Format("Error: {0}", ex.Message));
                            Globalsettings.Log(_controllerName, string.Format("Error: {0}", ex.InnerException));
                            transaction.Rollback();
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "Diesel Hisab has not been updated due to some technical issue. Please try again.";
                            // Consider logging the exception: _logger.LogError(ex, "Error updating Diesel Hisab");
                        }
                    }
                    #endregion
                }
            }
            else
            {
                Globalsettings.Log(_controllerName, string.Format("user details found null"));
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int tripId, int vehicleNo, int driverId, string tripStartDate,
                                   string tripEndDate, Int32 startOdometer, Int32 endOdometer,
                                   int openingDiesel, int runningKm, string tripRouteDescription)
        {
            DateOnly dtTripStartDate = DateOnly.Parse(tripStartDate);
            DateOnly dtTripEndDate = DateOnly.Parse(tripEndDate);
            VMDieselHisab model = new VMDieselHisab();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var dieselHisab = _context.TblDieselHeaders.Where(x => x.TripId.Equals(tripId)).FirstOrDefault();
                    if (dieselHisab == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Diesel Hisab does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        dieselHisab.TripId = tripId;
                        dieselHisab.DriverId = driverId;
                        dieselHisab.VehicleNo = vehicleNo;
                        dieselHisab.TripStartDate = dtTripStartDate.ToDateTime(TimeOnly.Parse("12:00 AM"));
                        dieselHisab.TripEndDate = dtTripEndDate.ToDateTime(TimeOnly.Parse("12:00 AM"));
                        dieselHisab.LastTripRouteDescr = tripRouteDescription;
                        dieselHisab.StartOdometer = startOdometer;
                        dieselHisab.EndOdometer = endOdometer;
                        dieselHisab.OpeningDiesel = openingDiesel;
                        dieselHisab.RunningKm = runningKm;
                        dieselHisab.IsActive = true;
                        dieselHisab.UpdateDate = utilityHelper.CurrentDateTime;
                        dieselHisab.UpdatedBy = userID;
                        _context.TblDieselHeaders.Update(dieselHisab);
                        _context.SaveChanges();
                        model.TripId = tripId;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Diesel Hisab has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Diesel Hisab not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deletedieselHisab(int tripId)
        {
            VMDriverMaster model = new VMDriverMaster();
            try
            {
                var dieselHisab = _context.TblDieselHeaders.Where(x => x.TripId.Equals(tripId));
                _context.TblDieselHeaders.RemoveRange(dieselHisab);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Diesel Hisab has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Diesel Hisab has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public async Task<JsonResult> getTopDieselHisabList()
        {
            try
            {
                var model = await DieselHisabContext.searchDieselHisabMaster(_connectionString, 0, 0, 0, DateTime.Now.AddDays(-30).ToString("dd-MM-yyyy"),
                DateTime.Now.ToString("dd-MM-yyyy"), 0, 0, 0);

                if (model != null)
                {
                    return Json(model);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }

            //var model = (from x in _context.TblDieselHeaders
            //            join vehicle in _context.TblVehicleMasters
            //                on x.VehicleNo equals vehicle.Id
            //            where x.IsActive == true
            //            select new
            //{
            //    TripId = x.TripId,
            //    VehicleNo = vehicle.VehicleNo,
            //    DriverId = x.DriverId,
            //    TripStartDate = Convert.ToString(Convert.ToDateTime(x.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
            //    TripEndDate = Convert.ToString(Convert.ToDateTime(x.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
            //    LastTripRouteDescr = x.LastTripRouteDescr,
            //                CreationDate = Convert.ToString(Convert.ToDateTime(x.CreationDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
            //                UpdateDate = Convert.ToString(Convert.ToDateTime(x.UpdateDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
            //    StartOdometer = x.StartOdometer,
            //    EndOdometer = x.EndOdometer,
            //    OpeningDiesel = x.OpeningDiesel,
            //    RunningKm = x.RunningKm,
            //    IsActive = x.IsActive,
            //    LastTripVendor = "Test Vendor",
            //    DieselHeaderCreatedBy = x.CreatedBy,
            //    DieselHeaderUpdatedBy = x.UpdatedBy,
            //    DriverName = _context.TblDriverMasters
            //                    .Where(p => p.Id == x.DriverId)
            //                    .Select(p => p.DriverName).FirstOrDefault(),
            //    DriverFatherName = _context.TblDriverMasters
            //                               .Where(p => p.Id == x.DriverId)
            //                               .Select(p => p.FatherName).FirstOrDefault(),
            //    DieselHeaderCreatedByName = _context.TblUserMasters
            //                .Where(p => p.Id == x.CreatedBy)
            //                .Select(p => p.UserName).FirstOrDefault(),
            //    DieselHeaderUpdatedByName = _context.TblUserMasters
            //                .Where(p => p.Id == x.UpdatedBy)
            //                .Select(p => p.UserName).FirstOrDefault(),
            //    ApprovedStatus = x.ApprovedBy<=0 ? "Pending Approval" : "Approved",
            //                ApprovedBy = _context.TblUserMasters
            //                .Where(p => p.Id == x.ApprovedBy)
            //                .Select(p => p.UserName).FirstOrDefault(),
            //                ApprovedDate = Convert.ToString(Convert.ToDateTime(x.ApprovedDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture))
            //            }).OrderByDescending(x=> x.TripId).ToList();

            //if (model.Count() == 0)
            //{
            //    return Json(null);
            //}
            //else
            //{
            //    return Json(model);
            //}
        }


        [HttpGet]
        public JsonResult getVendorMaster()
        {
            return Json(_context.TblCodeMasters.Where(x=> x.CodeType=="Vendor").Select(x => new
            {
                VendorId = x.Id,
                VendorName = x.Code,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getDistanceMasterById(int Id)
        {
            var model = _context.TblDistanceMasters.Where(x => x.Id == Id).Select(x => new
            {
                DistanceId = x.Id,
                Distance = x.Distance,
                RouteDescription = x.RouteDescription
            }).FirstOrDefault();
            return Json(model);
        }

        [HttpGet]
        public JsonResult getLoadTypeMaster()
        {
            return Json(_context.TblCodeMasters.Where(x => x.CodeType == "LOADTYPE").Select(x => new
            {
                LoadTypeId = x.Id,
                LoadType = x.Code,
            }).ToList());
        }

        public async Task<JsonResult> getDieselAverageAsPerLoadType(int vehicleNo, string LoadType)
        {
            var averageData = await DieselHisabContext.GetDieselAverage(_connectionString, vehicleNo, LoadType);
            return Json(averageData);
        }

        [HttpGet]
        public JsonResult getDifferenceRouteMasterId()
        {
            return Json(_context.TblDistanceMasters.Where(x=> x.RouteDescription.ToUpper()== "Difference".ToUpper()).Select(x => new
            {
                DistanceId = x.Id,
                RouteDescription = x.RouteDescription,
            }).ToList());
        }


        [HttpGet]
        public JsonResult getLoadingUnloadingRouteMasterId()
        {
            return Json(_context.TblDistanceMasters.Where(x => x.RouteDescription.ToUpper() == "Loading/Unloading".ToUpper()).Select(x => new
            {
                DistanceId = x.Id,
                RouteDescription = x.RouteDescription,
            }).ToList());
        }

        [HttpGet]
        public IActionResult DownloadDetailedExcelByTripId(int tripId)
        {
            //if (hisabData == null)
            //{
            //    return NotFound(); // Or handle the case where data is not found
            //}
            ExcelPackage.License.SetNonCommercialOrganization("Your Non-Commercial Organization Name");
            using (var package = new ExcelPackage())
            {
                DataSet ds = new DataSet();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[dbo].[DieselHisab_List_DownloadExcel]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@tripId", tripId);
                        // create data adapter
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        // this will query your database and return the result to your datatable
                        da.Fill(ds);
                        da.Dispose();
                    }
                }

                #region Add Master
                ExcelWorksheet worksheetMaster = package.Workbook.Worksheets.Add("Driver Hisab Details");
                if (ds.Tables[0].Rows.Count >= 0)
                {
                    // Add Column Headers (from DataTable columns)
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        worksheetMaster.Cells[1, i + 1].Value = ds.Tables[0].Columns[i].ColumnName;
                    }

                    // Add Data Rows (from DataTable rows)
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        for (int col = 0; col < ds.Tables[0].Columns.Count; col++)
                        {
                            worksheetMaster.Cells[row + 2, col + 1].Value = ds.Tables[0].Rows[row][col];
                        }
                    }

                    // Auto-fit columns for better readability
                    worksheetMaster.Cells.AutoFitColumns();
                }
                #endregion

                #region Add DieselFillingList Detail
                ExcelWorksheet worksheetExpenseList = package.Workbook.Worksheets.Add("Diesel Filling Details");
                if (ds.Tables[1].Rows.Count >= 0)
                {
                    // Add Column Headers (from DataTable columns)
                    for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                    {
                        worksheetExpenseList.Cells[1, i + 1].Value = ds.Tables[1].Columns[i].ColumnName;
                    }

                    // Add Data Rows (from DataTable rows)
                    for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                    {
                        for (int col = 0; col < ds.Tables[1].Columns.Count; col++)
                        {
                            worksheetExpenseList.Cells[row + 2, col + 1].Value = ds.Tables[1].Rows[row][col];
                        }
                    }

                    // Auto-fit columns for better readability
                    worksheetExpenseList.Cells.AutoFitColumns();
                }
                #endregion

                #region Add stationList Detail
                ExcelWorksheet worksheetstationList = package.Workbook.Worksheets.Add("Station Details");
                if (ds.Tables[2].Rows.Count >= 0)
                {
                    // Add Column Headers (from DataTable columns)
                    for (int i = 0; i < ds.Tables[2].Columns.Count; i++)
                    {
                        worksheetstationList.Cells[1, i + 1].Value = ds.Tables[2].Columns[i].ColumnName;
                    }

                    // Add Data Rows (from DataTable rows)
                    for (int row = 0; row < ds.Tables[2].Rows.Count; row++)
                    {
                        for (int col = 0; col < ds.Tables[2].Columns.Count; col++)
                        {
                            worksheetstationList.Cells[row + 2, col + 1].Value = ds.Tables[2].Rows[row][col];
                        }
                    }

                    // Auto-fit columns for better readability
                    worksheetstationList.Cells.AutoFitColumns();
                }
                #endregion

                // 4. Convert the Excel package to a byte array
                byte[] excelBytes = package.GetAsByteArray();

                // 5. Return the byte array as a FileResult for download
                string fileName = $"DieselHisab_{tripId.ToString()}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

    }
}
