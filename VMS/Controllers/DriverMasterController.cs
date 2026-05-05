using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace VMS.Controllers
{
    public class DriverMasterController : Controller
    {
        private readonly ILogger<DriverMasterController> _logger;
        private readonly VmsDbContext _context;
        public DriverMasterController(VmsDbContext context)
        {
            _context = context;
        }
 
        public ActionResult Index()
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String driverMasterID)
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.driverMasterID = driverMasterID;
            return View("Details");
        }

        public ActionResult Update(String driverMasterID)
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.driverMasterID = driverMasterID;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMDriverMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMDriverMasterAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddDriverMasterAccess = userAcc.AddDriverMasterAccess;
            //    ViewBag.AddDriverMasterAddAccess = userAcc.AddDriverMasterAddAccess;
            //    ViewBag.AddDriverMasterUpdateAccess = userAcc.AddDriverMasterUpdateAccess;
            //    ViewBag.AddDriverMasterDeleteAccess = userAcc.AddDriverMasterDeleteAccess;
            //    ViewBag.AddDriverMasterViewAccess = userAcc.AddDriverMasterViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
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
        public JsonResult getDriverFather(int driverMasterId)
        {
            return Json(_context.TblDriverMasters.Where(x => x.Id.Equals(driverMasterId)).Select(x => new
            {
                DriverFatherName = x.FatherName,
            }).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult getDriverMasterByID(string driverMasterID)
        {
            VMDriverMaster model = new VMDriverMaster();
            int roleID = Convert.ToInt32(driverMasterID);
            model = _context.TblDriverMasters.Where(x => x.Id == roleID).Select(x => new VMDriverMaster
            {
                Id = x.Id,
                DriverName = x.DriverName,
                FatherName = x.FatherName,
                DriverAddress1 = x.DriverAddress1,
                DriverAddress2 = x.DriverAddress2,
                DriverAddress3 = x.DriverAddress3,
                CityId = x.CityId,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                PinCode = x.PinCode,
                DriverPhoto = x.DriverPhoto,
                AadharNo = x.AadharNo,
                AadharNoImage = x.AadharNoImage,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                BankName = x.BankName,
                BankAccountNumber = x.BankAccountNumber,
                BankIFSCCode = x.BankIfsccode,
                DrivingLicenceNo = x.DrivingLicenceNo,
                DrivingLicenceIssueAuth = x.DrivingLicenceIssueAuth,
                DrivingLicenceValidity = x.DrivingLicenceValidity.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                DrivingLicenceImage = x.DrivingLicenceImage,
                MobileNumber1 = x.MobileNumber1,
                MobileNumber2 = x.MobileNumber2,
                IsExistingReference = x.IsExistingReference,
                ReferenceName = x.ReferenceName.ToIntFromNull().ToStringFromNull(),
                ReferenceAddress1 = x.ReferenceAddress1,
                ReferenceAddress2 = x.ReferenceAddress2,
                ReferenceAddress3 = x.ReferenceAddress3,
                ReferenceCity = x.ReferenceCity,
                ReferencePinCode = x.ReferencePin,
                ReferenceMobile = x.ReferenceMobile,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                OldFirm   = x.OldFirm,
                Remark = x.Remark,
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
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
        public JsonResult getDriverMasterList()
        {
            List<VMDriverMaster> model = new List<VMDriverMaster>();
            var driverMaster = _context.TblDriverMasters.Where(x => x.ReferenceName == "undefined").ToList();
            foreach (var item in driverMaster)
            {
                item.ReferenceName = "0";
            }
            _context.TblDriverMasters.UpdateRange(driverMaster);
            _context.SaveChanges();

            try
            {
                model = _context.TblDriverMasters.Select(x => new VMDriverMaster
                {
                    Id = x.Id,
                    DriverName = x.DriverName,
                    FatherName = x.FatherName,
                    OldFirm = x.OldFirm,
                    Remark = x.Remark,
                    DriverAddress1 = x.DriverAddress1,
                    DriverAddress2 = x.DriverAddress2,
                    DriverAddress3 = x.DriverAddress3,
                    CityId = x.CityId,
                    StateId = x.StateId,
                    PinCode = x.PinCode,
                    DriverPhoto = x.DriverPhoto,
                    AadharNo = x.AadharNo,
                    AadharNoImage = x.AadharNoImage,
                    PanNo = x.PanNo,
                    PanNoImage = x.PanNoImage,
                    BankName = x.BankName,
                    BankAccountNumber = x.BankAccountNumber,
                    BankIFSCCode = x.BankIfsccode,
                    DrivingLicenceNo = x.DrivingLicenceNo,
                    DrivingLicenceIssueAuth = x.DrivingLicenceIssueAuth,
                    DrivingLicenceValidity = x.DrivingLicenceValidity.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    DrivingLicenceImage = x.DrivingLicenceImage,
                    MobileNumber1 = x.MobileNumber1,
                    MobileNumber2 = x.MobileNumber2,
                    IsExistingReference = x.IsExistingReference,
                    ReferenceName = _context.TblDriverMasters
                            .Where(p => p.Id == (string.IsNullOrEmpty(x.ReferenceName) ? 0 : Convert.ToInt32(x.ReferenceName)))
                            .Select(p => p.DriverName).SingleOrDefault() ?? "",
                    ReferenceAddress1 = x.ReferenceAddress1,
                    ReferenceAddress2 = x.ReferenceAddress2,
                    ReferenceAddress3 = x.ReferenceAddress3,
                    ReferenceCity = x.ReferenceCity,
                    ReferencePinCode = x.ReferencePin,
                    ReferenceMobile = x.ReferenceMobile,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                    CityName = _context.TblCities
                             .Where(p => p.Id == x.CityId)
                             .Select(p => p.CityName).FirstOrDefault(),
                    DistrictName = _context.TblDistricts
                             .Where(p => p.Id == x.DistrictId)
                             .Select(p => p.DistrictName).FirstOrDefault(),
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
            catch(Exception ex)
            {
                return Json(null);
            }
        }

        [HttpPost]
        public JsonResult searchDriverMaster(string driverName, string fatherName, string driverAddress1, string driverAddress2,
            string driverAddress3, Int32 stateId, Int32 districtId, Int32 cityId, string pinCode, string aadharNo,
            string panNo, string bankName, string bankAccountNumber, string bankIfscCode,
            string drivingLicenceNo, string drivingLicenceAuth, string drivingLicenceValidity,
            string mobileNo1, string mobileNo2, Boolean isExistingReference, string referenceName, string referenceAddress1,
            string referenceAddress2, string referenceAddress3, string referenceCity, string referencePin,
            string referenceMobile, Boolean isActive ,string  oldFirm , string remark ) 
        {
            List<VMDriverMaster> model = new List<VMDriverMaster>();

            var searchModel = _context.TblDriverMasters.Select(x => new VMDriverMaster
            {
                Id = x.Id,
                DriverName = x.DriverName,
                FatherName = x.FatherName,
                DriverAddress1 = x.DriverAddress1,
                DriverAddress2 = x.DriverAddress2,
                DriverAddress3 = x.DriverAddress3,
                CityId = x.CityId,
                StateId = x.StateId,
                PinCode = x.PinCode,
                DriverPhoto = x.DriverPhoto,
                AadharNo = x.AadharNo,
                AadharNoImage = x.AadharNoImage,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                BankName = x.BankName,
                BankAccountNumber = x.BankAccountNumber,
                BankIFSCCode = x.BankIfsccode,
                DrivingLicenceNo = x.DrivingLicenceNo,
                DrivingLicenceIssueAuth = x.DrivingLicenceIssueAuth,
                DrivingLicenceValidity = x.DrivingLicenceValidity.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                DrivingLicenceImage = x.DrivingLicenceImage,
                MobileNumber1 = x.MobileNumber1,
                MobileNumber2 = x.MobileNumber2,
                IsExistingReference = x.IsExistingReference,
                ReferenceName = x.ReferenceName,
                ReferenceAddress1 = x.ReferenceAddress1,
                ReferenceAddress2 = x.ReferenceAddress2,
                ReferenceAddress3 = x.ReferenceAddress3,
                ReferenceCity = x.ReferenceCity,
                ReferencePinCode = x.ReferencePin,
                ReferenceMobile = x.ReferenceMobile,
                IsActive = x.IsActive,
                OldFirm = x.OldFirm,
                Remark = x.Remark,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });

            if (!string.IsNullOrEmpty(driverName))
            {
                searchModel = searchModel.Where(s => s.DriverName == driverName);
            }

            if (!string.IsNullOrEmpty(fatherName))
            {
                searchModel = searchModel.Where(s => s.FatherName == fatherName);
            }

            if (!string.IsNullOrEmpty(driverAddress1))
            {
                searchModel = searchModel.Where(s => s.DriverAddress1 == driverAddress1);
            }

            if (!string.IsNullOrEmpty(driverAddress2))
            {
                searchModel = searchModel.Where(s => s.DriverAddress2 == driverAddress2);
            }

            if (!string.IsNullOrEmpty(driverAddress3))
            {
                searchModel = searchModel.Where(s => s.DriverAddress3 == driverAddress3);
            }

            if (stateId != 0)
            {
                searchModel = searchModel.Where(s => s.StateId == stateId);
            }

            if (districtId != 0)
            {
                searchModel = searchModel.Where(s => s.DistrictId == districtId);
            }

            if (cityId != 0)
            {
                searchModel = searchModel.Where(s => s.CityId == cityId);
            }

            if (!string.IsNullOrEmpty(pinCode))
            {
                searchModel = searchModel.Where(s => s.PinCode == pinCode);
            }

            if (!string.IsNullOrEmpty(aadharNo))
            {
                searchModel = searchModel.Where(s => s.AadharNo == aadharNo);
            }

            if (!string.IsNullOrEmpty(panNo))
            {
                searchModel = searchModel.Where(s => s.PanNo == panNo);
            }

            if (!string.IsNullOrEmpty(bankName))
            {
                searchModel = searchModel.Where(s => s.BankName == bankName);
            }

            if (!string.IsNullOrEmpty(bankAccountNumber))
            {
                searchModel = searchModel.Where(s => s.BankAccountNumber == bankAccountNumber);
            }

            if (!string.IsNullOrEmpty(bankIfscCode))
            {
                searchModel = searchModel.Where(s => s.BankIFSCCode == bankIfscCode);
            }

            if (!string.IsNullOrEmpty(drivingLicenceNo))
            {
                searchModel = searchModel.Where(s => s.DrivingLicenceNo == drivingLicenceNo);
            }

            if (!string.IsNullOrEmpty(drivingLicenceAuth))
            {
                searchModel = searchModel.Where(s => s.DrivingLicenceIssueAuth == drivingLicenceAuth);
            }

            if (!string.IsNullOrEmpty(drivingLicenceValidity))
            {
                searchModel = searchModel.Where(s => s.DrivingLicenceValidity.Equals(drivingLicenceValidity));
            }

            if (!string.IsNullOrEmpty(mobileNo1))
            {
                searchModel = searchModel.Where(s => s.MobileNumber1 == mobileNo1);
            }

            if (!string.IsNullOrEmpty(mobileNo2))
            {
                searchModel = searchModel.Where(s => s.MobileNumber2 == mobileNo2);
            }

            if (isExistingReference == true)
            {
                searchModel = searchModel.Where(s => s.IsExistingReference == isExistingReference);
            }

            if (!string.IsNullOrEmpty(referenceName))
            {
                searchModel = searchModel.Where(s => s.ReferenceName == referenceName);
            }

            if (!string.IsNullOrEmpty(referenceAddress1))
            {
                searchModel = searchModel.Where(s => s.ReferenceAddress1 == referenceAddress1);
            }

            if (!string.IsNullOrEmpty(referenceAddress2))
            {
                searchModel = searchModel.Where(s => s.ReferenceAddress2 == referenceAddress2);
            }

            if (!string.IsNullOrEmpty(referenceAddress3))
            {
                searchModel = searchModel.Where(s => s.ReferenceAddress3 == referenceAddress3);
            }

            if (!string.IsNullOrEmpty(referenceCity))
            {
                searchModel = searchModel.Where(s => s.ReferenceCity == referenceCity);
            }

            if (!string.IsNullOrEmpty(referencePin))
            {
                searchModel = searchModel.Where(s => s.ReferencePinCode == referencePin);
            }

            if (!string.IsNullOrEmpty(referenceMobile))
            {
                searchModel = searchModel.Where(s => s.ReferenceMobile == referenceMobile);
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

        [HttpGet]
        public ActionResult DisplayImage(string folderName, string uniqueModulePrefix, string fileName)
        {
            return File(utilityHelper.getFileName(folderName, uniqueModulePrefix, fileName), "image/jpeg"); // Adjust the content type based on the image type
        }


        [HttpPost]
        public ActionResult Save(string driverName, string fatherName, string driverAddress1, string driverAddress2,
            string driverAddress3, Int32 stateId, Int32 districtId, Int32 cityId, string pinCode, IFormFile driverPhoto, string aadharNo,
            IFormFile aadharNoImage, string panNo, IFormFile panNoImage, string bankName, string bankAccountNumber, string bankIfscCode,
            string drivingLicenceNo, string drivingLicenceAuth, string drivingLicenceValidity, IFormFile drivingLicenceImage,
            string mobileNo1, string mobileNo2, Boolean isExistingReference, string referenceName, string referenceAddress1,
            string referenceAddress2, string referenceAddress3, string referenceCity, string referencePin,
            string referenceMobile, Boolean isActive, string oldFirm, string remark)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblDriverMasters.Select(x => new
                {
                    Id = x.Id,
                    DriverName = x.DriverName,
                    FatherName = x.FatherName,
                    OldFirm  = x.OldFirm,
                    Remark = x.Remark,
                    DriverAddress1 = x.DriverAddress1,
                    DriverAddress2 = x.DriverAddress2,
                    DriverAddress3 = x.DriverAddress3,
                    CityId = x.CityId,
                    StateId = x.StateId,
                    PinCode = x.PinCode,
                    DriverPhoto = x.DriverPhoto,
                    AadharNo = x.AadharNo,
                    AadharNoImage = x.AadharNoImage,
                    PanNo = x.PanNo,
                    PanNoImage = x.PanNoImage,
                    BankName = x.BankName,
                    BankAccountNumber = x.BankAccountNumber,
                    BankIFSCCode = x.BankIfsccode,
                    DrivingLicenceNo = x.DrivingLicenceNo,
                    DrivingLicenceIssueAuth = x.DrivingLicenceIssueAuth,
                    DrivingLicenceValidity = x.DrivingLicenceValidity,
                    DrivingLicenceImage = x.DrivingLicenceImage,
                    MobileNumber1 = x.MobileNumber1,
                    MobileNumber2 = x.MobileNumber2,
                    IsExistingReference = x.IsExistingReference,
                    ReferenceName = x.ReferenceName.ToIntFromNull().ToStringFromNull(),
                    ReferenceAddress1 = x.ReferenceAddress1,
                    ReferenceAddress2 = x.ReferenceAddress2,
                    ReferenceAddress3 = x.ReferenceAddress3,
                    ReferenceCity = x.ReferenceCity,
                    ReferencePinCode = x.ReferencePin,
                    ReferenceMobile = x.ReferenceMobile,
                    IsActive = x.IsActive,
                    CreationDate = x.CreationDate,
                    UpdateDate = x.UpdateDate,
                    CreatedBy = x.CreatedBy,
                    UpdatedBy = x.UpdatedBy,
                    StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                    CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault(),
                    DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                    CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.DriverName == driverName
                && x.DrivingLicenceNo == drivingLicenceNo).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert Vehicle Release Info
                        var driverMaster = new TblDriverMaster();
                        //driverMaster.Id = id;
                        driverMaster.DriverName = driverName.ToStringFromNull();
                        driverMaster.FatherName = fatherName.ToStringFromNull();
                        driverMaster.DriverAddress1 = driverAddress1.ToStringFromNull();
                        driverMaster.DriverAddress2 = driverAddress2.ToStringFromNull();
                        driverMaster.DriverAddress3 = driverAddress3.ToStringFromNull();
                        driverMaster.StateId = stateId.ToIntFromNull();
                        driverMaster.DistrictId = districtId.ToIntFromNull();
                        driverMaster.CityId = cityId.ToIntFromNull();
                        driverMaster.PinCode = pinCode.ToStringFromNull();
                        driverMaster.DriverPhoto = utilityHelper.saveUploadFilesToFolder(driverPhoto, SiteConstants.driverPhotoFolder, drivingLicenceNo); 
                        driverMaster.AadharNo = aadharNo.ToStringFromNull();
                        driverMaster.AadharNoImage = utilityHelper.saveUploadFilesToFolder(aadharNoImage, SiteConstants.driverAadharFolder, drivingLicenceNo);
                        driverMaster.PanNo = panNo.ToStringFromNull();
                        driverMaster.PanNoImage = utilityHelper.saveUploadFilesToFolder(panNoImage, SiteConstants.driverPanNoFolder, drivingLicenceNo);
                        driverMaster.BankName = bankName.ToStringFromNull();
                        driverMaster.BankAccountNumber = bankAccountNumber.ToStringFromNull();
                        driverMaster.BankIfsccode = bankIfscCode.ToStringFromNull();
                        driverMaster.DrivingLicenceNo = drivingLicenceNo.ToStringFromNull();
                        driverMaster.DrivingLicenceIssueAuth = drivingLicenceAuth.ToStringFromNull();
                        driverMaster.DrivingLicenceValidity = DateTime.Parse(drivingLicenceValidity);
                        driverMaster.DrivingLicenceImage = utilityHelper.saveUploadFilesToFolder(drivingLicenceImage, SiteConstants.driverLicenceFolder, drivingLicenceNo);
                        driverMaster.MobileNumber1 = mobileNo1.ToStringFromNull();
                        driverMaster.MobileNumber2 = mobileNo2.ToStringFromNull();
                        driverMaster.IsExistingReference = isExistingReference;
                        driverMaster.ReferenceName = referenceName.ToStringFromNull();
                        driverMaster.ReferenceAddress1 = referenceAddress1.ToStringFromNull();
                        driverMaster.ReferenceAddress2 = referenceAddress2.ToStringFromNull();
                        driverMaster.ReferenceAddress3 = referenceAddress3.ToStringFromNull();
                        driverMaster.ReferenceCity = referenceCity.ToStringFromNull();
                        driverMaster.ReferencePin = referencePin.ToStringFromNull();
                        driverMaster.ReferenceMobile = referenceMobile.ToStringFromNull();
                        driverMaster.IsActive = isActive;
                        driverMaster.CreationDate = utilityHelper.CurrentDateTime;
                        driverMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        driverMaster.CreatedBy = userID;
                        driverMaster.UpdatedBy = userID;
                        driverMaster.OldFirm = oldFirm;
                        driverMaster.Remark = remark;
                           

                        _context.TblDriverMasters.Add(driverMaster);
                        _context.SaveChanges();
                        model.Id = driverMaster.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Driver Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Driver Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Driver Master Already Exist! Please try again with diffrent username.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, string driverName, string fatherName, string driverAddress1, string driverAddress2,
            string driverAddress3, Int32 stateId, Int32 districtId, Int32 cityId, string pinCode, IFormFile driverPhoto, string aadharNo,
            IFormFile aadharNoImage, string panNo, IFormFile panNoImage, string bankName, string bankAccountNumber, string bankIfscCode,
            string drivingLicenceNo, string drivingLicenceAuth, string drivingLicenceValidity, IFormFile drivingLicenceImage,
            string mobileNo1, string mobileNo2, Boolean isExistingReference, string referenceName, string referenceAddress1,
            string referenceAddress2, string referenceAddress3, string referenceCity, string referencePin,
            string referenceMobile, Boolean isActive, string oldFirm, string remark)
        {
            VMDriverMaster model = new VMDriverMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var driverMaster = _context.TblDriverMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                    if (driverMaster == null)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Driver Master does not Exists! Please check and try again.";
                        return Json(model);
                    }
                    else
                    {
                        //Updateing Existing User Details
                        driverMaster.DriverName = driverName.ToStringFromNull();
                        driverMaster.FatherName = fatherName.ToStringFromNull();
                        driverMaster.DriverAddress1 = driverAddress1.ToStringFromNull();
                        driverMaster.DriverAddress2 = driverAddress2.ToStringFromNull();
                        driverMaster.DriverAddress3 = driverAddress3.ToStringFromNull();
                        driverMaster.StateId = stateId.ToIntFromNull();
                        driverMaster.DistrictId = districtId.ToIntFromNull();
                        driverMaster.CityId = cityId.ToIntFromNull();
                        driverMaster.PinCode = pinCode.ToStringFromNull();
                        driverMaster.AadharNo = aadharNo.ToStringFromNull();
                        driverMaster.PanNo = panNo.ToStringFromNull();

                        string driverImage = utilityHelper.saveUploadFilesToFolder(driverPhoto, SiteConstants.driverPhotoFolder, drivingLicenceNo);
                        if (!string.IsNullOrEmpty(driverImage))
                        {
                            driverMaster.DriverPhoto = driverImage;
                        }
                       
                        string aadharImage = utilityHelper.saveUploadFilesToFolder(aadharNoImage, SiteConstants.driverAadharFolder, drivingLicenceNo);
                        if (!string.IsNullOrEmpty(aadharImage))
                        {
                            driverMaster.AadharNoImage = aadharImage;
                        }

                        string panImage = utilityHelper.saveUploadFilesToFolder(panNoImage, SiteConstants.driverPanNoFolder, drivingLicenceNo);
                        if (!string.IsNullOrEmpty(panImage))
                        {
                            driverMaster.PanNoImage = panImage;
                        }

                        string dlImage = utilityHelper.saveUploadFilesToFolder(drivingLicenceImage, SiteConstants.driverLicenceFolder, drivingLicenceNo);
                        if (!string.IsNullOrEmpty(dlImage))
                        {
                            driverMaster.DrivingLicenceImage = dlImage;
                        }

                        driverMaster.BankName = bankName.ToStringFromNull();
                        driverMaster.BankAccountNumber = bankAccountNumber.ToStringFromNull();
                        driverMaster.BankIfsccode = bankIfscCode.ToStringFromNull();
                        driverMaster.DrivingLicenceNo = drivingLicenceNo.ToStringFromNull();
                        driverMaster.DrivingLicenceIssueAuth = drivingLicenceAuth.ToStringFromNull();
                        driverMaster.DrivingLicenceValidity = DateTime.Parse(drivingLicenceValidity);
                        driverMaster.MobileNumber1 = mobileNo1.ToStringFromNull();
                        driverMaster.MobileNumber2 = mobileNo2.ToStringFromNull();
                        driverMaster.IsExistingReference = isExistingReference.ToboolFromNull();
                        driverMaster.ReferenceName = referenceName.ToStringFromNull();
                        driverMaster.ReferenceAddress1 = referenceAddress1.ToStringFromNull();
                        driverMaster.ReferenceAddress2 = referenceAddress2.ToStringFromNull();
                        driverMaster.ReferenceAddress3 = referenceAddress3.ToStringFromNull();
                        driverMaster.ReferenceCity = referenceCity.ToStringFromNull();
                        driverMaster.ReferencePin = referencePin.ToStringFromNull();
                        driverMaster.ReferenceMobile = referenceMobile.ToStringFromNull();
                        driverMaster.IsActive = isActive.ToboolFromNull();
                        //driverMaster.CreateDate = utilityHelper.CurrentDateTime;
                        driverMaster.UpdateDate = utilityHelper.CurrentDateTime;
                        //driverMaster.CreatedBy = userID;
                        driverMaster.UpdatedBy = userID.ToIntFromNull();

                        driverMaster.OldFirm = oldFirm;
                        driverMaster.Remark = remark;

                        _context.TblDriverMasters.Update(driverMaster);
                        _context.SaveChanges();
                        model.Id = id;
                    }
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Driver Master has been updated successfully.";
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Driver Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteDriverMaster(string driverMasterID)
        {
            VMDriverMaster model = new VMDriverMaster();
            try
            {
                var driverMaster = _context.TblDriverMasters.Where(x => x.Id == Convert.ToInt32(driverMasterID));
                _context.TblDriverMasters.RemoveRange(driverMaster);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Driver Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                if (ex.InnerException.Message.ToString().Contains("The DELETE statement conflicted with the REFERENCE"))
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Error Occured, Drive id has been used!";
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
        public JsonResult getTopDriverMastersList()
        {
            List<VMDriverMaster> model = new List<VMDriverMaster>();

            //Query to fetch last 10 saved records in driver master table
            model = _context.TblDriverMasters.Select(x => new VMDriverMaster
            {
                Id = x.Id,
                DriverName = x.DriverName,
                FatherName = x.FatherName,
                OldFirm = x.OldFirm,
                Remark = x.Remark,
                DriverAddress1 = x.DriverAddress1,
                DriverAddress2 = x.DriverAddress2,
                DriverAddress3 = x.DriverAddress3,
                CityId = x.CityId,
                StateId = x.StateId,
                PinCode = x.PinCode,
                DriverPhoto = x.DriverPhoto,
                AadharNo = x.AadharNo,
                AadharNoImage = x.AadharNoImage,
                PanNo = x.PanNo,
                PanNoImage = x.PanNoImage,
                BankName = x.BankName,
                BankAccountNumber = x.BankAccountNumber,
                BankIFSCCode = x.BankIfsccode,
                DrivingLicenceNo = x.DrivingLicenceNo,
                DrivingLicenceIssueAuth = x.DrivingLicenceIssueAuth,
                DrivingLicenceValidity = x.DrivingLicenceValidity.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                DrivingLicenceImage = x.DrivingLicenceImage,
                MobileNumber1 = x.MobileNumber1,
                MobileNumber2 = x.MobileNumber2,
                IsExistingReference = x.IsExistingReference,
                ReferenceName = x.ReferenceName,
                ReferenceAddress1 = x.ReferenceAddress1,
                ReferenceAddress2 = x.ReferenceAddress2,
                ReferenceAddress3 = x.ReferenceAddress3,
                ReferenceCity = x.ReferenceCity,
                ReferencePinCode = x.ReferencePin,
                ReferenceMobile = x.ReferenceMobile,
                IsActive = x.IsActive,
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = x.CreatedBy,
                UpdatedBy = x.UpdatedBy,
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
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
