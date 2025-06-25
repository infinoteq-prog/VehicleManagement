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
using OfficeOpenXml;
using DocumentFormat.OpenXml.Office.Word;

namespace VMS.Controllers
{
    public class ServiceDueController : Controller
    {
        private readonly ILogger<ServiceDueController> _logger;
        private readonly VmsDbContext _context; private readonly string _connectionString;
        private string _controllerName = "ServiceDue";
        public ServiceDueController(VmsDbContext context, IConfiguration configuration)
        {
            _context = context; 
            _connectionString = configuration.GetConnectionString("VMSContext"); 
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Print(String Id)
        {
            ViewBag.Id = Id;
            return View("Print");
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> searchServiceDueMaster(int id, int vehicleNo,  string dueDateFrom,
          string dueDateTo)
        {
            try
            {
                if (dueDateFrom.ToStringFromNull() == "" && dueDateTo.ToStringFromNull() == "")
                {
                    dueDateFrom = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    dueDateTo = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var model = await ServiceDueMasterContext.searchServiceDueMaster(_connectionString, id, vehicleNo, dueDateFrom,
                dueDateTo);


                if (model != null)
                {
                    return Json(model);
                }
                else
                {
                    return Json(null);
                }
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }
            
        [HttpPost]
        public JsonResult deleteServiceDue(int Id)
        {
            VMDriverMaster model = new VMDriverMaster();
            try
            {
                var dieselHisab = _context.TblServiceDueMaster.Where(x => x.Id.Equals(Id));
                _context.TblServiceDueMaster.RemoveRange(dieselHisab);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Service Due Master has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Service Due Master has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpPost]
        public JsonResult SaveUpdate([FromBody] VMServiceDueMaster model)
        {
            // You can use a generic response object for consistency
            var response = new { success = false, message = "" };
            try
            {
                Globalsettings.Log(_controllerName, string.Format("Before conversion StartDate {0}, EndDate {1}", model.StrPurchaseDate, model.StrDueDate));

                model.PurchaseDate = Convert.ToDateTime(model.StrPurchaseDate);
                model.DueDate = Convert.ToDateTime(model.StrDueDate);
                Globalsettings.Log(_controllerName, string.Format("After conversion StartDate {0}, EndDate {1}", Convert.ToDateTime(model.PurchaseDate), Convert.ToDateTime(model.DueDate)));

            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("Error occured while converting date {0}", ex.Message));
                response = new { success = false, message = ex.Message};
                return Json(response);
            }

            // Retrieve user ID from session
            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;
            }
            else
            {
                response = new { success = false, message = "User not authenticated. Please log in again." };            
                return Json(response);
            }

            try
            {
                if (!ModelState.IsValid || model == null || model.VehicleId <= 0 || string.IsNullOrWhiteSpace(model.ServiceCode) ||
                    model.IntervalKm <= 0 || model.IntervalMonth <= 0 || model.PurchaseDate == DateTime.MinValue ||
                    model.DueDate == DateTime.MinValue || model.PartCost < 0 || model.LabourCost < 0 ||
                    string.IsNullOrWhiteSpace(model.Workshop))
                {
                    response = new { success = false, message = "Please fill in all mandatory fields correctly." };
                    return Json(response);
                }

                model.TotalCost = model.PartCost + model.LabourCost;
                if (model.Id <= 0) // Insert new record
                {
                    #region Insert Section
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            TblServiceDueMaster mst = new TblServiceDueMaster();
                            mst.VehicleId  = model.VehicleId.ToIntFromNull();
                            mst.ServiceCode = model.ServiceCode;
                            mst.IntervalKm = model.IntervalKm.ToIntFromNull();
                            mst.IntervalMonth = model.IntervalMonth.ToIntFromNull();
                            mst.PurchaseDate = model.PurchaseDate;
                            mst.DueDate = model.DueDate;
                            mst.PartCost = model.LabourCost.To3Decimal();
                            mst.LabourCost = model.LabourCost.To3Decimal();
                            mst.TotalCost = model.TotalCost.To3Decimal(); 
                            mst.Workshop = model.Workshop;
                            mst.Remarks = model.Remarks;
                            mst.CreationDate = utilityHelper.CurrentDateTime;
                           mst.UpdateDate = utilityHelper.CurrentDateTime;
                           mst.CreatedBy = userID;
                           mst.UpdatedBy = userID;
                           mst.IsActive = model.IsActive; // Default to active for new entries

                            _context.TblServiceDueMaster.Add(mst);
                            _context.SaveChanges();

                            transaction.Commit();
                            response = new { success = true, message = "Service Due details have been saved successfully." };
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Globalsettings.Log(_controllerName, $"Error saving Service Due details: {ex.Message} - {ex.StackTrace}");
                            response = new { success = false, message = "Service Due details could not be saved due to a technical issue. Please try again." };
                        }
                    }
                    #endregion
                }
                else // Update existing record
                {
                    #region Update Section
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            var existingServiceDue = _context.TblServiceDueMaster.FirstOrDefault(sd => sd.Id == model.Id);

                            if (existingServiceDue != null)
                            {
                                // Update properties from the incoming model
                                existingServiceDue.VehicleId = model.VehicleId;
                                existingServiceDue.PurchaseDate = model.PurchaseDate;
                                existingServiceDue.ServiceCode = model.ServiceCode;
                                existingServiceDue.IntervalKm = model.IntervalKm;
                                existingServiceDue.IntervalMonth = model.IntervalMonth;
                                existingServiceDue.DueDate = model.DueDate;
                                existingServiceDue.PartCost = model.PartCost;
                                existingServiceDue.LabourCost = model.LabourCost;
                                existingServiceDue.TotalCost = model.TotalCost; 
                                existingServiceDue.Workshop = model.Workshop;
                                existingServiceDue.Remarks = model.Remarks;
                                existingServiceDue.IsActive = model.IsActive;
                                existingServiceDue.UpdateDate = utilityHelper.CurrentDateTime;
                                existingServiceDue.UpdatedBy = userID;

                                _context.TblServiceDueMaster.Update(existingServiceDue);
                                _context.SaveChanges();

                                transaction.Commit();
                                response = new { success = true, message = "Service Due details have been updated successfully." };
                            }
                            else
                            {
                                response = new { success = false, message = $"Service Due with ID '{model.Id}' not found for update." };
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Globalsettings.Log(_controllerName, $"Error updating Service Due details: {ex.Message} - {ex.StackTrace}");
                            response = new { success = false, message = "Service Due details could not be updated due to a technical issue. Please try again." };
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, $"General error in SaveUpdate: {ex.Message} - {ex.StackTrace}");
                response = new { success = false, message = "An unexpected error occurred. Please try again." };
            }

            return Json(response);
        }

        // Example Get method for Service Due By ID (for edit functionality)
        [HttpGet]
        public JsonResult GetServiceDueById(int id)
        {
            try
            {
                var serviceDue = _context.TblServiceDueMaster.FirstOrDefault(sd => sd.Id == id);
                if (serviceDue != null)
                {
                    return Json(serviceDue);
                }
                return Json(null);
            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, $"Error fetching Service Due by ID {id}: {ex.Message} - {ex.StackTrace}");
                return Json(new { success = false, message = "Error fetching details." });
            }
        }

    }
}
