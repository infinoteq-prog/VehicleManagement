
using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using System.Data.OleDb;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mail;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Reflection;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.parser;
using iTextSharp.tool.xml.pipeline.css;
using iTextSharp.tool.xml.pipeline.end;
using iTextSharp.tool.xml.pipeline.html;
//using SelectPdf;
using DocumentFormat.OpenXml.Spreadsheet;

namespace VMS.Controllers
{
    public class TransporterBillController : Controller
    {
        private readonly ILogger<TransporterBillController> _logger;
        private readonly VmsDbContext _context;
        private readonly Random _random = new Random();
        private IWebHostEnvironment _environment;
        //private readonly ISenderEmail _emailService;
        private readonly IConfiguration _configuration;
        private readonly ISenderEmail _emailService;

        public TransporterBillController(VmsDbContext context, IConfiguration configuration, IWebHostEnvironment? environment, ISenderEmail? emailService)  // 
        {
            _context = context;
            _configuration = configuration;
            _environment = environment;
            _emailService = emailService;
        }

        //public TransporterBillController(VmsDbContext context)
        //{
        //    _context = context;
        //}

        public ActionResult Index()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                var model = _context.TblTransporterMasters.Select(x => new
                {
                    Id = x.Id,
                    transporterCode = x.TransporterCode,
                    userId = x.UserId
                }).Where(x => x.userId == userDetails.Id).FirstOrDefault();

                if (model != null)
                {
                    ViewBag.transporterCode = model.transporterCode;
                    ViewBag.TransporterId = model.Id;
                }
            }
            return View();
        }

        public ActionResult ViewTransporterBill()
        {

            ////Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()

            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                var model = _context.TblTransporterMasters.Select(x => new
                {
                    Id = x.Id,
                    transporterCode = x.TransporterCode,
                    userId = x.UserId
                }).Where(x => x.userId == userDetails.Id).FirstOrDefault();

                if (model != null)
                {
                    ViewBag.transporterCode = model.transporterCode;
                    ViewBag.TransporterId = model.Id;
                }
            }
            return View();
        }

        public ActionResult UploadDispatchData()
        {
            ////Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()
            return View();
        }

        public ActionResult ViewBillDetails()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()
            return View();
        }

        public ActionResult ViewDispatchTransactions()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()
            return View();
        }

        public ActionResult ViewDispatchDetails()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()
            return View();
        }

        public ActionResult UpdateLrGrData()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            //_context.TransporterMasters.ToList()
            return View();
        }

        public ActionResult ViewPendingBills()
        {
            //Code for User Access Function
            //VMUserRoleAccess userAcc = HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            //if (userAcc != null)
            //{
            //    ViewBag.TransporterBillFullAccess = userAcc.TransporterBillFullAccess;
            //    ViewBag.TransporterBillInsertAccess = userAcc.TransporterBillInsertAccess;
            //    ViewBag.TransporterBillPendingAccess = userAcc.TransporterBillPendingAccess;
            //    ViewBag.TransporterBillPrintAccess = userAcc.TransporterBillPrintAccess;
            //    ViewBag.TransporterBillViewAccess = userAcc.TransporterBillViewAccess;
            //}
            //else
            //{
            //    //Logging out a user because user sesstion is null
            //    return RedirectToAction("Logout", "Login");
            //}
            return View();
        }

        [HttpGet]
        public JsonResult getTransporter(Int32 transporterId)
        {
            return Json(_context.TblTransporterMasters.Select(x => new
            {
                Id = x.Id,
                Name = x.TransporterName,
                userId = x.UserId
            }).Where(x => x.Id == transporterId).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult getHsnCode(string divisionCode)
        {
            return Json(_context.TblCodeMasters.Select(x => new
            {
                Id = x.Id,
                Code = x.Code,
                CodeType = x.CodeType,
                Description = x.Description
            }).Where(x => x.Code.Equals(divisionCode) && x.CodeType.Equals(SiteConstants.CodeType_Hsncode)).FirstOrDefault());
        }

        [HttpGet]
        public JsonResult getBillNumber(string transporterCode)
        {
            string billNumber = string.Empty;
            var model = _context.TblTransporterMasters.Where(x => x.TransporterCode.Equals(transporterCode)).FirstOrDefault();
            if (model != null)
            {
                //Concating BillPrefix and Dynamic Financial Year
                billNumber = model.BillPrefix + SiteConstants.Forwardslash + utilityHelper.getFinancialYear(DateTime.Now) + SiteConstants.Forwardslash;

                //Fetching Number of Records From Transporter Bill Details Table
                var count = _context.TblTransporterBills.Where(x => x.BillNumber.Contains(billNumber)).Count();

                if (count == 0)
                {
                    //Adding the count for current bull
                    count = count + 1;

                    //Generating the Final Bill Number
                    billNumber = billNumber + count.ToString();
                    //billNumber = billNumber + count.ToString().PadLeft(4, '0');
                }
                else
                {
                    var num = _context.TblTransporterBills.Where(x => x.BillNumber.Contains(billNumber))
                                                          .OrderByDescending(x => x.Id)
                                                          .Select(x => x.BillNumber).FirstOrDefault();
                    var parts = num.Split('/');
                    Int64 numb = Convert.ToInt64(parts.LastOrDefault());
                    numb = numb + 1;
                    billNumber = billNumber + numb.ToString();
                }
            }
            return Json(billNumber);
        }

        [NonAction]
        public Boolean checkUserSubscription()
        {
            Boolean userSubscription = false;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                int userId = userDetails.Id;

                //Checking Transporter State is Union Territory or not
                var userSub = _context.TblUserSubscriptions.Where(x => x.UserId == userId
                                                                              && DateOnly.FromDateTime(DateTime.Now) >= DateOnly.FromDateTime(x.StartDate)
                                                                              && DateOnly.FromDateTime(DateTime.Now) <= DateOnly.FromDateTime(x.EndDate)
                                                                             ).FirstOrDefault();
                if (userSub != null)
                {
                    userSubscription = true;
                }
                else
                {
                    userSubscription = false;
                }
            }
            return userSubscription;
        }

        [HttpPost]
        public JsonResult SaveDispatchDetails(IFormFile postedFile)
        {
            VMDispatchModel dispatchModel = new VMDispatchModel();
            string uniqueId = string.Empty;
            if (postedFile != null)
            {
                //Saving Uploaded File to a Folder
                string filePath = utilityHelper.saveUploadFilesToFolder(postedFile, SiteConstants.dispatchFileFolder, "Dispatch");

                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                if (userDetails != null)
                {
                    dispatchModel = utilityHelper.saveDispatchData(filePath, userDetails.Id);
                    ViewBag.DownloadDispatchErrorFile = dispatchModel.DisptachErrorFilePath;

                    if (dispatchModel.DispatchErrorModel == null && !string.IsNullOrEmpty(dispatchModel.UniqueDispatchId))
                    {
                        //Update LR_GR_DATE with Pgi_Date Column and EbitNetAmount with Frieght Road Column
                        _context.TblDispatchDetails.Where(x => x.DispatchUniqueId.Equals(dispatchModel.UniqueDispatchId)).ToList().ForEach(c =>
                        {
                            c.EbidNetAmt = c.FreightRoad;
                            c.LrGrDate = c.PgiDate;
                            c.EbidFrtRate = c.DispatchQtyRoad.Equals(Convert.ToDecimal(0.00)) ? c.EbidFrtRate : c.FreightRoad / c.DispatchQtyRoad;
                        });
                        _context.SaveChanges();
                    }
                }
            }
            return Json(dispatchModel);
        }

        [HttpPost]
        public JsonResult BulkUpdateLrGrNo(IFormFile postedFile)
        {
            VMDispatchModel dispatchModel = new VMDispatchModel();
            string uniqueId = string.Empty;
            if (postedFile != null)
            {
                //Saving Uploaded File to a Folder
                string filePath = utilityHelper.saveUploadFilesToFolder(postedFile, SiteConstants.lrGrNoUpdateFolder, "LrGrNoUpdate");

                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                if (userDetails != null)
                {
                    dispatchModel = utilityHelper.bulkUpdateLrGrNo(filePath, userDetails.Id);
                    ViewBag.DownloadDispatchErrorFile = dispatchModel.DisptachErrorFilePath;
                }
            }
            return Json(dispatchModel);
        }

        [HttpPost]
        public JsonResult updateLRGRNo(int id, string lrGrNo)
        {
            VMDispatchDetails model = new VMDispatchDetails();
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                var dispatchDetail = _context.TblDispatchDetails.Where(x => x.Id.Equals(id)).FirstOrDefault();

                if (dispatchDetail == null)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Dispatch entry does not Exists! Please check and try again.";
                    return Json(model);
                }
                else
                {
                    dispatchDetail.LrGrNo = lrGrNo;
                    _context.TblDispatchDetails.Update(dispatchDetail);
                    _context.SaveChanges();
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "LR GR No has been updated successfully.";
                }
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult updateExceptionalEntries(int id, string exceptionalEntry)
        {
            VMDispatchDetails model = new VMDispatchDetails();
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                var dispatchDetail = _context.TblDispatchDetails.Where(x => x.Id.Equals(id)).FirstOrDefault();

                if (dispatchDetail == null)
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Dispatch entry does not Exists! Please check and try again.";
                    return Json(model);
                }
                else
                {
                    dispatchDetail.ExceptionalEntry = exceptionalEntry;
                    _context.TblDispatchDetails.Update(dispatchDetail);
                    _context.SaveChanges();
                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Message = "Exceptional Entry has been updated successfully.";
                }
            }
            return Json(model);
        }


        [HttpGet]
        public JsonResult getTrasporterBills()
        {
            List<VMTransporterBill> transporterBills = new List<VMTransporterBill>();
            //Left outer join query
            var model = _context.TblTransporterBills.Select(x => new VMTransporterBill
            {
                Id = x.Id,
                BillNumber = x.BillNumber,
                BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                TransporterId = x.TransporterId,
                CompanyId = x.CompanyId,
                SgstAmount = x.SgstAmount,
                CgstAmount = x.CgstAmount,
                IgstAmount = x.IgstAmount,
                UgstAmount = x.UgstAmount,
                TotalBillAmount = x.TotalBillAmount,
                StartDate = x.StartDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                EndDate = x.EndDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreationDate,
                UpdatedBy = x.UpdatedBy,
                UpdateDate = x.UpdateDate,
                userId = _context.TblTransporterMasters
                            .Where(p => p.Id == x.TransporterId)
                            .Select(p => p.UserId).FirstOrDefault(),
                TransporterName = _context.TblTransporterMasters
                            .Where(p => p.Id == x.TransporterId)
                            .Select(p => p.TransporterCode).FirstOrDefault(),
                CompanyName = _context.TblBillToMasters
                            .Where(p => p.Id == x.CompanyId)
                            .Select(p => p.BillToCode).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    model = model.Where(s => s.userId.Equals(userDetails.Id));
                }
            }

            transporterBills = model.OrderByDescending(x => x.Id).ToList();

            if (transporterBills.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(transporterBills);
            }
        }

        [HttpGet]
        public JsonResult getDispatchDataForBillGeneration(string billTo,
                                                           string transporterCode,
                                                           string billCategory,
                                                           string billDate,
                                                           string division,
                                                           string incoTerm,
                                                           string hsnCode,
                                                           string fromDate,
                                                           string toDate)
        {
            VMTransporterBillList mainModel = new VMTransporterBillList();
            List<VMDispatchDetails> model = new List<VMDispatchDetails>();
            try
            {
                //Fetch BillTo
                billTo = utilityHelper.fetchBillTo(billTo);

                //Checking User Subscription
                if (checkUserSubscription())
                {
                    DateOnly? frmDate = new DateOnly(2024, 1, 1);
                    DateOnly? tDate = new DateOnly(2024, 1, 1);

                    if (!String.IsNullOrEmpty(billTo)
                        && !String.IsNullOrEmpty(transporterCode)
                        && !String.IsNullOrEmpty(billCategory)
                        && !String.IsNullOrEmpty(billDate)
                        && !String.IsNullOrEmpty(division)
                        && !String.IsNullOrEmpty(incoTerm)
                        && !String.IsNullOrEmpty(hsnCode)
                        && !String.IsNullOrEmpty(fromDate)
                        && !String.IsNullOrEmpty(toDate))
                    {
                        frmDate = DateOnly.Parse(fromDate);
                        tDate = DateOnly.Parse(toDate);

                        //Need to Fetch Both Companies Transactions if user select any one of them. C001 and C002
                        if (billTo.Equals(SiteConstants.BillTo_C001) || billTo.Equals(SiteConstants.BillTo_C002))
                        {
                            billTo = SiteConstants.BillTo_C001 + SiteConstants.Comma + SiteConstants.BillTo_C002;
                        }

                        model = _context.TblDispatchDetails.Where(x =>  string.IsNullOrEmpty(x.ExceptionalEntry)
                                                                        && string.IsNullOrEmpty(x.BillNumber)
                                                                        && x.Division.Trim().Equals(division)
                                                                        && x.IncoTerm.Trim().Equals(incoTerm)
                                                                        && x.DistributionChannel.Trim().Equals(billCategory)
                                                                        && billTo.Contains(x.SupplyingPlant)
                                                                        && x.ForwardingAgentCode.Trim().Equals(transporterCode)
                                                                        && DateOnly.FromDateTime((DateTime)x.PgiDate) >= frmDate
                                                                        && DateOnly.FromDateTime((DateTime)x.PgiDate) <= tDate)
                                                                        .Select(x => new VMDispatchDetails
                                                                        {
                                                                            Id = x.Id,
                                                                            SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                                                                            ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                                                                            DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                                                                            TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                                                                            LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                                                                            LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                                                            ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                                                                            DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                                                                            EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                                                                            EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                                                                            ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                                                                            ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                                                                            RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                                                                            PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                                                                            PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                                                            DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                                                                            Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                                                                            IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                                                                            RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                                                                            RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                                                                            EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                                                                            EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                                                                            BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                                                                            BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                                                            CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                                                                            SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                                                                            IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                                                                            UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                                                                            TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                                                                            CreationDate = x.CreationDate,
                                                                            UpdateDate = x.UpdateDate,
                                                                            IsActive = x.IsActive,
                                                                            CreatedByName = _context.TblUserMasters
                                                                                        .Where(p => p.Id == x.CreatedBy)
                                                                                        .Select(p => p.UserName).FirstOrDefault(),
                                                                            CreatedBy = x.CreatedBy,
                                                                            UpdatedByName = _context.TblUserMasters
                                                                                        .Where(p => p.Id == x.UpdatedBy)
                                                                                        .Select(p => p.UserName).FirstOrDefault(),
                                                                            UpdatedBy = x.UpdatedBy,
                                                                            ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                                                                        }).ToList();

                        mainModel.TransactionMessage.Status = TransactionStatus.Success;
                        mainModel.TransactionMessage.Title = "Dispatch Data Found!";
                        mainModel.TransactionMessage.Message = "Dispatch Data has been fetched successfully.";
                    }
                }
                else
                {
                    mainModel.TransactionMessage.Status = TransactionStatus.Failed;
                    mainModel.TransactionMessage.Title = "User Subscription Not Found / Subscription Expired!";
                    mainModel.TransactionMessage.Message = "Your have not Subscribed or your Subscription has been Expired! Please activate your Subscription and Try Again!";
                }
            }
            catch (Exception ex)
            {
                mainModel.TransactionMessage.Status = TransactionStatus.Error;
                mainModel.TransactionMessage.Message = "An unexpected error has occoured. Please check the Dispatch Data and try again.";
            }
            mainModel.dispatchList = model;
            return Json(mainModel);
        }

        [HttpPost]
        public JsonResult generateBillNo(string billTo,
                                        string billCategory,
                                        string transCode,
                                        string division,
                                        string incoTerm,
                                        string billDate,
                                        string hsnCode,
                                        string fromDate,
                                        string toDate,
                                        string uniqueDispatchId,
                                        string dispatchIds,
                                        int billToId,
                                        int transId,
                                        string billNo)
        {
            //Fetch BillTo
            billTo = utilityHelper.fetchBillTo(billTo);
            var getGstRates = _context.TblGstMasters.Where(x => x.TransporterCode.Contains(transCode)).FirstOrDefault();
            VMTransporterBill model = new VMTransporterBill();
            decimal? totalBillAmount = 0;
            decimal? cgstAmount = 0;
            decimal? sgstAmount = 0;
            decimal? igstAmount = 0;
            decimal? ugstAmount = 0;
            long transporterBillId = 0;
            //Boolean isTransporterBillSaved = false;
            // DateOnly frmDate = DateOnly.Parse(fromDate);
            //DateOnly tDate = DateOnly.Parse(toDate);
            IFormatProvider culture = new CultureInfo("en-US", true);

            DateTime frmDate = DateTime.ParseExact(fromDate, "yyyy-MM-dd", culture);
            DateTime tDate = DateTime.ParseExact(toDate, "yyyy-MM-dd", culture);
            DateOnly bDate = DateOnly.Parse(billDate);
            //Fetching Transporter Details
            var transporterDetails = _context.TblTransporterMasters.Where(x => x.TransporterCode.Equals(transCode)).FirstOrDefault();

            //Fetching Company Details 
            var companyDetails = _context.TblBillToMasters.Where(x => x.BillToCode.Equals(billTo)).FirstOrDefault();

            if (transporterDetails != null && companyDetails != null)
            {
                //Fetching gstRates from GST Table
                //var getGstRates = _context.TblGstMasters.Where(x => x.TransporterCode.Contains(transCode) && DateOnly.FromDateTime((DateTime)x.EffectiveDate) >= DateOnly.FromDateTime((DateTime)x..Now)).FirstOrDefault();
                //if (getGstRates == null)//If GST Rate Entry for transporter not found in Gst Rate Table
                //{
                //    //Setting Error Message and Retruning to the Page
                //    model.TransactionMessage.Status = TransactionStatus.Failed;
                //    model.TransactionMessage.Title = "GST Entry Not Found!";
                //    model.TransactionMessage.Message = "GST Entry not found for transporter. Please add a GST Rate Entry and try again!";
                //    return Json(model);
                //}
                //else
                //{

                var dispatchEntries = _context.TblDispatchDetails.Where(x => dispatchIds.Contains(x.DeliveryNo.Trim())).ToList();
                if (dispatchEntries.Count() > 0)
                {
                    //Updating GST details in Dispatch Details Table
                    foreach (TblDispatchDetail dispatchDetail in dispatchEntries)
                    {
                        getGstRates = _context.TblGstMasters.Where(x => x.TransporterCode.Contains(transCode) && DateOnly.FromDateTime((DateTime)x.EffectiveDate) <= DateOnly.FromDateTime((DateTime)dispatchDetail.PgiDate)).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
                        if (getGstRates == null)//If GST Rate Entry for transporter not found in Gst Rate Table
                        {
                            //Setting Error Message and Retruning to the Page
                            model.TransactionMessage.Status = TransactionStatus.Failed;
                            model.TransactionMessage.Title = "GST Entry Not Found!";
                            model.TransactionMessage.Message = "GST Entry not found for transporter. Please add a GST Rate Entry and try again!";
                            return Json(model);
                        }
                        else
                        {
                            if (!getGstRates.IsRcm) //If Is RCM is false means we need to calculate GST
                            {
                                dispatchDetail.CgstRate = getGstRates.CgstRate;
                                dispatchDetail.SgstRate = getGstRates.SgstRate;
                                dispatchDetail.IgstRate = getGstRates.IgstRate;
                                dispatchDetail.UtgstRate = getGstRates.UtgstRate;
                            }
                            totalBillAmount = totalBillAmount + dispatchDetail.TotalAmount;
                            dispatchDetail.BillNumber = billNo;
                            dispatchDetail.BillDate = bDate.ToDateTime(TimeOnly.Parse("12:00 AM"));
                            _context.TblDispatchDetails.Update(dispatchDetail);
                            _context.SaveChanges();
                        }
                    }

                    string gst_Type = utilityHelper.FetchApplicableTaxRates(transCode, billTo, transporterDetails.StateId, companyDetails.StateId);

                    //Adding Bill Details in Transporter Bill Details Table
                    TblTransporterBill billDetails = new TblTransporterBill();

                    billDetails.BillNumber = billNo;
                    billDetails.BillDate = bDate.ToDateTime(TimeOnly.Parse("12:00 AM"));
                    billDetails.TransporterId = transId;
                    billDetails.CompanyId = billToId;

                    if (!getGstRates.IsRcm) //If Is RCM is false means we need to calculate GST
                    {
                        if (gst_Type.Equals(SiteConstants.SGST_CGST_Tax))
                        {
                            //CGST AND SGST Tax Calcualated
                            cgstAmount = totalBillAmount * getGstRates.CgstRate / 100;
                            sgstAmount = totalBillAmount * getGstRates.SgstRate / 100;
                            billDetails.CgstAmount = cgstAmount;
                            billDetails.SgstAmount = sgstAmount;
                            billDetails.IgstAmount = 0;
                            billDetails.UgstAmount = 0;
                        }
                        else if (gst_Type.Equals(SiteConstants.IGST_Tax))
                        {
                            //IGST Tax Calcualated
                            igstAmount = totalBillAmount * getGstRates.IgstRate / 100;
                            billDetails.CgstAmount = 0;
                            billDetails.SgstAmount = 0;
                            billDetails.IgstAmount = igstAmount;
                            billDetails.UgstAmount = 0;
                        }
                        else
                        {
                            //UGST Tax Calcualated
                            ugstAmount = totalBillAmount * getGstRates.UtgstRate / 100;
                            billDetails.CgstAmount = 0;
                            billDetails.SgstAmount = 0;
                            billDetails.IgstAmount = 0;
                            billDetails.UgstAmount = ugstAmount;
                        }
                    }
                    else  //If Is RCM is ture means we need to calculate GST
                    {
                        billDetails.CgstAmount = 0;
                        billDetails.SgstAmount = 0;
                        billDetails.IgstAmount = 0;
                        billDetails.UgstAmount = 0;
                    }

                    billDetails.StartDate = frmDate;
                    billDetails.EndDate = tDate;
                    billDetails.TotalBillAmount = totalBillAmount.GetValueOrDefault(0);
                    billDetails.IsActive = true;
                    VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                    if (userDetails != null)
                    {
                        billDetails.CreatedBy = userDetails.Id;
                        billDetails.CreationDate = utilityHelper.CurrentDateTime;
                        billDetails.UpdatedBy = userDetails.Id;
                        billDetails.UpdateDate = utilityHelper.CurrentDateTime;
                    }
                    _context.TblTransporterBills.Add(billDetails);
                    _context.SaveChanges();
                    transporterBillId = billDetails.Id;
                    model.BillNumber = billNo;

                    model.TransactionMessage.Status = TransactionStatus.Success;
                    model.TransactionMessage.Title = "Transporter Bill Generation!";
                    model.TransactionMessage.Message = "Transporter Bill No Generated Successfully. ";
                }
                else
                {
                    //Setting Error Message and Retruning to the Page
                    model.TransactionMessage.Status = TransactionStatus.Failed;
                    model.TransactionMessage.Title = "Dispatch Data Not Found!";
                    model.TransactionMessage.Message = "Dispatch Data Not Found!. Please try again later!";
                    return Json(model);
                }

                //}
            }
            return Json(model);
        }

        [HttpGet]
        public JsonResult searchTransporterBills(string billNo,
                                                 string companyCode,
                                                 string transporterCode,
                                                 string billDate,
                                                 string fromDate,
                                                 string toDate)
        {
            //Fetch BillTo Company Code
            companyCode = utilityHelper.fetchBillTo(companyCode);

            //Fetch Transporter Code
            transporterCode = utilityHelper.fetchTransporterCode(transporterCode);

            List<VMTransporterBill> model = new List<VMTransporterBill>();

            //Left outer join query
            var searchModel = _context.TblTransporterBills.Select(x => new VMTransporterBill
            {
                Id = x.Id,
                BillNumber = x.BillNumber,
                BillDate =  String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                TransporterId = x.TransporterId,
                CompanyId = x.CompanyId,
                SgstAmount = x.SgstAmount,
                CgstAmount = x.CgstAmount,
                IgstAmount = x.IgstAmount,
                UgstAmount = x.UgstAmount,
                TotalBillAmount = x.TotalBillAmount,
                StartDate = x.StartDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                EndDate = x.EndDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                IsActive = x.IsActive,
                CreatedBy = x.CreatedBy,
                CreatedOn = x.CreationDate,
                UpdatedBy = x.UpdatedBy,
                UpdateDate = x.UpdateDate,
                userId = _context.TblTransporterMasters
                            .Where(p => p.Id == x.TransporterId)
                            .Select(p => p.UserId).FirstOrDefault(),
                TransporterName = _context.TblTransporterMasters
                            .Where(p => p.Id == x.TransporterId)
                            .Select(p => p.TransporterCode).FirstOrDefault(),
                CompanyName = _context.TblBillToMasters
                            .Where(p => p.Id == x.CompanyId)
                            .Select(p => p.BillToCode).FirstOrDefault(),
                CreatedByName = _context.TblUserMasters
                            .Where(p => p.Id == x.CreatedBy)
                            .Select(p => p.UserName).FirstOrDefault()
            });


            if (!string.IsNullOrEmpty(billNo))
            {
                searchModel = searchModel.Where(s => s.BillNumber.Equals(billNo));
            }

            if (!string.IsNullOrEmpty(companyCode))
            {
                if (!companyCode.ToLower().Equals("select bill to"))
                {
                    searchModel = searchModel.Where(s => s.CompanyName.Equals(companyCode));
                }
            }

            if (!string.IsNullOrEmpty(transporterCode))
            {
                if (!transporterCode.ToLower().Equals("select a transporter"))
                {
                    searchModel = searchModel.Where(s => s.TransporterName.Equals(transporterCode));
                }
            }

            if (!string.IsNullOrEmpty(billDate))
            {
                searchModel = searchModel.Where(s => s.BillDate.Equals(DateTime.Parse(billDate)));
            }

            if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate))
            {
                DateOnly? frmDate = new DateOnly(2024, 1, 1);
                DateOnly? tDate = new DateOnly(2024, 1, 1);

                frmDate = DateOnly.Parse(fromDate);
                tDate = DateOnly.Parse(toDate);

                searchModel = searchModel.Where(s => DateOnly.FromDateTime(Convert.ToDateTime(s.BillDate)) >= frmDate
                                                && DateOnly.FromDateTime(Convert.ToDateTime(s.BillDate)) <= tDate);
            }

            VMLogin userDetails = utilityHelper.getCurrentUserSession();
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.User))
                {
                    searchModel = searchModel.Where(s => s.userId.Equals(userDetails.Id));
                }
            }

            model = searchModel.OrderByDescending(x => x.Id).ToList();

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
        public JsonResult getDispatchItemList(string startDate, string endDate)
        {
            DateTime frmDate = DateTime.Parse(startDate);
            DateTime tDate = DateTime.Parse(endDate);

            List<VMDispatchDetails> model = new List<VMDispatchDetails>();
            //&& DateOnly.FromDateTime((DateTime)x.PgiDate) >= frmDate
            //&& DateOnly.FromDateTime((DateTime)x.PgiDate) <= tDate)

            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                //Fetching User is Admin or Not
                Boolean isUserAdmin = utilityHelper.isUserAdminOrSuperAdmin();

                var transporterDetails = _context.TblTransporterMasters.Where(x => x.UserId.Equals(userDetails.Id)).FirstOrDefault();

                var searchModel = _context.TblDispatchDetails.Where(x => x.PgiDate != null
                                                                      && x.PgiDate >= frmDate
                                                                      && x.PgiDate <= tDate);
                if (transporterDetails != null)
                {
                    searchModel = searchModel.Where(x => x.ForwardingAgentCode.Equals(transporterDetails.TransporterCode));
                }

                model = searchModel.Select(x => new VMDispatchDetails
                                                    {
                                                        Id = x.Id,
                                                        SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                                                        ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                                                        DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                                                        TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                                                        LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                                                        LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                                        ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                                                        DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                                                        EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                                                        EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                                                        ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                                                        ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                                                        RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                                                        PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                                                        PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                                        DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                                                        Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                                                        IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                                                        RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                                                        RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                                                        EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                                                        EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                                                        BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                                                        BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                                                        CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                                                        SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                                                        IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                                                        UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                                                        TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                                                        CreationDate = x.CreationDate,
                                                        UpdateDate = x.UpdateDate,
                                                        IsActive = x.IsActive,
                                                        isUserAdmin = isUserAdmin,
                                                        CreatedByName = _context.TblUserMasters
                                                                        .Where(p => p.Id == x.CreatedBy)
                                                                        .Select(p => p.UserName).FirstOrDefault(),
                                                        CreatedBy = x.CreatedBy,
                                                        UpdatedByName = _context.TblUserMasters
                                                                        .Where(p => p.Id == x.UpdatedBy)
                                                                        .Select(p => p.UserName).FirstOrDefault(),
                                                        UpdatedBy = x.UpdatedBy,
                                                        ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                                                    }).ToList();
            }

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        public JsonResult getTransporterBillTrans(string billNo)
        {
            List<VMDispatchDetails> model = new List<VMDispatchDetails>();

            model = _context.TblDispatchDetails.Where(x => x.BillNumber != null && x.BillNumber.Trim().Equals(billNo)).Select(x => new VMDispatchDetails
            {
                Id = x.Id,
                SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                IsActive = x.IsActive,
                ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                CreatedByName = _context.TblUserMasters
                                                                        .Where(p => p.Id == x.CreatedBy)
                                                                        .Select(p => p.UserName).FirstOrDefault(),
                CreatedBy = x.CreatedBy,
                UpdatedByName = _context.TblUserMasters
                                                                        .Where(p => p.Id == x.UpdatedBy)
                                                                        .Select(p => p.UserName).FirstOrDefault(),
                UpdatedBy = x.UpdatedBy,
                TransporterBill = _context.TblTransporterBills.Select(x => new VMTransporterBill
                {
                    Id = x.Id,
                    BillNumber = x.BillNumber,
                    BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                    TransporterId = x.TransporterId,
                    CompanyId = x.CompanyId,
                    SgstAmount = x.SgstAmount,
                    CgstAmount = x.CgstAmount,
                    IgstAmount = x.IgstAmount,
                    UgstAmount = x.UgstAmount,
                    TotalBillAmount = x.TotalBillAmount,
                    StartDate = x.StartDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndDate = x.EndDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreationDate,
                    UpdatedBy = x.UpdatedBy,
                    UpdateDate = x.UpdateDate,
                    TransporterName = _context.TblTransporterMasters
                                    .Where(p => p.Id == x.TransporterId)
                                    .Select(p => p.TransporterName).FirstOrDefault(),
                    CompanyName = _context.TblBillToMasters
                                  .Where(p => p.Id == x.CompanyId)
                                  .Select(p => p.BillToCompany).FirstOrDefault(),
                    CreatedByName = _context.TblUserMasters
                                    .Where(p => p.Id == x.CreatedBy)
                                    .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.BillNumber.Equals(billNo)).FirstOrDefault(),
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

        public JsonResult getTransporterBillTransByDispatchNo(string uniqueDispatchNo)
        {
            List<VMDispatchDetails> model = new List<VMDispatchDetails>();

            model = _context.TblDispatchDetails.Where(x => x.DispatchUniqueId.Trim().Equals(uniqueDispatchNo)).Select(x => new VMDispatchDetails
            {
                Id = x.Id,
                SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                IsActive = x.IsActive,
                ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                CreatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.CreatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                CreatedBy = x.CreatedBy,
                UpdatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.UpdatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                UpdatedBy = x.UpdatedBy
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

        public JsonResult getTransporterBillTransByLRGRUpdateDispatchNo(string uniqueDispatchNo)
        {
            List<VMDispatchDetails> model = new List<VMDispatchDetails>();

            model = _context.TblDispatchDetails.Where(x => x.LrGrNoUniqueId.Trim().Equals(uniqueDispatchNo)).Select(x => new VMDispatchDetails
            {
                Id = x.Id,
                SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                IsActive = x.IsActive,
                ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                CreatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.CreatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                CreatedBy = x.CreatedBy,
                UpdatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.UpdatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                UpdatedBy = x.UpdatedBy
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
        public JsonResult getDispatchDetailsById(Int64 dispatchId)
        {
            VMDispatchDetails model = new VMDispatchDetails();

            model = _context.TblDispatchDetails.Where(x => x.Id.Equals(dispatchId)).Select(x => new VMDispatchDetails
            {
                Id = x.Id,
                SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                IsActive = x.IsActive,
                ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                CreatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.CreatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                CreatedBy = x.CreatedBy,
                UpdatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.UpdatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                UpdatedBy = x.UpdatedBy
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
        public JsonResult getBillNumberByID(string billNo)
        {
            //Left outer join query
            var model = _context.TblDispatchDetails.Where(x => x.BillNumber != null && x.BillNumber.Trim().Equals(billNo)).Select(x => new VMDispatchDetails
            {
                Id = x.Id,
                SupplyingPlant = String.IsNullOrEmpty(x.SupplyingPlant) ? SiteConstants.Dash : x.SupplyingPlant,
                ShipmentNo = String.IsNullOrEmpty(x.ShipmentNo) ? SiteConstants.Dash : x.ShipmentNo,
                DeliveryNo = String.IsNullOrEmpty(x.DeliveryNo) ? SiteConstants.Dash : x.DeliveryNo,
                TruckNo = String.IsNullOrEmpty(x.TruckNo) ? SiteConstants.Dash : x.TruckNo,
                LrGrNo = String.IsNullOrEmpty(x.LrGrNo) ? SiteConstants.Dash : x.LrGrNo,
                LrGrDate = String.IsNullOrEmpty(Convert.ToString(x.LrGrDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.LrGrDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                ShipToPartyTzone = String.IsNullOrEmpty(x.ShipToPartyTzone) ? SiteConstants.Dash : x.ShipToPartyTzone,
                DispatchQtyRoad = String.IsNullOrEmpty(Convert.ToString(x.DispatchQtyRoad)) ? SiteConstants.Dash : Convert.ToString(x.DispatchQtyRoad),
                EbidNetAmt = String.IsNullOrEmpty(Convert.ToString(x.EbidNetAmt)) ? SiteConstants.Dash : Convert.ToString(x.EbidNetAmt),
                EbidFrtRate = String.IsNullOrEmpty(Convert.ToString(x.EbidFrtRate)) ? SiteConstants.Dash : Convert.ToString(x.EbidFrtRate),
                ForwardingAgentCode = String.IsNullOrEmpty(x.ForwardingAgentCode) ? SiteConstants.Dash : x.ForwardingAgentCode,
                ForwardingAgentName = String.IsNullOrEmpty(x.ForwardingAgentName) ? SiteConstants.Dash : x.ForwardingAgentName,
                RegionState = String.IsNullOrEmpty(x.RegionState) ? SiteConstants.Dash : x.RegionState,
                PgiNo = String.IsNullOrEmpty(x.PgiNo) ? SiteConstants.Dash : x.PgiNo,
                PgiDate = String.IsNullOrEmpty(Convert.ToString(x.PgiDate)) ? SiteConstants.Dash : Convert.ToString(x.PgiDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                DistributionChannel = String.IsNullOrEmpty(x.DistributionChannel) ? SiteConstants.Dash : x.DistributionChannel,
                Division = String.IsNullOrEmpty(x.Division) ? SiteConstants.Dash : x.Division,
                IncoTerm = String.IsNullOrEmpty(x.IncoTerm) ? SiteConstants.Dash : x.IncoTerm,
                RouteCode = String.IsNullOrEmpty(x.RouteCode) ? SiteConstants.Dash : x.RouteCode,
                RouteDescription = String.IsNullOrEmpty(x.RouteDescription) ? SiteConstants.Dash : x.RouteDescription,
                EpodNo = String.IsNullOrEmpty(x.EpodNo) ? SiteConstants.Dash : x.EpodNo,
                EpodDate = String.IsNullOrEmpty(Convert.ToString(x.EpodDate)) ? SiteConstants.Dash : Convert.ToString(x.EpodDate),
                BillNo = String.IsNullOrEmpty(x.BillNumber) ? SiteConstants.Dash : x.BillNumber,
                BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                CgstRate = String.IsNullOrEmpty(Convert.ToString(x.CgstRate)) ? SiteConstants.Dash : Convert.ToString(x.CgstRate),
                SgstRate = String.IsNullOrEmpty(Convert.ToString(x.SgstRate)) ? SiteConstants.Dash : Convert.ToString(x.SgstRate),
                IgstRate = String.IsNullOrEmpty(Convert.ToString(x.IgstRate)) ? SiteConstants.Dash : Convert.ToString(x.IgstRate),
                UtgstRate = String.IsNullOrEmpty(Convert.ToString(x.UtgstRate)) ? SiteConstants.Dash : Convert.ToString(x.UtgstRate),
                TotalAmount = String.IsNullOrEmpty(Convert.ToString(x.TotalAmount)) ? SiteConstants.Dash : Convert.ToString(x.TotalAmount),
                CreationDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                IsActive = x.IsActive,
                HsnCode = _context.TblCodeMasters
                          .Where(p => p.Code.Equals(x.Division) && p.CodeType.Equals(SiteConstants.CodeType_Hsncode))
                          .Select(p => p.Description).FirstOrDefault(),
                ExceptionalEntry = String.IsNullOrEmpty(x.ExceptionalEntry) ? SiteConstants.Dash : x.ExceptionalEntry,
                CreatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.CreatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                CreatedBy = x.CreatedBy,
                UpdatedByName = _context.TblUserMasters
                                .Where(p => p.Id == x.UpdatedBy)
                                .Select(p => p.UserName).FirstOrDefault(),
                UpdatedBy = x.UpdatedBy,
                TransporterBill = _context.TblTransporterBills.Select(t => new VMTransporterBill
                {
                    Id = t.Id,
                    BillNumber = t.BillNumber,
                    BillDate = String.IsNullOrEmpty(Convert.ToString(x.BillDate)) ? SiteConstants.Dash : Convert.ToString(Convert.ToDateTime(x.BillDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                    BillDateInString = t.BillDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    TransporterId = t.TransporterId,
                    CompanyId = t.CompanyId,
                    SgstAmount = t.SgstAmount,
                    CgstAmount = t.CgstAmount,
                    IgstAmount = t.IgstAmount,
                    UgstAmount = t.UgstAmount,
                    TotalBillAmount = t.TotalBillAmount,
                    StartDate = t.StartDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    EndDate = t.EndDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                    IsActive = t.IsActive,
                    CreatedBy = t.CreatedBy,
                    CreatedOn = t.CreationDate,
                    UpdatedBy = t.UpdatedBy,
                    UpdateDate = t.UpdateDate,
                    TransporterName = _context.TblTransporterMasters
                                              .Where(p => p.Id == t.TransporterId)
                                              .Select(p => p.TransporterName).FirstOrDefault(),
                    CompanyName = _context.TblBillToMasters
                                              .Where(p => p.Id == t.CompanyId)
                                              .Select(p => p.BillToCompany).FirstOrDefault(),
                    CreatedByName = _context.TblUserMasters
                                              .Where(p => p.Id == t.CreatedBy)
                                              .Select(p => p.UserName).FirstOrDefault()
                }).Where(x => x.BillNumber.Equals(billNo)).FirstOrDefault(),
                CompanyMaster = _context.TblBillToMasters.Select(c => new VMCompanyMaster
                {
                    Id = c.Id,
                    BillToCode = c.BillToCode,
                    BillToCompanyName = c.BillToCompany,
                    Address1 = c.Address1,
                    Address2 = c.Address2,
                    Address3 = c.Address3,
                    StateId = c.StateId,
                    DistrictId = c.DistrictId,
                    CityId = c.CityId,
                    PinCode = c.PinCode,
                    GSTINNo = c.GstinNo,
                    PanNo = c.PanNo,
                    IsActive = c.IsActive,
                    CreateDate = c.CreationDate,
                    UpdateDate = c.UpdateDate,
                    CreatedBy = Convert.ToString(c.CreatedBy),
                    UpdateBy = Convert.ToString(c.UpdatedBy),
                    StateCode = _context.TblStates
                    .Where(p => p.Id == c.StateId)
                    .Select(p => p.StateCode).FirstOrDefault(),
                    StateName = _context.TblStates.Where(p => p.Id == c.StateId)
                                                  .Select(p => p.StateName).FirstOrDefault(),
                    DistrictName = _context.TblDistricts.Where(p => p.Id == c.DistrictId)
                                           .Select(p => p.DistrictName).FirstOrDefault(),
                    CityName = _context.TblCities.Where(p => p.Id == c.CityId)
                                           .Select(p => p.CityName).FirstOrDefault()
                }).Where(c => c.BillToCode.Equals(x.SupplyingPlant)).FirstOrDefault(),
                TransporterMaster = _context.TblTransporterMasters.Where(t => t.TransporterCode.Equals(x.ForwardingAgentCode)).Select(t => new VMTransporterMaster
                {
                    Id = t.Id,
                    TransporterCode = t.TransporterCode,
                    TransporterName = t.TransporterName,
                    OwnerName = t.OwnerName,
                    MobileNo = t.MobileNumber,
                    EmailID = t.EmailId,
                    Address1 = t.Address1,
                    Address2 = t.Address2,
                    Address3 = t.Address3,
                    StateId = t.StateId,
                    DistrictId = t.DistrictId,
                    CityId = t.CityId,
                    PinCode = t.PinCode,
                    GSTINNo = t.GstinNo,
                    PanNo = t.PanNo,
                    UserId = t.UserId,
                    BillPrifix = t.BillPrefix,
                    IsActive = t.IsActive,
                    CreateDate = t.CreationDate,
                    UpdateDate = t.UpdateDate,
                    CreatedBy = Convert.ToString(t.CreatedBy),
                    UpdateBy = Convert.ToString(t.UpdatedBy),
                    StateCode = _context.TblStates
                    .Where(p => p.Id == t.StateId)
                    .Select(p => p.StateCode).FirstOrDefault(),
                    UserName = _context.TblUserMasters
                    .Where(p => p.Id == t.UserId)
                    .Select(p => p.UserName).FirstOrDefault(),
                    StateName = _context.TblStates
                    .Where(p => p.Id == t.StateId)
                    .Select(p => p.StateName).FirstOrDefault(),
                    DistrictName = _context.TblDistricts
                    .Where(p => p.Id == t.DistrictId)
                    .Select(p => p.DistrictName).FirstOrDefault(),
                    CityName = _context.TblCities
                    .Where(p => p.Id == t.CityId)
                    .Select(p => p.CityName).FirstOrDefault(),
                    IsUnionTerritory = _context.TblStates
                     .Where(p => p.Id == t.StateId)
                    .Select(p => p.IsUnionTerritory).FirstOrDefault(),
                    GstMaster = _context.TblGstMasters
                      .Where(p => p.TransporterCode == t.TransporterCode && DateOnly.FromDateTime((DateTime)p.EffectiveDate) <= DateOnly.FromDateTime((DateTime)x.PgiDate)).OrderByDescending(x => x.EffectiveDate)

                    .Select(g => new VMGstMaster
                    {
                        Id = g.Id,
                        TransporterCode = g.TransporterCode,
                        SgstRate = g.SgstRate,
                        CgstRate = g.CgstRate,
                        IgstRate = g.IgstRate,
                        UgstRate = g.UtgstRate,
                        EffectiveDate = g.EffectiveDate,
                        EndDate = g.EndDate,
                        IsRcm = g.IsRcm,
                        IsActive = g.IsActive,
                        CreationDate = g.CreationDate,
                        UpdateDate = g.UpdateDate,
                        CreatedBy = _context.TblUserMasters
                                    .Where(p => p.Id == x.CreatedBy)
                                    .Select(p => p.UserName).FirstOrDefault(),
                        UpdatedBy = _context.TblUserMasters
                                    .Where(p => p.Id == x.UpdatedBy)
                                    .Select(p => p.UserName).FirstOrDefault()
                    }).FirstOrDefault()
                }).FirstOrDefault()
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

        public FileStreamResult downlodPDF()
        {
            string transporterFileName = HttpContext.Session.GetObjectFromJson<String>("transporterFile");
            if (!String.IsNullOrEmpty(transporterFileName))
            {
                string wwwPath = this._environment.WebRootPath;
                wwwPath = wwwPath + SiteConstants.Backslash + SiteConstants.transporterBillsFolder + SiteConstants.Backslash;
                string fullFileName = wwwPath + transporterFileName;

                if (System.IO.File.Exists(fullFileName))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fullFileName, FileMode.Open))
                    {
                        stream.CopyTo(memory);
                    }
                    memory.Position = 0;
                    //Retruning File Content to Download
                    return File(memory, "application/pdf", transporterFileName);
                }
            }
            return null;
        }

        [HttpPost]
        public ActionResult saveITextSharpPDF(string divHTML, string billNo)
        {
            VMTransporterBill model = new VMTransporterBill();

            string HTMLContent = divHTML;// Put your html tempelate here

            HTMLContent = HTMLContent.Replace("printText", "pdfPrintText");

            HTMLContent = HTMLContent.Replace("printTextBold", "pdfPrintTextBold");

            HTMLContent = HTMLContent.Replace("printTextHeading", "pdfPrintHeading");

            HTMLContent = HTMLContent.Replace("printTextHeadingDispatch", "pdfPrintHeadingDispatch");

            HTMLContent = HTMLContent.Replace("pdfPrintTextHeading", "pdfPrintHeading");

            HTMLContent = HTMLContent.Replace("printTextDetails", "pdfPrintTextDetails");

            HTMLContent = HTMLContent.Replace("printTextDetails", "pdfPrintTextDetails");

            HTMLContent = HTMLContent.Replace("border=\"1\"", "");
            
            string wwwPath = this._environment.WebRootPath;
            wwwPath = wwwPath + SiteConstants.Backslash + SiteConstants.transporterBillsFolder + SiteConstants.Backslash;
            billNo = billNo.Replace(SiteConstants.Forwardslash, SiteConstants.Underscore);
            string fileName = SiteConstants.TransporterBillPrefix
                            + SiteConstants.Underscore + billNo
                            + SiteConstants.Underscore + DateTime.Now.ToString("ddMMyyyyHHss")
                            + SiteConstants.Dot + SiteConstants.pdfFileExtension;
            string fullFileName = wwwPath + fileName;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(HTMLContent);
                Document pdfDoc = new Document(PageSize.A4, 20f, 5f, 15f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                cssResolver.AddCssFile(_environment.WebRootPath + "\\css\\vertical-layout-light\\style.css", true);
                IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(pdfDoc, writer)));
                var worker = new XMLWorker(pipeline, true);
                var xmlParse = new XMLParser(true, worker);
                pdfDoc.Open();
                xmlParse.Parse(sr);
                xmlParse.Flush();
                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();

                byte[] bytes = stream.ToArray();

                //Saving File to Folder
                System.IO.File.WriteAllBytes(fullFileName, bytes);

                //Setting Filename and path to Session Object 
                HttpContext.Session.SetObjectAsJson("transporterFile", fileName);
            }

            model.TransactionMessage.Status = TransactionStatus.Success;
            model.TransactionMessage.Message = "PDF File Generated Successfully!";

            return Json(model);
        }

        [HttpPost]
        public ActionResult savePDFandEmail(string divHTML, string billNo,
                                            string transName, string transEmail,
                                            string companyName)
        {
            VMTransporterBill model = new VMTransporterBill();

            string HTMLContent = divHTML;// Put your html tempelate here

            HTMLContent = HTMLContent.Replace("printText", "pdfPrintText");

            HTMLContent = HTMLContent.Replace("printTextBold", "pdfPrintTextBold");

            HTMLContent = HTMLContent.Replace("printTextHeading", "pdfPrintHeading");

            HTMLContent = HTMLContent.Replace("printTextHeadingDispatch", "pdfPrintHeadingDispatch");

            HTMLContent = HTMLContent.Replace("pdfPrintTextHeading", "pdfPrintHeading");

            HTMLContent = HTMLContent.Replace("printTextDetails", "pdfPrintTextDetails");

            HTMLContent = HTMLContent.Replace("border=\"1\"", "");

            #region...[Commented Code for Previous Logicof Creating PDF]...
            //MemoryStream ms = new MemoryStream();
            //TextReader txtReader = new StringReader(HTMLContent);

            //// 1: create object of a itextsharp document class  
            //Document doc = new Document(PageSize.A1, 25, 25, 25, 25);

            //// 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
            //PdfWriter PdfWriter = PdfWriter.GetInstance(doc, ms);
            //PdfWriter.CloseStream = false;

            //// 3: we create a worker parse the document  
            //HTMLWorker htmlWorker = new HTMLWorker(doc);

            //// 4: we open document and start the worker on the document  
            //doc.Open();
            //htmlWorker.StartDocument();

            //// 5: parse the html into the document  
            //htmlWorker.Parse(txtReader);

            //// 6: close the document and the worker  
            //htmlWorker.EndDocument();
            //htmlWorker.Close();
            //doc.Close();

            //ms.Flush(); //Always catches me out
            //ms.Position = 0; //Not sure if this is required

            //byte[] bytes = ms.ToArray();

            //Saving File to Folder
            //System.IO.File.WriteAllBytes(fullFileName, bytes);
            #endregion

            string wwwPath = this._environment.WebRootPath;
            wwwPath = wwwPath + SiteConstants.Backslash + SiteConstants.transporterBillsFolder + SiteConstants.Backslash;
            billNo = billNo.Replace(SiteConstants.Forwardslash, SiteConstants.Underscore);
            string fileName = SiteConstants.TransporterBillPrefix
                            + SiteConstants.Underscore + billNo
                            + SiteConstants.Underscore + DateTime.Now.ToString("ddMMyyyyHHss")
                            + SiteConstants.Dot + SiteConstants.pdfFileExtension;
            string fullFileName = wwwPath + fileName;

            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringReader sr = new StringReader(HTMLContent);
                Document pdfDoc = new Document(PageSize.A4, 20f, 5f, 15f, 10f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);

                HtmlPipelineContext htmlContext = new HtmlPipelineContext(null);
                htmlContext.SetTagFactory(Tags.GetHtmlTagProcessorFactory());
                ICSSResolver cssResolver = XMLWorkerHelper.GetInstance().GetDefaultCssResolver(false);
                cssResolver.AddCssFile(_environment.WebRootPath + "\\css\\vertical-layout-light\\style.css", true);
                IPipeline pipeline = new CssResolverPipeline(cssResolver, new HtmlPipeline(htmlContext, new PdfWriterPipeline(pdfDoc, writer)));
                var worker = new XMLWorker(pipeline, true);
                var xmlParse = new XMLParser(true, worker);
                pdfDoc.Open();
                xmlParse.Parse(sr);
                xmlParse.Flush();
                //XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();

                byte[] bytes = stream.ToArray();

                //Saving File to Folder
                System.IO.File.WriteAllBytes(fullFileName, bytes);
            }

            if (System.IO.File.Exists(fullFileName))
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(fullFileName, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;
                //Setting FileName Session Object to Null
                string toEmailID = transEmail;
                string Subject = "Transporter Bill : " + billNo;
                string Body = String.Empty;

                //Preparing HTML Body
                StringBuilder sb = new StringBuilder();
                sb.Append("<table cellpadding='5' cellspacing='0' style='border: none;font-size: 9pt;font-family:Arial; width:100%;'>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none; padding-bottom:10px; text-align:left;'>Dear, <b>" + transName + "</b></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none; padding-bottom:10px;'>Your Bill Number is : " + billNo + "</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none; padding-bottom:10px;'>Please find the system generated Bill No attached with the email.</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none;'></td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none; font-weight:bold;'>Thanks & Regards</td>");
                sb.Append("</tr>");
                sb.Append("<tr>");
                sb.Append("<td style='border: none;padding-bottom:10px; font-weight:bold;'>" + companyName + "</td>");
                sb.Append("</tr>");
                sb.Append("</table>");

                Body = sb.ToString();

                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                if (userDetails != null)
                {
                    SendEmailAndAttachment(toEmailID, Subject, Body, memory, fileName, userDetails.Email, userDetails.UserName);
                }
            }
            model.TransactionMessage.Status = TransactionStatus.Success;
            model.TransactionMessage.Message = "PDF File Generated and Sent Email Successfully!";
            return Json(model);
        }

        [NonAction]
        public async Task SendEmailWithAttachment(string toEmailID, string Subject,
                                                  string Body, Stream memory,
                                                  string transporterFileName,
                                                  string Email, string FirstName,
                                                  string LastName)
        {
            await _emailService.SendEmailAsync(toEmailID, Subject, Body, memory, transporterFileName, Email, FirstName + " " + LastName, true);
        }

        [NonAction]
        public void SendEmailAndAttachment(string toEmailID, string Subject,
                                            string Body, Stream memory,
                                            string transporterFileName,
                                            string Email, string UserName)
        {
            _emailService.sendEmail(toEmailID, Subject, Body, memory, transporterFileName, Email, UserName, true);
        }

        public FileStreamResult downloadDispatchErrorFile(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                string wwwPath = this._environment.WebRootPath;
                wwwPath = wwwPath + SiteConstants.Backslash + SiteConstants.dispatchErrorFileFolder + SiteConstants.Backslash; // "\\TransporterBills\\";
                string fullFileName = wwwPath + fileName;

                if (System.IO.File.Exists(fullFileName))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fullFileName, FileMode.Open))
                    {
                        stream.CopyTo(memory);
                    }
                    memory.Position = 0;
                    //Retruning File Content to Download
                    return File(memory, "application/pdf", fileName);
                }
            }
            return null;
        }

        public FileStreamResult downloadLrGrNoUpdateErrorFileName(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                string wwwPath = this._environment.WebRootPath;
                wwwPath = wwwPath + SiteConstants.Backslash + SiteConstants.lrGrNoUpdateFolder + SiteConstants.Backslash; // "\\TransporterBills\\";
                string fullFileName = wwwPath + fileName;

                if (System.IO.File.Exists(fullFileName))
                {
                    var memory = new MemoryStream();
                    using (var stream = new FileStream(fullFileName, FileMode.Open))
                    {
                        stream.CopyTo(memory);
                    }
                    memory.Position = 0;
                    //Retruning File Content to Download
                    return File(memory, "application/pdf", fileName);
                }
            }
            return null;
        }

        #region ...[Commented Code for Old Logic of Temp Table and Excel Upload from form]...
        //[HttpPost]
        //public JsonResult SaveTempraryDispatchDetails(string billTo,
        //                                              string transporterCode,
        //                                              string billCategory,
        //                                              string billDate,
        //                                              string division,
        //                                              string incoTerm,
        //                                              string fromDate,
        //                                              string toDate,
        //                                              IFormFile postedFile)
        //{
        //    //Fetch BillTo
        //    billTo = utilityHelper.fetchBillTo(billTo);

        //    string uniqueId = string.Empty;
        //    if (postedFile != null)
        //    {
        //        //Saving Uploaded File to a Folder
        //        string filePath = utilityHelper.saveUploadFilesToFolder(postedFile, SiteConstants.dispatchFileFolder, "Dispatch");

        //        VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
        //        if (userDetails != null)
        //        {
        //            uniqueId = utilityHelper.saveDispatchDataInTempTable(filePath, userDetails.Id);
        //        }
        //    }
        //    return Json(uniqueId);
        //}
        #endregion

        #region...[Commented Code for Previous Logicof Creating PDF]...
        //[HttpPost]
        //public ActionResult savePDF(string divHTML, string billNo)
        //{
        //    VMTransporterBill model = new VMTransporterBill();

        //    string HTMLContent = divHTML;// Put your html tempelate here

        //    MemoryStream ms = new MemoryStream();
        //    TextReader txtReader = new StringReader(HTMLContent);

        //    // 1: create object of a itextsharp document class  
        //    Document doc = new Document(PageSize.A4, 13f, 10f, 10f, 11f);

        //    // 2: we create a itextsharp pdfwriter that listens to the document and directs a XML-stream to a file  
        //    PdfWriter PdfWriter = PdfWriter.GetInstance(doc, ms);
        //    PdfWriter.CloseStream = false;

        //    // 3: we create a worker parse the document  
        //    HTMLWorker htmlWorker = new HTMLWorker(doc);

        //    // 4: we open document and start the worker on the document  
        //    doc.Open();
        //    htmlWorker.StartDocument();

        //    // 5: parse the html into the document  
        //    htmlWorker.Parse(txtReader);

        //    // 6: close the document and the worker  
        //    htmlWorker.EndDocument();
        //    htmlWorker.Close();
        //    doc.Close();

        //    ms.Flush(); //Always catches me out
        //    ms.Position = 0; //Not sure if this is required

        //    byte[] bytes = ms.ToArray();

        //    string wwwPath = this._environment.WebRootPath;
        //    wwwPath = wwwPath + SiteConstants.Backslash + SiteConstants.transporterBillsFolder + SiteConstants.Backslash;
        //    billNo = billNo.Replace(SiteConstants.Forwardslash, SiteConstants.Underscore);
        //    string fileName = SiteConstants.TransporterBillPrefix 
        //                    + SiteConstants.Underscore + billNo
        //                    + SiteConstants.Underscore + DateTime.Now.ToString("ddMMyyyyHHss")
        //                    + SiteConstants.Dot + SiteConstants.pdfFileExtension;
        //    string fullFileName = wwwPath + fileName;

        //    //Saving File to Folder
        //    System.IO.File.WriteAllBytes(fullFileName, bytes);

        //    //Setting Filename and path to Session Object 
        //    HttpContext.Session.SetObjectAsJson("transporterFile", fileName);

        //    model.TransactionMessage.Status = TransactionStatus.Success;
        //    model.TransactionMessage.Message = "PDF File Generated Successfully!";
        //    return Json(model);
        //}
        #endregion
    }
}
