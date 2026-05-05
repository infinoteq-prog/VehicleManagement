using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Globalization;

namespace VMS.Controllers
{
    public class UserRoleAccessController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly VmsDbContext _context;

        public UserRoleAccessController(VmsDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
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

        public ActionResult Details(String userRoleAccessId)
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
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
            ViewBag.userRoleAccessId = userRoleAccessId;
            return View("Details");
        }

        public ActionResult Update(String userRoleAccessId)
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
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
            ViewBag.userRoleAccessId = userRoleAccessId;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
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
        public JsonResult getUsersList()
        {
            var model = _context.TblUserMasters.Select(x => new
            {
                Id = x.Id,
                Name = x.UserName,
            });

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    model = model.Where(s => s.Id.Equals(userDetails.Id));
                }
            }

            var userList = model.ToList();

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(userList);
            }
        }

        [HttpGet]
        public JsonResult getRolesList()
        {
            return Json(_context.TblRoleMasters.Select(x => new
            {
                Id = x.Id,
                Name = x.Role,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getFunctionsList()
        {
            return Json(_context.TblFunctionMasters.Select(x => new
            {
                Id = x.Id,
                Name = x.FunctionName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getUserRoleAccessbyId(string userRoleAccessId)
        {
            VMUserRoleAccess model = new VMUserRoleAccess();
            int roleAccessID = Convert.ToInt32(userRoleAccessId);
            model = _context.TblUserFunctions.Where(x => x.Id == roleAccessID).Select(x => new VMUserRoleAccess
            {
                Id = x.Id,
                UserId = x.UserId,
                RoleId = x.RoleId,
                FunctionId = x.FunctionId,
                FunctionName = x.FunctionName,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                MenuTypeId = x.MenuTypeId,
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
                UserName = _context.TblUserMasters
                            .Where(p => p.Id == x.UserId)
                            .Select(p => p.UserName).FirstOrDefault(),
                RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                FunctionMasterName = _context.TblFunctionMasters
                            .Where(p => p.Id == x.FunctionId)
                            .Select(p => p.FunctionName).FirstOrDefault(),
                Codes = _context.TblCodeMasters.Where(p => p.Id == x.MenuTypeId)
        .Select(p => p.Code).FirstOrDefault(),

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
        public JsonResult getUserRoleAccessList()
        {
            List<VMUserRoleAccess> model = new List<VMUserRoleAccess>();

            model = _context.TblUserFunctions.Select(x => new VMUserRoleAccess
            {
                Id = x.Id,
                UserId = x.UserId,
                RoleId = x.RoleId,
                FunctionId = x.FunctionId,
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
                UserName = _context.TblUserMasters
                            .Where(p => p.Id == x.UserId)
                            .Select(p => p.UserName).FirstOrDefault(),
                RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                FunctionMasterName = _context.TblFunctionMasters
                            .Where(p => p.Id == x.FunctionId)
                            .Select(p => p.FunctionName).FirstOrDefault(),

                Codes = x.MenuTypeId == 1 ? "All Menu" : _context.TblCodeMasters.Where(p => p.Id == x.MenuTypeId)
                            .Select(p => p.Code).FirstOrDefault()

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
        public JsonResult searchUserRoleAccess(Int32 userId, Int32 roleId, Int32 functionId, string functionName, string startDate, string endDate, Boolean isActive, int menuType)
        {
            List<VMUserRoleAccess> model = new List<VMUserRoleAccess>();

            var searchModel = _context.TblUserFunctions.Select(x => new VMUserRoleAccess
            {
                Id = x.Id,
                UserId = x.UserId,
                RoleId = x.RoleId,
                FunctionId = x.FunctionId,
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
                UserName = _context.TblUserMasters
                            .Where(p => p.Id == x.UserId)
                            .Select(p => p.UserName).FirstOrDefault(),
                RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                FunctionMasterName = _context.TblFunctionMasters
                            .Where(p => p.Id == x.FunctionId)
                            .Select(p => p.FunctionName).FirstOrDefault(),
                Codes = _context.TblCodeMasters.Where(p => p.Id == x.MenuTypeId)
                            .Select(p => p.Code).FirstOrDefault()
            });

            if (userId != 0)
            {
                searchModel = searchModel.Where(s => s.UserId == userId);
            }

            if (roleId != 0)
            {
                searchModel = searchModel.Where(s => s.RoleId == roleId);
            }

            if (functionId != 0)
            {
                searchModel = searchModel.Where(s => s.FunctionId == functionId);
            }

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
        public ActionResult Save(Int32 userId, Int32 roleId, Int32 functionId,
            string functionName, string startDate, string endDate, Boolean isActive, int menuType)
        {
            VMUser model = new VMUser();
            int loginUserID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                loginUserID = userDetails.Id;
                var user = _context.TblUserFunctions.Select(x => new
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    RoleId = x.RoleId,
                    FunctionId = x.FunctionId,
                    FunctionName = x.FunctionName,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreateDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    MenuTypeId = x.MenuTypeId,
                    CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UserName = _context.TblUserMasters
                            .Where(p => p.Id == x.UserId)
                            .Select(p => p.UserName).FirstOrDefault(),
                    RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                    FunctionMasterName = _context.TblFunctionMasters
                            .Where(p => p.Id == x.FunctionId)
                            .Select(p => p.FunctionName).FirstOrDefault(),
                    Codes = _context.TblCodeMasters.Where(p => p.Id == x.MenuTypeId)
                            .Select(p => p.Code).FirstOrDefault()
                }).Where(x => x.UserId == userId && x.FunctionId == functionId && x.MenuTypeId==menuType).ToList();

                if (user.Count() == 0)
                {
                    try
                    {

                        //Insert Vehicle Release Info
                        var userRoleAccess = new TblUserFunction();
                        //userRole.Id = id;
                        userRoleAccess.UserId = userId;
                        userRoleAccess.RoleId = roleId;
                        userRoleAccess.FunctionId = functionId;
                        userRoleAccess.FunctionName = functionName;
                        userRoleAccess.IsActive = isActive;
                        // userRoleAccess.StartDate = DateTime.Parse(startDate);
                        DateTime parsedStartDate;
                        if (DateTime.TryParseExact(startDate,
                                                   "dd-MM-yyyy HH:mm",
                                                   CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None,
                                                   out parsedStartDate))
                        {
                            userRoleAccess.StartDate = parsedStartDate;
                        }
                        else
                        {
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "Invalid Start Date format.";
                            return Json(model);
                        }
                        //userRoleAccess.EndDate = DateTime.Parse(endDate);
                        userRoleAccess.CreationDate = utilityHelper.CurrentDateTime;
                        userRoleAccess.UpdateDate = utilityHelper.CurrentDateTime;
                        userRoleAccess.CreatedBy = loginUserID;
                        userRoleAccess.UpdatedBy = loginUserID;
                        userRoleAccess.MenuTypeId = menuType;

                        _context.TblUserFunctions.Add(userRoleAccess);
                        _context.SaveChanges();
                        model.Id = userRoleAccess.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "User Role Access has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Role Access not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "User Role Access Already Exist for the User! Please try again with diffrent User.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, Int32 userId, Int32 roleId, Int32 functionId,
            string functionName, string startDate, string endDate, Boolean isActive, int menuType)
        {
            VMUserRoleAccess model = new VMUserRoleAccess();
            int loginUserID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                loginUserID = userDetails.Id;
                try
                {
                    var userRoleAccess = _context.TblUserFunctions.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (userRoleAccess == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Role Access does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        userRoleAccess.UserId = userId;
                        userRoleAccess.RoleId = roleId;
                        userRoleAccess.FunctionId = functionId;
                        userRoleAccess.FunctionName = functionName;
                        userRoleAccess.IsActive = isActive;
                        userRoleAccess.StartDate = DateTime.Parse(startDate);
                        if (!string.IsNullOrEmpty(endDate) && !endDate.Equals("-"))
                        {
                            userRoleAccess.EndDate = DateTime.Parse(endDate);
                        }
                        //usrRole.CreateDate = utilityHelper.CurrentDateTime;
                        userRoleAccess.UpdateDate = utilityHelper.CurrentDateTime;
                        //usrRole.CreatedBy = userID;
                        userRoleAccess.UpdatedBy = loginUserID;
                        userRoleAccess.MenuTypeId = menuType;

                        _context.TblUserFunctions.Update(userRoleAccess);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "User Role Access has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "User Role Access not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteUserRoleAccess(string userRoleAccessId)
        {
            VMUserRoleAccess model = new VMUserRoleAccess();
            try
            {
                var userRole = _context.TblUserFunctions.Where(x => x.Id == Convert.ToInt32(userRoleAccessId));
                _context.TblUserFunctions.RemoveRange(userRole);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "User Role Access has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "User Role Access has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopUserRoleAccessList()
        {
            List<VMUserRoleAccess> model = new List<VMUserRoleAccess>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblUserFunctions.Select(x => new VMUserRoleAccess
            {
                Id = x.Id,
                UserId = x.UserId,
                RoleId = x.RoleId,
                FunctionId = x.FunctionId,
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
                UserName = _context.TblUserMasters
                            .Where(p => p.Id == x.UserId)
                            .Select(p => p.UserName).FirstOrDefault(),
                RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                FunctionMasterName = _context.TblFunctionMasters
                            .Where(p => p.Id == x.FunctionId)
                            .Select(p => p.FunctionName).FirstOrDefault(),
                Codes = _context.TblCodeMasters.Where(p => p.Id == x.MenuTypeId)
        .Select(p => p.Code).FirstOrDefault()
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

        public JsonResult getMenuList()
        {
            var item = _context.TblCodeMasters.Where(n => n.CodeType == "MENUTYPE").Select(x => new
            {
                CodeTypeId = x.Id,
                Code = x.Code,
            }).Distinct().ToList();
            return Json(item);
        }
    }
}
