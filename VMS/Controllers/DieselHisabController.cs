using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;

namespace VMS.Controllers
{
    public class DieselHisabController : Controller
    {
        private readonly ILogger<DieselHisabController> _logger;
        private readonly VmsDbContext _context;
        public DieselHisabController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMDieselHisabAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDieselHisabAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDieselHisabAccess = userAcc.AddDieselHisabAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String driverMasterID)
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.driverMasterID = driverMasterID;
            return View("Details");
        }

        public ActionResult Update(String driverMasterID)
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.driverMasterID = driverMasterID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
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
            var model =  _context.TblDieselHeaders.Select(x => new
            {
                TripId = x.TripId == 0 ? 1 : x.TripId,
                vehicleId = x.VehicleNo
            }).Where(y => y.vehicleId.Equals(vehicleNo)).FirstOrDefault();

            if (model != null)
            {
                return Json(model);
            }
            else
            {
                VMTrip trip = new VMTrip()
                {
                    TripId = 0,
                };
                return Json(trip);
            }
        }

        [HttpGet]
        public JsonResult getLastDieselTripHistory(int vehicleNo)
        {
            var model = _context.TblDieselHeaders.Where(y => y.VehicleNo.Equals(vehicleNo))
                                                 .Select(x => new VMDieselHisab
            {

                TripId = x.TripId,
                LastTripStartDate = Convert.ToString(Convert.ToDateTime(x.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                LastTripEndDate = Convert.ToString(Convert.ToDateTime(x.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                LastTripRouteDescr = x.LastTripRouteDescr,
                OpeningDiesel = x.OpeningDiesel == null || x.OpeningDiesel == 0 ? 1 : x.OpeningDiesel,
                LastTripVendor = "Test Vendor",
                LastTripDriver = _context.TblDriverMasters
                                .Where(p => p.Id == x.DriverId)
                                .Select(p => p.DriverName).FirstOrDefault(),
                LastTripDriverFatherName = _context.TblDriverMasters
                                           .Where(p => p.Id == x.DriverId)
                                           .Select(p => p.FatherName).FirstOrDefault(),
            }).OrderByDescending(x => x.TripId).FirstOrDefault();
            return Json(model);
        }

        [HttpGet]
        public JsonResult getDieselHisablWithId(string tripId)
        {
            VMDieselHisab model = new VMDieselHisab();
            int TripId = Convert.ToInt32(tripId);
            model = _context.TblDieselHeaders.Where(x => x.TripId.Equals(TripId)).Select(x => new VMDieselHisab
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
            }).FirstOrDefault();

            if (model == null)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpGet]
        public JsonResult getDieselHisablListByDriver(int driverId)
        {
            List<VMDieselHisab> model = new List<VMDieselHisab>();
            model = _context.TblDieselHeaders.Where(x => x.DriverId.Equals(driverId)).Select(x => new VMDieselHisab
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
        public JsonResult getDieselHisablListByVehicle(int vehicleNo)
        {
            List<VMDieselHisab> model = new List<VMDieselHisab>();
            model = _context.TblDieselHeaders.Where(x => x.VehicleNo.Equals(vehicleNo)).Select(x => new VMDieselHisab
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

        [HttpPost]
        public JsonResult searchDieselHisabMaster(int id, int vehicleNo, int driverId, string tripStartDate,
            string tripEndDate, Int32 startOdometer, Int32 endOdometer, int openingDiesel)
        {
            List<VMDieselHisab> model = new List<VMDieselHisab>();

            var searchModel = _context.TblDieselHeaders.Select(x => new VMDieselHisab
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
            });

            if (driverId != 0)
            {
                searchModel = searchModel.Where(s => s.DriverId.Equals(driverId));
            }

            if (vehicleNo != 0)
            {
                searchModel = searchModel.Where(s => s.VehicleNo.Equals(vehicleNo));
            }

            if (!string.IsNullOrEmpty(tripStartDate))
            {
                searchModel = searchModel.Where(s => s.TripStartDate.Equals(DateTime.Parse(tripStartDate)));
            }

            if (!string.IsNullOrEmpty(tripEndDate))
            {
                searchModel = searchModel.Where(s => s.TripEndDate.Equals(DateTime.Parse(tripStartDate)));
            }

            if (startOdometer != 0)
            {
                searchModel = searchModel.Where(s => s.StartOdometer.Equals(startOdometer));
            }

            if (endOdometer != 0)
            {
                searchModel = searchModel.Where(s => s.EndOdometer.Equals(endOdometer));
            }

            if (openingDiesel != 0)
            {
                searchModel = searchModel.Where(s => s.OpeningDiesel.Equals(openingDiesel));
            }

            model = searchModel.ToList();

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
        public ActionResult DisplayImage(string folderName, string uniqueModulePrefix, string fileName)
        {
            return File(utilityHelper.getFileName(folderName, uniqueModulePrefix, fileName), "image/jpeg"); // Adjust the content type based on the image type
        }


        [HttpPost]
        public ActionResult Save(int vehicleNo, int driverId, int tripNo, string driverName, 
                                 string driverFatherName, string tripStartDate,
                                 string tripEndDate, Int32 startOdometer, Int32 endOdometer,
                                 int openingDiesel, int runningKm, string tripRouteDescription)
        {
            DateOnly dtTripStartDate = DateOnly.Parse(tripStartDate);
            DateOnly dtTripEndDate = DateOnly.Parse(tripEndDate);
            VMTrip model = new VMTrip();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
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
                    try
                    {
                        //Insert Vehicle Release Info
                        var dieselHisab = new TblDieselHeader();
                        dieselHisab.TripId = tripNo;
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
                        dieselHisab.CreationDate = utilityHelper.CurrentDateTime;
                        dieselHisab.UpdateDate = utilityHelper.CurrentDateTime;
                        dieselHisab.CreatedBy = userID;
                        dieselHisab.UpdatedBy = userID;

                        _context.TblDieselHeaders.Add(dieselHisab);
                        _context.SaveChanges();
                        model.TripId = dieselHisab.TripId;
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Diesel Hisab has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Diesel Hisab  not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Diesel Hisab  Already Exist! Please try again with diffrent username.";
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
        public JsonResult getTopDieselHisabList()
        {
            List<VMDieselHisab> model = new List<VMDieselHisab>();
            model = _context.TblDieselHeaders.Select(x => new VMDieselHisab
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
    }
}
