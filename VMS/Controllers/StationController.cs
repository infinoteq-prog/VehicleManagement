using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Globalization;
using VMS.Models;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace VMS.Controllers
{
    public class StationController : Controller
    {

        private readonly string _connectionString;

        public StationController(IConfiguration configuration)
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
        public ActionResult Create(DriverChangeModel model)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                using (SqlCommand cmd = new SqlCommand("SP_InsertStation", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;


                    cmd.Parameters.AddWithValue("@ID", model.ID);
                    cmd.Parameters.AddWithValue("@CityName", model.CityName);
                    cmd.Parameters.AddWithValue("@DistrictID", model.DistrictID);
                    cmd.Parameters.AddWithValue("@StateID", model.StateID);
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
        public JsonResult GetState()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_State", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                stateID = dr["StateID"],
                                stateName = dr["StateName"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(list);
        }
        [HttpGet]
        public JsonResult GetDistrict()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_District", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                districtID = dr["DistrictID"],
                                districtName = dr["DistrictName"].ToString()
                            })
                        
                    }
                }
            }
            return Json(list);
        }

        [HttpGet]
        public JsonResult GetAllStations()
        {
            List<DriverChangeModel> list = new List<DriverChangeModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_Get_Stations", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new DriverChangeModel
                    {
                        ID = rdr["ID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["ID"]),
                        CityName = rdr["CityName"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(rdr["CityName"]),

                        // IMPORTANT: include DriverID so client edit can select by id
                        DistrictID = rdr["DistrictID"] == DBNull.Value ? 0 : Convert.ToInt32(rdr["DistrictID"]),

                        // Return name and vehicle text
                        DistrictName = rdr["DistrictName"]?.ToString() ?? string.Empty,
                        StateName = rdr["StateName"]?.ToString() ?? string.Empty,
                        
                        IsActive = rdr["IsActive"] == DBNull.Value ? false : Convert.ToBoolean(rdr["IsActive"])
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
                cmd.Parameters.AddWithValue("@UpdatedBy", User.Identity?.Name ?? "Admin");
                cmd.Parameters.AddWithValue("@Salary", model.Salary);
                cmd.Parameters.AddWithValue("@SalaryType", model.SalaryType ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);

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
