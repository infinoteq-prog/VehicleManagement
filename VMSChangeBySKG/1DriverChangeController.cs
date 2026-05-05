using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Data;
using VMS.Models;

namespace VMS.Controllers
{
    public class DriverChangeController : Controller
    {

        private readonly string _connectionString;

        public DriverChangeController(IConfiguration configuration)
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

        // [HttpPost]
        //// public IActionResult Create(DriverChangeModel model)
        // public ActionResult Create(int vehicleNo, int driverName, string changeDate, string remark)
        // {
        //     using (SqlConnection con = new SqlConnection(_connectionString))
        //     {
        //         //SqlCommand cmd = new SqlCommand("SP_InsertDriverChange", con);
        //         //cmd.CommandType = CommandType.StoredProcedure;
        //         //cmd.Parameters.AddWithValue("@DriverChangeID", model.DriverChangeID);
        //         //cmd.Parameters.AddWithValue("@ChangeDate", model.ChangeDate);
        //         //cmd.Parameters.AddWithValue("@DriverID", model.DriverID);
        //         //cmd.Parameters.AddWithValue("@VehicleNo", model.VehicleNo ?? (object)DBNull.Value);
        //         //cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);
        //         //cmd.Parameters.AddWithValue("@Status", model.Status ?? (object)DBNull.Value);
        //         //cmd.Parameters.AddWithValue("@Created_By", User.Identity?.Name ?? "Admin");

        //         //con.Open();
        //         //cmd.ExecuteNonQuery();
        //         //con.Close();
        //     }
        //     return Json(new { success = true, message = "Entry added successfully!" });
        // }

        [HttpPost]
        public ActionResult Create(DriverChangeModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_InsertDriverChange", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                  
                    cmd.Parameters.AddWithValue("@DriverChangeID", model.DriverChangeID);
                    cmd.Parameters.AddWithValue("@ChangeDate", model.ChangeDate);
                    cmd.Parameters.AddWithValue("@DriverID", model.DriverID);
                    cmd.Parameters.AddWithValue("@VehicleNo", string.IsNullOrEmpty(model.VehicleNo) ? (object)DBNull.Value : model.VehicleNo);
                    cmd.Parameters.AddWithValue("@Remarks", string.IsNullOrEmpty(model.Remarks) ? (object)DBNull.Value : model.Remarks);
                    cmd.Parameters.AddWithValue("@Status", model.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Created_By", User?.Identity?.Name ?? "Admin");

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
        public JsonResult GetAllDriverChangeEntries()
        {
            List<DriverChangeModel> list = new List<DriverChangeModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllDriverChangeEntries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new DriverChangeModel
                    {
                        DriverChangeID = Convert.ToInt32(rdr["DriverChangeID"]),
                        ChangeDate = Convert.ToDateTime(rdr["ChangeDate"]),
                        DriverID = Convert.ToInt32(rdr["DriverID"]),
                        VehicleNo = rdr["VehicleNo"].ToString(),
                        Remarks = rdr["Remarks"].ToString(),
                        Status = rdr["Status"].ToString(),
                        Created_By = rdr["Created_By"].ToString(),
                        CreatedDate = Convert.ToDateTime(rdr["CreatedDate"]),
                        IsActive = Convert.ToBoolean(rdr["IsActive"]),
                        UpdatedBy = rdr["UpdatedBy"]?.ToString(),
                        UpdatedDate = rdr["UpdatedDate"] == DBNull.Value ? null : Convert.ToDateTime(rdr["UpdatedDate"])
                    });
                }
            }
            return Json(new { data = list });
        }

        [HttpPost]
        public IActionResult UpdateDriverChange(DriverChangeModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateDriverChange", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DriverChangeID", model.DriverChangeID);
                cmd.Parameters.AddWithValue("@ChangeDate", model.ChangeDate);
                cmd.Parameters.AddWithValue("@DriverID", model.DriverID);
                cmd.Parameters.AddWithValue("@VehicleNo", model.VehicleNo ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Status", model.Status ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Created_By", User.Identity?.Name ?? "Admin");

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true, message = "Driver Change Updated Successfully!" });
        }


        [HttpPost]
        public IActionResult DeleteDriverChange(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_DeleteDriverChange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DriverChangeID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }


    }
}
