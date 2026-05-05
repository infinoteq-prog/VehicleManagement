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
    public class DriverProfileController : Controller
    {
        private readonly ILogger<DriverProfileController> _logger;
        private readonly VmsDbContext _context;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DriverProfileController(VmsDbContext context,IConfiguration configuration,ILogger<DriverProfileController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("VMSContext");
            _httpContextAccessor = httpContextAccessor;
        }
        public ActionResult DriverProfile()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveDriverProfile(VMDriverProfile model, string ActionType)
        {
            string userId = HttpContext.Session.GetString("UserId");
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DriverProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ActionType", ActionType);

                    if (ActionType == "Edit")
                        cmd.Parameters.AddWithValue("@ProfileID", model.ProfileID);

                    cmd.Parameters.AddWithValue("@DriverID", model.DriverID);
                    cmd.Parameters.AddWithValue("@VehicleID", model.VehicleID);
                    cmd.Parameters.AddWithValue("@VehicleNo", model.VehicleNo ?? "");
                    cmd.Parameters.AddWithValue("@Offence", model.OffenceId);
                    cmd.Parameters.Add("@OffencePoint", SqlDbType.Decimal).Value =
                        model.OffencePoint == null ? (object)DBNull.Value : model.OffencePoint;

                    cmd.Parameters.Add("@HoldAmt", SqlDbType.Decimal).Value =
                        model.HoldAmt == null ? (object)DBNull.Value : model.HoldAmt;
                    cmd.Parameters.AddWithValue("@CreatedBy", (object)userId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedBy", (object)userId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? "");
                    cmd.Parameters.Add("@ProfileDate", SqlDbType.DateTime).Value = model.ProfileDate == null ? (object)DBNull.Value : model.ProfileDate;

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult GetDriverProfileList(int? DriverID, int? VehicleID, DateTime? FromDate, DateTime? ToDate)
        {
            List<VMDriverProfile> list = new List<VMDriverProfile>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DriverProfile", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ActionType", "List");

                cmd.Parameters.Add("@DriverID", SqlDbType.Int).Value =
                    DriverID.HasValue ? DriverID.Value : DBNull.Value;

                cmd.Parameters.Add("@VehicleID", SqlDbType.Int).Value =
                    VehicleID.HasValue ? VehicleID.Value : DBNull.Value;

                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value =
                    FromDate.HasValue ? FromDate.Value : DBNull.Value;

                cmd.Parameters.Add("@ToDate", SqlDbType.DateTime).Value =
                    ToDate.HasValue ? ToDate.Value : DBNull.Value;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new VMDriverProfile
                    {
                        ProfileID = dr["ProfileID"] != DBNull.Value ? Convert.ToInt32(dr["ProfileID"]) : 0,

                        VehicleNo = dr["VehicleNo"] != DBNull.Value ? dr["VehicleNo"].ToString() : null,

                        DriverName = dr["DriverName"] != DBNull.Value ? dr["DriverName"].ToString() : null,

                        Offence = dr["Offence"] != DBNull.Value ? dr["Offence"].ToString() : null,

                        OffencePoint = dr["OffencePoint"] != DBNull.Value
                       ? Convert.ToDecimal(dr["OffencePoint"])
                       : (decimal?)null,

                        HoldAmt = dr["HoldAmt"] != DBNull.Value
                   ? Convert.ToDecimal(dr["HoldAmt"])
                   : (decimal?)null,

                        CreationDate = dr["CreationDate"] != DBNull.Value
                       ? Convert.ToDateTime(dr["CreationDate"])
                       : (DateTime?)null,
                        Remarks = dr["Remarks"] != DBNull.Value ? dr["Remarks"].ToString() : null,
                        ProfileDate = dr["ProfileDate"] == DBNull.Value
                     ? (DateTime?)null
                             : Convert.ToDateTime(dr["ProfileDate"])
                    });

                }
            }

            return Json(list);
        }

        [HttpGet]
        public IActionResult GetDriverProfileById(int id)
        {
            var data = new VMDriverProfile();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DriverProfile", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ActionType", "Get");
                    cmd.Parameters.AddWithValue("@ProfileID", id);

                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            data.ProfileID = dr["ProfileID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ProfileID"]);
                            data.DriverID = dr["DriverID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DriverID"]);
                            data.VehicleID = dr["VehicleID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["VehicleID"]);
                            data.VehicleNo = dr["VehicleNo"]?.ToString();

                            data.OffenceId = dr["OffenceId"] == DBNull.Value || string.IsNullOrEmpty(dr["OffenceId"].ToString())
                                                ? 0
                                                : Convert.ToInt32(dr["OffenceId"]);

                            data.OffencePoint = dr["OffencePoint"] == DBNull.Value || string.IsNullOrEmpty(dr["OffencePoint"].ToString())
                                                    ? (decimal?)null
                                                    : Convert.ToDecimal(dr["OffencePoint"]);

                            data.HoldAmt = dr["HoldAmt"] == DBNull.Value || string.IsNullOrEmpty(dr["HoldAmt"].ToString())
                                                ? (decimal?)null
                                                : Convert.ToDecimal(dr["HoldAmt"]);

                            data.Remarks = dr["Remarks"]?.ToString();
                            data.ProfileDate = dr["ProfileDate"] == DBNull.Value
                           ? (DateTime?)null
                           : Convert.ToDateTime(dr["ProfileDate"]);
                        }
                    }
                }
            }

            return Json(data);
        }

        [HttpPost]
        public IActionResult DeleteDriverProfile(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DriverProfile", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@ActionType", "Delete");
                        cmd.Parameters.AddWithValue("@ProfileID", id);

                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult GetOffenceDropdown()
        {
            List<OffenceDropdownVM> list = new List<OffenceDropdownVM>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetOffenceDropdown", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodeType", "OFFENCE");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    int offenceId;
                    decimal offencePoint;

                    list.Add(new OffenceDropdownVM
                    {
                        OffenceId = int.TryParse(dr["OffenceId"]?.ToString(), out offenceId) ? offenceId : 0,
                        OffenceName = dr["OffenceName"]?.ToString(),
                        OffencePoint = decimal.TryParse(dr["OffencePoint"]?.ToString(), out offencePoint) ? offencePoint.ToString() : null
                    });
                }
            }

            return Ok(list);
        }
       
    }
}

