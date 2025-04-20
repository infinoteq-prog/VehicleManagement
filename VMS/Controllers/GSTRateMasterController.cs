using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Authentication;
using System.Globalization;

namespace VMS.Controllers
{
    public class GSTRateMasterController : Controller
    {
        private readonly ILogger<GSTRateMasterController> _logger;
        private readonly VmsDbContext _context;

        public GSTRateMasterController(VmsDbContext context)
        {
            _context = context;
        }
        public ActionResult Index()
        {
            //Code for User Access Function
            //VMGstRateMasterRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMGstRateMasterRoleAccess>("userAccess");
            //if (userAcc = null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddgstRateMasterEntryViewAccess = userAcc.AddgstRateMasterEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        public ActionResult Details(String gstRateMasterId)
        {
            //Code for User Access Function
            //VMGstRateMasterRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMGstRateMasterRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddgstRateMasterEntryViewAccess = userAcc.AddgstRateMasterEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userID = gstRateMasterId;
            return View("Details");
        }

        public ActionResult Update(String gstRateMasterId)
        {
            //Code for User Access Function
            //VMGstRateMasterRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMGstRateMasterRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddgstRateMasterEntryViewAccess = userAcc.AddgstRateMasterEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            ViewBag.userID = gstRateMasterId;
            return View("Update");
        }

        public ActionResult List()
        {
            //Code for User Access Function
            //VMGstRateMasterRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMGstRateMasterRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.AddUserFullAccess = userAcc.AddUserFullAccess;
            //    ViewBag.AddUserAddAccess = userAcc.AddUserAddAccess;
            //    ViewBag.AddUserUpdateAccess = userAcc.AddUserUpdateAccess;
            //    ViewBag.AddUserDeleteAccess = userAcc.AddUserDeleteAccess;
            //    ViewBag.AddgstRateMasterEntryViewAccess = userAcc.AddgstRateMasterEntryViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getGstRateMasterById(Int32 gstRateMasterId)
        {
            VMGstRateMaster model = new VMGstRateMaster();

            model = (from u in _context.TblGstRateMasters
                     //join rm in _context.TblRoleMasters on u.r equals rm.Id
                     select new VMGstRateMaster()
                     {
                         Id = u.Id,
                         SgstRate = u.SgstRate,
                         CgstRate = u.CgstRate,
                         IgstRate = u.IgstRate,
                         UgstRate = u.UtgstRate,
                         EffectiveDate = u.EffectiveDate,
                         EndDate = u.EndDate,
                         EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         IsActive = u.IsActive,
                         CreationDate = u.CreationDate,
                         UpdateDate = u.UpdateDate,
                         CreatedBy = u.CreatedBy,
                         UpdatedBy = u.UpdatedBy
                     }).Where(x => x.Id == gstRateMasterId).FirstOrDefault();

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
        public JsonResult getGSTRateMasterList()
        {
            List<VMGstRateMaster> model = new List<VMGstRateMaster>();

            model = (from u in _context.TblGstRateMasters
                     select new VMGstRateMaster()
                     {
                         Id = u.Id,
                         SgstRate = u.SgstRate,
                         CgstRate = u.CgstRate,
                         IgstRate = u.IgstRate,
                         UgstRate = u.UtgstRate,
                         EffectiveDate = u.EffectiveDate,
                         EndDate = u.EndDate,
                         EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                         IsActive = u.IsActive,
                         CreationDate = u.CreationDate,
                         UpdateDate = u.UpdateDate,
                         CreatedBy = u.CreatedBy,
                         UpdatedBy = u.UpdatedBy
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
        public JsonResult searchGstRateMasterList(string sgstRate, string cgstRate, string igstRate,
                                         string ugstRate, string effectiveDate, 
                                         string endDate, Boolean isActive)
        {
            List<VMGstRateMaster> model = new List<VMGstRateMaster>();

            var searchModel = (from u in _context.TblGstRateMasters
                               select new VMGstRateMaster()
                              {
                                   Id = u.Id,
                                   SgstRate = u.SgstRate,
                                   CgstRate = u.CgstRate,
                                   IgstRate = u.IgstRate,
                                   UgstRate = u.UtgstRate,
                                   EffectiveDate = u.EffectiveDate,
                                   EndDate = u.EndDate,
                                   EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                   EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                   IsActive = u.IsActive,
                                   CreationDate = u.CreationDate,
                                   UpdateDate = u.UpdateDate,
                                   CreatedBy = u.CreatedBy,
                                   UpdatedBy = u.UpdatedBy
                               });

            if (!string.IsNullOrEmpty(sgstRate))
            {
                searchModel = searchModel.Where(s => s.SgstRate.Equals(sgstRate));
            }
            if (!string.IsNullOrEmpty(cgstRate))
            {
                searchModel = searchModel.Where(s => s.CgstRate.Equals(cgstRate));
            }
            if (!string.IsNullOrEmpty(igstRate))
            {
                searchModel = searchModel.Where(s => s.IgstRate.Equals(igstRate));
            }
            if (!string.IsNullOrEmpty(ugstRate))
            {
                searchModel = searchModel.Where(s => s.UgstRate.Equals(ugstRate));
            }
            if (!string.IsNullOrEmpty(effectiveDate))
            {
                searchModel = searchModel.Where(s => s.EffectiveDate == DateTime.Parse(effectiveDate));
            }
            if (!string.IsNullOrEmpty(endDate))
            {
                searchModel = searchModel.Where(s => s.EndDate == DateTime.Parse(endDate));
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
        public ActionResult Save(string sgstRate, string cgstRate, string igstRate,
                                 string ugstRate, string effectiveDate, Boolean isActive)
        {
            VMGstRateMaster model = new VMGstRateMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                var user = _context.TblGstRateMasters.Select(u => new
                {
                    Id = u.Id,
                    SgstRate = u.SgstRate,
                    CgstRate = u.CgstRate,
                    IgstRate = u.IgstRate,
                    UgstRate = u.UtgstRate,
                    EffectiveDate = u.EffectiveDate,
                    EndDate = u.EndDate,
                    IsActive = u.IsActive,
                    CreationDate = u.CreationDate,
                    UpdateDate = u.UpdateDate,
                    CreatedBy = u.CreatedBy,
                    UpdatedBy = u.UpdatedBy
                }).Where(x => x.EffectiveDate == DateTime.Parse(effectiveDate)).ToList();

                if (user.Count() == 0)
                {
                    try
                    {
                        //Insert into User Master Table
                        var gstRateMasterEntry = new TblGstRateMaster();
                        gstRateMasterEntry.SgstRate = Convert.ToDecimal(sgstRate);
                        gstRateMasterEntry.CgstRate = Convert.ToDecimal(cgstRate);
                        gstRateMasterEntry.IgstRate = Convert.ToDecimal(igstRate);
                        gstRateMasterEntry.UtgstRate = Convert.ToDecimal(ugstRate);
                        gstRateMasterEntry.EffectiveDate = DateTime.Parse(effectiveDate);
                        //gstRateMasterEntry.EndDate = DateTime.Parse(endDate);
                        gstRateMasterEntry.CreationDate = utilityHelper.CurrentDateTime;
                        gstRateMasterEntry.UpdateDate = utilityHelper.CurrentDateTime;
                        gstRateMasterEntry.CreatedBy = userID;
                        gstRateMasterEntry.UpdatedBy = userID;
                        gstRateMasterEntry.IsActive = isActive;
                        _context.TblGstRateMasters.Add(gstRateMasterEntry);

                        _context.SaveChanges();
                        model.Id = gstRateMasterEntry.Id;
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Gst Rate Master Details has been saved successfully.";
                    }
                    catch (Exception ex)
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Gst Rate Master Details not saved due to some technical Issue. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Message = "Gst Rate Master Details Already Exist for given Effective Date! Please try again with diffrent Date.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public ActionResult Update(int id, string sgstRate, string cgstRate, string igstRate,
                                   string ugstRate, string effectiveDate,
                                   string endDate, Boolean isActive)
        {
            VMGstRateMaster model = new VMGstRateMaster();
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
                try
                {
                    var user = _context.TblGstRateMasters.Select(u => new
                    {
                        Id = u.Id,
                        SgstRate = u.SgstRate,
                        CgstRate = u.CgstRate,
                        IgstRate = u.IgstRate,
                        UgstRate = u.UtgstRate,
                        EffectiveDate = u.EffectiveDate,
                        EndDate = u.EndDate,
                        IsActive = u.IsActive,
                        CreationDate = u.CreationDate,
                        UpdateDate = u.UpdateDate,
                        CreatedBy = u.CreatedBy,
                        UpdatedBy = u.UpdatedBy
                    }).Where(x => x.EffectiveDate == DateTime.Parse(effectiveDate)
                            && x.Id != id).ToList();

                    if (user.Count() == 0)
                    {
                        var gstRateMasterEntry = _context.TblGstRateMasters.Where(x => x.Id == id).FirstOrDefault();

                        if (gstRateMasterEntry == null)
                        {
                            model.TransactionMessage.Status = TransactionStatus.Error;
                            model.TransactionMessage.Message = "Gst Rate Master Details does not Exists! Please check and try again.";
                            return Json(model);
                        }
                        else
                        {
                            //Updateing Existing Gst Rate Master Details
                            gstRateMasterEntry.SgstRate = Convert.ToDecimal(sgstRate);
                            gstRateMasterEntry.CgstRate = Convert.ToDecimal(cgstRate);
                            gstRateMasterEntry.IgstRate = Convert.ToDecimal(igstRate);
                            gstRateMasterEntry.UtgstRate = Convert.ToDecimal(ugstRate);
                            gstRateMasterEntry.EffectiveDate = DateTime.Parse(effectiveDate);
                            if (!string.IsNullOrEmpty(endDate))
                            {
                                gstRateMasterEntry.EndDate = DateTime.Parse(endDate);
                            }
                            gstRateMasterEntry.CreationDate = utilityHelper.CurrentDateTime;
                            gstRateMasterEntry.UpdateDate = utilityHelper.CurrentDateTime;
                            gstRateMasterEntry.CreatedBy = userID;
                            gstRateMasterEntry.UpdatedBy = userID;
                            gstRateMasterEntry.IsActive = isActive;

                            _context.TblGstRateMasters.Update(gstRateMasterEntry);
                            _context.SaveChanges();
                        }
                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Gst Rate Master Details has been updated successfully.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Gst Rate Master Details Already Exist for this effective date! Please try again with diffrent effective date.";
                    }
                }
                catch (Exception ex)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Gst Rate Master Details not saved due to some technical Issue. Please try again.";
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult deleteGstRateMaster(int gstRateMasterId)
        {
            VMGstRateMaster model = new VMGstRateMaster();
            try
            {
                var gstRateMasterEntry = _context.TblGstRateMasters.Where(x => x.Id == gstRateMasterId);
                _context.TblGstRateMasters.RemoveRange(gstRateMasterEntry);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Gst Rate Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Gst Rate Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult getTopGstRateMasterList()
        {
            List<VMGstRateMaster> model = new List<VMGstRateMaster>();
            //Query to fetch last 10 saved records in trasporter bill table
            model = (from u in _context.TblGstRateMasters
                          select new VMGstRateMaster()
                          {
                              Id = u.Id,
                              SgstRate = u.SgstRate,
                              CgstRate = u.CgstRate,
                              IgstRate = u.IgstRate,
                              UgstRate = u.UtgstRate,
                              EffectiveDate = u.EffectiveDate,
                              EndDate = u.EndDate,
                              EffectiveDateString = String.IsNullOrEmpty(Convert.ToString(u.EffectiveDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EffectiveDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                              EndDateString = String.IsNullOrEmpty(Convert.ToString(u.EndDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(u.EndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                              IsActive = u.IsActive,
                              CreationDate = u.CreationDate,
                              UpdateDate = u.UpdateDate,
                              CreatedBy = u.CreatedBy,
                              UpdatedBy = u.UpdatedBy
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
