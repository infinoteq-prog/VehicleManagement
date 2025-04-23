using VMS.Models;
using Microsoft.AspNetCore.Mvc;
using VMS.ViewModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using VMS.Helper;
using Microsoft.AspNetCore.Authentication;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace VMS.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<LoginController> _logger;
        private readonly VmsDbContext _context;
        //public LoginController(ILogger<LoginController> logger)
        //{
        //    _logger = logger;
        //}

        public LoginController(VmsDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var model = new VMLogin
            {
                UserName = "hemantsharma24",
                Password = "test"
            };
            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public JsonResult ValidateUser(string userName, string password)
        {
            VMLogin userDetails = utilityHelper.validateUserAndStoreSession(userName.Trim(), password.Trim());
            return Json(userDetails);
        }

        [HttpPost]
        public ActionResult ChangePasswordRequest(string newPassword)
        {
            VMChangePassword model = new VMChangePassword();

            VMLogin loginDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");

            if (loginDetails != null)
            {
                var userDetails = _context.TblUserMasters.Where(x => x.UserName.Equals(loginDetails.UserName.Trim())).FirstOrDefault();
                if (userDetails != null)
                {
                    userDetails.Password = newPassword;
                    _context.TblUserMasters.Update(userDetails);
                    _context.SaveChanges();
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Password has been updated successfully.";
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Username does not exists!";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            HttpContext.Session.SetObjectAsJson("userDetails", null);
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return Json(true);
        }

        [HttpGet]
        public ActionResult getUserDetails()
        {
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails == null)
            {
                HttpContext.Session.Clear();
                HttpContext.SignOutAsync();
                return Json(null);
            }
            else
            {
                return Json(userDetails);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
