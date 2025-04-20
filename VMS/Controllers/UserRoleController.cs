using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using System.Globalization;

namespace VMS.Controllers
{
    public class UserRoleController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly VmsDbContext _context;

        public UserRoleController(VmsDbContext context)
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

        public ActionResult Details(String userRoleID)
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
            ViewBag.userRoleID = userRoleID;
            return View("Details");
        }

        public ActionResult Update(String userRoleID)
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
            ViewBag.userRoleID = userRoleID;
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
        public JsonResult getUserRolebyID(string userRoleID)
        {
            VMUserRole model = new VMUserRole();
            int roleID = Convert.ToInt32(userRoleID);
            model = _context.TblRoleMasters.Where(x => x.Id == roleID).Select(x => new VMUserRole
            {
                Id = x.Id.ToString(),
                Role = x.Role,
                RoleName = x.RoleName,
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
        public JsonResult getUserRoleList()
        {
            List<VMUserRole> model = new List<VMUserRole>();

            model = _context.TblRoleMasters.Select(x => new VMUserRole
            {
                Id = x.Id.ToString(),
                Role = x.Role,
                RoleName = x.RoleName,
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
        public JsonResult searchUserRole(string roleName, Boolean isActive, string startDate, string endDate)
        {
            List<VMUserRole> model = new List<VMUserRole>();

            var searchModel = _context.TblRoleMasters.Select(x => new VMUserRole
            {
                Id = x.Id.ToString(),
                Role = x.Role,
                RoleName = x.RoleName,
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

            if (!string.IsNullOrEmpty(roleName))
            {
                searchModel = searchModel.Where(s => s.Role == roleName);
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
        public ActionResult Save(string roleName, string roleDescription, Boolean isActive, string startDate, string endDate)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblRoleMasters.Select(x => new
                {
                    Id = x.Id.ToString(),
                    Role = x.Role,
                    RoleName = x.RoleName,
                    IsActive = x.IsActive,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    CreateDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                }).Where(x => x.Role == roleName).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Int32 id = 0;
                        //Fetching Maximum User ID
                        //var maxID = _context.TblRoleMasters.Select(x => new
                        //{
                        //    Id = x.Id,
                        //}).OrderByDescending(u => u.Id).FirstOrDefault();

                        //if (maxID != null)
                        //{
                        //    id = Convert.ToInt32(maxID.Id) + 1;
                        //}

                        //Insert Vehicle Release Info
                        var userRole = new TblRoleMaster();
                        //userRole.Id = id;
                        userRole.Role = roleName;
                        userRole.RoleName = roleDescription;
                        userRole.IsActive = isActive;
                        userRole.StartDate = DateTime.Parse(startDate);
                        //userRole.EndDate = DateTime.Parse(endDate);
                        userRole.CreationDate = utilityHelper.CurrentDateTime;
                        userRole.UpdateDate = utilityHelper.CurrentDateTime;
                        userRole.CreatedBy = userID;
                        userRole.UpdatedBy = userID;

                        _context.TblRoleMasters.Add(userRole);
                        _context.SaveChanges();
                        model.Id = userRole.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "User Role has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Role not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "User Role Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }
        
        [HttpPost]
        public ActionResult Update(string id, string roleName, string roleDescription, Boolean isActive, string startDate, string endDate)
        {
            VMUserRole model = new VMUserRole();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var usrRole = _context.TblRoleMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (usrRole == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Role does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        usrRole.Role = roleName;
                        usrRole.RoleName = roleDescription;
                        usrRole.IsActive = isActive;
                        usrRole.StartDate = DateTime.Parse(startDate);
                        if (!string.IsNullOrEmpty(endDate) && !endDate.Equals("-"))
                        {
                            usrRole.EndDate = DateTime.Parse(endDate);
                        }
                        //usrRole.EndDate = DateTime.Parse(endDate);
                        //usrRole.CreateDate = utilityHelper.CurrentDateTime;
                        usrRole.UpdateDate = utilityHelper.CurrentDateTime;
                        //usrRole.CreatedBy = userID;
                        usrRole.UpdatedBy = userID;

                        _context.TblRoleMasters.Update(usrRole);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "User Role has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "User Role not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteUserRole(string userRoleID)
        {
            VMUserRole model = new VMUserRole();
            try
            {
                var userRole = _context.TblRoleMasters.Where(x => x.Id == Convert.ToInt32(userRoleID));
                _context.TblRoleMasters.RemoveRange(userRole);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "User Role has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "User Role has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopUserRoleList()
        {
            List<VMUserRole> model = new List<VMUserRole>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblRoleMasters.Select(x => new VMUserRole
            {
                Id = x.Id.ToString(),
                Role = x.Role,
                RoleName = x.RoleName,
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