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
    public class CityMasterController : Controller
    {
        private readonly ILogger<CityMasterController> _logger;
        private readonly VmsDbContext _context;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CityMasterController(VmsDbContext context,IConfiguration configuration,ILogger<CityMasterController> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("VMSContext");
            _httpContextAccessor = httpContextAccessor;
        }
        public ActionResult CityMaster()
        {
            return View();
        }
     
        [HttpGet]
        public IActionResult GetAllDistrict()
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllDistrict", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        list.Add(new
                        {
                            id = Convert.ToInt32(rdr["Id"]),
                            name = rdr["Name"].ToString()
                        });
                    }
                }
            }

            return Json(list);
        }
        [HttpGet]
        public IActionResult GetStateByDistrict(int districtId)
        {
            List<object> list = new List<object>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetStateByDistrictID", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DistrictId", districtId);

                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        list.Add(new
                        {
                            id = Convert.ToInt32(rdr["Id"]),
                            name = rdr["Name"].ToString()
                        });
                    }
                }
            }

            return Json(list);
        }

        [HttpGet]
        public IActionResult GetAllCities()
        {
            List<dynamic> cityList = new List<dynamic>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAllCities", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            cityList.Add(new
                            {
                                id = rdr["ID"],
                                cityName = rdr["City_Name"].ToString(),
                                districtName = rdr["District_Name"].ToString(),
                                stateName = rdr["State_Name"].ToString()
                            });
                        }
                    }
                }
            }

            return Json(cityList);
        }

        [HttpPost]
        public IActionResult DeleteCity(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_DeleteCity", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);

                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                bool status = Convert.ToBoolean(rdr["Status"]);
                                string message = rdr["Message"].ToString();
                                return Json(new { status = status ? 1 : 0, message });
                            }
                        }
                    }
                }
                return Json(new { status = 0, message = "No response from server!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Error: " + ex.Message });
            }
        }
        [HttpPost]
        public IActionResult SaveUpdateCity(int id, string cityName, int districtId, int stateId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_SaveCityMaster", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@CityName", cityName.Trim());
                        cmd.Parameters.AddWithValue("@DistrictId", districtId);
                        cmd.Parameters.AddWithValue("@StateId", stateId);

                        con.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            if (rdr.Read())
                            {
                                bool status = Convert.ToBoolean(rdr["Status"]);
                                string message = rdr["Message"].ToString();
                                return Json(new { status = status ? 1 : 0, message });
                            }
                        }
                    }
                }
                return Json(new { status = 0, message = "No response from server!" });
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, message = "Error: " + ex.Message });
            }
        }
    }
}

