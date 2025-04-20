using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace VMS.Controllers
{
    public class GSTMasterController : Controller
    {
        private readonly ILogger<GSTMasterController> _logger;
        private readonly VmsDbContext _context;

        public GSTMasterController(VmsDbContext context)
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

        public ActionResult Details(String gstMasterId)
        {
            //Code for User Access Function
            //VMGstMasterRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMGstMasterRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddGstMasterEntryViewAccess = userAcc.AddGstMasterEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userID = gstMasterId;
            return View("Details");
        }

        public ActionResult Update(String gstMasterId)
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
            ViewBag.userID = gstMasterId;
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
        public JsonResult getGstMasterById(Int32 gstMasterId)
        {
            VMGstMaster model = new VMGstMaster();

            model = (from u in _context.TblGstMasters
                     join t in _context.TblTransporterMasters on u.TransporterCode equals t.TransporterCode
                     join um in _context.TblUserMasters on t.UserId equals um.Id
                     select new VMGstMaster()
                     {
                         Id = u.Id,
                         TransporterName = t.TransporterName,
                         TransporterCode = u.TransporterCode,
                         TransporterId = t.Id,
                         SgstRate = u.SgstRate,
                         CgstRate = u.CgstRate,
                         IgstRate = u.IgstRate,
                         UgstRate = u.UtgstRate,
                         EffectiveDate = u.EffectiveDate,
                         EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDate = u.EndDate,
                         IsActive = u.IsActive,
                         CreationDate = u.CreationDate,
                         UpdateDate = u.UpdateDate,
                         IsRcm = u.IsRcm,
                         CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                         UpdatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                     }).Where(x => x.Id == gstMasterId).FirstOrDefault();

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
        public JsonResult getGSTMasterList()
        {
            List<VMGstMaster> gstMasterList = new List<VMGstMaster>();

            var model = (from u in _context.TblGstMasters
                         join t in _context.TblTransporterMasters on u.TransporterCode equals t.TransporterCode
                         join um in _context.TblUserMasters on t.UserId equals um.Id
                         select new VMGstMaster()
                         {
                             Id = u.Id,
                             TransporterName = t.TransporterName,
                             TransporterCode = u.TransporterCode,
                             TransporterId = t.Id,
                             UserId = um.Id,
                             SgstRate = u.SgstRate,
                             CgstRate = u.CgstRate,
                             IgstRate = u.IgstRate,
                             UgstRate = u.UtgstRate,
                             EffectiveDate = u.EffectiveDate,
                             EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             IsRcm = u.IsRcm,
                             EndDate = u.EndDate,
                             IsActive = u.IsActive,
                             CreationDate = u.CreationDate,
                             UpdateDate = u.UpdateDate,
                             CreatedBy = _context.TblUserMasters
                                .Where(p => p.Id == u.CreatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                             UpdatedBy = _context.TblUserMasters
                                .Where(p => p.Id == u.UpdatedBy)
                                .Select(p => p.UserName).FirstOrDefault()
                         });

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    model = model.Where(s => s.UserId.Equals(userDetails.Id));
                }
            }

            gstMasterList = model.OrderByDescending(n => n.Id).ToList();

            if (gstMasterList.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(gstMasterList);
            }
        }

        [HttpGet]
        public JsonResult searchGstMasterList(string transporterCode, string sgstRate, string cgstRate, string igstRate,
                                         string ugstRate, string effectiveDate,
                                         string endDate, Boolean isActive)
        {
            //Fetch Transporter Code
            transporterCode = utilityHelper.fetchTransporterCode(transporterCode);

            List<VMGstMaster> model = new List<VMGstMaster>();

            var searchModel = (from u in _context.TblGstMasters
                               join t in _context.TblTransporterMasters on u.TransporterCode equals t.TransporterCode
                               join um in _context.TblUserMasters on t.UserId equals um.Id
                               select new VMGstMaster()
                               {
                                   Id = u.Id,
                                   TransporterName = t.TransporterName,
                                   TransporterCode = u.TransporterCode,
                                   TransporterId = t.Id,
                                   SgstRate = u.SgstRate,
                                   CgstRate = u.CgstRate,
                                   IgstRate = u.IgstRate,
                                   UgstRate = u.UtgstRate,
                                   IsRcm = u.IsRcm,
                                   EffectiveDate = u.EffectiveDate,
                                   EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                   EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                   EndDate = u.EndDate,
                                   IsActive = u.IsActive,
                                   CreationDate = u.CreationDate,
                                   UpdateDate = u.UpdateDate,
                                   CreatedBy = _context.TblUserMasters
                                                .Where(p => p.Id == u.CreatedBy)
                                                .Select(p => p.UserName).FirstOrDefault(),
                                                       UpdatedBy = _context.TblUserMasters
                                                .Where(p => p.Id == u.UpdatedBy)
                                                .Select(p => p.UserName).FirstOrDefault()
                               });

           
            if (!string.IsNullOrEmpty(transporterCode))
            {
                searchModel = searchModel.Where(s => s.TransporterCode.Equals(transporterCode));
            }
            if (!string.IsNullOrEmpty(sgstRate))
            {
                searchModel = searchModel.Where(s => s.SgstRate.Equals(Decimal.Parse(sgstRate)));
            }
            if (!string.IsNullOrEmpty(cgstRate))
            {
                searchModel = searchModel.Where(s => s.CgstRate.Equals(Decimal.Parse(cgstRate)));
            }
            if (!string.IsNullOrEmpty(igstRate))
            {
                searchModel = searchModel.Where(s => s.IgstRate.Equals(Decimal.Parse(igstRate)));
            }
            if (!string.IsNullOrEmpty(ugstRate))
            {
                searchModel = searchModel.Where(s => s.UgstRate.Equals(Decimal.Parse(ugstRate)));
            }
            if (!string.IsNullOrEmpty(effectiveDate))
            {
                searchModel = searchModel.Where(s => s.EffectiveDate.Equals(DateTime.Parse(effectiveDate)));
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                searchModel = searchModel.Where(s => s.EndDate.Equals(DateTime.Parse(endDate)));
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
        public ActionResult Save(string transporterCode, string sgstRate, string cgstRate, string igstRate,
                                 string ugstRate, string effectiveDate, string endDate, Boolean isRCM, Boolean isActive)
        {
            //Fetch Transporter Code
            transporterCode = utilityHelper.fetchTransporterCode(transporterCode);

            VMGstMaster model = new VMGstMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblGstMasters.Select(u => new
                {
                    Id = u.Id,
                    TransporterCode = u.TransporterCode,
                    TransporterId = _context.TblTransporterMasters
                            .Where(p => p.TransporterCode.Equals(u.TransporterCode))
                            .Select(p => p.Id).FirstOrDefault(),
                    SgstRate = u.SgstRate,
                    CgstRate = u.CgstRate,
                    IgstRate = u.IgstRate,
                    UtgstRate = u.UtgstRate,
                    EffectiveDate = u.EffectiveDate,
                    EndDate = u.EndDate,
                    IsActive = u.IsActive,
                    CreationDate = u.CreationDate,
                    UpdateDate = u.UpdateDate,
                    IsRcm = u.IsRcm,
                    CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                    UpdatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                }).Where(x => x.EffectiveDate == DateTime.Parse(effectiveDate) &&
                         x.TransporterCode == transporterCode).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert into User Master Table
                        var GstMasterEntry = new TblGstMaster();
                        GstMasterEntry.TransporterCode = transporterCode;
                        GstMasterEntry.SgstRate = Convert.ToDecimal(sgstRate);
                        GstMasterEntry.CgstRate = Convert.ToDecimal(cgstRate);
                        GstMasterEntry.IgstRate = Convert.ToDecimal(igstRate);
                        GstMasterEntry.UtgstRate = Convert.ToDecimal(ugstRate);
                        GstMasterEntry.EffectiveDate = DateTime.Parse(effectiveDate);
                        //GstMasterEntry.EndDate = DateTime.Parse(endDate);
                        GstMasterEntry.CreationDate = utilityHelper.CurrentDateTime;
                        GstMasterEntry.UpdateDate = utilityHelper.CurrentDateTime;
                        GstMasterEntry.CreatedBy = userID;
                        GstMasterEntry.UpdatedBy = userID;
                        GstMasterEntry.IsActive = isActive;
                        GstMasterEntry.IsRcm = isRCM;
                        _context.TblGstMasters.Add(GstMasterEntry);

                        _context.SaveChanges();
                        model.Id = GstMasterEntry.Id;
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Gst Master Details has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Gst Master Details not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Gst Master Details Already Exist for given Effective Date! Please try again with diffrent Date.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, string transporterCode, string sgstRate, string cgstRate, string igstRate,
                                   string ugstRate, string effectiveDate,
                                   string endDate, Boolean isRCM, Boolean isActive)
        {
            //Fetch Transporter Code
            transporterCode = utilityHelper.fetchTransporterCode(transporterCode);

            VMGstMaster model = new VMGstMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var user = _context.TblGstMasters.Select(u => new
                    {
                        Id = u.Id,
                        TransporterCode = u.TransporterCode,
                        TransporterId = _context.TblTransporterMasters
                                                .Where(p => p.TransporterCode.Equals(u.TransporterCode))
                                                .Select(p => p.Id).FirstOrDefault(),
                        SgstRate = u.SgstRate,
                        CgstRate = u.CgstRate,
                        IgstRate = u.IgstRate,
                        UgstRate = u.UtgstRate,
                        EffectiveDate = u.EffectiveDate,
                        EndDate = u.EndDate,
                        IsActive = u.IsActive,
                        CreationDate = u.CreationDate,
                        UpdateDate = u.UpdateDate,
                        IsRcm = u.IsRcm,
                        CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                        UpdatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.UpdatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
                    }).Where(x => x.EffectiveDate == DateTime.Parse(effectiveDate) &&
                         x.TransporterCode == transporterCode &&
                         x.Id != id).ToList();

                    if (user.Count() == 0)
                    {
                        var GstMasterEntry = _context.TblGstMasters.Where(x => x.Id == id).FirstOrDefault();

                        if (GstMasterEntry == null)
                        {
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "Gst Master Details does not Exists! Please check and try again.";
                            return Json(model);
                        }
                        else
                        {
                            //Updateing Existing Gst Master Details
                            GstMasterEntry.TransporterCode = transporterCode;
                            GstMasterEntry.SgstRate = Convert.ToDecimal(sgstRate);
                            GstMasterEntry.CgstRate = Convert.ToDecimal(cgstRate);
                            GstMasterEntry.IgstRate = Convert.ToDecimal(igstRate);
                            GstMasterEntry.UtgstRate = Convert.ToDecimal(ugstRate);
                            GstMasterEntry.EffectiveDate = DateTime.Parse(effectiveDate);
                            if (!string.IsNullOrEmpty(endDate))
                            {
                                GstMasterEntry.EndDate = DateTime.Parse(endDate);
                            }
                            GstMasterEntry.CreationDate = utilityHelper.CurrentDateTime;
                            GstMasterEntry.UpdateDate = utilityHelper.CurrentDateTime;
                            GstMasterEntry.CreatedBy = userID;
                            GstMasterEntry.UpdatedBy = userID;
                            GstMasterEntry.IsActive = isActive;
                            GstMasterEntry.IsRcm = isRCM;

                            _context.TblGstMasters.Update(GstMasterEntry);
                            _context.SaveChanges();
                        }
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Gst Master Details has been updated successfully.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Gst Master Details Already Exist for this effective date! Please try again with diffrent effective date.";
                    }
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Gst Master Details not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteGstMaster(int gstMasterId)
        {
            VMGstMaster model = new VMGstMaster();
            try
            {
                var GstMasterEntry = _context.TblGstMasters.Where(x => x.Id == gstMasterId);
                _context.TblGstMasters.RemoveRange(GstMasterEntry);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Gst Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Gst Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopGstMasterList()
        {
            List<VMGstMaster> model = new List<VMGstMaster>();
            //Query to fetch last 10 saved records in trasporter bill table
            model = (from u in _context.TblGstMasters
                     join t in _context.TblTransporterMasters on u.TransporterCode equals t.TransporterCode
                     join um in _context.TblUserMasters on t.UserId equals um.Id
                     select new VMGstMaster()
                     {
                         Id = u.Id,
                         TransporterName = t.TransporterName,
                         TransporterCode = u.TransporterCode,
                         TransporterId = t.Id,
                         SgstRate = u.SgstRate,
                         CgstRate = u.CgstRate,
                         IgstRate = u.IgstRate,
                         UgstRate = u.UtgstRate,
                         EffectiveDate = u.EffectiveDate,
                         EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         IsRcm = u.IsRcm,
                         EndDate = u.EndDate,
                         IsActive = u.IsActive,
                         CreationDate = u.CreationDate,
                         UpdateDate = u.UpdateDate,
                         CreatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault(),
                         UpdatedBy = _context.TblUserMasters
                            .Where(p => p.Id == u.UpdatedBy)
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
