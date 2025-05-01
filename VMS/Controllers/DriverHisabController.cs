using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using static iTextSharp.text.pdf.PdfDiv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Bibliography;

namespace VMS.Controllers
{
    public class DriverHisabController : Controller
    {
        private readonly ILogger<DriverHisabController> _logger;
        private readonly VmsDbContext _context; private readonly string _connectionString;
        public DriverHisabController(VmsDbContext context, IConfiguration configuration)
        {
            _context = context; 
            _connectionString = configuration.GetConnectionString("VMSContext"); 
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(String tripid)
        {
            ViewBag.tripid = tripid;
            return View("Details");
        }

        public ActionResult Update(String tripid)
        {
            ViewBag.tripid = tripid;
            return View("Update");
        }

        public ActionResult Print(String tripid)
        {
            ViewBag.tripid = tripid;
            return View("Print");
        }

        public ActionResult List()
        {
            return View();
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = utilityHelper.getdownloadFilePath(fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpGet]
        public JsonResult getDriverMaster()
        {
            return Json(_context.TblDriverMasters.Select(x => new
            {
                DriverId = x.Id,
                DriverName = x.DriverName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getCurrentSettlementNumber(int driverId)
        {
            var model = _context.TblDriverHisabHeaders
                         .Where(y => y.IsActive==true && y.DriverId.Equals(driverId))
                         .OrderByDescending(x => x.SettlementNo) // Ensure highest TripId is picked
                         .Select(x => new
                         {
                             SettlementNo = x.SettlementNo == 0 ? 1 : x.SettlementNo + 1,
                             vehicleId = x.VehicleNo
                         })
                         .FirstOrDefault();

            if (model != null)
            {
                return Json(model);
            }
            else
            {
                var settlement = new
                {
                    SettlementNo = 0,
                };
                return Json(settlement);
            }
        }

        [HttpGet]
        public JsonResult getLastDriverTripHistory(int driverId)
        {
            var model = (from driver in _context.TblDriverHisabHeaders
                         join vehicle in _context.TblVehicleMasters
                             on driver.VehicleNo equals vehicle.Id
                         where driver.IsActive == true && driver.DriverId == Convert.ToInt32(driverId)
                         orderby driver.SettlementNo descending
                         select new
                         {
                             SettlementNo = driver.SettlementNo,
                             LastTripStartDate = Convert.ToString(Convert.ToDateTime(driver.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             LastTripEndDate = Convert.ToString(Convert.ToDateTime(driver.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             LastTripRouteDescr = driver.RouteDescription,
                             OpeningDiesel = driver.OpeningBalance == null || driver.OpeningBalance == 0
                                             ? 1
                                             : driver.OpeningBalance,
                             LastTripVendor = "Test Vendor",
                             LastTripDriver = _context.TblDriverMasters
                                                .Where(p => p.Id == driver.DriverId)
                                                .Select(p => p.DriverName)
                                                .FirstOrDefault(),
                             LastTripDriverFatherName = _context.TblDriverMasters
                                                        .Where(p => p.Id == driver.DriverId)
                                                        .Select(p => p.FatherName)
                                                        .FirstOrDefault(),
                             VehicleNumber = vehicle.VehicleNo // <-- From joined VehicleMaster
                         }).FirstOrDefault();

            return Json(model);
        }

        [HttpPost]
        public JsonResult searchDriverHisabMaster(int id, int vehicleNo, int driverId, string tripStartDate,
          string tripEndDate, int openingDiesel)
        {
           var model = _context.TblDriverHisabHeaders
               .Join(_context.TblVehicleMasters,
                   driver => driver.DriverId,
                   vehicle => vehicle.Id,
                   (driver, vehicle) => new { driver, vehicle })
               .Where(x => x.driver.IsActive == true &&
                   // Check if vehicleNo is greater than 0, otherwise ignore the condition
                   (vehicleNo > 0 ? x.driver.VehicleNo == vehicleNo : true) &&

                   // Check if driverId is greater than 0, otherwise ignore the condition
                   (driverId > 0 ? x.driver.DriverId == driverId : true) &&

                   // Check if tripStartDate is not null or empty
                   (!string.IsNullOrEmpty(tripStartDate) ? Convert.ToString(Convert.ToDateTime(x.driver.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)) == tripStartDate : true) &&

                   // Check if tripEndDate is not null or empty
                   (!string.IsNullOrEmpty(tripEndDate) ? Convert.ToString(Convert.ToDateTime(x.driver.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)) == tripEndDate : true) &&

                   // Check if openingDiesel is greater than 0
                   (openingDiesel > 0 ? x.driver.OpeningBalance == openingDiesel : true)
               )
               .Select(x => new
               {
                   SettlementNo = x.driver.SettlementNo,
                   VehicleNumber = x.vehicle.VehicleNo,
                   DriverId = x.driver.DriverId,
                   TripStartDate = Convert.ToString(Convert.ToDateTime(x.driver.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                   TripEndDate = Convert.ToString(Convert.ToDateTime(x.driver.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                   CreationDate = Convert.ToString(Convert.ToDateTime(x.driver.CreationDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                   UpdateDate = Convert.ToString(Convert.ToDateTime(x.driver.UpdateDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                   CreatedBy = "",
                   UpdatedBy = "",
                   LastTripRouteDescr = x.driver.RouteDescription,
                   OpeningDiesel = x.driver.OpeningBalance,
                   IsActive = x.driver.IsActive,
                   LastTripVendor = "Test Vendor",
                   DieselHeaderCreationDate = x.driver.CreationDate,
                   DieselHeaderUpdateDate = x.driver.UpdateDate,
                   DieselHeaderCreatedBy = x.driver.CreatedBy,
                   DieselHeaderUpdatedBy = x.driver.UpdatedBy,
                   DriverName = _context.TblDriverMasters
                       .Where(p => p.Id == x.driver.DriverId)
                       .Select(p => p.DriverName).FirstOrDefault(),
                   DriverFatherName = _context.TblDriverMasters
                       .Where(p => p.Id == x.driver.DriverId)
                       .Select(p => p.FatherName).FirstOrDefault(),
                   DriverHeaderCreatedByName = _context.TblUserMasters
                       .Where(p => p.Id == x.driver.CreatedBy)
                       .Select(p => p.UserName).FirstOrDefault(),
                   DriverHeaderUpdatedByName = _context.TblUserMasters
                       .Where(p => p.Id == x.driver.UpdatedBy)
                       .Select(p => p.UserName).FirstOrDefault()
               }).ToList();


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
        public JsonResult getTopDriverHisabList()
        {
            var model = (from x in _context.TblDriverHisabHeaders
                         join vehicle in _context.TblVehicleMasters
                             on x.VehicleNo equals vehicle.Id
                         where x.IsActive == true
                         select new
                         {
                             SettlementNo = x.SettlementNo,
                             VehicleNo = vehicle.VehicleNo,
                             DriverId = x.DriverId,
                             TripStartDate = Convert.ToString(Convert.ToDateTime(x.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             TripEndDate = Convert.ToString(Convert.ToDateTime(x.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             LastTripRouteDescr = x.RouteDescription,
                             CreationDate = Convert.ToString(Convert.ToDateTime(x.CreationDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             UpdateDate = Convert.ToString(Convert.ToDateTime(x.UpdateDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),                           
                             OpeningDiesel = x.OpeningBalance,
                             IsActive = x.IsActive,
                             LastTripVendor = "Test Vendor",
                             DriverHeaderCreatedBy = x.CreatedBy,
                             DriverHeaderUpdatedBy = x.UpdatedBy,
                             DriverName = _context.TblDriverMasters
                                 .Where(p => p.Id == x.DriverId)
                                 .Select(p => p.DriverName).FirstOrDefault(),
                             DriverFatherName = _context.TblDriverMasters
                                            .Where(p => p.Id == x.DriverId)
                                            .Select(p => p.FatherName).FirstOrDefault(),
                             DriverHeaderCreatedByName = _context.TblUserMasters
                             .Where(p => p.Id == x.CreatedBy)
                             .Select(p => p.UserName).FirstOrDefault(),
                             DriverHeaderUpdatedByName = _context.TblUserMasters
                             .Where(p => p.Id == x.UpdatedBy)
                             .Select(p => p.UserName).FirstOrDefault()
                         }).OrderByDescending(x => x.SettlementNo).ToList();

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
