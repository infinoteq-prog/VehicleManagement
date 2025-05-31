using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;

namespace VMS.Controllers
{
    public class VehicleMasterController : Controller
    {
        private readonly ILogger<VehicleMasterController> _logger;
        private readonly VmsDbContext _context;
        private string _controllerName = "VehicleMaster";
        public VehicleMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMVehicleMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleMasterAccess = userAcc.AddVehicleMasterAccess;
            //    ViewBag.AddVehicleMasterAddAccess = userAcc.AddVehicleMasterAddAccess;
            //    ViewBag.AddVehicleMasterUpdateAccess = userAcc.AddVehicleMasterUpdateAccess;
            //    ViewBag.AddVehicleMasterDeleteAccess = userAcc.AddVehicleMasterDeleteAccess;
            //    ViewBag.AddVehicleMasterViewAccess = userAcc.AddVehicleMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String vehicleMasterID)
        {
            //Code for User Access Function
            //VMVehicleMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleMasterAccess = userAcc.AddVehicleMasterAccess;
            //    ViewBag.AddVehicleMasterAddAccess = userAcc.AddVehicleMasterAddAccess;
            //    ViewBag.AddVehicleMasterUpdateAccess = userAcc.AddVehicleMasterUpdateAccess;
            //    ViewBag.AddVehicleMasterDeleteAccess = userAcc.AddVehicleMasterDeleteAccess;
            //    ViewBag.AddVehicleMasterViewAccess = userAcc.AddVehicleMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.vehicleMasterID = vehicleMasterID;
            return View("Details");
        }

        public ActionResult Update(String vehicleMasterID)
        {
            //Code for User Access Function
            //VMVehicleMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleMasterAccess = userAcc.AddVehicleMasterAccess;
            //    ViewBag.AddVehicleMasterAddAccess = userAcc.AddVehicleMasterAddAccess;
            //    ViewBag.AddVehicleMasterUpdateAccess = userAcc.AddVehicleMasterUpdateAccess;
            //    ViewBag.AddVehicleMasterDeleteAccess = userAcc.AddVehicleMasterDeleteAccess;
            //    ViewBag.AddVehicleMasterViewAccess = userAcc.AddVehicleMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.vehicleMasterID = vehicleMasterID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMVehicleMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMVehicleMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddVehicleMasterAccess = userAcc.AddVehicleMasterAccess;
            //    ViewBag.AddVehicleMasterAddAccess = userAcc.AddVehicleMasterAddAccess;
            //    ViewBag.AddVehicleMasterUpdateAccess = userAcc.AddVehicleMasterUpdateAccess;
            //    ViewBag.AddVehicleMasterDeleteAccess = userAcc.AddVehicleMasterDeleteAccess;
            //    ViewBag.AddVehicleMasterViewAccess = userAcc.AddVehicleMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getVehicleMaster()
         {
            var j = _context.TblVehicleMasters.Select(x => new
            {
                VehicleId = x.Id,
                VehicleName = x.VehicleNo,
            }).ToList();
            return Json(j);
        }

        [HttpGet]
        public JsonResult getVehicleMasterByID(string vehicleMasterID)
        {
            VMVehicleMaster model = new VMVehicleMaster();
            int roleID = Convert.ToInt32(vehicleMasterID);
            model = _context.TblVehicleMasters.Where(x => x.Id == roleID).Select(x => new VMVehicleMaster
            {
                Id = x.Id,
                VehicleNo = x.VehicleNo,
                VehicleOwner = x.VehicleOwner,
                PurchaseDate = x.PurchaseDate.HasValue ? x.PurchaseDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                MfgYear = x.MfgYear,
                MakeId = x.MakeId,
                ModelId = x.ModelId,
                NoOfTyres = x.NoOfTyres,
                EngineNo = x.EngineNo,
                ChasisNo = x.ChasisNo,
                VehicleTypeId = x.VehicleTypeId,
                BodyTypeId = x.BodyTypeId,
                FinancerName = x.FinancerName,
                BodyManufacturerId = x.BodyManufacturerId,
                RunningKm = x.RunningKm,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                InsuranceDue = x.InsuranceDue.HasValue ? x.InsuranceDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                InsuranceDoc = x.InsuranceDoc,
                NationalPermitDue = x.NationalPermitDue.HasValue ? x.NationalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                NationalPermitDoc = x.NationalPermitDoc,
                LocalPermitDue = x.LocalPermitDue.HasValue ? x.LocalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                LocalPermitDoc = x.LocalPermitDoc,
                RcValidityDue = x.RcValidityDue.HasValue ? x.RcValidityDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RcDoc = x.RcDoc,
                RtoDue = x.RtoDue.HasValue ? x.RtoDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RtoDoc = x.RtoDoc,
                PollutionDue = x.PollutionDue.HasValue ? x.PollutionDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                PollutionDoc = x.PollutionDoc,
                FitnessDue = x.FitnessDue.HasValue ? x.FitnessDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                FitnessDoc = x.FitnessDoc,
                Dimension = x.Dimension,
                BranchOffice = x.BranchOffice,
                RcCapicity = x.RcCapicity,
                ActualCapicity = x.ActualCapicity,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                ModelName = Convert.ToString(_context.TblModelAverageMasters
                            .Where(p => p.Id == x.ModelId)
                            .Select(p => p.ModelNo).FirstOrDefault()),
                VehicleTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.VehicleTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyManufacturerName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyManufacturerId)
                            .Select(p => p.Description).FirstOrDefault(),
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
        public JsonResult getVehicleMasterList()
        {
            List<VMVehicleMaster> model = new List<VMVehicleMaster>();

            model = _context.TblVehicleMasters.Select(x => new VMVehicleMaster
            {
                Id = x.Id,
                VehicleNo = x.VehicleNo,
                VehicleOwner = x.VehicleOwner,
                PurchaseDate = x.PurchaseDate.HasValue ? x.PurchaseDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                MfgYear = x.MfgYear,
                MakeId = x.MakeId,
                ModelId = x.ModelId,
                NoOfTyres = x.NoOfTyres,
                EngineNo = x.EngineNo,
                ChasisNo = x.ChasisNo,
                VehicleTypeId = x.VehicleTypeId,
                BodyTypeId = x.BodyTypeId,
                FinancerName = x.FinancerName,
                BodyManufacturerId = x.BodyManufacturerId,
                RunningKm = x.RunningKm,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                InsuranceDue = x.InsuranceDue.HasValue ? x.InsuranceDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                InsuranceDoc = x.InsuranceDoc,
                NationalPermitDue = x.NationalPermitDue.HasValue ? x.NationalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                NationalPermitDoc = x.NationalPermitDoc,
                LocalPermitDue = x.LocalPermitDue.HasValue ? x.LocalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                LocalPermitDoc = x.LocalPermitDoc,
                RcValidityDue = x.RcValidityDue.HasValue ? x.RcValidityDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RcDoc = x.RcDoc,
                RtoDue = x.RtoDue.HasValue ? x.RtoDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RtoDoc = x.RtoDoc,
                PollutionDue = x.PollutionDue.HasValue ? x.PollutionDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                PollutionDoc = x.PollutionDoc,
                FitnessDue = x.FitnessDue.HasValue ? x.FitnessDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                FitnessDoc = x.FitnessDoc,
                Dimension = x.Dimension,
                BranchOffice = x.BranchOffice,
                RcCapicity = x.RcCapicity,
                ActualCapicity = x.ActualCapicity,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                ModelName = Convert.ToString(_context.TblModelAverageMasters
                            .Where(p => p.Id == x.ModelId)
                            .Select(p => p.ModelNo).FirstOrDefault()),
                VehicleTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.VehicleTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyManufacturerName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyManufacturerId)
                            .Select(p => p.Description).FirstOrDefault(),
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

        [HttpPost]
        public JsonResult searchVehicleMaster(string vehicleNo, string vehicleOwner, string purchaseDate, string mfgYear,
            Int32 makeId, Int32 modelId, string noOfTyres, string engineNo, string chasisNo, Int32 vehicleTypeId,
            Int32 bodyTypeId, string financerName, Int32 bodyManufacturerId, string runningKm, string panNo,
            string insuranceDue, string nationalPermitDue, string localPermitDue,  string rcValidityDue,  string rtoDue,
            string pollutionDue,  string fitnessDue,string dimension, string branchOffice, 
            string rcCapicity, string actualCapicity, Boolean isActive)
        {
            List<VMVehicleMaster> model = new List<VMVehicleMaster>();

            var searchModel = _context.TblVehicleMasters.Select(x => new VMVehicleMaster
            {
                Id = x.Id,
                VehicleNo = x.VehicleNo,
                VehicleOwner = x.VehicleOwner,
                PurchaseDate = x.PurchaseDate.HasValue ? x.PurchaseDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                MfgYear = x.MfgYear,
                MakeId = x.MakeId,
                ModelId = x.ModelId,
                NoOfTyres = x.NoOfTyres,
                EngineNo = x.EngineNo,
                ChasisNo = x.ChasisNo,
                VehicleTypeId = x.VehicleTypeId,
                BodyTypeId = x.BodyTypeId,
                FinancerName = x.FinancerName,
                BodyManufacturerId = x.BodyManufacturerId,
                RunningKm = x.RunningKm,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                InsuranceDue = x.InsuranceDue.HasValue ? x.InsuranceDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                InsuranceDoc = x.InsuranceDoc,
                NationalPermitDue = x.NationalPermitDue.HasValue ? x.NationalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                NationalPermitDoc = x.NationalPermitDoc,
                LocalPermitDue = x.LocalPermitDue.HasValue ? x.LocalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                LocalPermitDoc = x.LocalPermitDoc,
                RcValidityDue = x.RcValidityDue.HasValue ? x.RcValidityDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RcDoc = x.RcDoc,
                RtoDue = x.RtoDue.HasValue ? x.RtoDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RtoDoc = x.RtoDoc,
                PollutionDue = x.PollutionDue.HasValue ? x.PollutionDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                PollutionDoc = x.PollutionDoc,
                FitnessDue = x.FitnessDue.HasValue ? x.FitnessDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                FitnessDoc = x.FitnessDoc,
                Dimension = x.Dimension,
                BranchOffice = x.BranchOffice,
                RcCapicity = x.RcCapicity,
                ActualCapicity = x.ActualCapicity,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                ModelName = Convert.ToString(_context.TblModelAverageMasters
                            .Where(p => p.Id == x.ModelId)
                            .Select(p => p.ModelNo).FirstOrDefault()),
                VehicleTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.VehicleTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyManufacturerName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyManufacturerId)
                            .Select(p => p.Description).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });

            if (!string.IsNullOrEmpty(vehicleNo))
            {
                searchModel = searchModel.Where(s => s.VehicleNo.Equals(vehicleNo));
            }

            if (!string.IsNullOrEmpty(vehicleOwner))
            {
                searchModel = searchModel.Where(s => s.VehicleOwner.Equals(vehicleOwner));
            }

            if (!string.IsNullOrEmpty(purchaseDate))
            {
                searchModel = searchModel.Where(s => s.PurchaseDate.Equals(DateTime.Parse(purchaseDate).Date));
            }

            if (Convert.ToInt32(mfgYear) != 0)
            {
                searchModel = searchModel.Where(s => s.MfgYear == Convert.ToString(mfgYear));
            }

            if (makeId != 0)
            {
                searchModel = searchModel.Where(s => s.MakeId.Equals(makeId));
            }

            if (modelId != 0)
            {
                searchModel = searchModel.Where(s => s.ModelId.Equals(modelId));
            }

            if (Convert.ToInt32(noOfTyres) != 0)
            {
                searchModel = searchModel.Where(s => s.NoOfTyres == Convert.ToInt32(noOfTyres));
            }

            if (!string.IsNullOrEmpty(engineNo))
            {
                searchModel = searchModel.Where(s => s.EngineNo.Equals(engineNo));
            }

            if (!string.IsNullOrEmpty(chasisNo))
            {
                searchModel = searchModel.Where(s => s.ChasisNo.Equals(chasisNo));
            }

            if (vehicleTypeId != 0)
            {
                searchModel = searchModel.Where(s => s.VehicleTypeId.Equals(vehicleTypeId));
            }

            if (bodyTypeId != 0)
            {
                searchModel = searchModel.Where(s => s.BodyTypeId.Equals(modelId));
            }

            if (!string.IsNullOrEmpty(financerName))
            {
                searchModel = searchModel.Where(s => s.FinancerName.Equals(financerName));
            }

            if (bodyManufacturerId != 0)
            {
                searchModel = searchModel.Where(s => s.BodyManufacturerId.Equals(bodyManufacturerId));
            }

            if (Convert.ToInt32(noOfTyres) != 0)
            {
                searchModel = searchModel.Where(s => s.NoOfTyres == Convert.ToInt32(noOfTyres));
            }

            if (!string.IsNullOrEmpty(runningKm))
            {
                searchModel = searchModel.Where(s => s.RunningKm.Equals(runningKm));
            }

            if (!string.IsNullOrEmpty(panNo))
            {
                searchModel = searchModel.Where(s => s.PanNo.Equals(panNo));
            }

            if (!string.IsNullOrEmpty(insuranceDue))
            {
                searchModel = searchModel.Where(s => s.InsuranceDue.Equals(insuranceDue));
            }

            if (!string.IsNullOrEmpty(nationalPermitDue))
            {
                searchModel = searchModel.Where(s => s.NationalPermitDue.Equals(nationalPermitDue));
            }

            if (!string.IsNullOrEmpty(localPermitDue))
            {
                searchModel = searchModel.Where(s => s.LocalPermitDue.Equals(localPermitDue));
            }

            if (!string.IsNullOrEmpty(rcValidityDue))
            {
                searchModel = searchModel.Where(s => s.RcValidityDue.Equals(rcValidityDue));
            }

            if (!string.IsNullOrEmpty(rtoDue))
            {
                searchModel = searchModel.Where(s => s.RtoDue.Equals(rtoDue));
            }

            if (!string.IsNullOrEmpty(pollutionDue))
            {
                searchModel = searchModel.Where(s => s.PollutionDue.Equals(pollutionDue));
            }

            if (!string.IsNullOrEmpty(fitnessDue))
            {
                searchModel = searchModel.Where(s => s.FitnessDue.Equals(fitnessDue));
            }

            if (!string.IsNullOrEmpty(dimension))
            {
                searchModel = searchModel.Where(s => s.Dimension.Equals(dimension));
            }

            if (!string.IsNullOrEmpty(branchOffice))
            {
                searchModel = searchModel.Where(s => s.BranchOffice.Equals(branchOffice));
            }

            if (!string.IsNullOrEmpty(rcCapicity))
            {
                searchModel = searchModel.Where(s => s.RcCapicity.Equals(rcCapicity));
            }

            if (!string.IsNullOrEmpty(actualCapicity))
            {
                searchModel = searchModel.Where(s => s.ActualCapicity.Equals(actualCapicity));
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
        public ActionResult Save(string vehicleNo, string vehicleOwner, string purchaseDate, string mfgYear,
            Int32 makeId, Int32 modelId, string noOfTyres, string engineNo, string chasisNo, Int32 vehicleTypeId,
            Int32 bodyTypeId, string financerName, Int32 bodyManufacturerId, string runningKm, string panNo,
            IFormFile panNoImage, string insuranceDue, IFormFile insuranceDoc, string nationalPermitDue, IFormFile nationalPermitDoc,
            string localPermitDue, IFormFile localPermitDoc, string rcValidityDue, IFormFile rcDoc, string rtoDue,
            IFormFile rtoDoc, string pollutionDue, IFormFile pollutionDoc, string fitnessDue, IFormFile fitnessDoc,
            string dimension, string branchOffice, string rcCapicity, string actualCapicity, Boolean isActive)
        {
            Globalsettings.Log(_controllerName, string.Format("Saving Started"));

            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblVehicleMasters.Select(x => new
                {
                    Id = x.Id,
                    VehicleNo = x.VehicleNo,
                    VehicleOwner = x.VehicleOwner,
                    PurchaseDate = x.PurchaseDate,
                    MfgYear = x.MfgYear,
                    MakeId = x.MakeId,
                    ModelId = x.ModelId,
                    NoOfTyres = x.NoOfTyres,
                    EngineNo = x.EngineNo,
                    ChasisNo = x.ChasisNo,
                    VehicleTypeId = x.VehicleTypeId,
                    BodyTypeId = x.BodyTypeId,
                    FinancerName = x.FinancerName,
                    BodyManufacturerId = x.BodyManufacturerId,
                    RunningKm = x.RunningKm,
                    PanNo = x.PanNo,
                    PanNoImage = x.PanNoImage,
                    InsuranceDue = x.InsuranceDue,
                    InsuranceDoc = x.InsuranceDoc,
                    NationalPermitDue = x.NationalPermitDue,
                    NationalPermitDoc = x.NationalPermitDoc,
                    LocalPermitDue = x.LocalPermitDue,
                    LocalPermitDoc = x.LocalPermitDoc,
                    RcValidityDue = x.RcValidityDue,
                    RcDoc = x.RcDoc,
                    RtoDue = x.RtoDue,
                    RtoDoc = x.RtoDoc,
                    PollutionDue = x.PollutionDue,
                    PollutionDoc = x.PollutionDoc,
                    FitnessDue = x.FitnessDue,
                    FitnessDoc = x.FitnessDoc,
                    Dimension = x.Dimension,
                    BranchOffice = x.BranchOffice,
                    RcCapicity = x.RcCapicity,
                    ActualCapicity = x.ActualCapicity,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                    ModelName = Convert.ToString(_context.TblModelAverageMasters
                            .Where(p => p.Id == x.ModelId)
                            .Select(p => p.ModelNo).FirstOrDefault()),
                    VehicleTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.VehicleTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                    BodyTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                    BodyManufacturerName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyManufacturerId)
                            .Select(p => p.Description).FirstOrDefault(),
                    CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.VehicleNo.Equals(vehicleNo)).ToList();

                if (user.Count() == 0)
                {
                    Globalsettings.Log(_controllerName, string.Format("Saving started after user count check"));
                    try
                    {
                        //Insert Vehicle Master Info
                        var vehicleMaster = new TblVehicleMaster();
                        //vehicleMaster.Id = id;
                        vehicleMaster.VehicleNo = vehicleNo.ToStringFromNull();
                        vehicleMaster.VehicleOwner = vehicleOwner.ToStringFromNull();
                        vehicleMaster.MfgYear = Convert.ToString(mfgYear.ToStringFromNull());
                        vehicleMaster.MakeId = Convert.ToInt32(makeId.ToIntFromNull());
                        vehicleMaster.ModelId = Convert.ToInt32(modelId.ToIntFromNull());
                        vehicleMaster.NoOfTyres = Convert.ToInt32(noOfTyres.ToIntFromNull());
                        vehicleMaster.EngineNo = engineNo.ToStringFromNull();
                        vehicleMaster.ChasisNo = chasisNo.ToStringFromNull();
                        vehicleMaster.VehicleTypeId = vehicleTypeId.ToIntFromNull();
                        vehicleMaster.BodyTypeId = bodyTypeId.ToIntFromNull();
                        vehicleMaster.FinancerName = financerName.ToStringFromNull();
                        vehicleMaster.BodyManufacturerId = bodyManufacturerId.ToIntFromNull();
                        vehicleMaster.RunningKm = Convert.ToInt32(runningKm.ToStringFromNull());
                        vehicleMaster.PanNo = panNo.ToStringFromNull();
                        vehicleMaster.PanNoImage = utilityHelper.saveUploadFilesToFolder(panNoImage, SiteConstants.companyPanNoFolder, panNo);                      
                        vehicleMaster.InsuranceDoc = utilityHelper.saveUploadFilesToFolder(insuranceDoc, SiteConstants.insurancePhotoFolder, panNo);                      
                        vehicleMaster.NationalPermitDoc= utilityHelper.saveUploadFilesToFolder(nationalPermitDoc, SiteConstants.nationalPermitPhotoFolder, panNo);                       
                        vehicleMaster.LocalPermitDoc = utilityHelper.saveUploadFilesToFolder(localPermitDoc, SiteConstants.localPermitPhotoFolder, panNo);                        
                        vehicleMaster.RcDoc = utilityHelper.saveUploadFilesToFolder(rcDoc, SiteConstants.rcDocPhotoFolder, panNo);                      
                        vehicleMaster.RtoDoc = utilityHelper.saveUploadFilesToFolder(rtoDoc, SiteConstants.rtoDocPhotoFolder, panNo);                       
                        vehicleMaster.PollutionDoc = utilityHelper.saveUploadFilesToFolder(pollutionDoc, SiteConstants.pollutionDocPhotoFolder, panNo);                       
                        vehicleMaster.FitnessDoc =  utilityHelper.saveUploadFilesToFolder(fitnessDoc, SiteConstants.fitnessPhotoFolder, panNo);
                        vehicleMaster.Dimension = dimension.ToStringFromNull();
                        vehicleMaster.BranchOffice = branchOffice.ToStringFromNull();
                        vehicleMaster.RcCapicity = rcCapicity.ToStringFromNull();
                        vehicleMaster.ActualCapicity = actualCapicity.ToStringFromNull();
                        vehicleMaster.IsActive = isActive;
                        //vehicleMaster.StartDate = DateTime.Parse(startDate);
                        //vehicleMaster.EndDate = DateTime.Parse(endDate);
                        vehicleMaster.CreationDate = utilityHelper.CurrentDateTime;
                        vehicleMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        vehicleMaster.CreatedBy = userID;
                        vehicleMaster.UpdatedBy = userID;

                        vehicleMaster.FitnessDue = Convert.ToDateTime(fitnessDue);
                        vehicleMaster.PollutionDue = Convert.ToDateTime(pollutionDue);
                        vehicleMaster.InsuranceDue = Convert.ToDateTime(insuranceDue);
                        vehicleMaster.NationalPermitDue = Convert.ToDateTime(nationalPermitDue);
                        vehicleMaster.LocalPermitDue = Convert.ToDateTime(localPermitDue);
                        vehicleMaster.RcValidityDue = Convert.ToDateTime(rcValidityDue);
                        vehicleMaster.RtoDue = Convert.ToDateTime(rtoDue);
                        vehicleMaster.PurchaseDate = Convert.ToDateTime(purchaseDate);


                        _context.TblVehicleMasters.Add(vehicleMaster);
                        _context.SaveChanges();
                        model.Id = vehicleMaster.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Vehicle Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        Globalsettings.Log(_controllerName, string.Format("Error: {0}", ex.Message.ToString()));
                        Globalsettings.Log(_controllerName, string.Format("Error: {0}", ex.InnerException.ToString()));
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Vehicle Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    Globalsettings.Log(_controllerName, string.Format("Error: user count is 0"));
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Vehicle Master Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                Globalsettings.Log(_controllerName, string.Format("user details is null"));
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, string vehicleNo, string vehicleOwner, string purchaseDate, string mfgYear,
            Int32 makeId, Int32 modelId, string noOfTyres, string engineNo, string chasisNo, Int32 vehicleTypeId,
            Int32 bodyTypeId, string financerName, Int32 bodyManufacturerId, string runningKm, string panNo,
            IFormFile panNoImage, string insuranceDue, IFormFile insuranceDoc, string nationalPermitDue, IFormFile nationalPermitDoc,
            string localPermitDue, IFormFile localPermitDoc, string rcValidityDue, IFormFile rcDoc, string rtoDue,
            IFormFile rtoDoc, string pollutionDue, IFormFile pollutionDoc, string fitnessDue, IFormFile fitnessDoc,
            string dimension, string branchOffice, string rcCapicity, string actualCapicity, Boolean isActive)
        {
            VMVehicleMaster model = new VMVehicleMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var vehicleMaster = _context.TblVehicleMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (vehicleMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Vehicle Master does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        vehicleMaster.VehicleNo = vehicleNo;
                        vehicleMaster.VehicleOwner = vehicleOwner;
                        vehicleMaster.PurchaseDate = DateTime.Parse(purchaseDate);
                        vehicleMaster.MfgYear = Convert.ToString(mfgYear);
                        vehicleMaster.MakeId = Convert.ToInt32(makeId);
                        vehicleMaster.ModelId = Convert.ToInt32(modelId);
                        vehicleMaster.NoOfTyres = Convert.ToInt32(noOfTyres);
                        vehicleMaster.EngineNo = engineNo;
                        vehicleMaster.ChasisNo = chasisNo;
                        vehicleMaster.VehicleTypeId = vehicleTypeId;
                        vehicleMaster.BodyTypeId = bodyTypeId;
                        vehicleMaster.FinancerName = financerName;
                        vehicleMaster.BodyManufacturerId = bodyManufacturerId;
                        vehicleMaster.RunningKm = Convert.ToInt32(runningKm);
                        vehicleMaster.PanNo = panNo;
                        string panImage = utilityHelper.saveUploadFilesToFolder(panNoImage, SiteConstants.companyPanNoFolder, panNo);
                        if (!string.IsNullOrEmpty(panImage))
                        {
                            vehicleMaster.PanNoImage = panImage;
                        }
                        vehicleMaster.InsuranceDue = DateTime.Parse(insuranceDue);
                        string insDoc = utilityHelper.saveUploadFilesToFolder(insuranceDoc, SiteConstants.insurancePhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(insDoc))
                        {
                            vehicleMaster.InsuranceDoc = insDoc;
                        }
                        vehicleMaster.NationalPermitDue = DateTime.Parse(nationalPermitDue);
                        string natPermitDoc = utilityHelper.saveUploadFilesToFolder(nationalPermitDoc, SiteConstants.nationalPermitPhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(natPermitDoc))
                        {
                            vehicleMaster.NationalPermitDoc = natPermitDoc;
                        }
                        vehicleMaster.LocalPermitDue = DateTime.Parse(localPermitDue);
                        string locPermitDoc = utilityHelper.saveUploadFilesToFolder(localPermitDoc, SiteConstants.localPermitPhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(natPermitDoc))
                        {
                            vehicleMaster.LocalPermitDoc = locPermitDoc;
                        }
                        vehicleMaster.RcValidityDue = DateTime.Parse(rcValidityDue);
                        string rcDocc = utilityHelper.saveUploadFilesToFolder(rcDoc, SiteConstants.rcDocPhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(rcDocc))
                        {
                            vehicleMaster.RcDoc = rcDocc;
                        }
                        vehicleMaster.RtoDue = DateTime.Parse(rtoDue);
                        string rtoDocc = utilityHelper.saveUploadFilesToFolder(rtoDoc, SiteConstants.rtoDocPhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(rtoDocc))
                        {
                            vehicleMaster.RtoDoc = rtoDocc;
                        }
                       
                        vehicleMaster.PollutionDue = DateTime.Parse(pollutionDue);
                        string pollDue = utilityHelper.saveUploadFilesToFolder(pollutionDoc, SiteConstants.pollutionDocPhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(pollDue))
                        {
                            vehicleMaster.PollutionDoc = pollDue;
                        }
                       
                        vehicleMaster.FitnessDue = DateTime.Parse(fitnessDue);
                        string fitDoc = utilityHelper.saveUploadFilesToFolder(fitnessDoc, SiteConstants.fitnessPhotoFolder, panNo);
                        if (!string.IsNullOrEmpty(fitDoc))
                        {
                            vehicleMaster.FitnessDoc = fitDoc;
                        }
                        
                        vehicleMaster.Dimension = dimension;
                        vehicleMaster.BranchOffice = branchOffice;
                        vehicleMaster.RcCapicity = rcCapicity;
                        vehicleMaster.ActualCapicity = actualCapicity;
                        vehicleMaster.IsActive = isActive;
                        //vehicleMaster.CreateDate = utilityHelper.CurrentDateTime;
                        vehicleMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //vehicleMaster.CreatedBy = userID;
                        vehicleMaster.UpdatedBy = userID;

                        _context.TblVehicleMasters.Update(vehicleMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Vehicle Master has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Vehicle Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteVehicleMaster(string vehicleMasterID)
        {
            VMVehicleMaster model = new VMVehicleMaster();
            try
            {
                var vehicleMaster = _context.TblVehicleMasters.Where(x => x.Id == Convert.ToInt32(vehicleMasterID));
                _context.TblVehicleMasters.RemoveRange(vehicleMaster);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Vehicle Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToString().Contains("The DELETE statement conflicted with the REFERENCE"))
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Error Occured, Vehicle id has been used!";
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Model Master has not been deleted. Please try again.";
                }
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopVehicleMastersList()
        {
            List<VMVehicleMaster> model = new List<VMVehicleMaster>();

            //Query to fetch last 10 saved records in driver master table
            model = _context.TblVehicleMasters.Select(x => new VMVehicleMaster
            {
                Id = x.Id,
                VehicleNo = x.VehicleNo,
                VehicleOwner = x.VehicleOwner,
                PurchaseDate = x.PurchaseDate.HasValue ? x.PurchaseDate.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                MfgYear = x.MfgYear,
                MakeId = x.MakeId,
                ModelId = x.ModelId,
                NoOfTyres = x.NoOfTyres,
                EngineNo = x.EngineNo,
                ChasisNo = x.ChasisNo,
                VehicleTypeId = x.VehicleTypeId,
                BodyTypeId = x.BodyTypeId,
                FinancerName = x.FinancerName,
                BodyManufacturerId = x.BodyManufacturerId,
                RunningKm = x.RunningKm,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                InsuranceDue = x.InsuranceDue.HasValue ? x.InsuranceDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                InsuranceDoc = x.InsuranceDoc,
                NationalPermitDue = x.NationalPermitDue.HasValue ? x.NationalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                NationalPermitDoc = x.NationalPermitDoc,
                LocalPermitDue = x.LocalPermitDue.HasValue ? x.LocalPermitDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                LocalPermitDoc = x.LocalPermitDoc,
                RcValidityDue = x.RcValidityDue.HasValue ? x.RcValidityDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RcDoc = x.RcDoc,
                RtoDue = x.RtoDue.HasValue ? x.RtoDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                RtoDoc = x.RtoDoc,
                PollutionDue = x.PollutionDue.HasValue ? x.PollutionDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                PollutionDoc = x.PollutionDoc,
                FitnessDue = x.FitnessDue.HasValue ? x.FitnessDue.Value.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture) : "N/A",
                FitnessDoc = x.FitnessDoc,
                Dimension = x.Dimension,
                BranchOffice = x.BranchOffice,
                RcCapicity = x.RcCapicity,
                ActualCapicity = x.ActualCapicity,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                MakeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.MakeId)
                            .Select(p => p.Description).FirstOrDefault(),
                ModelName = Convert.ToString(_context.TblModelAverageMasters
                            .Where(p => p.Id == x.ModelId)
                            .Select(p => p.ModelNo).FirstOrDefault()),
                VehicleTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.VehicleTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyTypeName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyTypeId)
                            .Select(p => p.Description).FirstOrDefault(),
                BodyManufacturerName = _context.TblCodeMasters
                            .Where(p => p.Id == x.BodyManufacturerId)
                            .Select(p => p.Description).FirstOrDefault(),
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

        [HttpGet]
        public JsonResult getFinancierMaster()
        {
            return Json(_context.TblCodeMasters.Where(x => x.CodeType == "FINANCER").Select(x => new
            {
                FinancerId = x.Id,
                Financer = x.Code,
            }).ToList());
        }
    }
}
