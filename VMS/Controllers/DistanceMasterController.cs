using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;

namespace VMS.Controllers
{
    public class DistanceMasterController : Controller
    {
        private readonly ILogger<DistanceMasterController> _logger;
        private readonly VmsDbContext _context;

        public DistanceMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMDistanceMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDistanceMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDistanceMasterFullAccess = userAcc.AddDistanceMasterFullAccess;
            //    ViewBag.AddDistanceMasterAddAccess = userAcc.AddDistanceMasterAddAccess;
            //    ViewBag.AddDistanceMasterUpdateAccess = userAcc.AddDistanceMasterUpdateAccess;
            //    ViewBag.AddDistanceMasterDeleteAccess = userAcc.AddDistanceMasterDeleteAccess;
            //    ViewBag.AddDistanceMasterViewAccess = userAcc.AddDistanceMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String distanceMasterId)
        {
            //Code for User Access Function
            //VMDistanceMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDistanceMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDistanceMasterFullAccess = userAcc.AddDistanceMasterFullAccess;
            //    ViewBag.AddDistanceMasterAddAccess = userAcc.AddDistanceMasterAddAccess;
            //    ViewBag.AddDistanceMasterUpdateAccess = userAcc.AddDistanceMasterUpdateAccess;
            //    ViewBag.AddDistanceMasterDeleteAccess = userAcc.AddDistanceMasterDeleteAccess;
            //    ViewBag.AddDistanceMasterViewAccess = userAcc.AddDistanceMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.DistanceMasterID = distanceMasterId;
            return View("Details");
        }

        public ActionResult Update(String distanceMasterId)
        {
            //Code for User Access Function
            //VMDistanceMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDistanceMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDistanceMasterFullAccess = userAcc.AddDistanceMasterFullAccess;
            //    ViewBag.AddDistanceMasterAddAccess = userAcc.AddDistanceMasterAddAccess;
            //    ViewBag.AddDistanceMasterUpdateAccess = userAcc.AddDistanceMasterUpdateAccess;
            //    ViewBag.AddDistanceMasterDeleteAccess = userAcc.AddDistanceMasterDeleteAccess;
            //    ViewBag.AddDistanceMasterViewAccess = userAcc.AddDistanceMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.DistanceMasterID = distanceMasterId;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMDistanceMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDistanceMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDistanceMasterFullAccess = userAcc.AddDistanceMasterFullAccess;
            //    ViewBag.AddDistanceMasterAddAccess = userAcc.AddDistanceMasterAddAccess;
            //    ViewBag.AddDistanceMasterUpdateAccess = userAcc.AddDistanceMasterUpdateAccess;
            //    ViewBag.AddDistanceMasterDeleteAccess = userAcc.AddDistanceMasterDeleteAccess;
            //    ViewBag.AddDistanceMasterViewAccess = userAcc.AddDistanceMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getDistanceMaster()
        {
            return Json(_context.TblDistanceMasters.Select(x => new
            {
                DistanceId = x.Id,
                Distance = x.Distance,
                RouteDescription = x.RouteDescription
            }).ToList());
        }

        [HttpGet]
        public JsonResult getDistanceMasterById(string distanceMasterId)
        {
            VMDistanceMaster model = new VMDistanceMaster();
            int DistanceMasterId = Convert.ToInt32(distanceMasterId);
            model = _context.TblDistanceMasters.Where(x => x.Id == DistanceMasterId).Select(x => new VMDistanceMaster
            {
                Id = x.Id,
                RouteDescription = x.RouteDescription,
                Distance = x.Distance,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
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
        public JsonResult getDistanceMasterList()
        {
            List<VMDistanceMaster> model = new List<VMDistanceMaster>();

            model = _context.TblDistanceMasters.Select(x => new VMDistanceMaster
            {
                Id = x.Id,
                RouteDescription = x.RouteDescription,
                Distance = x.Distance,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
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
        public JsonResult searchDistanceMaster(string routeDescription, string distance, Boolean isActive)
        {
            List<VMDistanceMaster> model = new List<VMDistanceMaster>();

            var searchModel = _context.TblDistanceMasters.Select(x => new VMDistanceMaster
            {
                Id = x.Id,
                RouteDescription = x.RouteDescription,
                Distance = x.Distance,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });

            if (!string.IsNullOrEmpty(routeDescription))
            {
                searchModel = searchModel.Where(s => s.RouteDescription.Equals(routeDescription));
            }

            if (!string.IsNullOrEmpty(distance))
            {
                searchModel = searchModel.Where(s => s.Distance.Equals(Convert.ToInt32(distance)));
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
        public ActionResult Save(string routeDescription, string distance, Boolean isActive)
        {
            VMDistanceMaster model = new VMDistanceMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblDistanceMasters.Select(x => new
                {
                    Id = x.Id,
                    RouteDescription = x.RouteDescription,
                    Distance = x.Distance,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.RouteDescription.Equals(routeDescription)).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Into Distance Master
                        var DistanceMaster = new TblDistanceMaster();
                        //DistanceMaster.Id = id;
                        DistanceMaster.RouteDescription = routeDescription;
                        DistanceMaster.Distance = Convert.ToInt32(distance);
                        DistanceMaster.IsActive = isActive;
                        DistanceMaster.CreationDate = utilityHelper.CurrentDateTime;
                        DistanceMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        DistanceMaster.CreatedBy = userID;
                        DistanceMaster.UpdatedBy = userID;

                        _context.TblDistanceMasters.Add(DistanceMaster);
                        _context.SaveChanges();
                        model.Id = DistanceMaster.Id;

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Distance Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Distance Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Route/Station Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, string routeDescription, string distance, Boolean isActive)
        {
            VMDistanceMaster model = new VMDistanceMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var DistanceMaster = _context.TblDistanceMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (DistanceMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Distance Master does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        DistanceMaster.RouteDescription = routeDescription;
                        DistanceMaster.Distance = Convert.ToInt32(distance);
                        DistanceMaster.IsActive = isActive;
                        //DistanceMaster.CreateDate = utilityHelper.CurrentDateTime;
                        DistanceMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //DistanceMaster.CreatedBy = userID;
                        DistanceMaster.UpdatedBy = userID;

                        _context.TblDistanceMasters.Update(DistanceMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Distance Master has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Distance Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteDistanceMaster(string distanceMasterId)
        {
            VMDistanceMaster model = new VMDistanceMaster();
            try
            {
                var DistanceMaster = _context.TblDistanceMasters.Where(x => x.Id == Convert.ToInt32(distanceMasterId));
                _context.TblDistanceMasters.RemoveRange(DistanceMaster);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Distance Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Distance Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopDistanceMasterList()
        {
            List<VMDistanceMaster> model = new List<VMDistanceMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblDistanceMasters.Select(x => new VMDistanceMaster
            {
                Id = x.Id,
                RouteDescription = x.RouteDescription,
                Distance = x.Distance,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
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
