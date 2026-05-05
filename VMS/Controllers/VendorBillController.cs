using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using VMS.ViewModel;
using System.Data;
using VMS.Helper;
using VMS.Models;
using Microsoft.EntityFrameworkCore;




namespace VMS.Controllers
{
    public class VendorBillController : Controller
    {

        //private readonly string _connectionString;

        //public VendorBillController(IConfiguration configuration)
        //{
        //    _connectionString = configuration.GetConnectionString("VMSContext");
        //}

        private readonly ILogger<DispatchEntryController> _logger;
        private readonly VmsDbContext _context;
        private readonly string _connectionString;

        public VendorBillController(
            VmsDbContext context,
            IConfiguration configuration,
            ILogger<DispatchEntryController> logger)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("VMSContext");
        }

        public IActionResult Create()
        {
            ViewBag.VendorTypeList = new List<SelectListItem>
        {
            new SelectListItem { Text = "Own vendors", Value = "Own vendors" },
            new SelectListItem { Text = "Market Vendors", Value = "Market Vendors" }
        };

            return View();
        }

        [HttpPost]
        public IActionResult Create(VendorViewModel model)
        {
            if (ModelState.IsValid)
            {
                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                string CreatedBy = userDetails?.UserName ?? "System";

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("USP_InsertVendorMaster", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VendorCode", model.VendorCode);
                        cmd.Parameters.AddWithValue("@VendorType", model.VendorType);
                        cmd.Parameters.AddWithValue("@VendorName", model.VendorName);
                        cmd.Parameters.AddWithValue("@Address1", model.Address1 ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Address2", string.IsNullOrEmpty(model.Address2) ? (object)DBNull.Value : model.Address2);
                        cmd.Parameters.AddWithValue("@Address3", string.IsNullOrEmpty(model.Address3) ? (object)DBNull.Value : model.Address3);

                        // These will now receive the names directly
                        cmd.Parameters.AddWithValue("@City", model.City ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@District", model.District ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@State", model.State ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@Pin", model.Pin ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GSTIN", model.GSTIN ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PAN", model.PAN ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LRNoPrefix", string.IsNullOrEmpty(model.LRNoPrefix) ? (object)DBNull.Value : model.LRNoPrefix);
                        cmd.Parameters.AddWithValue("@LRNoStart", string.IsNullOrEmpty(model.LRNoStart) ? (object)DBNull.Value : model.LRNoStart);
                        cmd.Parameters.AddWithValue("@ContactPerson", model.ContactPerson ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ContactMobileNo", model.ContactMobileNo ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);

                        // Output parameter for result message
                        SqlParameter resultParam = new SqlParameter("@ResultMessage", SqlDbType.NVarChar, 200)
                        {
                            Direction = ParameterDirection.Output
                        };
                        cmd.Parameters.Add(resultParam);

                        conn.Open();
                        cmd.ExecuteNonQuery();

                        string result = resultParam.Value.ToString();
                        if (result == "Success")
                        {
                            TempData["Success"] = "Vendor created successfully.";
                            return RedirectToAction("Index");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, result);
                        }
                    }
                }
            }

            // Repopulate dropdown in case of failure
            ViewBag.VendorTypeList = new List<SelectListItem>
    {
        new SelectListItem { Text = "Own vendors", Value = "Own vendors" },
        new SelectListItem { Text = "Market Vendors", Value = "Market Vendors" }
    };

            return View(model);
        }
        public IActionResult Index()
        {
            List<VendorViewModel> vendors = new List<VendorViewModel>();
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            string CreatedBy = userDetails?.UserName;

            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SelectVendors", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.AddWithValue("@CreatedBy", (object)CreatedBy ?? DBNull.Value);

                con.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vendors.Add(new VendorViewModel
                        {
                            VendorID = Convert.ToInt32(reader["VendorID"]),
                            VendorCode = reader["VendorCode"]?.ToString(),
                            VendorType = reader["VendorType"]?.ToString(),
                            VendorName = reader["VendorName"]?.ToString(),
                            Address1 = reader["Address1"]?.ToString(),
                            Address2 = reader["Address2"]?.ToString(),
                            Address3 = reader["Address3"]?.ToString(),
                            City = reader["City"]?.ToString(),
                            District = reader["District"]?.ToString(),
                            State = reader["State"]?.ToString(),
                            Pin = reader["Pin"]?.ToString(),
                            GSTIN = reader["GSTIN"]?.ToString(),
                            PAN = reader["PAN"]?.ToString(),
                            LRNoPrefix = reader["LRNoPrefix"]?.ToString(),
                            LRNoStart = reader["LRNoStart"]?.ToString(),
                            ContactPerson = reader["ContactPerson"]?.ToString(),
                            ContactMobileNo = reader["ContactMobileNo"]?.ToString()
                        });
                    }
                }
            }

            return View(vendors);
        }

        // POST: Vendor/Delete
        [HttpPost]
        public IActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand("sp_DeleteVendorById", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            TempData["Success"] = "Vendor deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            VendorViewModel vendor = new VendorViewModel();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetVendorByCode", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VendorCode", id);
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        vendor.VendorID = Convert.ToInt32(rdr["VendorId"]);
                        vendor.VendorCode = rdr["VendorCode"].ToString();
                        vendor.VendorType = rdr["VendorType"].ToString();
                        vendor.VendorName = rdr["VendorName"].ToString();
                        vendor.Address1 = rdr["Address1"]?.ToString() ?? "";
                        vendor.Address2 = rdr["Address2"]?.ToString() ?? "";
                        vendor.Address3 = rdr["Address3"]?.ToString() ?? "";
                        vendor.City = rdr["City"]?.ToString() ?? "";
                        vendor.District = rdr["District"]?.ToString() ?? "";
                        vendor.State = rdr["State"]?.ToString() ?? "";
                        vendor.Pin = rdr["Pin"]?.ToString() ?? "";
                        vendor.GSTIN = rdr["GSTIN"]?.ToString() ?? "";
                        vendor.PAN = rdr["PAN"]?.ToString() ?? "";
                        vendor.LRNoPrefix = rdr["LRNoPrefix"]?.ToString() ?? "";
                        vendor.LRNoStart = rdr["LRNoStart"]?.ToString() ?? "";
                        vendor.ContactPerson = rdr["ContactPerson"]?.ToString() ?? "";
                        vendor.ContactMobileNo = rdr["ContactMobileNo"]?.ToString() ?? "";
                    }
                }
            }

            ViewBag.VendorTypeList = new List<SelectListItem>
             {
                 new SelectListItem { Text = "-- Select Vendor Type --", Value = "" },
                 new SelectListItem { Text = "Own Vendors", Value = "Own Vendors" },
                 new SelectListItem { Text = "Market Vendors", Value = "Market Vendors" }
             };

            return View(vendor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(VendorViewModel model)
        {
            if (ModelState.IsValid)
            {
                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                string CreatedBy = userDetails?.UserName ?? "System";
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UpdateVendor", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@VendorId", model.VendorID);
                        cmd.Parameters.AddWithValue("@VendorCode", model.VendorCode);
                        cmd.Parameters.AddWithValue("@VendorType", model.VendorType);
                        cmd.Parameters.AddWithValue("@VendorName", model.VendorName);
                        cmd.Parameters.AddWithValue("@Address1", model.Address1 ?? "");
                        cmd.Parameters.AddWithValue("@Address2", model.Address2 ?? "");
                        cmd.Parameters.AddWithValue("@Address3", model.Address3 ?? "");
                        cmd.Parameters.AddWithValue("@City", model.City);
                        cmd.Parameters.AddWithValue("@District", model.District);
                        cmd.Parameters.AddWithValue("@State", model.State);
                        cmd.Parameters.AddWithValue("@Pin", model.Pin ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GSTIN", model.GSTIN ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@PAN", model.PAN ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@LRNoPrefix", model.LRNoPrefix ?? "");
                        cmd.Parameters.AddWithValue("@LRNoStart", model.LRNoStart ?? "");
                        cmd.Parameters.AddWithValue("@ContactPerson", model.ContactPerson ?? "");
                        cmd.Parameters.AddWithValue("@ContactMobileNo", model.ContactMobileNo ?? "");
                        cmd.Parameters.AddWithValue("@createdby", CreatedBy);
                        con.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
                TempData["Success"] = "Vendor updated successfully.";
                return RedirectToAction("Index");
            }

            return View(model);
        }
        public JsonResult GetCities()
        {
            List<object> cities = new List<object>();
            using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
            using (SqlCommand cmd = new SqlCommand("SP_GetCities", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    cities.Add(new
                    {
                        CityName = dr["CityName"].ToString()
                    });
                }
            }
            return Json(cities);
        }

        [HttpGet]
        public JsonResult GetDistrictsByCity(string cityName)
        {
            List<object> districts = new List<object>();
            using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
            using (var cmd = new SqlCommand("usp_GetDistrictsByCity", con)) // Changed stored procedure
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CityName", cityName); // Changed parameter
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    districts.Add(new
                    {
                        DistrictName = reader["DistrictName"].ToString()
                    });
                }
                con.Close();
            }
            return Json(districts);
        }

        [HttpGet]
        public JsonResult GetStateByDistrict(string districtName)
        {
            List<object> states = new List<object>();
            using (SqlConnection con = new SqlConnection(_context.Database.GetConnectionString()))
            using (var cmd = new SqlCommand("usp_GetStatesByDistrict", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DistrictName", districtName);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    states.Add(new
                    {
                        StateName = reader["StateName"].ToString()
                    });
                }
                con.Close();
            }
            return Json(states);
        }

    }
}
