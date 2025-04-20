using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;

namespace VMS.Controllers
{
    public class CompanyMasterController : Controller
    {
        private readonly ILogger<TransporterMasterController> _logger;
        private readonly VmsDbContext _context;
        public CompanyMasterController(VmsDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAccess = utilityHelper.getCurrentUserAccess();
            //if (userAccess != null)
            //{
                //ViewBag.AddUserRoleFullAccess = userAcc.AddUserRoleFullAccess;
                //ViewBag.AddUserRoleAddAccess = userAcc.AddUserRoleAddAccess;
                //ViewBag.AddUserRoleUpdateAccess = userAcc.AddUserRoleUpdateAccess;
                //ViewBag.AddUserRoleDeleteAccess = userAcc.AddUserRoleDeleteAccess;
                //ViewBag.AddUserRoleViewAccess = userAcc.AddUserRoleViewAccess;
            //}
            //else
            //{
                //Logging out a user because user sesstion is null
                //return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String companyMasterId)
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
            ViewBag.companyMasterId = companyMasterId;
            return View("Details");
        }

        public ActionResult Update(String companyMasterId)
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
            ViewBag.companyMasterId = companyMasterId;
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
            return View();
        }

        [HttpGet]
        public JsonResult getCompanyMasterById(string companyMasterId)
        {
            VMCompanyMaster model = new VMCompanyMaster();
            int companyID = Convert.ToInt32(companyMasterId);
            model = _context.TblBillToMasters.Where(x => x.Id == companyID).Select(x => new VMCompanyMaster
            {
                Id = x.Id,
                BillToCode = x.BillToCode,
                BillToCompanyName = x.BillToCompany,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                IsActive = x.IsActive,
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
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
        public JsonResult getCompanyMasterList()
        {
            List<VMCompanyMaster> model = new List<VMCompanyMaster>();

            model = _context.TblBillToMasters.Select(x => new VMCompanyMaster
            {
                Id = x.Id,
                BillToCode = x.BillToCode,
                BillToCompanyName = x.BillToCompany,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                IsActive = x.IsActive,
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
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
        public JsonResult searchCompanyMaster(string companyCode, string companyName, string address1, string address2,
            string address3, Int32 stateId, Int32 districtId, Int32 cityId, string pinCode,
            string gstinNo, string panNo, Boolean isActive)
        {
            List<VMCompanyMaster> model = new List<VMCompanyMaster>();

            var searchModel = _context.TblBillToMasters.Select(x => new VMCompanyMaster
            {
                Id = x.Id,
                BillToCode = x.BillToCode,
                BillToCompanyName = x.BillToCompany,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                IsActive = x.IsActive,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
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

            if (!string.IsNullOrEmpty(companyCode))
            {
                searchModel = searchModel.Where(s => s.BillToCode == companyCode);
            }

            if (!string.IsNullOrEmpty(companyName))
            {
                searchModel = searchModel.Where(s => s.BillToCompanyName == companyName);
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
        public ActionResult Save(string companyCode, string companyName, string address1, string address2, 
            string address3, Int32 stateId, Int32 districtId, Int32 cityId, string pinCode,
            string gstinNo, string panNo, Boolean isActive)
        {
            VMUser model = new VMUser();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;

                var companyMasterEntry = _context.TblBillToMasters.Where(x => x.BillToCode == companyCode
                                                                            || x.GstinNo == gstinNo
                                                                            || x.PanNo == panNo).ToList();
                if (companyMasterEntry.Count() == 0)
                {
                    try
                    {
                        //Insert Vehicle Release Info
                        var companyMasters = new TblBillToMaster();
                        //companyMasters.Id = id;
                        companyMasters.BillToCode = companyCode;
                        companyMasters.BillToCompany = companyName;
                        companyMasters.Address1 = address1;
                        companyMasters.Address2 = address2;
                        companyMasters.Address3 = address3;
                        companyMasters.StateId = stateId;
                        companyMasters.DistrictId = districtId;
                        companyMasters.CityId = cityId;
                        companyMasters.PinCode = pinCode;
                        companyMasters.GstinNo = gstinNo;
                        companyMasters.PanNo = panNo;
                        companyMasters.IsActive = isActive;
                        companyMasters.StartDate = utilityHelper.CurrentDateTime;
                        companyMasters.EndDate = utilityHelper.CurrentDateTime;
                        companyMasters.CreationDate = utilityHelper.CurrentDateTime;
                        companyMasters.UpdateDate = utilityHelper.CurrentDateTime;
                        companyMasters.CreatedBy = userID;
                        companyMasters.UpdatedBy = userID;

                        _context.TblBillToMasters.Add(companyMasters);
                        _context.SaveChanges();
                        model.Id = companyMasters.Id.ToString();

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Company Master has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Company Master not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "CompanyCode, GST Number or Pan Number Already Exist for other company! Please check and try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(Int32 id, string companyCode, string companyName, string address1, string address2,
            string address3, Int32 stateId, Int32 districtId, Int32 cityId, string pinCode,
            string gstinNo, string panNo, Boolean isActive)
        {
            VMCompanyMaster model = new VMCompanyMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var companyMasterEntry = _context.TblBillToMasters.Where(x => (x.BillToCode == companyCode
                                                                                            || x.GstinNo == gstinNo
                                                                                            || x.PanNo == panNo)
                                                                                            && (!x.Id.Equals(id))).ToList();
                    if (companyMasterEntry.Count() == 0)
                    {
                        var companyMasters = _context.TblBillToMasters.Where(x => x.Id == Convert.ToInt32(id)).FirstOrDefault();
                        if (companyMasters == null)
                        {
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "Company Master does not Exists! Please check and try again.";
                            return Json(model);
                        }
                        else
                        {
                            //Updateing Existing User Details
                            companyMasters.BillToCompany = companyCode;
                            companyMasters.BillToCompany = companyName;
                            companyMasters.Address1 = address1;
                            companyMasters.Address2 = address2;
                            companyMasters.Address3 = address3;
                            companyMasters.StateId = stateId;
                            companyMasters.DistrictId = districtId;
                            companyMasters.CityId = cityId;
                            companyMasters.PinCode = pinCode;
                            companyMasters.GstinNo = gstinNo;
                            companyMasters.PanNo = panNo;
                            companyMasters.StartDate = utilityHelper.CurrentDateTime;
                            companyMasters.EndDate = utilityHelper.CurrentDateTime;
                            companyMasters.IsActive = isActive;
                            //companyMasters.CreateDate = utilityHelper.CurrentDateTime;
                            companyMasters.UpdateDate = utilityHelper.CurrentDateTime;
                            //companyMasters.CreatedBy = userID;
                            companyMasters.UpdatedBy = userID;

                            _context.TblBillToMasters.Update(companyMasters);
                            _context.SaveChanges();
                            model.Id = id;
                            model.TransactionMessage.Status = TransactionStatus.Success;
                            model.TransactionMessage.Message = "Company Master has been updated successfully.";

                        }
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "CompanyCode, GST Number or Pan Number Already Exist for other company! Please check and try again.";
                    }
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Company Master not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteCompanyMaster(string companyMastersId)
        {
            VMCompanyMaster model = new VMCompanyMaster();
            try
            {
                var companyMasters = _context.TblBillToMasters.Where(x => x.Id == Convert.ToInt32(companyMastersId));
                _context.TblBillToMasters.RemoveRange(companyMasters);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Company Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Company Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopTransporterMastersList()
        {
            List<VMCompanyMaster> model = new List<VMCompanyMaster>();

            //Query to fetch last 10 saved records in trasporter bill table
            model = _context.TblBillToMasters.Select(x => new VMCompanyMaster
            {
                Id = x.Id,
                BillToCode = x.BillToCode,
                BillToCompanyName = x.BillToCompany,
                Address1 = x.Address1,
                Address2 = x.Address2,
                Address3 = x.Address3,
                StateId = x.StateId,
                DistrictId = x.DistrictId,
                CityId = x.CityId,
                PinCode = x.PinCode,
                GSTINNo = x.GstinNo,
                PanNo = x.PanNo,
                IsActive = x.IsActive,
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                            .Where(p => p.Id == x.UpdatedBy)
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
