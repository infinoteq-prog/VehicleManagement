using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;

namespace VMS.Controllers
{
    public class TransporterMasterController : Controller
    {
        private readonly ILogger<TransporterMasterController> _logger;
        private readonly VmsDbContext _context;
        public TransporterMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMTransporterMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMTransporterMasterAccess>("userAccess");
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
            
            ViewBag.UserRole = utilityHelper.getCurrentRole();
            return View();
        }

        public ActionResult Details(String transporterMasterId)
        {
            //Code for User Access Function
            //VMTransporterMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMTransporterMasterAccess>("userAccess");
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
            ViewBag.transporterMasterId = transporterMasterId;
            return View("Details");
        }

        public ActionResult Update(String transporterMasterId)
        {
            //Code for User Access Function
            //VMTransporterMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMTransporterMasterAccess>("userAccess");
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
            ViewBag.transporterMasterId = transporterMasterId;
            ViewBag.UserRole = utilityHelper.getCurrentRole();
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMTransporterMasterAccess userAcc = HttpContext.Session.GetObjectFromJson<VMTransporterMasterAccess>("userAccess");
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
            ViewBag.UserRole = utilityHelper.getCurrentRole();
            return View();
        }

        [HttpGet]
        public JsonResult getBillPrefix(string transporterCode)
        {
            //string billPrefixFormat = transporterCode + "/" + DateTimeExtensions.ToFinancialYear(DateTime.Now) + "/" + "0001";
            string billPrefixFormat = transporterCode + "/" + utilityHelper.getFinancialYear(DateTime.Now) + "/";

            var jsonData = new
            {
                BillPrefix = billPrefixFormat
            };

            return Json(jsonData);
        }

        [HttpGet]
        public JsonResult getTransporterMasterById(string transporterMasterId)
        {
            VMTransporterMaster model = new VMTransporterMaster();
            int roleID = Convert.ToInt32(transporterMasterId);
            model = _context.TblTransporterMasters.Where(x => x.Id == roleID).Select(x => new VMTransporterMaster
            {
                Id = x.Id,
                TransporterCode = x.TransporterCode,
                TransporterName = x.TransporterName,
                OwnerName = x.OwnerName,
                MobileNo = x.MobileNumber,
                EmailID = x.EmailId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                UserId = x.UserId,
                BillPrifix = x.BillPrefix,
                IsActive = x.IsActive,
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
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault()
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
        public JsonResult getTransporterMasterByUserId(string userId)
        {
            VMTransporterMaster model = new VMTransporterMaster();
            int usrId = Convert.ToInt32(userId);
            model = _context.TblTransporterMasters.Where(x => x.UserId == usrId).Select(x => new VMTransporterMaster
            {
                Id = x.Id,
                TransporterCode = x.TransporterCode,
                TransporterName = x.TransporterName,
                OwnerName = x.OwnerName,
                MobileNo = x.MobileNumber,
                EmailID = x.EmailId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                UserId = x.UserId,
                BillPrifix = x.BillPrefix,
                IsActive = x.IsActive,
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
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault()
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
        public JsonResult getTransporterMasterList()
        {
            List<VMTransporterMaster> transporterList = new List<VMTransporterMaster>();

            var model = _context.TblTransporterMasters.Select(x => new VMTransporterMaster
            {
                Id = x.Id,
                TransporterCode = x.TransporterCode,
                TransporterName = x.TransporterName,
                OwnerName = x.OwnerName,
                MobileNo = x.MobileNumber,
                EmailID = x.EmailId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                UserId = x.UserId,
                BillPrifix = x.BillPrefix,
                IsActive = x.IsActive,
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
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault()
            });

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    model = model.Where(s => s.UserId.Equals(userDetails.Id));
                }
            }

            transporterList = model.ToList();

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
        public JsonResult searchTransporterMaster(string transporterCode, string transporterName, string ownerName, string mobileNo,
            string emailId, string address1, string address2, string address3, Int32 stateId, Int32 districtId,
            Int32 cityId, string pinCode, string gstinNo, string panNo, Int32 userId, string billPrefix, Boolean isActive)
        {
            List<VMTransporterMaster> model = new List<VMTransporterMaster>();

            var searchModel = _context.TblTransporterMasters.Select(x => new VMTransporterMaster
            {
                Id = x.Id,
                TransporterCode = x.TransporterCode,
                TransporterName = x.TransporterName,
                OwnerName = x.OwnerName,
                MobileNo = x.MobileNumber,
                EmailID = x.EmailId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                UserId = x.UserId,
                BillPrifix = x.BillPrefix,
                IsActive = x.IsActive,
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
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault()
            });

            if (!string.IsNullOrEmpty(transporterCode))
            {
                searchModel = searchModel.Where(s => s.TransporterCode == transporterCode);
            }

            if (!string.IsNullOrEmpty(transporterName))
            {
                searchModel = searchModel.Where(s => s.TransporterName == transporterName);
            }

            if (!string.IsNullOrEmpty(ownerName))
            {
                searchModel = searchModel.Where(s => s.OwnerName == ownerName);
            }

            if (!string.IsNullOrEmpty(mobileNo))
            {
                searchModel = searchModel.Where(s => s.MobileNo == mobileNo);
            }

            if (!string.IsNullOrEmpty(emailId))
            {
                searchModel = searchModel.Where(s => s.EmailID == emailId);
            }

            if (!string.IsNullOrEmpty(address1))
            {
                searchModel = searchModel.Where(s => s.Address1 == address1);
            }

            if (!string.IsNullOrEmpty(address2))
            {
                searchModel = searchModel.Where(s => s.Address2 == address2);
            }

            if (!string.IsNullOrEmpty(address3))
            {
                searchModel = searchModel.Where(s => s.Address3 == address3);
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

            if (!string.IsNullOrEmpty(gstinNo))
            {
                searchModel = searchModel.Where(s => s.GSTINNo == gstinNo);
            }

            if (!string.IsNullOrEmpty(panNo))
            {
                searchModel = searchModel.Where(s => s.PanNo == panNo);
            }

            if (userId != 0)
            {
                searchModel = searchModel.Where(s => s.UserId == userId);
            }

            if (!string.IsNullOrEmpty(billPrefix))
            {
                searchModel = searchModel.Where(s => s.BillPrifix == billPrefix);
            }

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    searchModel = searchModel.Where(s => s.UserId.Equals(userDetails.Id));
                }
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
        public ActionResult Save(string transporterCode, string transporterName, string ownerName, string mobileNo,
            string emailId, string address1, string address2, string address3, Int32 userId, Int32 stateId, Int32 districtId,
            Int32 cityId, string pinCode, string gstinNo, string panNo, string billPrefix, Boolean isActive)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;

                var transporterMasterEntry = _context.TblTransporterMasters.Where(x => x.TransporterCode == transporterCode
                                                                                    || x.GstinNo == gstinNo
                                                                                    || x.PanNo == panNo).ToList();
                if (transporterMasterEntry.Count() == 0)
                {
                    try
                    {
                        //Insert Vehicle Release Info
                        var transporterMasters = new TblTransporterMaster();
                        //transporterMasters.Id = id;
                        transporterMasters.TransporterCode = transporterCode;
                        transporterMasters.TransporterName = transporterName;
                        transporterMasters.OwnerName = ownerName;
                        transporterMasters.MobileNumber = mobileNo;
                        transporterMasters.EmailId = emailId;
                        transporterMasters.Address1 = address1;
                        transporterMasters.Address2 = address2;
                        transporterMasters.Address3 = address3;
                        transporterMasters.StateId = stateId;
                        transporterMasters.DistrictId = districtId;
                        transporterMasters.CityId = cityId;
                        transporterMasters.PinCode = pinCode;
                        transporterMasters.GstinNo = gstinNo;
                        transporterMasters.PanNo = panNo;
                        transporterMasters.UserId = userId;
                        transporterMasters.BillPrefix = billPrefix;
                        transporterMasters.IsActive = isActive;
                        //transporterMasters.StartDate = DateTime.Parse(startDate);
                        //transporterMasters.EndDate = DateTime.Parse(endDate);
                        transporterMasters.CreationDate = utilityHelper.CurrentDateTime;
                        transporterMasters.UpdateDate = utilityHelper.CurrentDateTime;
                        transporterMasters.CreatedBy = userID;
                        transporterMasters.UpdatedBy = userID;

                        _context.TblTransporterMasters.Add(transporterMasters);
                        _context.SaveChanges();
                        model.Id = transporterMasters.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Transporter Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Transporter Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "TransporterCode, GST Number or Pan Number Already Exist for other Transporter! Please check and try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, string transporterCode, string transporterName, string ownerName, string mobileNo,
            string emailId, string address1, string address2, string address3, Int32 userId, Int32 stateId, Int32 districtId,
            Int32 cityId, string pinCode, string gstinNo, string panNo, string billPrefix, Boolean isActive)
        {
            VMTransporterMaster model = new VMTransporterMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var transporterMasterEntry = _context.TblTransporterMasters.Where(x => (x.TransporterCode == transporterCode
                                                                                            || x.GstinNo == gstinNo 
                                                                                            || x.PanNo == panNo) 
                                                                                            && (!x.Id.Equals(id))).ToList();
                    if (transporterMasterEntry.Count() == 0)
                    {
                        var transporterMasters = _context.TblTransporterMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                        if (transporterMasters == null)
                        {
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "Transporter Master does not Exists! Please check and try again.";
                            return Json(model);
                        }
                        else
                        {
                            //Updateing Existing User Details
                            transporterMasters.TransporterCode = transporterCode;
                            transporterMasters.TransporterName = transporterName;
                            transporterMasters.OwnerName = ownerName;
                            transporterMasters.MobileNumber = mobileNo;
                            transporterMasters.EmailId = emailId;
                            transporterMasters.Address1 = address1;
                            transporterMasters.Address2 = address2;
                            transporterMasters.Address3 = address3;
                            transporterMasters.StateId = stateId;
                            transporterMasters.DistrictId = districtId;
                            transporterMasters.CityId = cityId;
                            transporterMasters.PinCode = pinCode;
                            transporterMasters.GstinNo = gstinNo;
                            transporterMasters.PanNo = panNo;
                            transporterMasters.UserId = userId;
                            transporterMasters.BillPrefix = billPrefix;
                            transporterMasters.IsActive = isActive;
                            //transporterMasters.CreateDate = utilityHelper.CurrentDateTime;
                            transporterMasters.UpdateDate = utilityHelper.CurrentDateTime;
                            //transporterMasters.CreatedBy = userID;
                            transporterMasters.UpdatedBy = userID;

                            _context.TblTransporterMasters.Update(transporterMasters);
                            _context.SaveChanges();
                            model.Id = id;
                        }
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Transporter Master has been updated successfully.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "TransporterCode, GST Number or Pan Number Already Exist for other Transporter! Please check and try again.";
                    }
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Transporter Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteTransporterMaster(string transporterMasterId)
        {
            VMTransporterMaster model = new VMTransporterMaster();
            try
            {
                var transporterMasters = _context.TblTransporterMasters.Where(x => x.Id == Convert.ToInt32(transporterMasterId));
                _context.TblTransporterMasters.RemoveRange(transporterMasters);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Transporter Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Transporter Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopTransporterMastersList()
        {
            List<VMTransporterMaster> model = new List<VMTransporterMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblTransporterMasters.Select(x => new VMTransporterMaster
            {
                Id = x.Id,
                TransporterCode = x.TransporterCode,
                TransporterName = x.TransporterName,
                OwnerName = x.OwnerName,
                MobileNo = x.MobileNumber,
                EmailID = x.EmailId,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                UserId = x.UserId,
                BillPrifix = x.BillPrefix,
                IsActive = x.IsActive,
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
                StateName = _context.TblStates
                            .Where(p => p.Id == x.StateId)
                            .Select(p => p.StateName).FirstOrDefault(),
                DistrictName = _context.TblDistricts
                            .Where(p => p.Id == x.DistrictId)
                            .Select(p => p.DistrictName).FirstOrDefault(),
                CityName = _context.TblCities
                            .Where(p => p.Id == x.CityId)
                            .Select(p => p.CityName).FirstOrDefault()
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
