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
    public class DispatchEntryController : Controller
    {
        private readonly ILogger<DispatchEntryController> _logger;
        private readonly VmsDbContext _context;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DispatchEntryController(
            VmsDbContext context,
            IConfiguration configuration,
            ILogger<DispatchEntryController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("VMSContext");
            _httpContextAccessor = httpContextAccessor;
        }

        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]
        public JsonResult GetDrivers()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_Drivers", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                driverID = dr["DriverID"],
                                driverName = dr["DriverName"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(list);
        }
        [HttpGet]
        public JsonResult GetCustomerNameList()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_Vendors", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                vendorID = dr["VendorID"],
                                vendorName = dr["VendorName"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(list);
        }

        [HttpGet]
        public JsonResult GetVendorType(int vendorId)
        {
            string vendorType = "";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("USP_GetVendorTypeByVendorID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VendorID", vendorId);

                    con.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            vendorType = dr["VendorType"]?.ToString();
                        }
                    }
                }
            }

            return Json(new { vendorType = vendorType });
        }

        [HttpGet]
        public JsonResult GetDispatchByDateRange(string vehicleno = null, string material = null, int? customerId = null, DateTime? startDate = null, DateTime? endDate = null, string station = null, string state = null, string district = null)
        {
            List<VMDispatchEntryList> model = new List<VMDispatchEntryList>();

            try
            {
                using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_GetDispatchByDateRange", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    string startDateFormatted = startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : null;

                    string endDateFormatted = endDate.HasValue ? endDate.Value.ToString("yyyy-MM-dd") : null;
                    cmd.Parameters.AddWithValue("@VehicleNo", string.IsNullOrWhiteSpace(vehicleno) ? (object)DBNull.Value : vehicleno.Trim());
                    cmd.Parameters.AddWithValue("@Material", string.IsNullOrWhiteSpace(material) ? (object)DBNull.Value : material.Trim());
                    cmd.Parameters.AddWithValue("@CustomerId", (object)customerId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Station", string.IsNullOrWhiteSpace(station) ? (object)DBNull.Value : station.Trim());
                    cmd.Parameters.AddWithValue("@State", string.IsNullOrWhiteSpace(state) ? (object)DBNull.Value : state.Trim());
                    cmd.Parameters.AddWithValue("@District", string.IsNullOrWhiteSpace(district) ? (object)DBNull.Value : district.Trim());
                    cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = (object)startDateFormatted ?? DBNull.Value;
                    cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = (object)endDateFormatted ?? DBNull.Value;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        model.Add(new VMDispatchEntryList
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            LRNo = dr["LRNo"].ToString(),
                            // LoadingDate = ParseDate(dr["LoadingDate"]),
                            LoadingDate = dr["LoadingDate"] == DBNull.Value ? null : Convert.ToDateTime(dr["LoadingDate"]).ToString("dd-MM-yyyy"),
                            VehicleNo = dr["VehicleNo"].ToString(),
                            Material = dr["Material"].ToString(),
                            FromStation = dr["FromStation"].ToString(),
                            ToStation = dr["ToStation"].ToString(),
                            UnloadWeight = dr["UnloadWeight"] == DBNull.Value ? null : (decimal?)dr["UnloadWeight"],
                            FreightRate = dr["FreightRate"] == DBNull.Value ? null : (decimal?)dr["FreightRate"],
                            TotalFreight = dr["FreightAmt"] == DBNull.Value ? null : (decimal?)dr["FreightAmt"],
                            FreightForTally = dr["TallyFreight"] == DBNull.Value ? null : (decimal?)dr["TallyFreight"],
                            LoadWeight = dr["LoadWeight"] == DBNull.Value ? null : (decimal?)dr["LoadWeight"]
                        });
                    }
                }

                return Json(model);
            }
            catch
            {
                return Json(null);
            }
        }

        [HttpGet]
        public JsonResult GetDispatchById(int id)
        {
            VMDispatchEntry model = new VMDispatchEntry();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetDispatchById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        model.VehicleNo = dr["VehicleNo"] == DBNull.Value ? string.Empty : dr["VehicleNo"].ToString();
                        model.OwnOtherType = dr["OwnOtherType"] == DBNull.Value ? string.Empty : dr["OwnOtherType"].ToString();
                        model.BillType = dr["BillType"] == DBNull.Value ? string.Empty : dr["BillType"].ToString();
                        if (dr["LoadingDate"] != DBNull.Value)
                        {
                            string dateStr = dr["LoadingDate"].ToString();
                            string[] formats =
                            {
                                "dd/MM/yyyy","dd-MM-yyyy","yyyy-MM-dd",
                                "yyyy/MM/dd","MM/dd/yyyy","MM-dd-yyyy"
                            };
                            if (DateTime.TryParseExact(dateStr, formats,
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out DateTime parsedDate))
                            {
                                model.LoadingDate = parsedDate;
                            }
                            else
                            {
                                model.LoadingDate = DateTime.MinValue;
                            }
                        }
                        else
                        {
                            model.LoadingDate = DateTime.MinValue;
                        }
                        model.LRNo = dr["LRNo"] == DBNull.Value ? string.Empty : dr["LRNo"].ToString();
                        model.CustomerId = dr["CustomerId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["CustomerId"]);
                        model.BillNo = dr["BillNo"] == DBNull.Value ? string.Empty : dr["BillNo"].ToString();
                        model.DriverId = dr["DriverId"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DriverId"]);
                        model.FromStation = dr["FromStation"] == DBNull.Value ? string.Empty : dr["FromStation"].ToString();
                        model.ToStation = dr["ToStation"] == DBNull.Value ? string.Empty : dr["ToStation"].ToString();
                        model.Material = dr["Material"] == DBNull.Value ? string.Empty : dr["Material"].ToString();
                        model.LoadWeight = dr["LoadWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["LoadWeight"]);
                        model.UnloadWeight = dr["UnloadWeight"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["UnloadWeight"]);
                        model.Shortage = dr["Shortage"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Shortage"]);
                        model.FreightRate = dr["FreightRate"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["FreightRate"]);
                        model.TotalFreight = dr["FreightAmt"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["FreightAmt"]);
                        model.Deduction = dr["Deduction"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["Deduction"]);
                        model.FreightForTally = dr["TallyFreight"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["TallyFreight"]);
                        model.Remarks = dr["Remarks"] == DBNull.Value ? string.Empty : dr["Remarks"].ToString();
                        model.InvoiceNo = dr["InvoiceNo"] == DBNull.Value ? string.Empty : dr["InvoiceNo"].ToString();
                        model.ShipmentNo = dr["ShipmentNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["ShipmentNo"]);
                        model.DeliveryNo = dr["DeliveryNo"] == DBNull.Value ? 0 : Convert.ToInt32(dr["DeliveryNo"]);                        
                        model.TradeNT = dr["TradeNT"] == DBNull.Value ? string.Empty : dr["TradeNT"].ToString();
                        model.Other1 = dr["Other1"] == DBNull.Value ? string.Empty : dr["Other1"].ToString();
                        model.Other2 = dr["Other2"] == DBNull.Value ? string.Empty : dr["Other2"].ToString();
                    }
                }
            }
            return Json(model);
        }

        [HttpGet]
        public IActionResult GetStationDropdown()
        {
            List<CityDropdownVM> list = new List<CityDropdownVM>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetStateDistrictCityList", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new CityDropdownVM
                    {
                        CityId = Convert.ToInt32(dr["CityId"]),
                        CityName = dr["CityName"].ToString()
                    });
                }
            }

            return Ok(list);
        }
        public IActionResult GetBillTypeDropdown()
        {
            List<BillTypeDropdownVM> list = new List<BillTypeDropdownVM>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetBillTypeDropdown", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodeType", "BILLTYPE");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new BillTypeDropdownVM
                    {
                        BillTypeId = Convert.ToInt32(dr["BillTypeId"]),
                        BillTypeName = dr["BillTypeName"].ToString()
                    });
                }
            }

            return Ok(list);

        }
        public IActionResult GetMaterialTypeDropdown()
        {
            List<MaterailTypeDropdownVM> list = new List<MaterailTypeDropdownVM>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_GetMaterialTypeDropdown", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CodeType", "MATERIAL");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(new MaterailTypeDropdownVM
                    {
                        MateriaId = Convert.ToInt32(dr["MateriaId"]),
                        MaterialName = dr["MaterialName"].ToString()
                    });
                }
            }

            return Ok(list);

        }
        private void SplitStationInfo(string stationText, out string city, out string district, out string state)
        {
            city = "";
            district = "";
            state = "";

            if (!string.IsNullOrWhiteSpace(stationText) &&
                stationText.Contains("[") &&
                stationText.Contains("]"))
            {
                int bracketIndex = stationText.IndexOf('[');
                city = stationText.Substring(0, bracketIndex).Trim();
                string bracketValue = stationText.Substring(bracketIndex + 1,
                    stationText.IndexOf(']') - bracketIndex - 1);
                string[] parts = bracketValue.Split('-');

                if (parts.Length == 2)
                {
                    district = parts[0].Trim();
                    state = parts[1].Trim();
                }
            }
        }

        [HttpPost]
        public IActionResult SaveOrUpdateDispatch(VMDispatchEntry model)
        {
            model.VehicleNo = model.VehicleNo?.Trim();
            model.OwnOtherType = model.OwnOtherType?.Trim();
            model.BillType = model.BillType?.Trim();
            model.LRNo = model.LRNo?.Trim();
            model.BillNo = model.BillNo?.Trim();
            model.Material = model.Material?.Trim();

            string fromCity = null, fromDistrict = null, fromState = null;
            string toCity = null, toDistrict = null, toState = null;

            if (!model.IsInlineUpdate)
            {
                SplitStationInfo(model.FromStation,
                    out fromCity, out fromDistrict, out fromState);

                SplitStationInfo(model.ToStation,
                    out toCity, out toDistrict, out toState);
            }

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("SP_DispatchInsertUpdate", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ID", model.Id);
                cmd.Parameters.AddWithValue("@VehicleNo", (object)model.VehicleNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@OwnOtherType", (object)model.OwnOtherType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BillType", (object)model.BillType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LoadingDate", model.LoadingDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@LRNo", (object)model.LRNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@CustomerId", model.CustomerId == 0 ? DBNull.Value : model.CustomerId);
                cmd.Parameters.AddWithValue("@BillNo", (object)model.BillNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BillDate", model.BillDate ?? (object)DBNull.Value);

                cmd.Parameters.AddWithValue("@DriverId", model.DriverId == 0 ? DBNull.Value : model.DriverId);

                cmd.Parameters.AddWithValue("@FromStation", (object)fromCity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FromDistrict", (object)fromDistrict ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@FromState", (object)fromState ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@ToStation", (object)toCity ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToDistrict", (object)toDistrict ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ToState", (object)toState ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@Material", (object)model.Material ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@LoadWeight", model.LoadWeight == 0 ? DBNull.Value : model.LoadWeight);
                cmd.Parameters.AddWithValue("@UnloadWeight", model.UnloadWeight == 0 ? DBNull.Value : model.UnloadWeight);
                cmd.Parameters.AddWithValue("@Shortage", model.Shortage == 0 ? DBNull.Value : model.Shortage);
                cmd.Parameters.AddWithValue("@FreightRate", model.FreightRate == 0 ? DBNull.Value : model.FreightRate);
                cmd.Parameters.AddWithValue("@FreightAmt", model.TotalFreight == 0 ? DBNull.Value : model.TotalFreight);
                cmd.Parameters.AddWithValue("@Deduction", model.Deduction == 0 ? DBNull.Value : model.Deduction);
                cmd.Parameters.AddWithValue("@TallyFreight", model.FreightForTally == 0 ? DBNull.Value : model.FreightForTally);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks == "" ? DBNull.Value : model.Remarks);

                cmd.Parameters.AddWithValue("@Status", "Active");
                cmd.Parameters.AddWithValue("@UpdatedBy", "Admin");
                cmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@CreatedBy", "Admin");
                cmd.Parameters.AddWithValue("@CreationDate", DateTime.Now);
                cmd.Parameters.AddWithValue("@InvoiceNo", (object)model.InvoiceNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ShipmentNo", (object)model.ShipmentNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DeliveryNo", (object)model.DeliveryNo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TradeNT", (object)model.TradeNT ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Other1", (object)model.Other1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Other2", (object)model.Other2 ?? DBNull.Value);

               
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true, message = "Dispatch saved successfully" });
        }

        [HttpPost]
        public JsonResult DeleteDispatch(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
                using (SqlCommand cmd = new SqlCommand("SP_DeleteDispatch", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ID", id);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true, message = "Dispatch deleted successfully" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public IActionResult ViewBillDetails()
        {
            var data = FetchDispatchReportData();

            return new Rotativa.AspNetCore.ViewAsPdf("ViewBillDetails", data)
            {
                FileName = "DispatchReport.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        [HttpGet]
        public IActionResult GetDispatchReportData()
        {
            try
            {
                var list = FetchDispatchReportData();
                return Json(list);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDispatchReportData: {ex.Message}");
                return StatusCode(500, new { error = "Internal server error", message = ex.Message });
            }
        }
        private List<VMDispatchReport> FetchDispatchReportData()
        {
            List<VMDispatchReport> list = new List<VMDispatchReport>();
            using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetDispatchReports", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 30;
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        var report = new VMDispatchReport
                        {
                            BillNo = rdr["BillNo"]?.ToString() ?? "",
                            //BillDate = rdr["Date"]?.ToString() ?? "",
                            BillDate = rdr["Date"] != DBNull.Value ? Convert.ToDateTime(rdr["Date"]).ToString("dd-MM-yyyy") : "",
                            VehicleNo = rdr["VehicleNo"]?.ToString() ?? "",
                            GRNo = rdr["GRNo"]?.ToString() ?? "",
                            PartyName = rdr["PartyName"]?.ToString() ?? "",
                            Material = rdr["Material"]?.ToString() ?? "",
                            FromLocation = rdr["FromLocation"]?.ToString() ?? "",
                            ToLocation = rdr["ToLocation"]?.ToString() ?? "",
                            LoadWeight = rdr["LoadWeight"] != DBNull.Value ? Convert.ToDecimal(rdr["LoadWeight"]) : 0,
                            Freight = rdr["Freight"] != DBNull.Value ? Convert.ToDecimal(rdr["Freight"]) : 0,
                            TotalFreight = rdr["TotalFreight"] != DBNull.Value ? Convert.ToDecimal(rdr["TotalFreight"]) : 0,
                            Deduction = rdr["Deduction"] != DBNull.Value ? Convert.ToDecimal(rdr["Deduction"]) : 0,
                            Balance = rdr["Balance"] != DBNull.Value ? Convert.ToDecimal(rdr["Balance"]) : 0
                        };
                        list.Add(report);
                    }
                }
            }
            return list;
        }
        //{
        //    List<VMDispatchReport> list = new List<VMDispatchReport>();
        //    using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
        //    {
        //        using (SqlCommand cmd = new SqlCommand("sp_GetDispatchReports", con))
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 30;
        //            con.Open();
        //            SqlDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                var report = new VMDispatchReport
        //                {
        //                    BillNo = rdr["BillNo"]?.ToString() ?? "",
        //                    BillDate = rdr["Date"]?.ToString() ?? "",
        //                    VehicleNo = rdr["VehicleNo"]?.ToString() ?? "",
        //                    GRNo = rdr["GRNo"]?.ToString() ?? "",
        //                    PartyName = rdr["PartyName"]?.ToString() ?? "",
        //                    Material = rdr["Material"]?.ToString() ?? "",
        //                    FromLocation = rdr["FromLocation"]?.ToString() ?? "",
        //                    ToLocation = rdr["ToLocation"]?.ToString() ?? "",
        //                    LoadWeight = rdr["LoadWeight"] != DBNull.Value ? Convert.ToDecimal(rdr["LoadWeight"]) : 0,
        //                    Freight = rdr["Freight"] != DBNull.Value ? Convert.ToDecimal(rdr["Freight"]) : 0,
        //                    TotalFreight = rdr["TotalFreight"] != DBNull.Value ? Convert.ToDecimal(rdr["TotalFreight"]) : 0,
        //                    Deduction = rdr["Deduction"] != DBNull.Value ? Convert.ToDecimal(rdr["Deduction"]) : 0,
        //                    Balance = rdr["Balance"] != DBNull.Value ? Convert.ToDecimal(rdr["Balance"]) : 0
        //                };
        //                list.Add(report);
        //            }
        //        }
        //    }
        //    return list;
        //}
        [HttpGet]
        public IActionResult GetNextBillNo()
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(
                @"SELECT ISNULL(MAX(CAST(BillNo AS BIGINT)), 0) + 1 
          FROM Dispatch 
          WHERE ISNUMERIC(BillNo) = 1", con))
            {
                con.Open();
                var nextBillNo = cmd.ExecuteScalar()?.ToString();
                return Json(new { success = true, billNo = nextBillNo });
            }
        }

        //Import Excel Data
        public IActionResult UploadExcel()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadExcel(IFormFile excelFile)
        {
            var userLogin = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");

            string userId = userLogin.Id.ToString();
            string userName = userLogin.UserName;
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["Error"] = "Please select a valid Excel file.";
                return RedirectToAction("UploadExcel");
            }

            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            try
            {
                using (var stream = excelFile.OpenReadStream())
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable table = result.Tables[0];

                    int validRowCount = 0;

                    foreach (DataRow row in table.Rows)
                    {
                        int rowNumber = table.Rows.IndexOf(row) + 2;

                        if (IsRowCompletelyEmpty(row))
                        {
                            continue;
                        }

                        validRowCount++;
                        
                        string vehicleNo = GetValue(row, "VehicleNo");
                        string loadingDateRaw = GetValue(row, "LoadingDate");
                        string totalFreightRaw = GetValue(row, "TotalFreight");

                        DateTime? loadingDate = TryParseDate(loadingDateRaw);
                        decimal? totalFreight = TryParseDecimal(totalFreightRaw);

                        if (string.IsNullOrWhiteSpace(vehicleNo))
                        {
                            TempData["Error"] = $"Vehicle Number is mandatory at row {rowNumber}.";
                            return RedirectToAction("UploadExcel");
                        }

                        if (!loadingDate.HasValue)
                        {
                            TempData["Error"] = $"Loading Date is mandatory at row {rowNumber}.";
                            return RedirectToAction("UploadExcel");
                        }

                        if (!totalFreight.HasValue)
                        {
                            TempData["Error"] = $"Total Freight is mandatory at row {rowNumber}.";
                            return RedirectToAction("UploadExcel");
                        }

                        UpdateFreightData(
                            GetValue(row, "LR_No"),
                            vehicleNo,
                            TryParseDecimal(GetValue(row, "FreightRate")),
                            totalFreight,
                            GetValue(row, "OwnOther"),
                            loadingDate,
                            GetValue(row, "CustomerName"),
                            GetValue(row, "BillNo"),
                            GetValue(row, "BillType"),
                            GetValue(row, "DriverName"),
                            GetValue(row, "FromStation"),
                            GetValue(row, "ToStation"),
                            GetValue(row, "Material"),
                            TryParseDecimal(GetValue(row, "LoadWeight")),
                            TryParseDecimal(GetValue(row, "UnLoadWeight")),
                            TryParseDecimal(GetValue(row, "Shortage")),
                            TryParseDecimal(GetValue(row, "Deduction")),
                            TryParseDecimal(GetValue(row, "FreightTally")),
                            GetValue(row, "Remarks"),
                            userName

                        );
                    }
                    if (validRowCount == 0)
                    {
                        TempData["Error"] = "Excel is empty. Please fill required fields.";
                        return RedirectToAction("UploadExcel");
                    }
                }

                TempData["Success"] = "Excel uploaded successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("UploadExcel");
        }

        private void UpdateFreightData(string lrNo, string vehicleNo, decimal? freightRate, decimal? totalFreight, string ownOther, DateTime? loadingDate, string customerName,
                                        string billNo, string billType, string driverName, string fromStation, string toStation, string material, decimal? loadWeight,
                                         decimal? unLoadWeight, decimal? shortage, decimal? deduction, decimal? freightTally, string remarks,string userName)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("USP_UpdateFreightFromExcel", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LRNo", SqlDbType.VarChar, 50).Value = (object)lrNo ?? DBNull.Value;
                cmd.Parameters.Add("@VehicleNo", SqlDbType.VarChar, 50).Value = (object)vehicleNo ?? DBNull.Value;
                cmd.Parameters.Add("@OwnOtherType", SqlDbType.VarChar, 50).Value = (object)ownOther ?? DBNull.Value;
                cmd.Parameters.Add("@CustomerName", SqlDbType.VarChar, 100).Value = (object)customerName ?? DBNull.Value;
                cmd.Parameters.Add("@BillNo", SqlDbType.VarChar, 50).Value = (object)billNo ?? DBNull.Value;
                cmd.Parameters.Add("@BillType", SqlDbType.VarChar, 50).Value = (object)billType ?? DBNull.Value;
                cmd.Parameters.Add("@DriverName", SqlDbType.VarChar, 100).Value = (object)driverName ?? DBNull.Value;
                cmd.Parameters.Add("@FromStation", SqlDbType.VarChar, 100).Value = (object)fromStation ?? DBNull.Value;
                cmd.Parameters.Add("@ToStation", SqlDbType.VarChar, 100).Value = (object)toStation ?? DBNull.Value;
                cmd.Parameters.Add("@Material", SqlDbType.VarChar, 100).Value = (object)material ?? DBNull.Value;
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, 500).Value = (object)remarks ?? DBNull.Value;
                cmd.Parameters.Add("@FreightRate", SqlDbType.Decimal).Value = (object)freightRate ?? DBNull.Value;
                cmd.Parameters.Add("@FreightAmt", SqlDbType.Decimal).Value = (object)totalFreight ?? DBNull.Value;
                cmd.Parameters.Add("@LoadWeight", SqlDbType.Decimal).Value = (object)loadWeight ?? DBNull.Value;
                cmd.Parameters.Add("@UnloadWeight", SqlDbType.Decimal).Value = (object)unLoadWeight ?? DBNull.Value;
                cmd.Parameters.Add("@Shortage", SqlDbType.Decimal).Value = (object)shortage ?? DBNull.Value;
                cmd.Parameters.Add("@Deduction", SqlDbType.Decimal).Value = (object)deduction ?? DBNull.Value;
                cmd.Parameters.Add("@TallyFreight", SqlDbType.Decimal).Value = (object)freightTally ?? DBNull.Value;
                cmd.Parameters.Add("@LoadingDate", SqlDbType.Date).Value = (object)loadingDate ?? DBNull.Value;
                // Add UserID parameter
                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 50).Value = (object)userName ?? DBNull.Value;

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private string GetValue(DataRow row, string columnName)
        {
            var col = row.Table.Columns.Cast<DataColumn>()
                       .FirstOrDefault(c => string.Equals(c.ColumnName.Trim(), columnName, StringComparison.OrdinalIgnoreCase));

            return col != null ? row[col]?.ToString().Trim() : null;
        }
        private decimal? TryParseDecimal(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return null;

            if (decimal.TryParse(value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                return result;

            return null;
        }
        private DateTime? TryParseDate(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (double.TryParse(value, out double oaDate))
            {
                try
                {
                    return DateTime.FromOADate(oaDate);
                }
                catch
                {
                    return null;
                }
            }

            string[] formats =
                    {
                "dd/MM/yyyy",
                "d/M/yyyy",
                "dd-MM-yyyy",
                "d-M-yyyy",
                "MM/dd/yyyy",
                "M/d/yyyy",
                "yyyy-MM-dd",
                "dd/MM/yyyy HH:mm:ss",
                "dd-MM-yyyy HH:mm:ss"
            };

            if (DateTime.TryParseExact(
                value.Trim(),
                formats,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out DateTime parsedDate))
            {
                return parsedDate;
            }

            return null;
        }
        private bool IsRowCompletelyEmpty(DataRow row)
        {
            foreach (var item in row.ItemArray)
            {
                if (item != null && item != DBNull.Value && !string.IsNullOrWhiteSpace(item.ToString()))
                {
                    return false;
                }
            }
            return true;
        }
    }
}

