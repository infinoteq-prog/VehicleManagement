using VMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VMS.Helper;
using VMS.ViewModel;

namespace VMS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VmsDbContext _context;
        //private readonly UserRoleAccessController userAccess;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}


        public HomeController(VmsDbContext context)
        {
            _context = context;
            //userAccess = new UserRoleAccessController(_context);
        }

        public IActionResult Index()
        {
            var userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails == null)
            {
                //Logging out a user because user sesstion is null
                return RedirectToAction("Index", "Login");
            }
            return View(userDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
