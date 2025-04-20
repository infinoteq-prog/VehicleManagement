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
    public class UserSubscriptionController : Controller
    {
        private readonly ILogger<UserRoleController> _logger;
        private readonly VmsDbContext _context;

        public UserSubscriptionController(VmsDbContext context)
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

        public ActionResult Details(String userSubscriptionId)
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
            ViewBag.userSubscriptionId = userSubscriptionId;
            return View("Details");
        }

        public ActionResult Update(String userSubscriptionId)
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
            ViewBag.userSubscriptionId = userSubscriptionId;
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
        public JsonResult getUserSubscriptionByID(string userSubscriptionId)
        {
            VMUserSubscription model = new VMUserSubscription();
            int subscriptionId = Convert.ToInt32(userSubscriptionId);
            model = _context.TblUserSubscriptions.Where(x => x.Id == subscriptionId).Select(x => new VMUserSubscription
            {
                Id = x.Id,
                UserId = x.UserId,
                FinYear = x.FinYear,
                Amount = x.Amount,
                IsActive = x.IsActive,
                PaidDate = x.PaidDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PaidDateString = String.IsNullOrEmpty(Convert.ToString(x.PaidDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.PaidDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
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
        public JsonResult getUserSubscriptionList()
        {
            List<VMUserSubscription> model = new List<VMUserSubscription>();

            model = _context.TblUserSubscriptions.Select(x => new VMUserSubscription
            {
                Id = x.Id,
                UserId = x.UserId,
                FinYear = x.FinYear,
                Amount = x.Amount,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                PaidDate = x.PaidDate,
                EndDate = x.EndDate,
                PaidDateString = String.IsNullOrEmpty(Convert.ToString(x.PaidDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.PaidDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
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
        public JsonResult searchUserSubscription(Int32 userId, string finYear, string amount, string paidDate, string startDate, string endDate, Boolean isActive)
        {
            List<VMUserSubscription> model = new List<VMUserSubscription>();

            var searchModel = _context.TblUserSubscriptions.Select(x => new VMUserSubscription
            {
                Id = x.Id,
                UserId = x.UserId,
                FinYear = x.FinYear,
                Amount = x.Amount,
                IsActive = x.IsActive,
                PaidDate = x.PaidDate,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PaidDateString = String.IsNullOrEmpty(Convert.ToString(x.PaidDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.PaidDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
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
                            .Select(p => p.UserName).FirstOrDefault()
            });

            if (userId != 0)
            {
                searchModel = searchModel.Where(s => s.UserId == userId);
            }

            if (!string.IsNullOrEmpty(finYear))
            {
                searchModel = searchModel.Where(s => s.FinYear == finYear);
            }

            if (!string.IsNullOrEmpty(amount))
            {
                searchModel = searchModel.Where(s => s.Amount == Convert.ToDecimal(amount));
            }

            if (!string.IsNullOrEmpty(paidDate))
            {
                searchModel = searchModel.Where(s => s.PaidDate == DateTime.Parse(paidDate));
            }
            
            if (!string.IsNullOrEmpty(startDate))
            {
                searchModel = searchModel.Where(s => s.StartDate == DateTime.Parse(startDate));
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                searchModel = searchModel.Where(s => s.EndDate == DateTime.Parse(endDate));
            }

            //archModel = searchModel.Where(s => s.IsActive == isActive);

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
        public ActionResult Save(Int32 id, Int32 userId, string finYear, string amount, string paidDate, string startDate, string endDate, Boolean isActive)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblUserSubscriptions.Select(x => new
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    FinYear = x.FinYear,
                    Amount = x.Amount,
                    IsActive = x.IsActive,
                    PaidDate = x.PaidDate,
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
                }).Where(x => x.UserId == userId).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Vehicle Release Info
                        var userSubscription = new TblUserSubscription();
                        //userSubscription.Id = id;
                        userSubscription.UserId = userId;
                        userSubscription.FinYear = finYear;
                        userSubscription.Amount = Convert.ToDecimal(amount);
                        userSubscription.IsActive = isActive;
                        userSubscription.PaidDate = DateTime.Parse(paidDate);
                        userSubscription.StartDate = DateTime.Parse(startDate);
                        userSubscription.EndDate = DateTime.Parse(endDate);
                        userSubscription.CreationDate = utilityHelper.CurrentDateTime;
                        userSubscription.UpdateDate = utilityHelper.CurrentDateTime;
                        userSubscription.CreatedBy = userID;
                        userSubscription.UpdatedBy = userID;

                        _context.TblUserSubscriptions.Add(userSubscription);
                        _context.SaveChanges();
                        model.Id = userSubscription.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "User Subacription has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Subacription not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "User Subacription Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, Int32 UserId, string finYear, string Amount, Boolean isActive, string paidDate, string startDate, string endDate)
        {
            VMUserSubscription model = new VMUserSubscription();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var userSubscription = _context.TblUserSubscriptions.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (userSubscription == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "User Subacription does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        userSubscription.UserId = UserId;
                        userSubscription.FinYear = finYear;
                        userSubscription.Amount = Convert.ToDecimal(Amount);
                        userSubscription.IsActive = isActive;
                        userSubscription.StartDate = DateTime.Parse(startDate);
                        userSubscription.EndDate = DateTime.Parse(endDate);
                        userSubscription.PaidDate = DateTime.Parse(paidDate);
                        //userSubscription.CreateDate = utilityHelper.CurrentDateTime;
                        userSubscription.UpdateDate = utilityHelper.CurrentDateTime;
                        //userSubscription.CreatedBy = userID;
                        userSubscription.UpdatedBy = userID;

                        _context.TblUserSubscriptions.Update(userSubscription);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "User Subacription has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "User Subacription not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteUserSubscription(string userSubscriptionId)
        {
            VMUserSubscription model = new VMUserSubscription();
            try
            {
                var userSubscription = _context.TblUserSubscriptions.Where(x => x.Id == Convert.ToInt32(userSubscriptionId));
                _context.TblUserSubscriptions.RemoveRange(userSubscription);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "User Subacription has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "User Subacription has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopUserSubscriptionsList()
        {
            List<VMUserSubscription> model = new List<VMUserSubscription>();

            //Query to fetch last 10 saved records in user subscription list from table
            model = _context.TblUserSubscriptions.Select(x => new VMUserSubscription
            {
                Id = x.Id,
                UserId = x.UserId,
                FinYear = x.FinYear,
                Amount = x.Amount,
                PaidDate = x.PaidDate,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                PaidDateString = String.IsNullOrEmpty(Convert.ToString(x.PaidDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.PaidDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
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
