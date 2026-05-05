using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Globalization;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp.text;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Office.Word;
using DocumentFormat.OpenXml.Drawing;
using Rotativa.AspNetCore;
using ExcelDataReader;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
namespace VMS.Controllers
{
    public class ChallanEntryController : Controller
    {
        private readonly ILogger<ChallanEntryController> _logger;
        private readonly VmsDbContext _context;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ChallanEntryController(VmsDbContext context,IConfiguration configuration,ILogger<ChallanEntryController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("VMSContext");
            _httpContextAccessor = httpContextAccessor;
        }
        public ActionResult ChallanEntry()
        {
            return View();
        }

        // Replace code 3-Mar-2026 
        public IActionResult SaveChallanEntry(int? ChallanId, DateTime ChallanEntryDate, DateTime ChallanDate, string ChallanNo, string VehicleNo, string DriverName, decimal Amount, string Remark, string ActionName, IFormFile ChallanFile, IFormFile ReceiptFile, string GrievancesTicket, string ChallanStatus, DateTime? PaidDate, int DriverId)
        {
            byte[] challanFileData = null;
            byte[] receiptFileData = null;
            string challanFileName = null;
            string challanContentType = null;
            string receiptFileName = null;
            string receiptContentType = null;

            try
            {
                string userId = HttpContext.Session.GetString("UserId");
                if (ChallanFile != null && ChallanFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    ChallanFile.CopyTo(ms);
                    challanFileData = ms.ToArray();
                    challanFileName = ChallanFile.FileName;
                    challanContentType = ChallanFile.ContentType;
                }

                if (ReceiptFile != null && ReceiptFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    ReceiptFile.CopyTo(ms);
                    receiptFileData = ms.ToArray();
                    receiptFileName = ReceiptFile.FileName;
                    receiptContentType = ReceiptFile.ContentType;
                }
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("SP_SaveChallanEntry", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ChallanId", (object)ChallanId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChallanEntryDate", ChallanEntryDate);
                cmd.Parameters.AddWithValue("@ChallanDate", ChallanDate);
                cmd.Parameters.AddWithValue("@ChallanNo", ChallanNo);
                cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmd.Parameters.AddWithValue("@DriverName", DriverName);
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@Remark", (object)Remark ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionName", string.IsNullOrEmpty(ActionName) ? (object)DBNull.Value : ActionName);
                var fileParam = cmd.Parameters.Add("@ChallanAttachment", SqlDbType.VarBinary, -1);
                fileParam.Value = challanFileData ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@ChallanFileName", (object)challanFileName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChallanContentType", (object)challanContentType ?? DBNull.Value);
                var fileParamReceipt = cmd.Parameters.Add("@ReceiptAttachment", SqlDbType.VarBinary, -1);
                fileParamReceipt.Value = receiptFileData ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@ReceiptFileName", (object)receiptFileName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReceiptContentType", (object)receiptContentType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", (object)userId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@GrievancesTicket", (object)GrievancesTicket ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChallanStatus", (object)ChallanStatus ?? DBNull.Value); ;
                cmd.Parameters.AddWithValue("@PaidDate", PaidDate.HasValue ? (object)PaidDate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@DriverId", (object)DriverId ?? DBNull.Value); ;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Json(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

      /*  // commented on 3-Mar-2026
        public IActionResult SaveChallanEntry(int? ChallanId,DateTime ChallanEntryDate,DateTime ChallanDate,string ChallanNo,string VehicleNo,string DriverName,decimal Amount,string Remark,string ActionName,IFormFile ChallanFile,IFormFile ReceiptFile,string GrievancesTicket, string ChallanStatus, DateTime? PaidDate, int DriverId)
        {
            byte[] challanFileData = null;
            byte[] receiptFileData = null;
            string challanFileName = null;
            string challanContentType = null;
            string receiptFileName = null;
            string receiptContentType = null;

            try
            {
                if (ChallanFile != null && ChallanFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    ChallanFile.CopyTo(ms);
                    challanFileData = ms.ToArray();
                    challanFileName = ChallanFile.FileName;
                    challanContentType = ChallanFile.ContentType;
                }

                if (ReceiptFile != null && ReceiptFile.Length > 0)
                {
                    using var ms = new MemoryStream();
                    ReceiptFile.CopyTo(ms);
                    receiptFileData = ms.ToArray();
                    receiptFileName = ReceiptFile.FileName;
                    receiptContentType = ReceiptFile.ContentType;
                }
                using SqlConnection con = new SqlConnection(_connectionString);
                using SqlCommand cmd = new SqlCommand("SP_SaveChallanEntry", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@ChallanId", (object)ChallanId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChallanEntryDate", ChallanEntryDate);
                cmd.Parameters.AddWithValue("@ChallanDate", ChallanDate);
                cmd.Parameters.AddWithValue("@ChallanNo", ChallanNo);
                cmd.Parameters.AddWithValue("@VehicleNo", VehicleNo);
                cmd.Parameters.AddWithValue("@DriverName", DriverName);
                cmd.Parameters.AddWithValue("@Amount", Amount);
                cmd.Parameters.AddWithValue("@Remark", (object)Remark ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ActionName", string.IsNullOrEmpty(ActionName) ? (object)DBNull.Value : ActionName);
                var fileParam = cmd.Parameters.Add("@ChallanAttachment", SqlDbType.VarBinary, -1);
                fileParam.Value = challanFileData ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@ChallanFileName", (object)challanFileName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChallanContentType", (object)challanContentType ?? DBNull.Value);
                var fileParamReceipt = cmd.Parameters.Add("@ReceiptAttachment", SqlDbType.VarBinary, -1);
                fileParamReceipt.Value = receiptFileData ?? (object)DBNull.Value;
                cmd.Parameters.AddWithValue("@ReceiptFileName", (object)receiptFileName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ReceiptContentType", (object)receiptContentType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", User.Identity?.Name ?? "Admin");
                cmd.Parameters.AddWithValue("@GrievancesTicket", (object)GrievancesTicket ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ChallanStatus", (object)ChallanStatus ?? DBNull.Value);;
                cmd.Parameters.AddWithValue("@PaidDate",PaidDate.HasValue ? (object)PaidDate.Value : DBNull.Value);
                cmd.Parameters.AddWithValue("@DriverId", (object)DriverId ?? DBNull.Value); ;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                return Json(new { success = true, message = "Saved Successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        */
        public IActionResult GetChallanEntryList(string vehicleNo,string driverName,DateTime? startDate,DateTime? endDate)
        {
            List<object> challanList = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetChallanEntryList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VehicleNo",string.IsNullOrEmpty(vehicleNo) ? (object)DBNull.Value : vehicleNo);
                    cmd.Parameters.AddWithValue("@DriverName",string.IsNullOrEmpty(driverName) ? (object)DBNull.Value : driverName);
                    cmd.Parameters.AddWithValue("@StartDate",(object)startDate ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate",(object)endDate ?? DBNull.Value);
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            challanList.Add(new
                            {
                                ChallanId = rdr["ChallanId"] != DBNull.Value ? Convert.ToInt32(rdr["ChallanId"]) : 0,
                                VehicleNo = rdr["VehicleNo"] != DBNull.Value? rdr["VehicleNo"].ToString(): "",
                                DriverName = rdr["DriverName"] != DBNull.Value? rdr["DriverName"].ToString(): "",
                                ChallanDate = FormatChallanDate(rdr["ChallanDate"]),
                                Amount = rdr["Amount"] != DBNull.Value? Convert.ToDecimal(rdr["Amount"]): 0,
                                ActionName = rdr["ActionName"] != DBNull.Value? rdr["ActionName"].ToString(): "",
                                ChallanNo = rdr["ChallanNo"] != DBNull.Value? rdr["ChallanNo"].ToString(): "",
                                Remark = rdr["Remark"] != DBNull.Value? rdr["Remark"].ToString(): "",
                                FileName = rdr["ChallanFileName"] != DBNull.Value? rdr["ChallanFileName"].ToString(): "",
                                CreatedOn = FormatChallanDate(rdr["CreatedOn"])

                            });
                        }
                    }
                }
            }
            return Json(challanList);
        }
        public IActionResult ViewChallanFile(int id)
        {
            byte[] fileBytes = null;
            string contentType = "";
            string fileName = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ChallanAttachment, ChallanFileName, ChallanContentType FROM ChallanEntry WHERE ChallanId = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            fileBytes = rdr["ChallanAttachment"] as byte[];
                            fileName = rdr["ChallanFileName"].ToString();
                            contentType = rdr["ChallanContentType"].ToString();
                        }
                    }

                    con.Close();
                }
            }

            if (fileBytes == null)
                return NotFound();
            return File(fileBytes, contentType);
        }
        public IActionResult ViewReceiptFile(int id)
        {
            byte[] fileBytes = null;
            string contentType = "";
            string fileName = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT ReceiptAttachment, ReceiptFileName, ReceiptContentType FROM ChallanEntry WHERE ChallanId = @Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    con.Open();

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            fileBytes = rdr["ReceiptAttachment"] as byte[];
                            fileName = rdr["ReceiptFileName"].ToString();
                            contentType = rdr["ReceiptContentType"].ToString();
                        }
                    }

                    con.Close();
                }
            }

            if (fileBytes == null)
                return NotFound();
            return File(fileBytes, contentType);
        }

        public IActionResult GetChallanById(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetChallanById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanId", id);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    if (rdr.Read())
                    {
                        return Json(new
                        {
                            challanId = rdr["ChallanId"] != DBNull.Value ? rdr["ChallanId"] : null,

                            challanEntryDate = rdr["ChallanEntryDate"] != DBNull.Value
                                ? Convert.ToDateTime(rdr["ChallanEntryDate"]).ToString("yyyy-MM-dd")
                                : null,

                            challanDate = rdr["ChallanDate"] != DBNull.Value
                                ? Convert.ToDateTime(rdr["ChallanDate"]).ToString("yyyy-MM-dd")
                                : null,

                            challanNo = rdr["ChallanNo"] != DBNull.Value ? rdr["ChallanNo"].ToString() : null,
                            vehicleNo = rdr["VehicleNo"] != DBNull.Value ? rdr["VehicleNo"].ToString() : null,
                            driverName = rdr["DriverName"] != DBNull.Value ? rdr["DriverName"].ToString() : null,

                            amount = rdr["Amount"] != DBNull.Value ? Convert.ToDecimal(rdr["Amount"]) : (decimal?)null,

                            remark = rdr["Remark"] != DBNull.Value ? rdr["Remark"].ToString() : null,
                            grievancesTicket = rdr["GrievancesTicket"] != DBNull.Value ? rdr["GrievancesTicket"].ToString() : null,
                            challanStatus = rdr["ChallanStatus"] != DBNull.Value ? rdr["ChallanStatus"].ToString() : null,

                            paidDate = rdr["PaidDate"] != DBNull.Value
                                ? Convert.ToDateTime(rdr["PaidDate"]).ToString("yyyy-MM-dd")
                                : null,

                            actionName = rdr["ActionName"] != DBNull.Value ? rdr["ActionName"].ToString() : null
                        });
                    }
                }
            }

            return Json(null);
        }
        [HttpPost]
        public IActionResult DeleteChallan(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DeleteChallan", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ChallanId", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            return Json(new { success = true, message = "Record Deleted Successfully" });
        }
        private string? FormatChallanDate(object? dbValue)
        {
            if (dbValue == null || dbValue == DBNull.Value)
                return null;   

            DateTime parsedDate;
            string[] formats =
            {
              "dd-MM-yyyy",
              "yyyy-MM-dd",
              "yyyy-dd-MM",
              "MM-dd-yyyy",
              "dd/MM/yyyy",
              "yyyy/MM/dd"
             };

            // If already DateTime (normal SQL case)
            if (dbValue is DateTime dt)
            {
                return dt.ToString("dd-MM-yyyy");
            }
            // If string format
            if (DateTime.TryParseExact(
                    dbValue.ToString(),
                    formats,
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None,
                    out parsedDate))
            {
                return parsedDate.ToString("dd-MM-yyyy");
            }
            return null;
        }
    }
}

