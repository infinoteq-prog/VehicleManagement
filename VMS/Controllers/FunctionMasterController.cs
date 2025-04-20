using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Globalization;

namespace VMS.Controllers
{
    public class FunctionMasterController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly VmsDbContext _context;

        public FunctionMasterController(VmsDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            //Code for User Access Function
            //VMFunctionMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMFunctionMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserRoleFullAccess = userAcc.AddUserRoleFullAccess;
            //    ViewBag.AddUserRoleAddAccess = userAcc.AddUserRoleAddAccess;
            //    ViewBag.AddUserRoleUpdateAccess = userAcc.AddUserRoleUpdateAccess;
            //    ViewBag.AddUserRoleDeleteAccess = userAcc.AddUserRoleDeleteAccess;
            //    ViewBag.AddUserRoleViewAccess = userAcc.AddUserRoleViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String functionMasterId)
        {
            //Code for User Access Function
            //VMFunctionMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMFunctionMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserRoleFullAccess = userAcc.AddUserRoleFullAccess;
            //    ViewBag.AddUserRoleAddAccess = userAcc.AddUserRoleAddAccess;
            //    ViewBag.AddUserRoleUpdateAccess = userAcc.AddUserRoleUpdateAccess;
            //    ViewBag.AddUserRoleDeleteAccess = userAcc.AddUserRoleDeleteAccess;
            //    ViewBag.AddUserRoleViewAccess = userAcc.AddUserRoleViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userRoleID = functionMasterId;
            return View("Details");
        }

        public ActionResult Update(String functionMasterId)
        {
            //Code for User Access Function
            //VMFunctionMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMFunctionMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserRoleFullAccess = userAcc.AddUserRoleFullAccess;
            //    ViewBag.AddUserRoleAddAccess = userAcc.AddUserRoleAddAccess;
            //    ViewBag.AddUserRoleUpdateAccess = userAcc.AddUserRoleUpdateAccess;
            //    ViewBag.AddUserRoleDeleteAccess = userAcc.AddUserRoleDeleteAccess;
            //    ViewBag.AddUserRoleViewAccess = userAcc.AddUserRoleViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userRoleID = functionMasterId;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMFunctionMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMFunctionMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserRoleFullAccess = userAcc.AddUserRoleFullAccess;
            //    ViewBag.AddUserRoleAddAccess = userAcc.AddUserRoleAddAccess;
            //    ViewBag.AddUserRoleUpdateAccess = userAcc.AddUserRoleUpdateAccess;
            //    ViewBag.AddUserRoleDeleteAccess = userAcc.AddUserRoleDeleteAccess;
            //    ViewBag.AddUserRoleViewAccess = userAcc.AddUserRoleViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getFunctionMasterById(string functionMasterId)
        {
            VMFunctionMaster model = new VMFunctionMaster();
            int funcMasterId = Convert.ToInt32(functionMasterId);
            model = _context.TblFunctionMasters.Where(x => x.Id == funcMasterId).Select(x => new VMFunctionMaster
            {
                Id = x.Id.ToString(),
                FunctionName = x.FunctionName,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartDateString = String.IsNullOrEmpty(Convert.ToString(x.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                EndDateString = String.IsNullOrEmpty(Convert.ToString(x.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
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
        public JsonResult getFunctionList()
        {
            List<VMFunctionMaster> model = new List<VMFunctionMaster>();

            model = _context.TblFunctionMasters.Select(x => new VMFunctionMaster
            {
                Id = x.Id.ToString(),
                FunctionName = x.FunctionName,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartDateString = String.IsNullOrEmpty(Convert.ToString(x.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                EndDateString = String.IsNullOrEmpty(Convert.ToString(x.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
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
        public JsonResult searchFunctionMaster(string functionName, Boolean isActive, string startDate, string endDate)
        {
            List<VMFunctionMaster> model = new List<VMFunctionMaster>();

            var searchModel = _context.TblFunctionMasters.Select(x => new VMFunctionMaster
            {
                Id = x.Id.ToString(),
                FunctionName = x.FunctionName,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartDateString = String.IsNullOrEmpty(Convert.ToString(x.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                EndDateString = String.IsNullOrEmpty(Convert.ToString(x.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
            });

            if (!string.IsNullOrEmpty(functionName))
            {
                searchModel = searchModel.Where(s => s.FunctionName == functionName);
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                searchModel = searchModel.Where(s => s.StartDate == DateTime.Parse(startDate));
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                searchModel = searchModel.Where(s => s.EndDate == DateTime.Parse(endDate));
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
        public ActionResult Save(string functionName, Boolean isActive, string startDate, string endDate)
        {
            VMFunctionMaster model = new VMFunctionMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var func = _context.TblFunctionMasters.Select(x => new
                {
                    Id = x.Id.ToString(),
                    FunctionName = x.FunctionName,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    //EndDate = x.EndDate,
                    CreateDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                }).Where(x => x.FunctionName == functionName).ToList();

                if (func.Count() == 0)
                {
                    try
                    {
                        //Insert Vehicle Release Info
                        var functionMaster = new TblFunctionMaster();
                        //userRole.Id = id;
                        functionMaster.FunctionName = functionName;
                        functionMaster.IsActive = isActive;
                        functionMaster.StartDate = DateTime.Parse(startDate);
                        //functionMaster.EndDate = DateTime.Parse(endDate);
                        functionMaster.CreationDate = utilityHelper.CurrentDateTime;
                        functionMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        functionMaster.CreatedBy = userID;
                        functionMaster.UpdatedBy = userID;

                        _context.TblFunctionMasters.Add(functionMaster);
                        _context.SaveChanges();
                        model.Id = functionMaster.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Function Name has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Function Name not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Function Name Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }
        
        [HttpPost]
        public ActionResult Update(string id, string functionName, Boolean isActive, string startDate, string endDate)
        {
            VMFunctionMaster model = new VMFunctionMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var functionMaster = _context.TblFunctionMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (functionMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Function Name does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        functionMaster.FunctionName = functionName;
                        functionMaster.IsActive = isActive;
                        functionMaster.StartDate = DateTime.Parse(startDate);

                        if (!string.IsNullOrEmpty(endDate) && !endDate.Equals("-"))
                        {
                            functionMaster.EndDate = DateTime.Parse(endDate);
                        }
                           
                        //functionMaster.CreateDate = utilityHelper.CurrentDateTime;
                        functionMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //functionMaster.CreatedBy = userID;
                        functionMaster.UpdatedBy = userID;

                        _context.TblFunctionMasters.Update(functionMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Function Name has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Function Name not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteFunctionMaster(string functionMasterId)
        {
            VMFunctionMaster model = new VMFunctionMaster();
            try
            {
                var userRole = _context.TblFunctionMasters.Where(x => x.Id == Convert.ToInt32(functionMasterId));
                _context.TblFunctionMasters.RemoveRange(userRole);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Function Name has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Function Name has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getFunctionMasterList()
        {
            List<VMFunctionMaster> model = new List<VMFunctionMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblFunctionMasters.Select(x => new VMFunctionMaster
            {
                Id = x.Id.ToString(),
                FunctionName = x.FunctionName,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                StartDateString = String.IsNullOrEmpty(Convert.ToString(x.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                EndDateString = String.IsNullOrEmpty(Convert.ToString(x.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
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