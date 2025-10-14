using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Data;
using VMS.Models;

namespace VMS.Controllers
{
    public class DieselEntryController : Controller
    {

        private readonly string _connectionString;

        public DieselEntryController(IConfiguration configuration)
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
        public IActionResult Create(DieselEntryModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertDieselEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@DieselRate", model.DieselRate);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Created_By", User.Identity?.Name ?? "Admin");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            return Json(new { success = true, message = "Entry added successfully!" });
        }

        [HttpGet]
        public JsonResult GetAllDieselEntries()
        {
            List<DieselEntryModel> list = new List<DieselEntryModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllDieselEntries", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    list.Add(new DieselEntryModel
                    {
                        DieselEntryID = Convert.ToInt32(rdr["DieselEntryID"]),
                        StartDate = Convert.ToDateTime(rdr["StartDate"]),
                        EndDate = Convert.ToDateTime(rdr["EndDate"]),
                        DieselRate = Convert.ToDecimal(rdr["DieselRate"]),
                        Remarks = rdr["Remarks"].ToString(),
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
        public IActionResult UpdateDieselEntry(DieselEntryModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_UpdateDieselEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DieselEntryID", model.DieselEntryID);
                cmd.Parameters.AddWithValue("@StartDate", model.StartDate);
                cmd.Parameters.AddWithValue("@EndDate", model.EndDate);
                cmd.Parameters.AddWithValue("@DieselRate", model.DieselRate);
                cmd.Parameters.AddWithValue("@Remarks", (object?)model.Remarks ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@UpdatedBy", User.Identity?.Name ?? "Admin");
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true, message = "Diesel Entry updated successfully!" });
        }


        [HttpPost]
        public IActionResult DeleteDieselEntry(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_DeleteDieselEntry", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DieselEntryID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true });
        }

        // GET: AddRoute
        public IActionResult AddRoute(int? id)
        {
            RouteModel model = new RouteModel();

            if (id.HasValue)
            {
                // Load existing route for edit
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    SqlCommand cmd = new SqlCommand("SP_GetRouteByID", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RouteID", id.Value);
                    conn.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        model.RouteID = Convert.ToInt32(rdr["RouteID"]);
                        model.RouteName = rdr["RouteName"].ToString();
                        model.Status = rdr["Status"].ToString();
                    }
                }
            }

            return View(model); // Pass model to view
        }


        // POST: Insert or Update
        [HttpPost]
        public IActionResult SaveRoute(RouteModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_InsertOrUpdateRoute", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RouteID", model.RouteID == 0 ? DBNull.Value : model.RouteID);
                cmd.Parameters.AddWithValue("@RouteName", model.RouteName);
                cmd.Parameters.AddWithValue("@Status", model.Status);
                cmd.Parameters.AddWithValue("@CreatedBy", User.Identity?.Name ?? "Admin");
                con.Open();
                cmd.ExecuteNonQuery();
            }

            return Json(new { success = true, message = model.RouteID == 0 ? "Route added successfully!" : "Route updated successfully!" });
        }

        // GET: All Routes (for grid)
        public IActionResult GetAllRoutes()
        {
            List<RouteModel> routes = new List<RouteModel>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllRoutes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    routes.Add(new RouteModel
                    {
                        RouteID = Convert.ToInt32(rdr["RouteID"]),
                        RouteName = rdr["RouteName"].ToString(),
                        Status = rdr["Status"].ToString(),
                        IsActive = Convert.ToBoolean(rdr["IsActive"]),
                        CreatedBy = rdr["CreatedBy"].ToString(),
                        CreatedDate = rdr["CreatedDate"] as DateTime?,
                        UpdatedBy = rdr["UpdatedBy"].ToString(),
                        UpdatedDate = rdr["UpdatedDate"] as DateTime?
                    });
                }
            }
            return Json(new { data = routes });
        }

        // POST: Delete
        [HttpPost]
        public IActionResult DeleteRoute(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("SP_DeleteRoute", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RouteID", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            return Json(new { success = true, message = "Route deleted successfully!" });
        }

        // GET: RouteList grid
        public IActionResult RouteList()
        {
            return View();
        }


    }
}
