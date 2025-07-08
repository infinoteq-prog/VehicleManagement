using System.Web;
using System.Resources;
using System.Reflection;
using System.Collections;
using System.Net;
using VMS.Helper;
using Org.BouncyCastle.Asn1.Ocsp;
using Microsoft.Data.SqlClient;
using System.Data.OleDb;
using System.Data;
using VMS.ViewModel;
using Microsoft.AspNetCore.Mvc;
using VMS.Models;
using System.Diagnostics.Metrics;
using Org.BouncyCastle.Utilities;
using System.Linq;
using Org.BouncyCastle.Asn1.Tsp;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Http;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.IdentityModel.Tokens;
using DocumentFormat.OpenXml.Spreadsheet;

namespace VMS.Helper
{
    public static class utilityHelper
    {
        private static IConfiguration _configuration;
        private static IWebHostEnvironment _environment;
        private static VmsDbContext _context;
        private static IHttpContextAccessor _accessor;
        public static void configure(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static void environment(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public static void tbsDataContext(VmsDbContext context)
        {
            _context = context;
        }
        public static void sessionExtension(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public static string getdownloadFilePath(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(_environment.WebRootPath) + fileName;
            return path;
        }
        public static string getCurrentRole()
        {
            String roleName = string.Empty;
            VMLogin userDetails = _accessor.HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                roleName = userDetails.RoleName;
            }
            return roleName;
        }

        public static VMLogin getCurrentUserSession()
        {
            VMLogin userSession = _accessor.HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userSession != null)
            {
                userSession = _accessor.HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            }
            return userSession;
        }

        public static VMLogin validateUserAndStoreSession(string userName, string password)
        {
            string _controllerName = "Utility Helper: validateUserAndStoreSession";
            VMLogin userDetails = _context.TblUserMasters.Select(x => new VMLogin
            {
                Id = x.Id,
                UserId = x.UserId,
                UserName = x.UserName,
                Email = x.EmailId,
                PhoneNumber = x.MobileNo,
                Password = x.Password,
                IsActive = x.IsActive,
                RoleId = x.RoleId,
                RoleName = _context.TblRoleMasters
                                    .Where(p => p.Id == x.RoleId)
                                    .Select(p => p.Role).FirstOrDefault(),
                TransporterName = _context.TblTransporterMasters.Where(t => t.UserId.Equals(x.Id))
                                    .Select(t => t.TransporterName).FirstOrDefault(),
                LastLoginTime = DateTime.Now.ToString("dd MMMM yyyy, HH:mm:ss"),
                Message = "User Authentication Successful!!! Redirecting to Dashboard Page.",
                Success = true
            }).Where(x => x.UserId.Equals(userName) && x.Password.Equals(password)).FirstOrDefault();

            try
            {
                var menuTypeIds = _context.TblUserFunctions
                              .Where(uf => uf.UserId == userDetails.Id)
                              .Select(uf => uf.MenuTypeId)
                              .ToList();
                if (menuTypeIds.Any())
                {
                    var menuCodes = new List<string>();

                    foreach (var menuTypeId in menuTypeIds)
                    {
                        if (menuTypeId == 1)
                        {
                            menuCodes.Add("1");
                        }
                        else
                        {
                            var code = _context.TblCodeMasters
                                               .Where(cm => cm.Id == menuTypeId)
                                               .Select(cm => cm.Code)
                                               .FirstOrDefault();

                            if (code != null)
                            {
                                menuCodes.Add(code);
                            }
                        }
                    }

                    userDetails.MenuCode = string.Join(",", menuCodes);
                }
                else
                {
                    if (userDetails.RoleName.Equals(SiteConstants.Admin) || userDetails.RoleName.Equals(SiteConstants.SuperAdmin))
                    {
                        userDetails.MenuCode = "1";
                    }
                    else
                    {
                        userDetails.MenuCode = string.Empty;
                    }
                }
            }
            catch(Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("Error occured while converting date {0}", ex.Message));
                if (userDetails.RoleName.Equals(SiteConstants.Admin) || userDetails.RoleName.Equals(SiteConstants.SuperAdmin))
                {
                    userDetails.MenuCode = "1";
                }
                else
                {
                    userDetails.MenuCode = string.Empty;
                }
            }

            if (userDetails != null)
            {
                #region...[Commented Authorization Code...]
                //Fetching User Access from Database 
                //VMUserRoleAccess model = new VMUserRoleAccess();

                //model = (from ur in _context.AspNetUserRolesNews
                //         join u in _context.AspNetUsers on ur.UserId equals u.Id
                //         join r in _context.AspNetRoles on ur.RoleId equals r.Id
                //         select new VMUserRoleAccess()
                //         {
                //             UserId = ur.UserId,
                //             RoleId = ur.RoleId,
                //             UserName = u.UserName,
                //             RoleName = r.Name,
                //             VehicleReleaseFullAccess = ur.VehicleReleaseFullAccess,
                //             VehicleReleaseInsertAccess = ur.VehicleReleaseViewAccess,
                //             VehicleReleaseUpdateAccess = ur.VehicleReleaseUpdateAccess,
                //             VehicleReleaseDeleteAccess = ur.VehicleReleaseDeleteAccess,
                //             VehicleReleaseViewAccess = ur.VehicleReleaseViewAccess,
                //             TransporterBillFullAccess = ur.TransporterBillFullAccess,
                //             TransporterBillInsertAccess = ur.TransporterBillInsertAccess,
                //             TransporterBillViewAccess = ur.TransporterBillViewAccess,
                //             TransporterBillPrintAccess = ur.TransporterBillPrintAccess,
                //             TransporterBillPendingAccess = ur.TransporterBillPendingAccess,
                //             RateMasterFullAccess = ur.RateMasterFullAccess,
                //             RateMasterAddAccess = ur.RateMasterAddAccess,
                //             RateMasterUpdateAccess = ur.RateMasterUpdateAccess,
                //             RateMasterDeleteAccess = ur.RateMasterDeleteAccess,
                //             RateMasterViewAccess = ur.RateMasterViewAccess,
                //             DieselRateMasterFullAccess = ur.DieselRateMasterFullAccess,
                //             DieselRateMasterAddAccess = ur.DieselRateMasterAddAccess,
                //             DieselRateMasterUpdateAccess = ur.DieselRateMasterUpdateAccess,
                //             DieselRateMasterDeleteAccess = ur.DieselRateMasterDeleteAccess,
                //             DieselRateMasterViewAccess = ur.DieselRateMasterViewAccess,
                //             DieselIssueEntryFullAccess = ur.DieselIssueEntryFullAccess,
                //             DieselIssueEntryAddAccess = ur.DieselIssueEntryAddAccess,
                //             DieselIssueEntryUpdateAccess = ur.DieselIssueEntryUpdateAccess,
                //             DieselIssueEntryDeleteAccess = ur.DieselIssueEntryDeleteAccess,
                //             DieselIssueEntryViewAccess = ur.DieselIssueEntryViewAccess,

                //             AddUserFullAccess = ur.AddUserFullAccess,
                //             AddUserAddAccess = ur.AddUserAddAccess,
                //             AddUserUpdateAccess = ur.AddUserUpdateAccess,
                //             AddUserDeleteAccess = ur.AddUserDeleteAccess,
                //             AddUserEntryViewAccess = ur.AddUserEntryViewAccess,
                //             AddUserRoleFullAccess = ur.AddUserRoleFullAccess,
                //             AddUserRoleAddAccess = ur.AddUserRoleAddAccess,
                //             AddUserRoleUpdateAccess = ur.AddUserRoleUpdateAccess,
                //             AddUserRoleDeleteAccess = ur.AddUserRoleDeleteAccess,
                //             AddUserRoleViewAccess = ur.AddUserRoleViewAccess,
                //             UserAccessFull = ur.UserAccessFull,
                //             UserAccessForAdd = ur.UserAccessForAdd,
                //             UserAccessForUpdate = ur.UserAccessForUpdate,
                //             UserAccessForDelete = ur.UserAccessForDelete,
                //             UserAccessForView = ur.UserAccessForView,
                //             DashboardFullAccess = ur.DashboardFullAccess,
                //         }).Where(x => x.UserId == userDetails.Id).FirstOrDefault();

                //if (model != null)
                //{
                //    //Adding User Access Details to Sesstion Object 
                //    HttpContext.Session.SetObjectAsJson("userAccess", model);
                //}
                #endregion

                //Adding User Login Details to Sesstion Object 
                _accessor.HttpContext.Session.SetObjectAsJson("userDetails", userDetails);

                //Fetching User Access Details and Saving it in Session Object
                utilityHelper.setCurrentUserAccess(userDetails.Id);
            }
            else
            {
                userDetails = new VMLogin
                {
                    Message = "Invalid UserID or Password!",
                    Success = false
                };
            }

            return userDetails;
        }

        public static Boolean isUserAdminOrSuperAdmin()
        {
            Boolean isAdmin = false;
            VMLogin userDetails = _accessor.HttpContext.Session.GetObjectFromJson<VMLogin>("userDetails");
            if (userDetails != null)
            {
                if (userDetails.RoleName.Equals(SiteConstants.Admin) || userDetails.RoleName.Equals(SiteConstants.Admin))
                {
                    isAdmin = true;
                }
            }
            return isAdmin;
        }

        public static void setCurrentUserAccess(int userId)
        {
            List<VMUserRoleAccess> userAccess = new List<VMUserRoleAccess>();

            //Code for User Access Function
            userAccess = _context.TblUserFunctions.Where(x => x.UserId.Equals(userId)).Select(x => new VMUserRoleAccess
            {
                Id = x.Id,
                UserId = x.UserId,
                RoleId = x.RoleId,
                FunctionId = x.FunctionId,
                FunctionName = x.FunctionName,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                CreateDate = x.CreationDate,
                UpdateDate = x.UpdateDate,
                CreatedBy = _context.TblUserMasters
                                                        .Where(p => p.Id == x.CreatedBy)
                                                        .Select(p => p.UserName).FirstOrDefault(),
                UpdateBy = _context.TblUserMasters
                                                        .Where(p => p.Id == x.UpdatedBy)
                                                        .Select(p => p.UserName).FirstOrDefault(),
                UserName = _context.TblUserMasters
                                                        .Where(p => p.Id == x.UserId)
                                                        .Select(p => p.UserName).FirstOrDefault(),
                RoleName = _context.TblRoleMasters
                                                        .Where(p => p.Id == x.RoleId)
                                                        .Select(p => p.RoleName).FirstOrDefault(),
                FunctionMasterName = _context.TblFunctionMasters
                                                        .Where(p => p.Id == x.FunctionId)
                                                        .Select(p => p.FunctionName).FirstOrDefault()
            }).ToList();

            if (userAccess.Count > 0)
            {
                _accessor.HttpContext.Session.SetObjectAsJson("userAccess", userAccess);
            }
        }

        public static VMUserRoleAccess getCurrentUserAccess()
        {
            VMUserRoleAccess userAcc = _accessor.HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            if (userAcc != null)
            {
                userAcc = _accessor.HttpContext.Session.GetObjectFromJson<VMUserRoleAccess>("userAccess");
            }
            return userAcc;
        }

        public static string generateUniqueId()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;

            characters += alphabets + small_alphabets + numbers;

            int length = 10;
            string uniqueId = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (uniqueId.IndexOf(character) != -1);
                uniqueId += character;
            }
            return uniqueId;
        }

        //Function to Fetch Company Code from Full Name
        public static string fetchBillTo(string billTo)
        {
            if (!string.IsNullOrEmpty(billTo))
            {
                if (!billTo.Equals("Select Bill To"))
                {
                    string[] str = billTo.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (str.Length > 0)
                    {
                        billTo = str[1].Trim();
                    }
                }
            }
            return billTo;
        }

        //Function to Fetch Transporter Code from Full Name
        public static string fetchTransporterCode(string transporterCode)
        {
            if (!string.IsNullOrEmpty(transporterCode))
            {
                if (!transporterCode.Equals("Select a transporter"))
                {
                    string[] str = transporterCode.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (str.Length > 0)
                    {
                        transporterCode = str[1].Trim();
                    }
                }
            }
            return transporterCode;
        }

        public static string getFileName(string folderName, string uniqueModulePrefix, string fileName)
        {
            string finalFileName = string.Empty;
            //Create a Folder.
            string path = Path.Combine(_environment.WebRootPath, folderName);

            finalFileName = Path.Combine(path, uniqueModulePrefix + SiteConstants.Underscore + fileName);
            return finalFileName;
        }

        public static string saveUploadFilesToFolder(IFormFile postedFile, string folderName, string uniqueModulePrefix)
        {
            string returnFilePath = string.Empty;
            if (postedFile != null)
            {
                //Create a Folder.
                string path = Path.Combine(_environment.WebRootPath, folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                //Save the uploaded file.
                string fileName = Path.GetFileName(postedFile.FileName);
                string filePath = Path.Combine(path, uniqueModulePrefix + SiteConstants.Underscore + fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                returnFilePath = folderName + SiteConstants.Backslash + uniqueModulePrefix + SiteConstants.Underscore + fileName;
            }
            return returnFilePath;
        }

        public static VMDispatchModel bulkUpdateLrGrNo(string filePath, int userId)
        {
            string uniqueId = string.Empty;
            Boolean isDeliveryColFound = false;
            DataTable dtDispatchError = new DataTable("Grid");
            VMDispatchModel dispatchModel = new VMDispatchModel();
            List<VMDispatchErrorModel> dispatchErrorModelList = new List<VMDispatchErrorModel>();
            try
            {
                //Read the connection string for the Excel file.
                string constr = _configuration.GetConnectionString("ExcelConnString");
                DataTable dt = new DataTable();
                constr = string.Format(constr, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(constr))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();

                            //Excel Column Validations
                            connExcel.Open();
                            DataTable dtCol = new DataTable();
                            cmdExcel.CommandText = "SELECT TOP 1 * FROM [" + sheetName + "] ";
                            var adapter = new OleDbDataAdapter(cmdExcel);
                            adapter.Fill(dtCol);

                            int sno = 0;

                            //Creating a List of Columns that needs to be checked
                            List<String> excelColumns = new List<String>();
                            excelColumns.Add(SiteConstants.Serial_No);
                            excelColumns.Add(SiteConstants.Delivery_No);
                            excelColumns.Add(SiteConstants.LR_GR_No);

                            //Created Datatable To Save Error Details
                            dtDispatchError.Columns.AddRange(new DataColumn[4] { new DataColumn("SERIAL_NUMBER"),
                                                                                 new DataColumn("ERROR_TYPE"),
                                                                                 new DataColumn("ERROR_REASON"),
                                                                                 new DataColumn("ERROR_DESCRIPTION")
                                                                                 });
                            //Code to Validate Excel File Column Names
                            foreach (DataColumn column in dtCol.Columns)
                            {
                                if (!column.ColumnName.Contains(" "))
                                {
                                    bool exists = excelColumns.Any(s => s.Contains(column.ColumnName));
                                    if (!exists)
                                    {
                                        sno = sno + 1;
                                        //dtDispatchError.Rows.Add(sno, "Column Not Found", column.ColumnName + " not found.", column.ColumnName + " not found in uploaded file.");
                                        VMDispatchErrorModel dispatchErrorModel = new VMDispatchErrorModel();
                                        dispatchErrorModel.Sno = sno;
                                        dispatchErrorModel.ErrorType = "Invalid Column Name";
                                        dispatchErrorModel.ErrorReason = column.ColumnName + " is Invalid. Please correct and try again.";
                                        dispatchErrorModel.ErrorDescription = column.ColumnName + " is Invalid. Please correct and try again.";
                                        dispatchErrorModelList.Add(dispatchErrorModel);
                                    }
                                }

                                if (column.ColumnName.Contains(SiteConstants.Delivery_No))
                                {
                                    isDeliveryColFound = true;
                                }
                            }
                            #region ...[Commented Code]...
                            //if (isDeliveryColFound)
                            //{
                            //    //Code to Validate Excel File Data
                            //    var list = dt.AsEnumerable().Select(r => r["Delivery_No"].ToString());
                            //    string value = string.Join(",", list);

                            //    var dispatchEntry = _context.TblDispatchDetails.Where(x => value.Contains(x.DeliveryNo)).ToList();
                            //    if (dispatchEntry.Count() > 0)
                            //    {
                            //        foreach (TblDispatchDetail dispatchDetail in dispatchEntry)
                            //        {
                            //            sno = sno + 1;
                            //            //dtDispatchError.Rows.Add(sno, "Duplicate Delivery Number", dispatchDetail.DeliveryNo + " Already Exists!", dispatchDetail.DeliveryNo + " already Exists in the database.");
                            //            VMDispatchErrorModel dispatchErrorModel = new VMDispatchErrorModel();
                            //            dispatchErrorModel.Sno = sno;
                            //            dispatchErrorModel.ErrorType = "Duplicate Delivery Number";
                            //            dispatchErrorModel.ErrorReason = "Delivery Number " + dispatchDetail.DeliveryNo + " Already Exists!";
                            //            dispatchErrorModel.ErrorDescription = "Delivery Number " + dispatchDetail.DeliveryNo + " already Exists in the database.";
                            //            dispatchErrorModelList.Add(dispatchErrorModel);
                            //        }
                            //    }
                            //}
                            #endregion
                        }
                    }
                }

                //If No Error Found In Data Validation. Sql Bulk Copy will be done.
                if (dispatchErrorModelList.Count == 0)
                {
                    //Generating a Unique Dispatch Id and add it to the datatable for bulk insert
                    System.Data.DataColumn newColumn = new System.Data.DataColumn(SiteConstants.Lr_Gr_No_Unique_Id, typeof(System.String));
                    uniqueId = utilityHelper.generateUniqueId();
                    newColumn.DefaultValue = uniqueId;
                    dt.Columns.Add(newColumn);

                    //Insert the Data read from the Excel file to Database Table and Save All Data in Temp Table.
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[SiteConstants.Delivery_No])))
                        {
                            var item = _context.TblDispatchDetails.Where(x => x.DeliveryNo.Equals(dr[SiteConstants.Delivery_No])).FirstOrDefault();
                            if (item != null)
                            {
                                item.LrGrNoUniqueId = Convert.ToString(dr[SiteConstants.Lr_Gr_No_Unique_Id]);
                                item.LrGrNo = Convert.ToString(dr[SiteConstants.LR_GR_No]);
                                _context.TblDispatchDetails.Update(item);
                                _context.SaveChanges();
                            }
                        }
                    }

                    dispatchModel.DispatchErrorModel = null;
                    dispatchModel.UniqueDispatchId = null;
                    dispatchModel.DisptachErrorFilePath = string.Empty;
                    dispatchModel.LrGrNoUniqueId = uniqueId;
                }
                else
                {
                    //Converting Linq Data to Datatable 
                    DataTable dtDispatchData = utilityHelper.LINQResultToDataTable(dispatchErrorModelList);

                    //Saving Excel File in Folder
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        //Create a Folder if not exists
                        string path = Path.Combine(_environment.WebRootPath, SiteConstants.lrGrNoUpdateFolder);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string fileName = SiteConstants.lrGrNoUpdateErrorFileName
                                        + SiteConstants.Underscore + DateTime.Now.ToString(SiteConstants.ddMMyyyyHHss)
                                        + SiteConstants.Dot + SiteConstants.excelFileExtension;

                        string fullFilePath = Path.Combine(path, fileName);
                        wb.Worksheets.Add(dtDispatchData);

                        using (FileStream stream = new FileStream(fullFilePath, FileMode.Create))
                        {
                            wb.SaveAs(stream);
                        }

                        dispatchModel.DispatchErrorModel = dispatchErrorModelList;
                        dispatchModel.UniqueDispatchId = null;
                        dispatchModel.DisptachErrorFilePath = fileName;
                        dispatchModel.LrGrNoUniqueId = uniqueId;
                    }
                }
            }
            catch (Exception ex)
            {
                dispatchModel.DispatchErrorModel = null;
                dispatchModel.UniqueDispatchId = null;
                dispatchModel.DisptachErrorFilePath = string.Empty;
                dispatchModel.LrGrNoUniqueId = null;
            }
            return dispatchModel;
        }

        public static VMDispatchModel saveDispatchData(string filePath, int userId)
        {
            string uniqueId = string.Empty;
            Boolean isDeliveryColFound = false;
            DataTable dtDispatchError = new DataTable("Grid");
            VMDispatchModel dispatchModel = new VMDispatchModel();
            List<VMDispatchErrorModel> dispatchErrorModelList = new List<VMDispatchErrorModel>();
            try
            {
                //Read the connection string for the Excel file.
                string constr = _configuration.GetConnectionString("ExcelConnString");
                DataTable dt = new DataTable();
                constr = string.Format(constr, filePath);

                using (OleDbConnection connExcel = new OleDbConnection(constr))
                {
                    using (OleDbCommand cmdExcel = new OleDbCommand())
                    {
                        using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                        {
                            cmdExcel.Connection = connExcel;

                            //Get the name of First Sheet.
                            connExcel.Open();
                            DataTable dtExcelSchema;
                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                            connExcel.Close();

                            //Read Data from First Sheet.
                            connExcel.Open();
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                            odaExcel.SelectCommand = cmdExcel;
                            odaExcel.Fill(dt);
                            connExcel.Close();

                            //Excel Column Validations
                            connExcel.Open();
                            DataTable dtCol = new DataTable();
                            cmdExcel.CommandText = "SELECT TOP 1 * FROM [" + sheetName + "] ";
                            var adapter = new OleDbDataAdapter(cmdExcel);
                            adapter.Fill(dtCol);

                            int sno = 0;

                            //Creating a List of Columns that needs to be checked
                            List<String> excelColumns = new List<String>();
                            //excelColumns.Add(SiteConstants.Dispatch_Unique_Id);
                            excelColumns.Add(SiteConstants.Supplying_Plant);
                            excelColumns.Add(SiteConstants.Shipment_Doc);
                            excelColumns.Add(SiteConstants.Delivery_No);
                            excelColumns.Add(SiteConstants.Dispatch_Category);
                            excelColumns.Add(SiteConstants.Truck_No);
                            excelColumns.Add(SiteConstants.LR_GR_No);
                            excelColumns.Add(SiteConstants.LR_GR_date);
                            excelColumns.Add(SiteConstants.Ship_To_Party_TZone);
                            excelColumns.Add(SiteConstants.Dispatch_Qty_Road);
                            excelColumns.Add(SiteConstants.RR_LR_No);
                            excelColumns.Add(SiteConstants.Freight_Road);
                            excelColumns.Add(SiteConstants.Ebid_net_amt);
                            excelColumns.Add(SiteConstants.Ebid_frt_rate);
                            excelColumns.Add(SiteConstants.Forwarding_Agent_Code);
                            excelColumns.Add(SiteConstants.Forwarding_Agent_Name);
                            excelColumns.Add(SiteConstants.Region_State);
                            //excelColumns.Add(SiteConstants.Pgi_No);
                            excelColumns.Add(SiteConstants.Pgi_Date);
                            excelColumns.Add(SiteConstants.Distribution_Channel);
                            excelColumns.Add(SiteConstants.Division);
                            excelColumns.Add(SiteConstants.Inco_Term);
                            excelColumns.Add(SiteConstants.Route_Code);
                            excelColumns.Add(SiteConstants.Route_Description);
                            //excelColumns.Add(SiteConstants.Total_Amount);

                            //Created Datatable To Save Error Details
                            dtDispatchError.Columns.AddRange(new DataColumn[4] { new DataColumn("SERIAL_NUMBER"),
                                                                                 new DataColumn("ERROR_TYPE"),
                                                                                 new DataColumn("ERROR_REASON"),
                                                                                 new DataColumn("ERROR_DESCRIPTION")
                                                                                 });
                            //Code to Validate Excel File Column Names
                            foreach (DataColumn column in dtCol.Columns)
                            {
                                if (!column.ColumnName.Contains(" "))
                                {
                                    bool exists = excelColumns.Any(s => s.Equals(column.ColumnName));
                                    if (!exists)
                                    {
                                        sno = sno + 1;
                                        //dtDispatchError.Rows.Add(sno, "Column Not Found", column.ColumnName + " not found.", column.ColumnName + " not found in uploaded file.");
                                        VMDispatchErrorModel dispatchErrorModel = new VMDispatchErrorModel();
                                        dispatchErrorModel.Sno = sno;
                                        dispatchErrorModel.ErrorType = "Invalid Column Name";
                                        dispatchErrorModel.ErrorReason = column.ColumnName + " is Invalid. Please correct and try again.";
                                        dispatchErrorModel.ErrorDescription = column.ColumnName + " is Invalid. Please correct and try again.";
                                        dispatchErrorModelList.Add(dispatchErrorModel);
                                    }
                                }

                                if (column.ColumnName.Contains(SiteConstants.Delivery_No))
                                {
                                    isDeliveryColFound = true;
                                }
                            }

                            //if (isDeliveryColFound)
                            //{
                            //    //Code to Validate Excel File Data
                            //    var list = dt.AsEnumerable().Select(r => r["Delivery_No"].ToString());
                            //    string value = string.Join(",", list);

                            //    var dispatchEntry = _context.TblDispatchDetails.Where(x => value.Contains(x.DeliveryNo)).ToList();
                            //    if (dispatchEntry.Count() > 0)
                            //    {
                            //        foreach (TblDispatchDetail dispatchDetail in dispatchEntry)
                            //        {
                            //            sno = sno + 1;
                            //            //dtDispatchError.Rows.Add(sno, "Duplicate Delivery Number", dispatchDetail.DeliveryNo + " Already Exists!", dispatchDetail.DeliveryNo + " already Exists in the database.");
                            //            VMDispatchErrorModel dispatchErrorModel = new VMDispatchErrorModel();
                            //            dispatchErrorModel.Sno = sno;
                            //            dispatchErrorModel.ErrorType = "Duplicate Delivery Number";
                            //            dispatchErrorModel.ErrorReason = "Delivery Number " + dispatchDetail.DeliveryNo + " Already Exists!";
                            //            dispatchErrorModel.ErrorDescription = "Delivery Number " + dispatchDetail.DeliveryNo + " already Exists in the database.";
                            //            dispatchErrorModelList.Add(dispatchErrorModel);
                            //        }
                            //    }
                            //}
                        }
                    }
                }

                //If No Error Found In Data Validation. Sql Bulk Copy will be done.
                if (dispatchErrorModelList.Count == 0)
                {
                    //Generating a Unique Dispatch Id and add it to the datatable for bulk insert
                    System.Data.DataColumn newColumn = new System.Data.DataColumn(SiteConstants.Dispatch_Unique_Id, typeof(System.String));
                    uniqueId = utilityHelper.generateUniqueId();
                    newColumn.DefaultValue = uniqueId;
                    dt.Columns.Add(newColumn);

                    System.Data.DataColumn creationDate = new System.Data.DataColumn(SiteConstants.Creation_Date, typeof(System.DateTime));
                    creationDate.DefaultValue = DateTime.Now;
                    dt.Columns.Add(creationDate);

                    System.Data.DataColumn updateDate = new System.Data.DataColumn(SiteConstants.Update_Date, typeof(System.DateTime));
                    updateDate.DefaultValue = DateTime.Now;
                    dt.Columns.Add(updateDate);

                    System.Data.DataColumn createdBy = new System.Data.DataColumn(SiteConstants.Created_By, typeof(System.Int32));
                    createdBy.DefaultValue = userId;
                    dt.Columns.Add(createdBy);

                    System.Data.DataColumn updatedBy = new System.Data.DataColumn(SiteConstants.Updated_By, typeof(System.Int32));
                    updatedBy.DefaultValue = userId;
                    dt.Columns.Add(updatedBy);

                    string deliveryNo = string.Empty;
                    string updatedNo = string.Empty;
                    string billExistsRecords = string.Empty;
                    string finalDeliveryNo = String.Empty;

                    //Converting Delivery No to comman seperated values
                    var list = dt.AsEnumerable().Select(r => r["Delivery_No"].ToString());
                    if (list.Count() > 0)
                    {
                        deliveryNo = string.Join(",", list);
                    }

                    //Before Inserting Bulk Data Need to Update the records where Delievery_No already exists and bill is not yet created and LRGRNo is not null
                    var updateItem = _context.TblDispatchDetails.Where(x => deliveryNo.Contains(x.DeliveryNo) && string.IsNullOrEmpty(x.BillNumber) && !string.IsNullOrEmpty(x.LrGrNo)).ToList();
                    if (updateItem.Count > 0)
                    {
                        foreach (TblDispatchDetail dispDetail in updateItem)
                        {
                            var row = dt.AsEnumerable().Where(x => x.Field<string?>("Delivery_No").Equals(dispDetail.DeliveryNo)).FirstOrDefault();
                            decimal freightRd = Convert.ToDecimal(row["Freight_Road"]);
                            dispDetail.DispatchUniqueId = uniqueId;
                            dispDetail.FreightRoad = freightRd;
                            _context.TblDispatchDetails.Update(dispDetail);
                            _context.SaveChanges();
                        }
                        //Fetching DelieveryNo whoes records are updated in previous query
                        updatedNo = string.Join(",", updateItem.Select(r => r.DeliveryNo));
                    }

                    DataTable dtNew = dt.AsEnumerable().Where(x => !updatedNo.Contains(x["Delivery_No"].ToString())).CopyToDataTable();

                    //Before Inserting Bulk Data Need to Delete the records where Delievery_No already exists and bill number is already generated
                    var item = _context.TblDispatchDetails.Where(x => deliveryNo.Contains(x.DeliveryNo) && !string.IsNullOrEmpty(x.BillNumber)).ToList();
                    if (item.Count > 0)
                    {
                        //Fetching DelieveryNo whoes bill number is already generated
                        billExistsRecords = string.Join(",", item.Select(r => r.DeliveryNo));
                    }

                    DataTable dtFinal = dtNew.AsEnumerable().Where(x => !billExistsRecords.Contains(x["Delivery_No"].ToString())).CopyToDataTable();

                    //Converting Delivery No to comman seperated values
                    var finalDelNo = dt.AsEnumerable().Select(r => r["Delivery_No"].ToString());
                    if (finalDelNo.Count() > 0)
                    {
                        finalDeliveryNo = string.Join(",", finalDelNo);
                    }

                    //Before Inserting Bulk Data Need to Delete the records where Delievery_No already exists and bill is not yet created
                    var deleteItem = _context.TblDispatchDetails.Where(x => finalDeliveryNo.Contains(x.DeliveryNo) && string.IsNullOrEmpty(x.BillNumber) && string.IsNullOrEmpty(x.LrGrNo)).ToList();
                    if (deleteItem.Count > 0)
                    {
                        _context.TblDispatchDetails.RemoveRange(deleteItem);
                        _context.SaveChanges();
                    }

                    //Insert the Data read from the Excel file to Database Table and Save All Data in Temp Table.
                    constr = _configuration.GetConnectionString("TBSContext");
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(constr, SqlBulkCopyOptions.TableLock))
                        {
                            //Set the database table name.
                            sqlBulkCopy.DestinationTableName = SiteConstants.tbl_Dispatch_Details;

                            sqlBulkCopy.BatchSize = SiteConstants.Batch_Size;

                            //[OPTIONAL]: Map the Excel columns with that of the database table.
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Dispatch_Unique_Id, "Dispatch_Unique_ID");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Supplying_Plant, "Supplying_Plant");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Shipment_Doc, "Shipment_No");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Delivery_No, "Delivery_No");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Truck_No, "Truck_No");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.LR_GR_date, "LR_GR_date");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Ship_To_Party_TZone, "Ship_To_Party_TZone");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Dispatch_Qty_Road, "Dispatch_Qty_Road");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Freight_Road, "Freight_Road");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Ebid_net_amt, "Ebid_net_amt");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Ebid_frt_rate, "Ebid_frt_rate");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Forwarding_Agent_Code, "Forwarding_Agent_Code");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Forwarding_Agent_Name, "Forwarding_Agent_Name");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Region_State, "Region_State");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Pgi_Date, "Pgi_Date");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Distribution_Channel, "Distribution_Channel");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Division, "Division");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Inco_Term, "Inco_Term");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Route_Code, "Route_Code");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Route_Description, "Route_Description");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Freight_Road, "Total_Amount");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Creation_Date, "Creation_Date");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Update_Date, "Update_Date");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Created_By, "Created_By");
                            sqlBulkCopy.ColumnMappings.Add(SiteConstants.Updated_By, "Updated_By");

                            con.Open();
                            sqlBulkCopy.WriteToServer(dtFinal);
                            con.Close();
                        }
                    }

                    dispatchModel.DispatchErrorModel = null;
                    dispatchModel.UniqueDispatchId = uniqueId;
                    dispatchModel.DisptachErrorFilePath = string.Empty;
                }
                else
                {
                    //Converting Linq Data to Datatable 
                    DataTable dtDispatchData = utilityHelper.LINQResultToDataTable(dispatchErrorModelList);

                    //Saving Excel File in Folder
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        //Create a Folder if not exists
                        string path = Path.Combine(_environment.WebRootPath, SiteConstants.dispatchErrorFileFolder);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        string fileName = SiteConstants.dispatchErrorFileName
                                        + SiteConstants.Underscore + DateTime.Now.ToString(SiteConstants.ddMMyyyyHHss)
                                        + SiteConstants.Dot + SiteConstants.excelFileExtension;

                        string fullFilePath = Path.Combine(path, fileName);
                        wb.Worksheets.Add(dtDispatchData);

                        using (FileStream stream = new FileStream(fullFilePath, FileMode.Create))
                        {
                            wb.SaveAs(stream);
                        }

                        dispatchModel.DispatchErrorModel = dispatchErrorModelList;
                        dispatchModel.UniqueDispatchId = uniqueId;
                        dispatchModel.DisptachErrorFilePath = fileName;
                    }
                }
            }
            catch (Exception ex)
            {
                dispatchModel.DispatchErrorModel = null;
                dispatchModel.UniqueDispatchId = uniqueId;
                dispatchModel.DisptachErrorFilePath = string.Empty;
            }
            return dispatchModel;
        }

        public static DataTable LINQResultToDataTable<T>(IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                Type colType = prop.PropertyType;
                dataTable.Columns.Add(new DataColumn(prop.Name, Nullable.GetUnderlyingType(colType) ?? colType));
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public static string FetchApplicableTaxRates(string transporterCode, string billToCode, int transporterStateId, int billtoCodeStateId)
        {
            string rateType = String.Empty;
            //Checking Transporter State is Union Territory or not
            var checkUnionTerritory = _context.TblStates.Where(x => x.Id == transporterStateId && x.IsUnionTerritory == true).FirstOrDefault();
            if (checkUnionTerritory == null) //Transporter State is not a union territory
            {
                if (transporterStateId.Equals(billtoCodeStateId)) //SGST, CGST or UGST tax will be applied
                {
                    rateType = SiteConstants.SGST_CGST_Tax;
                }
                else  //IGST UGST tax will be applied
                {
                    rateType = SiteConstants.IGST_Tax;
                }
            }
            else //Transporter State is a union territory so UGST Tax will be applicable
            {
                rateType = SiteConstants.UGST_Tax;
            }
            return rateType;
        }

        public static string getFinancialYear(this DateTime dateTime)
        {
            int financialYear = (dateTime.Month >= 4 ? dateTime.Year + 1 : dateTime.Year) - 1;
            string yr = (dateTime.Month >= 4 ? dateTime.Year + 1 : dateTime.Year).ToString();

            yr = yr.Remove(0, 2);

            string finYear = financialYear.ToString() + "-" + yr;

            return finYear;
        }

        public static string? SiteUrl()
        {
            return _configuration.GetValue<String>("ConfigurationSettings:SiteUrl");
        }

        public static string? reqFieldMessage
        {
            get
            {
                return _configuration.GetValue<String>("ConfigurationSettings:ReqFieldMessage");
            }
        }

        public static DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
            }
        }

        public static string CurrentDateTimeString
        {
            get
            {
                return DateTime.Now.ToString("dddd, dd MMMM yyyy");
            }
        }

        public static Double shortagePercentage
        {
            get
            {
                return SiteConstants.shortagePercentage;
            }
        }

        public static Decimal shortageRate
        {
            get
            {
                return SiteConstants.shortageRate;
            }
        }

        public static DateTime StrToDate(string strDate)
        {
            System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CreateSpecificCulture("en-US"); //en-US //fr-FR //de-DE
            DateTime dt;
            //if convertion failed, return default value 
            return DateTime.TryParse(strDate, culture, System.Globalization.DateTimeStyles.None, out dt) ? dt : new DateTime(1899, 12, 31);
        }

        private static String ConvertToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents    
                        endStr = "Paisa " + endStr;//Cents    
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }

        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }

        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX    
                bool isDone = false;//test if already translated    
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))    
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric    
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping    
                    String place = "";//digit grouping name:hundres,thousand,etc...    
                    switch (numDigits)
                    {
                        case 1://ones' range    

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range    
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range    
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range    
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range    
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range    
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...    
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)    
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }

                        //check for trailing zeros    
                        //if (beginsZero) word = " and " + word.Trim();    
                    }
                    //ignore digit grouping names    
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }

        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }

        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }

        #region ...[Commented Code Previous Logic Using Temp Table and Excel Upload from Form]...
        //public static string saveDispatchDataInTempTable(string filePath, int userId)
        //{
        //    string uniqueId = string.Empty;
        //    try
        //    {
        //        //Read the connection string for the Excel file.
        //        string constr = _configuration.GetConnectionString("ExcelConnString");
        //        DataTable dt = new DataTable();
        //        constr = string.Format(constr, filePath);

        //        using (OleDbConnection connExcel = new OleDbConnection(constr))
        //        {
        //            using (OleDbCommand cmdExcel = new OleDbCommand())
        //            {
        //                using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
        //                {
        //                    cmdExcel.Connection = connExcel;

        //                    //Get the name of First Sheet.
        //                    connExcel.Open();
        //                    DataTable dtExcelSchema;
        //                    dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                    connExcel.Close();

        //                    //Read Data from First Sheet.
        //                    connExcel.Open();
        //                    cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
        //                    odaExcel.SelectCommand = cmdExcel;
        //                    odaExcel.Fill(dt);
        //                    connExcel.Close();
        //                }
        //            }
        //        }
        //        //Generating a Unique Dispatch Id and add it to the datatable for bulk insert
        //        System.Data.DataColumn newColumn = new System.Data.DataColumn("Dispatch_Unique_ID", typeof(System.String));
        //        uniqueId = utilityHelper.generateUniqueId();
        //        newColumn.DefaultValue = uniqueId;
        //        dt.Columns.Add(newColumn);

        //        System.Data.DataColumn creationDate = new System.Data.DataColumn("Creation_Date", typeof(System.DateTime));
        //        creationDate.DefaultValue = DateTime.Now;
        //        dt.Columns.Add(creationDate);

        //        System.Data.DataColumn updateDate = new System.Data.DataColumn("Update_Date", typeof(System.DateTime));
        //        updateDate.DefaultValue = DateTime.Now;
        //        dt.Columns.Add(updateDate);

        //        System.Data.DataColumn createdBy = new System.Data.DataColumn("Created_By", typeof(System.Int32));
        //        createdBy.DefaultValue = userId;
        //        dt.Columns.Add(createdBy);

        //        System.Data.DataColumn updatedBy = new System.Data.DataColumn("Updated_By", typeof(System.Int32));
        //        updatedBy.DefaultValue = userId;
        //        dt.Columns.Add(updatedBy);

        //        //Insert the Data read from the Excel file to Database Table and Save All Data in Temp Table.
        //        constr = _configuration.GetConnectionString("TBSContext");
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //            {
        //                //Set the database table name.
        //                sqlBulkCopy.DestinationTableName = "dbo.tbl_Temp_Dispatch_Details";

        //                //[OPTIONAL]: Map the Excel columns with that of the database table.
        //                sqlBulkCopy.ColumnMappings.Add("Dispatch_Unique_ID", "Dispatch_Unique_ID");
        //                sqlBulkCopy.ColumnMappings.Add("Supplying_Plant", "Supplying_Plant");
        //                sqlBulkCopy.ColumnMappings.Add("Shipment_No", "Shipment_No");
        //                sqlBulkCopy.ColumnMappings.Add("Delivery_No", "Delivery_No");
        //                sqlBulkCopy.ColumnMappings.Add("Truck_No", "Truck_No");
        //                //sqlBulkCopy.ColumnMappings.Add("LR_GR_No", "LR_GR_No");
        //                sqlBulkCopy.ColumnMappings.Add("LR_GR_Date", "LR_GR_date");
        //                sqlBulkCopy.ColumnMappings.Add("Ship_To_Party_Tzone", "Ship_To_Party_TZone");
        //                sqlBulkCopy.ColumnMappings.Add("Dispatch_Qty_Road", "Dispatch_Qty_Road");
        //                sqlBulkCopy.ColumnMappings.Add("E-Bidding_Net_Price", "Ebid_net_amt");
        //                sqlBulkCopy.ColumnMappings.Add("E-Bidding_Rate", "Ebid_frt_rate");
        //                sqlBulkCopy.ColumnMappings.Add("Forwarding_Agent_Code", "Forwarding_Agent_Code");
        //                sqlBulkCopy.ColumnMappings.Add("Forwarding_Agent_Name", "Forwarding_Agent_Name");
        //                sqlBulkCopy.ColumnMappings.Add("Region_State", "Region_State");
        //                //sqlBulkCopy.ColumnMappings.Add("Pgi_No", "Pgi_No");
        //                sqlBulkCopy.ColumnMappings.Add("Pgi_Date", "Pgi_Date");
        //                sqlBulkCopy.ColumnMappings.Add("Distribution_Channel", "Distribution_Channel");
        //                sqlBulkCopy.ColumnMappings.Add("Division", "Division");
        //                sqlBulkCopy.ColumnMappings.Add("Incoterm", "Inco_Term");
        //                sqlBulkCopy.ColumnMappings.Add("Route_Code", "Route_Code");
        //                sqlBulkCopy.ColumnMappings.Add("Route_Description", "Route_Description");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Epod_No");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Epod_Date");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Bill_No");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Bill_Date");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Cgst_Rate");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Sgst_Rate");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Igst_Rate");
        //                //sqlBulkCopy.ColumnMappings.Add("Country", "Utgst_Rate");
        //                sqlBulkCopy.ColumnMappings.Add("E-Bidding_Net_Price", "Total_Amount");
        //                sqlBulkCopy.ColumnMappings.Add("Creation_Date", "Creation_Date");
        //                sqlBulkCopy.ColumnMappings.Add("Update_Date", "Update_Date");
        //                sqlBulkCopy.ColumnMappings.Add("Created_By", "Created_By");
        //                sqlBulkCopy.ColumnMappings.Add("Updated_By", "Updated_By");

        //                con.Open();
        //                sqlBulkCopy.WriteToServer(dt);
        //                con.Close();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        uniqueId = string.Empty;
        //    }
        //    return uniqueId;
        //}

        //private static void ValidateExcelColumns(string filePath)
        //{
        //    var connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;IMEX=1;HDR=Yes;TypeGuessRows=0;ImportMixedTypes=Text\""; ;
        //    using (var conn = new OleDbConnection(connectionString))
        //    {
        //        conn.Open();

        //        DataTable dt = new DataTable();
        //        var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = "SELECT TOP 1 * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";
        //            var adapter = new OleDbDataAdapter(cmd);
        //            adapter.Fill(dt);
        //        }

        //        foreach (DataColumn column in dt.Columns)
        //        {
        //            //Do something with your columns
        //        }
        //    }
        //}

        //public static Boolean copyDispatchDataFromTempTableToMainTable(DataTable dtDispatchData)
        //{
        //    Boolean isDataCopied = false;
        //    string constr = string.Empty;
        //    try
        //    {
        //        //Insert the Data read from the Excel file to Database Table and Save All Data in Temp Table.
        //        constr = _configuration.GetConnectionString("TBSContext");
        //        using (SqlConnection con = new SqlConnection(constr))
        //        {
        //            using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
        //            {
        //                //Set the database table name.
        //                sqlBulkCopy.DestinationTableName = "dbo.tbl_Dispatch_Details";

        //                //[OPTIONAL]: Map the Excel columns with that of the database table.
        //                sqlBulkCopy.ColumnMappings.Add("DispatchUniqueID", "Dispatch_Unique_ID");
        //                sqlBulkCopy.ColumnMappings.Add("SupplyingPlant", "Supplying_Plant");
        //                sqlBulkCopy.ColumnMappings.Add("ShipmentNo", "Shipment_No");
        //                sqlBulkCopy.ColumnMappings.Add("DeliveryNo", "Delivery_No");
        //                sqlBulkCopy.ColumnMappings.Add("TruckNo", "Truck_No");
        //                sqlBulkCopy.ColumnMappings.Add("LrGrNo", "LR_GR_No");
        //                sqlBulkCopy.ColumnMappings.Add("LrGrDate", "LR_GR_date");
        //                sqlBulkCopy.ColumnMappings.Add("ShipToPartyTzone", "Ship_To_Party_TZone");
        //                sqlBulkCopy.ColumnMappings.Add("DispatchQtyRoad", "Dispatch_Qty_Road");
        //                sqlBulkCopy.ColumnMappings.Add("EbidNetAmt", "Ebid_net_amt");
        //                sqlBulkCopy.ColumnMappings.Add("EbidFrtRate", "Ebid_frt_rate");
        //                sqlBulkCopy.ColumnMappings.Add("ForwardingAgentCode", "Forwarding_Agent_Code");
        //                sqlBulkCopy.ColumnMappings.Add("ForwardingAgentName", "Forwarding_Agent_Name");
        //                sqlBulkCopy.ColumnMappings.Add("RegionState", "Region_State");
        //                sqlBulkCopy.ColumnMappings.Add("PgiNo", "Pgi_No");
        //                sqlBulkCopy.ColumnMappings.Add("PgiDate", "Pgi_Date");
        //                sqlBulkCopy.ColumnMappings.Add("DistributionChannel", "Distribution_Channel");
        //                sqlBulkCopy.ColumnMappings.Add("Division", "Division");
        //                sqlBulkCopy.ColumnMappings.Add("IncoTerm", "Inco_Term");
        //                sqlBulkCopy.ColumnMappings.Add("RouteCode", "Route_Code");
        //                sqlBulkCopy.ColumnMappings.Add("RouteDescription", "Route_Description");
        //                sqlBulkCopy.ColumnMappings.Add("EpodNo", "Epod_No");
        //                sqlBulkCopy.ColumnMappings.Add("EpodDate", "Epod_Date");
        //                sqlBulkCopy.ColumnMappings.Add("BillNo", "Bill_Number");
        //                sqlBulkCopy.ColumnMappings.Add("BillDate", "Bill_Date");
        //                sqlBulkCopy.ColumnMappings.Add("CgstRate", "Cgst_Rate");
        //                sqlBulkCopy.ColumnMappings.Add("SgstRate", "Sgst_Rate");
        //                sqlBulkCopy.ColumnMappings.Add("IgstRate", "Igst_Rate");
        //                sqlBulkCopy.ColumnMappings.Add("UtgstRate", "Utgst_Rate");
        //                sqlBulkCopy.ColumnMappings.Add("TotalAmount", "Total_Amount");
        //                sqlBulkCopy.ColumnMappings.Add("IsActive", "Is_Active");
        //                sqlBulkCopy.ColumnMappings.Add("CreationDate", "Creation_Date");
        //                sqlBulkCopy.ColumnMappings.Add("UpdateDate", "Update_Date");
        //                sqlBulkCopy.ColumnMappings.Add("CreatedBy", "Created_By");
        //                sqlBulkCopy.ColumnMappings.Add("UpdatedBy", "Updated_By");

        //                con.Open();
        //                sqlBulkCopy.WriteToServer(dtDispatchData);
        //                con.Close();

        //                isDataCopied = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        isDataCopied = false;
        //    }
        //    return isDataCopied;
        //}

        #endregion
    }

    public static class DateTimeExtensions
    {
        public static string ToFinancialYear(this DateTime dateTime)
        {
            return /*"Financial Year " +*/ (dateTime.Month >= 4 ? dateTime.Year + 1 : dateTime.Year).ToString();
        }

        public static string ToFinancialYearShort(this DateTime dateTime)
        {
            return/* "FY" +*/ (dateTime.Month >= 4 ? dateTime.AddYears(1).ToString("yy") : dateTime.ToString("yy"));
        }
    }
}
