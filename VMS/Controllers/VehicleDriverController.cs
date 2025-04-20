using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
namespace VMS.Controllers
{
    public class VehicleDriverController : Controller
    {
        private readonly ILogger<VehicleDriverController> _logger;
        private readonly VmsDbContext _context;
        public VehicleDriverController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMVehicleDriverAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleDriverAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleDriverAccess = userAcc.AddVehicleDriverAccess;
            //    ViewBag.AddVehicleDriverAddAccess = userAcc.AddVehicleDriverAddAccess;
            //    ViewBag.AddVehicleDriverUpdateAccess = userAcc.AddVehicleDriverUpdateAccess;
            //    ViewBag.AddVehicleDriverDeleteAccess = userAcc.AddVehicleDriverDeleteAccess;
            //    ViewBag.AddVehicleDriverViewAccess = userAcc.AddVehicleDriverViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String vehicleDriverID)
        {
            //Code for User Access Function
            //VMVehicleDriverAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleDriverAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleDriverAccess = userAcc.AddVehicleDriverAccess;
            //    ViewBag.AddVehicleDriverAddAccess = userAcc.AddVehicleDriverAddAccess;
            //    ViewBag.AddVehicleDriverUpdateAccess = userAcc.AddVehicleDriverUpdateAccess;
            //    ViewBag.AddVehicleDriverDeleteAccess = userAcc.AddVehicleDriverDeleteAccess;
            //    ViewBag.AddVehicleDriverViewAccess = userAcc.AddVehicleDriverViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.vehicleDriverID = vehicleDriverID;
            return View("Details");
        }

        public ActionResult Update(String vehicleDriverID)
        {
            //Code for User Access Function
            //VMVehicleDriverAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleDriverAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleDriverAccess = userAcc.AddVehicleDriverAccess;
            //    ViewBag.AddVehicleDriverAddAccess = userAcc.AddVehicleDriverAddAccess;
            //    ViewBag.AddVehicleDriverUpdateAccess = userAcc.AddVehicleDriverUpdateAccess;
            //    ViewBag.AddVehicleDriverDeleteAccess = userAcc.AddVehicleDriverDeleteAccess;
            //    ViewBag.AddVehicleDriverViewAccess = userAcc.AddVehicleDriverViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.vehicleDriverID = vehicleDriverID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMVehicleDriverAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleDriverAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleDriverAccess = userAcc.AddVehicleDriverAccess;
            //    ViewBag.AddVehicleDriverAddAccess = userAcc.AddVehicleDriverAddAccess;
            //    ViewBag.AddVehicleDriverUpdateAccess = userAcc.AddVehicleDriverUpdateAccess;
            //    ViewBag.AddVehicleDriverDeleteAccess = userAcc.AddVehicleDriverDeleteAccess;
            //    ViewBag.AddVehicleDriverViewAccess = userAcc.AddVehicleDriverViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public ActionResult checkVehicleDriverLinking(Int32 driverId, Int32 vehicleId)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                if (driverId != 0 && vehicleId != 0)
                {
                    userID = userDetails.Id;
                    var vehicleDriverLinking = _context.TblVehicleDrivers.Where(x => x.DriverId == driverId
                             && x.VehicleId == vehicleId).FirstOrDefault();

                    if (vehicleDriverLinking != null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Vehicle & Driver Linking Already Exist! Please try again with different Vehicle & Driver.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Vehicle & Driver Linking not found!";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Vehicle & Driver Linking not found!";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpGet]
        public ActionResult checkVehicleDriverLinkingOnUpdate(Int32 id, Int32 driverId, Int32 vehicleId)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                if (driverId != 0 && vehicleId != 0)
                {
                    userID = userDetails.Id;

                    var vehicleDriverLinking = _context.TblVehicleDrivers.Where( x => x.DriverId == driverId &&
                                                                        x.VehicleId == vehicleId &&
                                                                        x.Id != id).FirstOrDefault();
                    if (vehicleDriverLinking == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Vehicle & Driver Linking not found!";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Vehicle & Driver Linking Already Exist! Please try again with different Vehicle & Driver.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Vehicle & Driver Linking not found!";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getVehicleDriverByID(string vehicleDriverID)
        {
            VMVehicleDriver model = new VMVehicleDriver();
            int roleID = Convert.ToInt32(vehicleDriverID);
            model = _context.TblVehicleDrivers.Where(x => x.Id == roleID).Select(x => new VMVehicleDriver
            {
                Id = x.Id,
                DriverId = x.DriverId,
                VehicleId = x.VehicleId,
                LinkDate = x.LinkDate,
                EndDate = x.EndDate,
                ReasonOfDlink = String.IsNullOrEmpty(x.ReasonOfDlink) ? "N/A" : x.ReasonOfDlink,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                DriverName = _context.TblDriverMasters
                            .Where(p => p.Id == x.DriverId)
                            .Select(p => p.DriverName).FirstOrDefault(),
                VehicleName = Convert.ToString(_context.TblAssetMasters
                            .Where(p => p.Id == x.VehicleId)
                            .Select(p => p.VehicleNo).FirstOrDefault()),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
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
        public JsonResult getVehicleDriverLinkingList()
        {
            List<VMVehicleDriver> model = new List<VMVehicleDriver>();

            model = _context.TblVehicleDrivers.Select(x => new VMVehicleDriver
            {
                Id = x.Id,
                DriverId = x.DriverId,
                VehicleId = x.VehicleId,
                LinkDate = x.LinkDate,
                EndDate = x.EndDate,
                ReasonOfDlink = String.IsNullOrEmpty(x.ReasonOfDlink) ? "N/A" : x.ReasonOfDlink,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                DriverName = _context.TblDriverMasters
                            .Where(p => p.Id == x.DriverId)
                            .Select(p => p.DriverName).FirstOrDefault(),
                VehicleName = Convert.ToString(_context.TblAssetMasters
                            .Where(p => p.Id == x.VehicleId)
                            .Select(p => p.VehicleNo).FirstOrDefault()),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            }).OrderByDescending(n => n.Id).ToList();

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
        public JsonResult searchVehicleDriverLinking(Int32 driverId, Int32 vehicleId, string linkDate, string endDate, string reasonOfDlink, Boolean isActive)
        {
            List<VMVehicleDriver> model = new List<VMVehicleDriver>();

            var searchModel = _context.TblVehicleDrivers.Select(x => new VMVehicleDriver
            {
                Id = x.Id,
                DriverId = x.DriverId,
                VehicleId = x.VehicleId,
                LinkDate = x.LinkDate,
                EndDate = x.EndDate,
                ReasonOfDlink = String.IsNullOrEmpty(x.ReasonOfDlink) ? "N/A" : x.ReasonOfDlink,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                DriverName = _context.TblDriverMasters
                            .Where(p => p.Id == x.DriverId)
                            .Select(p => p.DriverName).FirstOrDefault(),
                VehicleName = Convert.ToString(_context.TblAssetMasters
                            .Where(p => p.Id == x.VehicleId)
                            .Select(p => p.VehicleNo).FirstOrDefault()),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });

            if (driverId != 0)
            {
                searchModel = searchModel.Where(s => s.DriverId == driverId);
            }

            if (vehicleId != 0)
            {
                searchModel = searchModel.Where(s => s.VehicleId == vehicleId);
            }

            if (!string.IsNullOrEmpty(linkDate))
            {
                searchModel = searchModel.Where(s => s.LinkDate == DateTime.Parse(linkDate));
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                searchModel = searchModel.Where(s => s.EndDate == DateTime.Parse(endDate));
            }

            if (!string.IsNullOrEmpty(reasonOfDlink))
            {
                searchModel = searchModel.Where(s => s.ReasonOfDlink == reasonOfDlink);
            }

            //searchModel = searchModel.Where(s => s.IsActive == isActive);

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

        [HttpPost]
        public ActionResult Save(Int32 driverId, Int32 vehicleId, string linkDate, string endDate, string reasonOfDlink, Boolean isActive)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblVehicleDrivers.Select(x => new
                {
                    Id = x.Id,
                    DriverId = x.DriverId,
                    VehicleId = x.VehicleId,
                    LinkDate = x.LinkDate,
                    EndDate = x.EndDate,
                     ReasonOfDlink = String.IsNullOrEmpty(x.ReasonOfDlink) ? "N/A" : x.ReasonOfDlink,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    DriverName = _context.TblDriverMasters
                            .Where(p => p.Id == x.DriverId)
                            .Select(p => p.DriverName).FirstOrDefault(),
                    VehicleName = Convert.ToString(_context.TblAssetMasters
                            .Where(p => p.Id == x.VehicleId)
                            .Select(p => p.VehicleNo).FirstOrDefault()),
                    CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.DriverId == driverId
                         && x.VehicleId == vehicleId).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Vehicle & Driver Info
                        var vehicleDriver = new TblVehicleDriver();
                        //vehicleDriver.Id = id;
                        vehicleDriver.DriverId = driverId;
                        vehicleDriver.VehicleId = vehicleId;
                        vehicleDriver.LinkDate = DateTime.Parse(linkDate);
                        //vehicleDriver.EndDate = DateTime.Parse(endDate);
                        //vehicleDriver.ReasonOfDlink = reasonOfDlink;
                        vehicleDriver.IsActive = isActive;
                        //vehicleDriver.StartDate = DateTime.Parse(startDate);
                        //vehicleDriver.EndDate = DateTime.Parse(endDate);
                        vehicleDriver.CreationDate = utilityHelper.CurrentDateTime;
                        vehicleDriver.UpdateDate = utilityHelper.CurrentDateTime;
                        vehicleDriver.CreatedBy = userID;
                        vehicleDriver.UpdatedBy = userID;

                        _context.TblVehicleDrivers.Add(vehicleDriver);
                        _context.SaveChanges();
                        model.Id = vehicleDriver.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Vehicle & Driver has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Vehicle & Driver not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Vehicle & Driver Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, Int32 driverId, Int32 vehicleId, string linkDate, 
                                   string endDate, string reasonOfDlink, Boolean isActive)
        {
            VMVehicleDriver model = new VMVehicleDriver();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    TblVehicleDriver vehicleDriver = new TblVehicleDriver();
                    //var vehicleDriver = _context.TblVehicleDrivers.Where(x => x.Id == id &&
                    //                                                     x.DriverId == driverId &&
                    //                                                     x.VehicleId == vehicleId).FirstOrDefault();
                    //if (vehicleDriver == null)
                    //{
                    //    model.TransactionMessage.Status = TransactionStatus.Failed;
                    //    model.TransactionMessage.Message = "Vehicle & Driver does not Exists! Please check and try again.";
                    //    return Json(model);
                    //}
                    //else
                    //{
                    //Updateing Existing User Details
                        vehicleDriver.Id = id;
                        vehicleDriver.DriverId = driverId;
                        vehicleDriver.VehicleId = vehicleId;
                        vehicleDriver.LinkDate = DateTime.Parse(linkDate);
                        if (!string.IsNullOrEmpty(endDate))
                        {
                            vehicleDriver.EndDate = DateTime.Parse(endDate);
                        }

                        if (!string.IsNullOrEmpty(reasonOfDlink))
                        {
                            vehicleDriver.ReasonOfDlink = reasonOfDlink;
                        }

                        vehicleDriver.IsActive = isActive;
                        vehicleDriver.CreationDate = utilityHelper.CurrentDateTime;
                        vehicleDriver.UpdateDate = utilityHelper.CurrentDateTime;
                        //vehicleDriver.CreatedBy = userID;
                        vehicleDriver.UpdatedBy = userID;

                        _context.TblVehicleDrivers.Update(vehicleDriver);
                        _context.SaveChanges();
                        model.Id = id;
                    //}
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Vehicle & Driver has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Vehicle & Driver not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteVehicleDriver(string vehicleDriverID)
        {
            VMVehicleDriver model = new VMVehicleDriver();
            try
            {
                var vehicleDriver = _context.TblVehicleDrivers.Where(x => x.Id == Convert.ToInt32(vehicleDriverID));
                _context.TblVehicleDrivers.RemoveRange(vehicleDriver);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Vehicle & Driver has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Vehicle & Driver has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopVehicleDriverList()
        {
            List<VMVehicleDriver> model = new List<VMVehicleDriver>();

            //Query to fetch last 10 saved records in driver master table
            model = _context.TblVehicleDrivers.Select(x => new VMVehicleDriver
            {
                Id = x.Id,
                DriverId = x.DriverId,
                VehicleId = x.VehicleId,
                LinkDate = x.LinkDate,
                EndDate = x.EndDate,
                 ReasonOfDlink = String.IsNullOrEmpty(x.ReasonOfDlink) ? "N/A" : x.ReasonOfDlink,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                DriverName = _context.TblDriverMasters
                            .Where(p => p.Id == x.DriverId)
                            .Select(p => p.DriverName).FirstOrDefault(),
                VehicleName = Convert.ToString(_context.TblAssetMasters
                            .Where(p => p.Id == x.VehicleId)
                            .Select(p => p.VehicleNo).FirstOrDefault()),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            }).OrderByDescending(n => n.Id).Take(10).ToList();

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