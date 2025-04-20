using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;

namespace VMS.Controllers
{
    public class MakeMasterController : Controller
    {
        private readonly ILogger<MakeMasterController> _logger;
        private readonly VmsDbContext _context;

        public MakeMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMMakeMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMMakeMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddMakeMasterFullAccess = userAcc.AddMakeMasterFullAccess;
            //    ViewBag.AddMakeMasterAddAccess = userAcc.AddMakeMasterAddAccess;
            //    ViewBag.AddMakeMasterUpdateAccess = userAcc.AddMakeMasterUpdateAccess;
            //    ViewBag.AddMakeMasterDeleteAccess = userAcc.AddMakeMasterDeleteAccess;
            //    ViewBag.AddMakeMasterViewAccess = userAcc.AddMakeMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String makeMasterID)
        {
            //Code for User Access Function
            //VMMakeMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMMakeMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddMakeMasterFullAccess = userAcc.AddMakeMasterFullAccess;
            //    ViewBag.AddMakeMasterAddAccess = userAcc.AddMakeMasterAddAccess;
            //    ViewBag.AddMakeMasterUpdateAccess = userAcc.AddMakeMasterUpdateAccess;
            //    ViewBag.AddMakeMasterDeleteAccess = userAcc.AddMakeMasterDeleteAccess;
            //    ViewBag.AddMakeMasterViewAccess = userAcc.AddMakeMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.makeMasterID = makeMasterID;
            return View("Details");
        }

        public ActionResult Update(String makeMasterID)
        {
            //Code for User Access Function
            //VMMakeMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMMakeMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddMakeMasterFullAccess = userAcc.AddMakeMasterFullAccess;
            //    ViewBag.AddMakeMasterAddAccess = userAcc.AddMakeMasterAddAccess;
            //    ViewBag.AddMakeMasterUpdateAccess = userAcc.AddMakeMasterUpdateAccess;
            //    ViewBag.AddMakeMasterDeleteAccess = userAcc.AddMakeMasterDeleteAccess;
            //    ViewBag.AddMakeMasterViewAccess = userAcc.AddMakeMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.makeMasterID = makeMasterID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMMakeMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMMakeMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddMakeMasterFullAccess = userAcc.AddMakeMasterFullAccess;
            //    ViewBag.AddMakeMasterAddAccess = userAcc.AddMakeMasterAddAccess;
            //    ViewBag.AddMakeMasterUpdateAccess = userAcc.AddMakeMasterUpdateAccess;
            //    ViewBag.AddMakeMasterDeleteAccess = userAcc.AddMakeMasterDeleteAccess;
            //    ViewBag.AddMakeMasterViewAccess = userAcc.AddMakeMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getMakeMaster()
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description
            }).Where(x => x.CodeType.Equals(SiteConstants.CodeType_Make)).ToList());
        }

        [HttpGet]
        public JsonResult getExpenseMaster()
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description
            }).Where(x => x.CodeType.Equals(SiteConstants.CodeType_ExpenseType)).ToList());
        }

        [HttpGet]
        public JsonResult getVehicleTypeMaster()
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description
            }).Where(x => x.CodeType.Equals(SiteConstants.CodeType_VehicleType)).ToList());
        }

        [HttpGet]
        public JsonResult getBodyTypeMaster()
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description
            }).Where(x => x.CodeType.Equals(SiteConstants.CodeType_BodyType)).ToList());
        }

        [HttpGet]
        public JsonResult getManufacturerTypeMaster()
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description
            }).Where(x => x.CodeType.Equals(SiteConstants.CodeType_Manufacturer)).ToList());
        }

        [HttpGet]
        public JsonResult getMakeMasterByID(string makeMasterID)
        {
            VMCodeMaster model = new VMCodeMaster();
            int makeMasterId = Convert.ToInt32(makeMasterID);
            model = _context.TblCodeMasters.Where(x => x.Id == makeMasterId).Select(x => new VMCodeMaster
            {
                Id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description,
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
        public JsonResult getMakeMasterList()
        {
            List<VMCodeMaster> model = new List<VMCodeMaster>();

            model = _context.TblCodeMasters.Select(x => new VMCodeMaster
            {
                Id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description,
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
        public JsonResult searchMakeMaster(string codeType, string code, string description, Boolean isActive)
        {
            List<VMCodeMaster> model = new List<VMCodeMaster>();

            var searchModel = _context.TblCodeMasters.Select(x => new VMCodeMaster
            {
                Id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description,
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

            if (!string.IsNullOrEmpty(codeType))
            {
                searchModel = searchModel.Where(s => s.CodeType.Equals(codeType));
            }

            if (!string.IsNullOrEmpty(code))
            {
                searchModel = searchModel.Where(s => s.Code.Equals(code));
            }

            if (!string.IsNullOrEmpty(description))
            {
                searchModel = searchModel.Where(s => s.Description.Equals(description));
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
        public ActionResult Save(string codeType, string code, string description, Boolean isActive)
        {
            VMCodeMaster model = new VMCodeMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblCodeMasters.Select(x => new
                {
                    Id = x.Id,
                    CodeType = x.CodeType,
                    Code = x.Code,
                    Description = x.Description,
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
                }).Where(x => x.Code == code).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Into Make Master
                        var makeMaster = new TblCodeMaster();
                        //makeMaster.Id = id;
                        makeMaster.CodeType = codeType;
                        makeMaster.Code = code;
                        makeMaster.Description = description;
                        makeMaster.IsActive = isActive;
                        int year = DateTime.Now.Year;
                        DateTime firstDay = new DateTime(year, 1, 1);
                        makeMaster.StartDate = firstDay;
                        makeMaster.CreationDate = utilityHelper.CurrentDateTime;
                        makeMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        makeMaster.CreatedBy = userID;
                        makeMaster.UpdatedBy = userID;

                        _context.TblCodeMasters.Add(makeMaster);
                        _context.SaveChanges();
                        model.Id = makeMaster.Id;

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Code Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Code Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Code Master Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, string codeType, string code, string description, Boolean isActive)
        {
            VMCodeMaster model = new VMCodeMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var makeMaster = _context.TblCodeMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (makeMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Make Master does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        makeMaster.CodeType = codeType;
                        makeMaster.Code = code;
                        makeMaster.Description = description;
                        makeMaster.IsActive = isActive;
                        //makeMaster.CreateDate = utilityHelper.CurrentDateTime;
                        makeMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //makeMaster.CreatedBy = userID;
                        makeMaster.UpdatedBy = userID;

                        _context.TblCodeMasters.Update(makeMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Code Master has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Code Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteMakeMaster(string makeMasterID)
        {
            VMCodeMaster model = new VMCodeMaster();
            try
            {
                var makeMaster = _context.TblCodeMasters.Where(x => x.Id == Convert.ToInt32(makeMasterID));
                _context.TblCodeMasters.RemoveRange(makeMaster);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Code Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Code Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopMakeMasterList()
        {
            List<VMCodeMaster> model = new List<VMCodeMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblCodeMasters.Select(x => new VMCodeMaster
            {
                Id = x.Id,
                CodeType = x.CodeType,
                Code = x.Code,
                Description = x.Description,
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
