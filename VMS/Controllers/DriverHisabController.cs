using VMS.Helper;
using VMS.Models;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using DocumentFormat.OpenXml.Office2010.Excel;
using static iTextSharp.text.pdf.PdfDiv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using DocumentFormat.OpenXml.Bibliography;
using OfficeOpenXml;
using DocumentFormat.OpenXml.Office.Word;

namespace VMS.Controllers
{
    public class DriverHisabController : Controller
    {
        private readonly ILogger<DriverHisabController> _logger;
        private readonly VmsDbContext _context; private readonly string _connectionString;
        private string _controllerName = "DriverHisab";
        public DriverHisabController(VmsDbContext context, IConfiguration configuration)
        {
            _context = context; 
            _connectionString = configuration.GetConnectionString("VMSContext"); 
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(String settlementId)
        {
            ViewBag.settlementId = settlementId;
            return View("Details");
        }

        public ActionResult Update(String settlementId)
        {
            ViewBag.settlementId = settlementId;
            return View("Update");
        }

        public ActionResult Print(String settlementId)
        {
            ViewBag.settlementId = settlementId;
            return View("Print");
        }

        public ActionResult List()
        {
            return View();
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = utilityHelper.getdownloadFilePath(fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        [HttpGet]
        public JsonResult getDriverMaster()
        {
            return Json(_context.TblDriverMasters.Select(x => new
            {
                DriverId = x.Id,
                DriverName = x.DriverName,
            }).ToList());
        }

        [HttpGet]
        public JsonResult getCurrentSettlementNumber(int driverId)
        {
            var model = _context.TblDriverHisabHeaders
                         //.Where(y => y.IsActive==true && y.DriverId.Equals(driverId))
                         .OrderByDescending(x => x.SettlementNo) // Ensure highest TripId is picked
                         .Select(x => new
                         {
                             SettlementNo = x.SettlementNo == 0 ? 1 : x.SettlementNo + 1,
                             vehicleId = x.VehicleNo
                         })
                         .FirstOrDefault();

            if (model != null)
            {
                return Json(model);
            }
            else
            {
                var settlement = new
                {
                    SettlementNo = 1,
                };
                return Json(settlement);
            }
        }

        [HttpGet]
        public JsonResult getLastDriverTripHistory(int driverId)
        {
            var model = (from driver in _context.TblDriverHisabHeaders
                         join vehicle in _context.TblVehicleMasters
                             on driver.VehicleNo equals vehicle.Id
                         where driver.IsActive == true && driver.DriverId == Convert.ToInt32(driverId)
                         orderby driver.SettlementNo descending
                         select new
                         {
                             SettlementNo = driver.SettlementNo,
                             LastTripStartDate = Convert.ToString(Convert.ToDateTime(driver.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             LastTripEndDate = Convert.ToString(Convert.ToDateTime(driver.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             NextTripStartDate = Convert.ToString(Convert.ToDateTime(driver.TripEndDate).AddDays(1).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             LastTripRouteDescr = driver.RouteDescription,
                             OpeningBalance = driver.OpeningBalance == null || driver.OpeningBalance == 0
                                             ? 1
                                             : driver.OpeningBalance,
                             ClosingBalance = driver.ClosingBalance == null || driver.ClosingBalance == 0
                                             ? 1
                                             : driver.ClosingBalance,
                             LastTripVendor = "Test Vendor",
                             LastTripDriver = _context.TblDriverMasters
                                                .Where(p => p.Id == driver.DriverId)
                                                .Select(p => p.DriverName)
                                                .FirstOrDefault(),
                             LastTripDriverFatherName = _context.TblDriverMasters
                                                        .Where(p => p.Id == driver.DriverId)
                                                        .Select(p => p.FatherName)
                                                        .FirstOrDefault(),
                             VehicleNumber = vehicle.VehicleNo // <-- From joined VehicleMaster
                         }).FirstOrDefault();

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> searchDriverHisabMaster(int id, int vehicleNo, int driverId, string tripStartDate,
          string tripEndDate, int openingDiesel)
        {
            try
            {
                if (tripStartDate.ToStringFromNull() == "" && tripEndDate.ToStringFromNull() == "")
                {
                    tripStartDate = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
                    tripEndDate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                var model = await DriverHisabContext.searchDriverHisabMaster(_connectionString, id, vehicleNo, driverId, tripStartDate,
                tripEndDate,  openingDiesel);


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

        [HttpGet]
        public JsonResult getTopDriverHisabList()
        {
            var model = (from x in _context.TblDriverHisabHeaders
                         join vehicle in _context.TblVehicleMasters
                             on x.VehicleNo equals vehicle.Id
                         where x.IsActive == true
                         select new
                         {
                             SettlementNo = x.SettlementNo,
                             VehicleNo = vehicle.VehicleNo,
                             DriverId = x.DriverId,
                             TripStartDate = Convert.ToString(Convert.ToDateTime(x.TripStartDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             TripEndDate = Convert.ToString(Convert.ToDateTime(x.TripEndDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             LastTripRouteDescr = x.RouteDescription,
                             CreationDate = Convert.ToString(Convert.ToDateTime(x.CreationDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),
                             UpdateDate = Convert.ToString(Convert.ToDateTime(x.UpdateDate).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)),                           
                             OpeningDiesel = x.OpeningBalance,
                             ClosingDiesel = x.ClosingBalance,
                             IsActive = x.IsActive,
                             LastTripVendor = "Test Vendor",
                             DriverHeaderCreatedBy = x.CreatedBy,
                             DriverHeaderUpdatedBy = x.UpdatedBy,
                             DriverName = _context.TblDriverMasters
                                 .Where(p => p.Id == x.DriverId)
                                 .Select(p => p.DriverName).FirstOrDefault(),
                             DriverFatherName = _context.TblDriverMasters
                                            .Where(p => p.Id == x.DriverId)
                                            .Select(p => p.FatherName).FirstOrDefault(),
                             DriverHeaderCreatedByName = _context.TblUserMasters
                             .Where(p => p.Id == x.CreatedBy)
                             .Select(p => p.UserName).FirstOrDefault(),
                             DriverHeaderUpdatedByName = _context.TblUserMasters
                             .Where(p => p.Id == x.UpdatedBy)
                             .Select(p => p.UserName).FirstOrDefault()
                         }).OrderByDescending(x => x.SettlementNo).ToList();

            if (model.Count() == 0)
            {
                return Json(null);
            }
            else
            {
                return Json(model);
            }
        }

        [HttpGet]
        public JsonResult getExpenseHeadMaster()
        {
            return Json(_context.TblCodeMasters.Where(x => x.CodeType == "EXPTYPE").Select(x => new
            {
                ExpenseHeadId = x.Id,
                ExpenseHead = x.Code,
                Description = x.Description,
            }).ToList());
        }

        [HttpPost]
        public ActionResult SaveUpdate(int settlementNo, int lastSettlementId,int settlementNumber, int vehicleNo, int driverId, string driverName,
                                 string driverFatherName, string tripStartDate,
                                 string tripEndDate,string settlementDate,decimal weight,string remarks,
                                 int openingBalance, int closingBalance, string tripRouteDescription,  List<TblDriverHisabLine> _lstDriverLine)
        {
            VMTrip model = new VMTrip();
            DateTime dtTripStartDate = DateTime.Now;
            DateTime dtTripEndDate = DateTime.Now;
            DateTime dtSettlementDate = DateTime.Now;
            try
            {
                Globalsettings.Log(_controllerName, string.Format("Before conversion StartDate {0}, EndDate {1}", tripStartDate, tripEndDate));

                dtTripStartDate = Convert.ToDateTime(tripStartDate);
                dtTripEndDate = Convert.ToDateTime(tripEndDate);
                dtSettlementDate = Convert.ToDateTime(settlementDate);
                Globalsettings.Log(_controllerName, string.Format("After conversion StartDate {0}, EndDate {1}", Convert.ToDateTime(dtTripStartDate), Convert.ToDateTime(dtTripEndDate)));

            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("Error occured while converting date {0}", ex.Message));
                model.TransactionMessage.Status = TransactionStatus.Failed;
                model.TransactionMessage.Message = "Driver Hisab Date Conversion Issue!";
                return Json(model);
            }

            int userID = 0;
            VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                userID = userDetails.Id;

                if (settlementNo <= 0)
                {
                    #region Insert Section
                    var driverHeader = _context.TblDriverHisabHeaders.Select(x => new
                    {
                        SettlementNo = x.SettlementNo
                    }).Where(x => x.SettlementNo.Equals(settlementNo)).ToList();

                    if (driverHeader.Count() == 0)
                    {
                        using (var transaction = _context.Database.BeginTransaction())
                        {
                            try
                            {
                                // Insert Vehicle Release Info
                                var driverHisab = new TblDriverHisabHeader
                                {
                                    SettlementNo = settlementNo,
                                    LastSettlementId = lastSettlementId,
                                    DriverId = driverId,
                                    VehicleNo = vehicleNo,
                                    TripStartDate = Convert.ToDateTime(dtTripStartDate.ToString("yyyy-MM-dd")),
                                    TripEndDate = Convert.ToDateTime(dtTripEndDate.ToString("yyyy-MM-dd")),
                                    SettlementDate = Convert.ToDateTime(dtSettlementDate.ToString("yyyy-MM-dd")),
                                    RouteDescription = tripRouteDescription,
                                    OpeningBalance = openingBalance,
                                    ClosingBalance = closingBalance,
                                    Weight = weight,
                                    Remarks = remarks.ToStringFromNull(),
                                    IsActive = true,
                                    CreationDate = utilityHelper.CurrentDateTime,
                                    UpdateDate = utilityHelper.CurrentDateTime,
                                    CreatedBy = userID,
                                    UpdatedBy = userID
                                };

                                _context.TblDriverHisabHeaders.Add(driverHisab);
                                _context.SaveChanges();

                                // Save Expense
                                if (_lstDriverLine != null && _lstDriverLine.Count > 0)
                                {
                                    foreach (var item in _lstDriverLine)
                                    {
                                        var line = new TblDriverHisabLine
                                        {
                                            SettlementNo = driverHisab.SettlementNo,
                                            DriverId = driverId,
                                            ExpenseCode = item.ExpenseCode,
                                            ExpenseType = item.ExpenseType,
                                            CrAmt = item.CrAmt,
                                            DrAmt = item.DrAmt,
                                            CreationDate = utilityHelper.CurrentDateTime,
                                            UpdateDate = utilityHelper.CurrentDateTime,
                                            CreatedBy = userID,
                                            UpdatedBy = userID
                                        };

                                        _context.TblDriverHisabLines.Add(line);
                                    }
                                    _context.SaveChanges();
                                }

                                transaction.Commit();
                                model.TripId = driverHisab.SettlementNo;
                                model.TransactionMessage.Status = TransactionStatus.Success;
                                model.TransactionMessage.Message = "Driver Hisab has been saved successfully.";
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                model.TripId = 0;
                                model.TransactionMessage.Status = TransactionStatus.Error;
                                model.TransactionMessage.Message = "Driver Hisab has not been saved due to some technical issue. Please try again.";
                            }
                        }
                    }
                    else
                    {

                        model.TripId = 0;
                        model.TransactionMessage.Status = TransactionStatus.Failed;
                        model.TransactionMessage.Message = "Driver Hisab Already Exist! Please try again with diffrent username.";

                    }
                    #endregion
                }
                else
                {
                    #region Update Section
                    using (var transaction = _context.Database.BeginTransaction())
                    {
                        try
                        {
                            // Retrieve the existing TblDieselHeader record based on TripId
                            var existingDriverHisab = _context.TblDriverHisabHeaders.FirstOrDefault(d => d.SettlementNo == settlementNo);

                            if (existingDriverHisab != null)
                            {
                                // Update the properties of the existing TblDieselHeader
                                existingDriverHisab.DriverId = driverId;
                                existingDriverHisab.VehicleNo = vehicleNo;
                                existingDriverHisab.TripStartDate = Convert.ToDateTime(dtTripStartDate.ToString("yyyy-MM-dd"));
                                existingDriverHisab.TripEndDate = Convert.ToDateTime(dtTripEndDate.ToString("yyyy-MM-dd"));
                                existingDriverHisab.SettlementDate = Convert.ToDateTime(dtSettlementDate.ToString("yyyy-MM-dd"));
                                existingDriverHisab.RouteDescription = tripRouteDescription;                            
                                existingDriverHisab.OpeningBalance = openingBalance;
                                existingDriverHisab.ClosingBalance = closingBalance;
                                existingDriverHisab.Weight = weight;
                                existingDriverHisab.Remarks = remarks.ToStringFromNull();
                                existingDriverHisab.IsActive = true; // You might want to control this based on input
                                existingDriverHisab.UpdateDate = utilityHelper.CurrentDateTime;
                                existingDriverHisab.UpdatedBy = userID;

                                _context.TblDriverHisabHeaders.Update(existingDriverHisab);
                                _context.SaveChanges(); // Save changes to the header first to ensure TripId is consistent
                            }
                            else
                            {
                                // Handle the case where the TblDriverHisabHeaders doesn't exist (you might want to log this or throw an error)                               

                                model.TripId = existingDriverHisab.SettlementNo;
                                model.TransactionMessage.Status = TransactionStatus.Error;
                                model.TransactionMessage.Message = $"Driver Hisab with SettlementId '{settlementNo}' not found for update.";

                            }

                            // Update TblDriverHisabLine
                            if (_lstDriverLine != null)
                            {
                                // Get existing lines for the current TripId
                                var existingLines = _context.TblDriverHisabLines.Where(l => l.SettlementNo == settlementNo).ToList();

                                // Identify lines to add
                                var linesToAdd = _lstDriverLine.Where(item => !existingLines.Any(e =>
                                    e.Sno==item.Sno)).Select(item => new TblDriverHisabLine
                                    //e.ExpenseCode == item.ExpenseCode && (e.CrAmt==item.CrAmt && e.DrAmt==item.DrAmt))).Select(item => new TblDriverHisabLine
                                    {
                                        SettlementNo = existingDriverHisab.SettlementNo,
                                        DriverId = driverId,
                                        ExpenseCode = item.ExpenseCode,
                                        ExpenseType = item.ExpenseType,
                                        CrAmt = item.CrAmt,
                                        DrAmt = item.DrAmt,
                                        CreationDate = utilityHelper.CurrentDateTime,
                                        UpdateDate = utilityHelper.CurrentDateTime,
                                        CreatedBy = userID,
                                        UpdatedBy = userID
                                    }).ToList();
                                _context.TblDriverHisabLines.AddRange(linesToAdd);

                                // Identify lines to remove
                                var linesToRemove = existingLines.Where(existing => !_lstDriverLine.Any(item =>
                                    existing.Sno==item.Sno)).ToList();
                                    //existing.ExpenseCode == item.ExpenseCode && (existing.CrAmt == item.CrAmt && existing.DrAmt == item.DrAmt))).ToList();
                                _context.TblDriverHisabLines.RemoveRange(linesToRemove);

                                // Check for Existing Row Update
                                // Identify Lines to update and remove
                                foreach (var existingLine in existingLines)
                                {
                                    var matchingItem = _lstDriverLine.FirstOrDefault(item =>
                                                       existingLine.Sno == item.Sno);

                                    if (matchingItem != null)
                                    {
                                        existingLine.CrAmt = matchingItem.CrAmt;
                                        existingLine.DrAmt = matchingItem.DrAmt;
                                        existingLine.UpdateDate = utilityHelper.CurrentDateTime;
                                        existingLine.UpdatedBy = userID;
                                        _context.TblDriverHisabLines.Update(existingLine);
                                    }
                                }
                                _context.SaveChanges();
                            }
                            else
                            {
                                // If TblDriverHisabLines is null, you might want to remove all existing lines for this TripId
                                var existingLines = _context.TblDriverHisabLines.Where(l => l.SettlementNo == settlementNo).ToList();
                                _context.TblDriverHisabLines.RemoveRange(existingLines);
                                _context.SaveChanges();
                            }

                            transaction.Commit();
                            model.TripId = existingDriverHisab.SettlementNo;
                            model.TransactionMessage.Status = TransactionStatus.Success;
                            model.TransactionMessage.Message = "Driver Hisab has been updated successfully.";
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            model.TripId =0;
                            model.TransactionMessage.Status = TransactionStatus.Failed;
                            model.TransactionMessage.Message = "Driver Hisab has not been updated due to some technical issue. Please try again.";

                            // Consider logging the exception: _logger.LogError(ex, "Error updating Diesel Hisab");
                        }
                    }
                    #endregion
                }
            }
            else
            {
                return RedirectToAction("Logout", "Login");
            }

            return Json(model);
        }
        [HttpPost]
        public JsonResult deleteDriverHisab(int settlementId)
        {
            VMDriverMaster model = new VMDriverMaster();
            try
            {
                var dieselHisab = _context.TblDriverHisabHeaders.Where(x => x.SettlementNo.Equals(settlementId));
                _context.TblDriverHisabHeaders.RemoveRange(dieselHisab);
                _context.SaveChanges();

                model.TransactionMessage.Status = TransactionStatus.Success;
                model.TransactionMessage.Message = "Driver Hisab has been deleted successfully.";
            }
            catch (Exception ex)
            {
                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = "Driver Hisab has not been deleted. Please try again.";
            }
            return Json(model);
        }

        [HttpGet]
        public async Task<JsonResult> getDriverHisablWithId(string settlementId)
        {
            int SettlementId = Convert.ToInt32(settlementId);
            object model = null;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string sql = @" SELECT
                                   dh.Settlement_No,dh.Last_Settlement_Id,dh.Vehicle_No,vm.Vehicle_No[VehicleNumber], 
                                   CONVERT(VARCHAR, dh.Trip_Start_Date, 105) AS TripStartDate,
                                   CONVERT(VARCHAR, dh.Trip_End_Date, 105) AS TripEndDate,
                                   dh.Route_Description,dh.Opening_Balance,dh.Closing_Balance,
                                   dh.Weight, dh.Is_Active,dh.Remarks,
                                   CONVERT(VARCHAR(10),dh.Creation_Date, 105) AS DieselHeaderCreationDate,
                                   CONVERT(VARCHAR(10),dh.Update_Date, 105) AS DieselHeaderUpdateDate,
                                   dh.Created_By AS DieselHeaderCreatedBy,
                                   dh.Updated_By AS DieselHeaderUpdatedBy,
                                   dh.Driver_Id,dm.Driver_Name,dm.Father_Name AS DriverFatherName,
                                   uc.User_Name AS DieselHeaderCreatedByName,
                                   uu.User_Name AS DieselHeaderUpdatedByName
                                   FROM [dbo].[tbl_Driver_Hisab_Header] dh 
                                   INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.Vehicle_No = vm.Id
                                   LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.Driver_Id = dm.Id
                                   LEFT JOIN [dbo].[tbl_UserMaster] uc ON dh.Created_By = uc.Id
                                   LEFT JOIN [dbo].[tbl_UserMaster] uu ON dh.Updated_By = uu.Id
                                  WHERE dh.Is_Active = 1 AND dh.Settlement_No =@SettlementId;";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@SettlementId", SettlementId);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                model = new
                                {
                                    SettlementNo = reader.GetInt32("Settlement_No"),
                                    LastSettlementId = reader.GetInt32("Last_Settlement_Id"),
                                    VehicleNo = reader.GetInt32("Vehicle_No").ToIntFromNull(),
                                    VehicleNumber = reader.GetString("VehicleNumber"),
                                    DriverId = reader.GetInt32("Driver_Id"),
                                    TripStartDate = reader.GetString("TripStartDate"),
                                    TripEndDate = reader.GetString("TripEndDate"),
                                    TripStartAndEndDate= reader.GetString("TripStartDate")+" To " + reader.GetString("TripEndDate"),
                                    TripRouteDescr = reader.GetString("Route_Description"),
                                    OpeningBalance = reader.GetDecimal("Opening_Balance").ToIntFromNull(),
                                    ClosingBalance = reader.GetDecimal("Closing_Balance").ToIntFromNull(),
                                    Weight = reader.GetDecimal("Weight").To2Decimal(),
                                    IsActive = reader.GetBoolean("Is_Active"),
                                    Remarks = reader.GetString("Remarks"),
                                    DieselHeaderCreationDate = reader.GetString("DieselHeaderCreationDate"),
                                    DieselHeaderUpdateDate = reader.GetString("DieselHeaderUpdateDate"),
                                    DieselHeaderCreatedBy = reader.GetInt32("DieselHeaderCreatedBy"),
                                    DieselHeaderUpdatedBy = reader.GetInt32("DieselHeaderUpdatedBy"),
                                    DriverName = reader.GetString("Driver_Name"),
                                    DriverFatherName = reader.GetString("DriverFatherName"),
                                    DieselHeaderCreatedByName = reader.GetString("DieselHeaderCreatedByName"),
                                    DieselHeaderUpdatedByName = reader.GetString("DieselHeaderUpdatedByName"),
                                    LastTripHistory = await DriverHisabContext.GetLastTripHistoryBySettlementNoAsync(_connectionString, reader.GetInt32("Last_Settlement_Id")),
                                    expenseList = await DriverHisabContext.GetExpenseListAsync(_connectionString, SettlementId)
                                };
                            }
                        }
                    }
                }

                return Json(model);
            }
            catch (Exception ex)
            {
                return Json(null);
            }
        }
        [HttpGet]
        public IActionResult DownloadDetailedExcelBySettlementId(int settlementId)
        {
            //if (hisabData == null)
            //{
            //    return NotFound(); // Or handle the case where data is not found
            //}
            ExcelPackage.License.SetNonCommercialOrganization("Your Non-Commercial Organization Name");
            using (var package = new ExcelPackage())
            {
                DataSet ds = new DataSet();
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("[dbo].[DriverHisab_List_DownloadExcel]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@SettlementNo", settlementId);
                        // create data adapter
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        // this will query your database and return the result to your datatable
                        da.Fill(ds);
                        da.Dispose();
                    }
                }

                #region Add Master
                ExcelWorksheet worksheetMaster = package.Workbook.Worksheets.Add("Driver Hisab Details");
                if (ds.Tables[0].Rows.Count >= 0)
                {
                    // Add Column Headers (from DataTable columns)
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        worksheetMaster.Cells[1, i + 1].Value = ds.Tables[0].Columns[i].ColumnName;
                    }

                    // Add Data Rows (from DataTable rows)
                    for (int row = 0; row < ds.Tables[0].Rows.Count; row++)
                    {
                        for (int col = 0; col < ds.Tables[0].Columns.Count; col++)
                        {
                            worksheetMaster.Cells[row + 2, col + 1].Value = ds.Tables[0].Rows[row][col];
                        }
                    }

                    // Auto-fit columns for better readability
                    worksheetMaster.Cells.AutoFitColumns();
                }
                #endregion

                #region Add Expense Detail
                ExcelWorksheet worksheetExpenseList = package.Workbook.Worksheets.Add("Expense Details");
                if (ds.Tables[1].Rows.Count >= 0)
                {
                    // Add Column Headers (from DataTable columns)
                    for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                    {
                        worksheetExpenseList.Cells[1, i + 1].Value = ds.Tables[1].Columns[i].ColumnName;
                    }

                    // Add Data Rows (from DataTable rows)
                    for (int row = 0; row < ds.Tables[1].Rows.Count; row++)
                    {
                        for (int col = 0; col < ds.Tables[1].Columns.Count; col++)
                        {
                            worksheetExpenseList.Cells[row + 2, col + 1].Value = ds.Tables[1].Rows[row][col];
                        }
                    }

                    // Auto-fit columns for better readability
                    worksheetExpenseList.Cells.AutoFitColumns();
                }
                #endregion

                // 4. Convert the Excel package to a byte array
                byte[] excelBytes = package.GetAsByteArray();

                // 5. Return the byte array as a FileResult for download
                string fileName = $"DriverHisab_{settlementId.ToString()}.xlsx";
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
            }
        }

        [HttpPost]
        public async Task<JsonResult> Approve(int settlementId)
        {
            VMDriverMaster model = new VMDriverMaster();

            try
            {
                int userID = 0;
                VMLogin userDetails = HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
                if (userDetails != null)
                {
                    userID = userDetails.Id;
                    // Retrieve the existing TblDriverHisabHeaders record based on settlementId
                    var UpdateDriverHisab = _context.TblDriverHisabHeaders.Where(d => d.SettlementNo == settlementId).FirstOrDefault();

                    if (UpdateDriverHisab != null)
                    {
                        // Update the properties of the existing TblDriverHisabHeaders

                        if (UpdateDriverHisab.ApprovedBy != 0)
                        {
                            UpdateDriverHisab.ApprovedBy = 0;
                            UpdateDriverHisab.ApprovedDate = null;
                        }
                        else
                        {
                            UpdateDriverHisab.ApprovedBy = userID;
                            UpdateDriverHisab.ApprovedDate = utilityHelper.CurrentDateTime;
                        }

                        _context.TblDriverHisabHeaders.Update(UpdateDriverHisab);
                        _context.SaveChanges(); // Save changes to the header first to ensure TripId is consistent

                        model.TransactionMessage.Status = TransactionStatus.Success;
                        model.TransactionMessage.Message = "Driver Hisab approved status has been changed successfully.";
                    }
                    else
                    {
                        model.TransactionMessage.Status = TransactionStatus.Error;
                        model.TransactionMessage.Message = "Driver Hisab has not been approved. Please try again.";
                    }
                }
                else
                {
                    model.TransactionMessage.Status = TransactionStatus.Error;
                    model.TransactionMessage.Message = "Driver Hisab has not been approved. Please try again.";
                }
            }
            catch (Exception ex)
            {

                model.TransactionMessage.Status = TransactionStatus.Error;
                model.TransactionMessage.Message = ex.Message.ToString();
            }
            return Json(model);
        }
    }
}
