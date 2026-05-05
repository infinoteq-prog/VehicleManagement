using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.Globalization;
using VMS.Models;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace VMS.Controllers
{
    public class DriverAttendanceController : Controller
    {

        private readonly string _connectionString;

        public DriverAttendanceController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("VMSContext");
        }
        public IActionResult Create()
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(DriverAttendanceModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_InsertDriverAttendance", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    
                    cmd.Parameters.AddWithValue("@DriverChangeID", model.DriverChangeID);
                    cmd.Parameters.AddWithValue("@ChangeDate", model.ChangeDate);
                    cmd.Parameters.AddWithValue("@DriverID", model.DriverID);
                    cmd.Parameters.AddWithValue("@VehicleNo", string.IsNullOrEmpty(model.VehicleNo) ? (object)DBNull.Value : model.VehicleNo);
                    cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks);
                    cmd.Parameters.AddWithValue("@Status", model.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Created_By", User?.Identity?.Name ?? "Admin");
                    cmd.Parameters.AddWithValue("@Salary", model.Salary);
                    cmd.Parameters.AddWithValue("@SalaryType", model.SalaryType ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@EndDate", model.EndDate);

                    con.Open();
                    cmd.ExecuteNonQuery();
                }

                return Json(new { success = true, message = "Entry added successfully!" });
            }
            catch (Exception ex)
            {
                // log ex if you have logging
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }
        [HttpGet]
        public JsonResult GetVehicles()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_Vehicles", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                vehicleID = dr["VehicleID"],
                                vehicleNo = dr["VehicleNo"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(list);
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
        public JsonResult GetAllDriverAttendanceEntries()
        {
            List<DriverChangeModel> list = new List<DriverChangeModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllDriverAttendanceEntries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new DriverChangeModel
                    {
                        DriverChangeID = rdr["DriverChangeID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["DriverChangeID"]),
                        ChangeDate = rdr["ChangeDate"] == DBNull.Value ? (DateTime?)null: Convert.ToDateTime(rdr["ChangeDate"]),

                        // IMPORTANT: include DriverID so client edit can select by id
                        DriverID = rdr["DriverID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["DriverID"]),

                        // Return name and vehicle text
                        DriverName = rdr["DriverName"]?.ToString() ?? string.Empty,
                        VehicleNo = rdr["VehicleNo"]?.ToString() ?? string.Empty,

                        // Salary included so grid shows it
                        Salary = rdr["Salary"] == DBNull.Value ? 0m : Convert.ToDecimal(rdr["Salary"]),
                        // Added on 15-12-2025 by skg
                       EndDate = rdr["EndDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(rdr["EndDate"]),
                        SalaryType = rdr["SalaryType"]?.ToString() ?? string.Empty,
                        Remarks = rdr["Remarks"]?.ToString() ?? string.Empty,
                        Status = rdr["Status"]?.ToString() ?? string.Empty,
                        
                        IsActive = rdr["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsActive"])
                    });
                }
            }
            return Json(new { data = list });
        }

        [HttpPost]
        public IActionResult UpdateDriverAttendance(DriverChangeModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateDriverAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DriverChangeID", model.DriverChangeID);
               // cmd.Parameters.AddWithValue("@ChangeDate", model.ChangeDate);
                cmd.Parameters.AddWithValue("@DriverID", model.DriverID);
                cmd.Parameters.AddWithValue("@VehicleNo", model.VehicleNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", User.Identity?.Name ?? "Admin");
                cmd.Parameters.AddWithValue("@Salary", model.Salary);
                cmd.Parameters.AddWithValue("@SalaryType", model.SalaryType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@ChangeDate",model.ChangeDate <= SqlDateTime.MinValue.Value ? (object)DBNull.Value : model.ChangeDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate ?? (object)DBNull.Value);



                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true, message = "Driver Change Updated Successfully!" });
        }


        [HttpPost]
        public IActionResult DeleteDriverAttendance(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_DeleteDriverAttendance", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DriverChangeID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }
        // below block added on 26-12-2026 for Route description
        [HttpGet]
        public async Task<JsonResult> getRouteNameMaster()
        {
            try
            {
                var model = await DieselHisabContext.getRouteMaster(_connectionString);

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

    }
}
