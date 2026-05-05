using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;
using DocumentFormat.OpenXml.Spreadsheet;

namespace VMS.Controllers
{
    public class ReportController : Controller
    {
        private readonly string _connectionString;
        public ReportController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("VMSContext");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExportReportToExcel(string reportName, string vehicleNo, string driverName, string routeName, DateTime? fromDate, DateTime? toDate)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Report_Export", con)) // your SP name here
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ReportName", string.IsNullOrEmpty(reportName) ? (object)DBNull.Value : reportName);
                    cmd.Parameters.AddWithValue("@VehicleNo", string.IsNullOrEmpty(vehicleNo) ? (object)DBNull.Value : vehicleNo);
                    cmd.Parameters.AddWithValue("@DriverName", string.IsNullOrEmpty(driverName) ? (object)DBNull.Value : driverName);
                    cmd.Parameters.AddWithValue("@RouteName", string.IsNullOrEmpty(routeName) ? (object)DBNull.Value : routeName);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ToDate", toDate ?? (object)DBNull.Value);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            if (dt.Rows.Count == 0)
                return Content("No data found for selected filters.");
            int bankAccountColumnIndex = 0;

            string html = $@"
    <html>
    <head>
        <meta charset='utf-8'/>
        <style>
            table{{border-collapse:collapse;width:100%;font-family:Calibri;font-size:11pt;}}
            th{{background-color:#4472C4;color:white;padding:5px;text-align:center;}}
            td{{border:1px solid #ccc;padding:5px;text-align:center;}}
            tr:nth-child(even){{background-color:#F2F2F2;}}
            h2{{text-align:center;color:#2F5597;}}
        </style>
    </head>
    <body>
        <h2>{System.Net.WebUtility.HtmlEncode(reportName ?? "Report")}</h2>
        <table><tr>";

        //    foreach (DataColumn col in dt.Columns) 
        //        html += $"<th>{System.Net.WebUtility.HtmlEncode(col.ColumnName)}</th>";
        //    html += "</tr>";

           

                for (int i = 0; i < dt.Columns.Count; i++)
            {
                if (dt.Columns[i].ColumnName.Equals("BankAccountNo", StringComparison.OrdinalIgnoreCase))
                { bankAccountColumnIndex = i;
                  
                }
                html += $"<th>{System.Net.WebUtility.HtmlEncode(dt.Columns[i].ColumnName)}</th>";
            }
            html += "</tr>";
            



            foreach (DataRow row in dt.Rows)
            {
                html += "<tr>";
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    var item = row.ItemArray[i];
                    if (i==bankAccountColumnIndex)
                    {
                       
                        html += $"<td style='mso-number-format:\\@'>{item}</td>";
                    }
                    else
                    {
                        html += $"<td>{System.Net.WebUtility.HtmlEncode(item?.ToString() ?? "")}</td>";
                    }
                }
                html += "</tr>";
            }
          
                /*
            foreach (DataRow row in dt.Rows)
            {
                html += "<tr>";
                foreach (var item in row.ItemArray)

                    html += $"<td>{System.Net.WebUtility.HtmlEncode(item?.ToString() ?? "")}</td>";
                html += "</tr>";
            }
*/
            html += "</table></body></html>";

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(html);
            return File(bytes, "application/vnd.ms-excel", $"Report_{DateTime.Now:yyyyMMddHHmmss}.xls");
        }


        [HttpGet]
        public JsonResult GetReports()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_ReportNames", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                reportCode = dr["ReportName"].ToString(),
                                reportName = dr["ReportName"].ToString()
                            });
                        }
                    }
                }
            }

            // ✅ This is the important part
            return Json(list, new System.Text.Json.JsonSerializerOptions());
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
        public JsonResult GetRoutes()
        {
            var list = new List<object>();
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_Get_Routes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (var dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new
                            {
                                routeName = dr["RouteName"].ToString()
                            });
                        }
                    }
                }
            }
            return Json(list);
        }


    }
}
