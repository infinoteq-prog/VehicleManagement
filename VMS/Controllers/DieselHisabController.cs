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

namespace VMS.Controllers
{
    public class DieselHisabController : Controller
    {
        private readonly ILogger<DieselHisabController> _logger;
        private readonly VmsDbContext _context; private readonly string _connectionString;
        public DieselHisabController(VmsDbContext context, IConfiguration configuration)
        {
            _context = context; 
            _connectionString = configuration.GetConnectionString("VMSContext"); 
        }

        public ActionResult Index()
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
                        model.TransactionMessage.Message = "Diesel Hisab has been approved successfully.";
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
        public JsonResult getCurrentTripNumber(int vehicleNo)
        {
            var model = _context.TblDieselHeaders
                         .Where(y => y.IsActive==true && y.VehicleNo.Equals(vehicleNo))
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

                    string sql = @"
                SELECT
                 dh.TripId,dh.Last_Trip_Id[LastTripId],dh.VehicleNo,vm.Vehicle_No[VehicleNumber], dh.DriverId,
                 CONVERT(VARCHAR, dh.Trip_Start_Date, 105) AS TripStartDate,
                 CONVERT(VARCHAR, dh.Trip_End_Date, 105) AS TripEndDate,
                 dh.Last_Trip_Route_Descr,dh.Start_Odometer,
                 dh.End_Odometer, dh.Opening_Diesel,dh.Closing_Diesel,
                 dh.RunningKm, dh.Is_Active,
                 CONVERT(VARCHAR(10),dh.Creation_Date, 105) AS DieselHeaderCreationDate,
                 CONVERT(VARCHAR(10),dh.Update_Date, 105) AS DieselHeaderUpdateDate,
                 dh.Created_By AS DieselHeaderCreatedBy,
                 dh.Updated_By AS DieselHeaderUpdatedBy,
                 dm.Driver_Name,dm.Father_Name AS DriverFatherName,
                 uc.User_Name AS DieselHeaderCreatedByName,
                 uu.User_Name AS DieselHeaderUpdatedByName
                 FROM [dbo].[tbl_Diesel_Header] dh
                 INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.VehicleNo = vm.Id
                 LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.DriverId = dm.Id
                 LEFT JOIN [dbo].[tbl_UserMaster] uc ON dh.Created_By = uc.Id
                 LEFT JOIN [dbo].[tbl_UserMaster] uu ON dh.Updated_By = uu.Id
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
                                    DriverId = reader.GetInt32("DriverId"),
                                    TripStartDate = reader.GetString("TripStartDate"),
                                    TripEndDate = reader.GetString("TripEndDate"),
                                    LastTripRouteDescr = reader.GetString("Last_Trip_Route_Descr"),
                                    StartOdometer = reader.GetInt64("Start_Odometer"),
                                    EndOdometer = reader.GetInt64("End_Odometer"),
                                    OpeningDiesel = reader.GetInt64("Opening_Diesel").ToIntFromNull(),
                                    ClosingDiesel = reader.GetInt64("Closing_Diesel").ToIntFromNull(),
                                    RunningKm = reader.GetInt32("RunningKm").ToIntFromNull(),
                                    IsActive = reader.GetBoolean("Is_Active"),
                                    DieselHeaderCreationDate = reader.GetString("DieselHeaderCreationDate"),
                                    DieselHeaderUpdateDate = reader.GetString("DieselHeaderUpdateDate"),
                                    DieselHeaderCreatedBy = reader.GetInt32("DieselHeaderCreatedBy"),
                                    DieselHeaderUpdatedBy = reader.GetInt32("DieselHeaderUpdatedBy"),
                                    DriverName = reader.GetString("Driver_Name"),
                                    DriverFatherName = reader.GetString("DriverFatherName"),
                                    DieselHeaderCreatedByName = reader.GetString("DieselHeaderCreatedByName"),
                                    DieselHeaderUpdatedByName = reader.GetString("DieselHeaderUpdatedByName"),
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
            }catch(Exception ex)
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
                                 int openingDiesel, int closingDiesel, int runningKm, string tripRouteDescription,List<TblDieselFilling> _lstDieselFilling, List<TblDieselLine> _lstDieselLine)
        {
            DateOnly dtTripStartDate = DateOnly.Parse(tripStartDate);
            DateOnly dtTripEndDate = DateOnly.Parse(tripEndDate);
            VMTrip model = new VMTrip();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");            
            if (userDetails != null)
            {
                userID = userDetails.Id;

                if (tripId <= 0)
                {
                    #region Insert Section
                    var dieselHeader = _context.TblDieselHeaders.Select(x => new
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
                        ClosingDiesel = x.ClosingDiesel,
                        runningKm = x.RunningKm,
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
                    }).Where(x => x.TripId.Equals(tripNo)).ToList();

                    if (dieselHeader.Count() == 0)
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                // Insert Vehicle Release Info
                                var dieselHisab = new TblDieselHeader
                                {
                                    TripId = tripNo,
                                    LastTripId = lastTripId,
                                    DriverId = driverId,
                                    VehicleNo = vehicleNo,
                                    TripStartDate = dtTripStartDate.ToDateTime(TimeOnly.Parse("12:00 AM")),
                                    TripEndDate = dtTripEndDate.ToDateTime(TimeOnly.Parse("12:00 AM")),
                                    LastTripRouteDescr = tripRouteDescription,
                                    StartOdometer = startOdometer,
                                    EndOdometer = endOdometer,
                                    OpeningDiesel = openingDiesel,
                                    ClosingDiesel = closingDiesel,
                                    RunningKm = runningKm,
                                    IsActive = true,
                                    CreationDate = utilityHelper.CurrentDateTime,
                                    UpdateDate = utilityHelper.CurrentDateTime,
                                    CreatedBy = userID,
                                    UpdatedBy = userID
                                };

                                _context.TblDieselHeaders.Add(dieselHisab);
                                _context.SaveChanges();

                                // Save Diesel Filling
                                if (_lstDieselFilling != null && _lstDieselFilling.Count > 0)
                                {
                                    foreach (var item in _lstDieselFilling)
                                    {
                                        var filling = new TblDieselFilling
                                        {
                                            TripId = dieselHisab.TripId,
                                            VendorId = item.VendorId,
                                            DieselFillingDate = Convert.ToDateTime(item.DieselFillingDate),
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
                                transaction.Rollback();
                                model.TransactionMessage.Status = TransactionStatus.Error;
                                model.TransactionMessage.Message = "Diesel Hisab has not been saved due to some technical issue. Please try again.";
                            }
                        }
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Diesel Hisab Already Exist! Please try again with diffrent username.";
                    }
                    #endregion
                }
                else
                {
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
                                existingDieselHisab.TripStartDate = dtTripStartDate.ToDateTime(TimeOnly.Parse("12:00 AM"));
                                existingDieselHisab.TripEndDate = dtTripEndDate.ToDateTime(TimeOnly.Parse("12:00 AM"));
                                existingDieselHisab.LastTripRouteDescr = tripRouteDescription;
                                existingDieselHisab.StartOdometer = startOdometer;
                                existingDieselHisab.EndOdometer = endOdometer;
                                existingDieselHisab.OpeningDiesel = openingDiesel;
                                existingDieselHisab.ClosingDiesel = closingDiesel;
                                existingDieselHisab.RunningKm = runningKm;
                                existingDieselHisab.IsActive = true; // You might want to control this based on input
                                existingDieselHisab.UpdateDate = utilityHelper.CurrentDateTime;
                                existingDieselHisab.UpdatedBy = userID;

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
                                // Get existing fillings for the current TripId
                                var existingFillings = _context.TblDieselFillings.Where(f => f.TripId == tripNo).ToList();

                                // Identify fillings to add (New Rows)
                                var fillingsToAdd = _lstDieselFilling.Where(item => !existingFillings.Any(e =>
                                    e.VendorId == item.VendorId)).Select(item => new TblDieselFilling
                                    {
                                        TripId = existingDieselHisab.TripId,
                                        VendorId = item.VendorId,
                                        DieselFillingDate = Convert.ToDateTime(item.DieselFillingDate),
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
                                    existing.DieselFillingDate == Convert.ToDateTime(item.DieselFillingDate) &&
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
                                        existingFilling.DieselFillingDate = matchingItem.DieselFillingDate;
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

                                // Identify lines to add
                                var linesToAdd = _lstDieselLine.Where(item => !existingLines.Any(e =>
                                    e.RouteId == item.RouteId)).Select(item => new TblDieselLine
                                    {
                                        TripId = existingDieselHisab.TripId,
                                        RouteId = item.RouteId,
                                        RouteDesc = item.RouteDesc,
                                        LoadType = item.LoadType,
                                        Average = item.Average,
                                        EstimatedDiesel = item.EstimatedDiesel,
                                        CreationDate = utilityHelper.CurrentDateTime,
                                        UpdateDate = utilityHelper.CurrentDateTime,
                                        CreatedBy = userID,
                                        UpdatedBy = userID
                                    }).ToList();
                                _context.TblDieselLines.AddRange(linesToAdd);

                                // Identify lines to remove
                                var linesToRemove = existingLines.Where(existing => !_lstDieselLine.Any(item =>
                                    existing.RouteId == item.RouteId)).ToList();
                                _context.TblDieselLines.RemoveRange(linesToRemove);

                                // Check for Existing Row Update
                                // Identify Lines to update and remove
                                foreach (var existingLine in existingLines)
                                {
                                    var matchingItem = _lstDieselLine.FirstOrDefault(item =>
                                                       existingLine.RouteId == item.RouteId);

                                    if (matchingItem != null)
                                    {
                                        existingLine.Average = matchingItem.Average;
                                        existingLine.EstimatedDiesel = matchingItem.EstimatedDiesel;
                                        existingLine.LoadType = matchingItem.LoadType;
                                        existingLine.UpdateDate = utilityHelper.CurrentDateTime;
                                        existingLine.UpdatedBy = userID;
                                        _context.TblDieselLines.Update(existingLine);
                                    }
                                }
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

      
    }
}
