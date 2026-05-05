using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace VMS.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly VmsDbContext _context;

        public UserController(VmsDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc = null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddUserEntryViewAccess = userAcc.AddUserEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String userID)
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddUserEntryViewAccess = userAcc.AddUserEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userID = userID;
            return View("Details");
        }

        public ActionResult Update(String userID)
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddUserEntryViewAccess = userAcc.AddUserEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userID = userID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddUserEntryViewAccess = userAcc.AddUserEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getUserRole()
        {
            return Json(_context.TblRoleMasters.Select(x => new
            {
                RoleID = x.Id,
                RoleName = x.RoleName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getUsersList()
        {
            return Json(_context.TblUserMasters.Select(x => new
            {
                UserID = x.Id,
                UserName = x.UserName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getUserListByRole()
        {
            Int32 roleId = 4;
            return Json(_context.TblUserMasters.Select(x => new
            {
                id = x.Id,
                UserName = x.UserName,
                UserId = x.UserId,
                roleId = x.RoleId
            }).Where(x => x.roleId.Equals(roleId)).ToList());
        }

        [HttpGet]
        public JsonResult getUserbyID(string userID)
        {
            VMUser model = new VMUser();

            model = (from u in _context.TblUserMasters
                     //join rm in _context.TblRoleMasters on u.r equals rm.Id
                     select new VMUser()
                     {
                         Id =u.Id.ToString(),
                         UserId = u.UserId,
                         UserName = u.UserName,
                         Email = u.EmailId,
                         PhoneNumber = u.MobileNo,
                         Password = u.Password,
                         StartDate = u.StartDate,
                         EndDate = u.EndDate,
                         StartDateString = String.IsNullOrEmpty(Convert.ToString(u.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         IsActive = u.IsActive,
                         CreateDate = u.CreationDate,
                         UpdateDate = u.UpdateDate,
                         CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                         UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == u.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                         RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == u.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                     }).Where(x => x.Id == userID).FirstOrDefault();

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
        public JsonResult getUserList()
        {
            List<VMUser> model = new List<VMUser>();

            model = (from u in _context.TblUserMasters
                     //join ur in _context.AspNetUserRolesNews on u.Id equals ur.UserId
                     //join sr in _context.AspNetRoles on ur.RoleId equals sr.Id
                     select new VMUser()
                     {
                         Id = Convert.ToString(u.Id),
                         UserId = u.UserId,
                         UserName = u.UserName,
                         Email = u.EmailId,
                         PhoneNumber = u.MobileNo,
                         Password = u.Password,
                         StartDate = u.StartDate,
                         EndDate = u.EndDate,
                         StartDateString = String.IsNullOrEmpty(Convert.ToString(u.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         IsActive = u.IsActive,
                         CreateDate = u.CreationDate,
                         UpdateDate = u.UpdateDate,
                         CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                         UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == u.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                         RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == u.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
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
        public JsonResult searchUserList(string role, string startDate, string endDate,
                                         string userName, string userId, string email, 
                                         string phone, Boolean isActive)
        {
            List<VMUser> model = new List<VMUser>();

            var searchModel = (from u in _context.TblUserMasters
                                   //join ur in _context.AspNetUserRolesNews on u.Id equals ur.UserId
                                   //join sr in _context.AspNetRoles on ur.RoleId equals sr.Id
                               select new VMUser()
                              {
                                  Id = Convert.ToString(u.Id),
                                  UserId = u.UserId,
                                  UserName = u.UserName,
                                  Email = u.EmailId,
                                  PhoneNumber = u.MobileNo,
                                  Password = u.Password,
                                  StartDate = u.StartDate,
                                  EndDate = u.EndDate,
                                  StartDateString = String.IsNullOrEmpty(Convert.ToString(u.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                  EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                  IsActive = u.IsActive,
                                  CreateDate = u.CreationDate,
                                  UpdateDate = u.UpdateDate,
                                  CreatedBy = _context.TblUserMasters
                                                .Where(p => p.Id == u.CreatedBy)
                                                .Select(p => p.UserName).FirstOrDefault(),
                                  UpdateBy = _context.TblUserMasters
                                                .Where(p => p.Id == u.UpdatedBy)
                                                .Select(p => p.UserName).FirstOrDefault(),
                                  RoleName = _context.TblRoleMasters
                                            .Where(p => p.Id == u.RoleId)
                                            .Select(p => p.RoleName).FirstOrDefault(),
                               });

            if (!string.IsNullOrEmpty(role) && role != "Select a Role")
            {
                searchModel = searchModel.Where(s => s.RoleName.Equals(role));
            }

            if (!string.IsNullOrEmpty(startDate))
            {
                searchModel = searchModel.Where(s => s.StartDate == DateTime.Parse(startDate));
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                searchModel = searchModel.Where(s => s.EndDate == DateTime.Parse(endDate));
            }

            if (!string.IsNullOrEmpty(userName))
            {
                searchModel = searchModel.Where(s => s.UserName.Equals(userName));
            }

            if (!string.IsNullOrEmpty(userId))
            {
                searchModel = searchModel.Where(s => s.UserId.Equals(userId));
            }

            if (!string.IsNullOrEmpty(email))
            {
                searchModel = searchModel.Where(s => s.Email.Equals(email));
            }
            if (!string.IsNullOrEmpty(phone))
            {
                searchModel = searchModel.Where(s => s.PhoneNumber.Equals(phone));
            }

           // searchModel = searchModel.Where(s => s.IsActive == isActive);

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
        public ActionResult Save(string role, string startDate, string endDate,
                                 string userName, string userId, string password, string email,
                                 string phone, Boolean isActive)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblUserMasters.Select(x => new
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    UserName = x.UserName,
                    Email = x.EmailId,
                    Phone = x.MobileNo,
                    Password = x.Password,
                    StartDate = x.StartDate,
                    EndDate = x.EndDate,
                    IsActive = x.IsActive,
                    CreateDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                }).Where(x => x.UserName == userName
                        || x.Email == email
                        || x.Phone == phone).ToList();


                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert into User Master Table
                        var userEntry = new TblUserMaster();
                        userEntry.UserName = userName;
                        userEntry.UserId = userId;
                        userEntry.EmailId = email;
                        userEntry.MobileNo = phone;
                        userEntry.Password = password;
                        if (!string.IsNullOrEmpty(role))
                        {
                            userEntry.RoleId = Convert.ToInt32(role);
                        }
                        // userEntry.StartDate = DateTime.Parse(startDate);
                        if (!string.IsNullOrEmpty(startDate))
                        {
                            try
                            {
                                var parts = startDate.Split(' ');
                                var year = parts[0];
                                var timeAndDate = parts[1];

                                var timeDateParts = timeAndDate.Split('-');
                                var time = timeDateParts[0];
                                var month = timeDateParts[1];
                                var day = timeDateParts[2];

                                var correctedDate = $"{year}-{month}-{day} {time}";

                                userEntry.StartDate = DateTime.ParseExact(
                                    correctedDate,
                                    "yyyy-MM-dd HH:mm",
                                    System.Globalization.CultureInfo.InvariantCulture
                                );
                            }
                            catch
                            {
                                model.TransactionMessage.Status = TransactionStatus.Failed;
                                model.TransactionMessage.Message = "Invalid Start Date format.";
                                return Json(model);
                            }
                        }
                        //userEntry.EndDate = DateTime.Parse(endDate);
                        userEntry.CreationDate = utilityHelper.CurrentDateTime;
                        userEntry.UpdateDate = utilityHelper.CurrentDateTime;
                        userEntry.CreatedBy = userID;
                        userEntry.UpdatedBy = userID;
                        userEntry.IsActive = isActive;
                        _context.TblUserMasters.Add(userEntry);

                        _context.SaveChanges();
                        model.Id = userEntry.Id.ToString();
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "User Details has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Details not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "User Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, string role, string startDate, string endDate,
                                   string userName, string userId, string password, string email,
                                   string phone, Boolean isActive)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var user = _context.TblUserMasters.Select(x => new
                    {
                        Id = x.Id,
                        UserId = x.UserId,
                        UserName = x.UserName,
                        Email = x.EmailId,
                        Phone = x.MobileNo,
                        Password = x.Password,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        IsActive = x.IsActive,
                        CreateDate = x.CreationDate,
                        UpdateDate = x.UpdateDate,
                        CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                        UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                        RoleName = _context.TblRoleMasters
                            .Where(p => p.Id == x.RoleId)
                            .Select(p => p.RoleName).FirstOrDefault(),
                    }).Where(x => x.UserName == userName
                            && x.Id != id).ToList();

                    if (user.Count() == 0)
                    {
                        var userEntry = _context.TblUserMasters.Where(x => x.Id == id).FirstOrDefault();

                        if (userEntry == null)
                        {
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "User does not Exists! Please check and try again.";
                            return Json(model);
                        }
                        else
                        {
                            //Updateing Existing User Details
                            userEntry.UserName = userName;
                            if (!string.IsNullOrEmpty(role))
                            {
                                userEntry.RoleId = Convert.ToInt32(role);
                            }
                            userEntry.UserId = userId;
                            userEntry.EmailId = email;
                            userEntry.MobileNo = phone;
                            userEntry.Password = password;
                            userEntry.StartDate = DateTime.Parse(startDate);
                            if (!string.IsNullOrEmpty(endDate) && !endDate.Equals("-"))
                            {
                                userEntry.EndDate = DateTime.Parse(endDate);
                            }
                            userEntry.CreationDate = utilityHelper.CurrentDateTime;
                            userEntry.UpdateDate = utilityHelper.CurrentDateTime;
                            userEntry.CreatedBy = userID;
                            userEntry.UpdatedBy = userID;
                            userEntry.IsActive = isActive;

                            _context.TblUserMasters.Update(userEntry);
                            _context.SaveChanges();
                            // model.Id = id;

                            //Updating User Roles
                            //var userRole = _context.AspNetUserRolesNews.Where(x => x.UserId == id).FirstOrDefault();
                            //if (userRole != null)
                            //{
                            //    userRole.UserId = id;
                            //    userRole.RoleId = role;
                            //    _context.AspNetUserRolesNews.Update(userRole);
                            //    _context.SaveChanges();
                            //}  
                        }
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "User Details has been updated successfully.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "UserName Already Exist for a diffrent user! Please try again with diffrent username.";
                    }
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "User Details not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteUser(int userID)
        {
            VMUser model = new VMUser();
            try
            {
                var userEntry = _context.TblUserMasters.Where(x => x.Id == userID);
                _context.TblUserMasters.RemoveRange(userEntry);
                _context.SaveChanges();

                //var userRole = _context.AspNetUserRolesNews.Where(x => x.UserId == userID).FirstOrDefault();
                //if (userRole != null)
                //{
                //    _context.AspNetUserRolesNews.RemoveRange(userRole);
                //    _context.SaveChanges();
                //}

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "User has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "User has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopUserList()
        {
            List<VMUser> model = new List<VMUser>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = (from u in _context.TblUserMasters
                          //join ur in _context.AspNetUserRolesNews on u.Id equals ur.UserId
                          //join sr in _context.AspNetRoles on ur.RoleId equals sr.Id
                          select new VMUser()
                          {
                              Id = u.Id.ToString(),
                              UserId = u.UserId,
                              UserName = u.UserName,
                              Email = u.EmailId,
                              PhoneNumber = u.MobileNo,
                              Password = u.Password,
                              StartDate = u.StartDate,
                              EndDate = u.EndDate,
                              StartDateString = String.IsNullOrEmpty(Convert.ToString(u.StartDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.StartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                              EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                              IsActive = u.IsActive,
                              CreateDate = u.CreationDate,
                              UpdateDate = u.UpdateDate,
                              CreatedBy = _context.TblUserMasters
                                            .Where(p => p.Id == u.CreatedBy)
                                            .Select(p => p.UserName).FirstOrDefault(),
                              UpdateBy = _context.TblUserMasters
                                            .Where(p => p.Id == u.UpdatedBy)
                                            .Select(p => p.UserName).FirstOrDefault(),
                              //RoleName = rm.Role,
                              RoleName = _context.TblRoleMasters
                                            .Where(p => p.Id == u.RoleId)
                                            .Select(p => p.RoleName).FirstOrDefault(),
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
