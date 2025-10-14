using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.Data.SqlClient;
using System.Data;
using VMS.Models;

namespace VMS
{
    public static class DieselHisabContext
    {
        private static string _controllerName = "DieselHisabContext";
        public static async Task<object> GetLastTripHistoryAsync(string _connectionString, int vehicleNo)
        {
            object lastTrip = null;
            string sql = @"
                            SELECT 
                            dh.TripId,
                            CONVERT(VARCHAR, dh.Trip_Start_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_Start_Date, 108) AS LastTripStartDate,
                            CONVERT(VARCHAR, dh.Trip_End_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_End_Date, 108) AS LastTripEndDate,
                            dh.Last_Trip_Route_Descr,
                            dh.Opening_Diesel,
                            vm.Vehicle_No AS VehicleNumber,
                            dm.Driver_Name AS LastTripDriver,
                            dm.Father_Name AS LastTripDriverFatherName
                            FROM [dbo].[tbl_Diesel_Header] dh
                            INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.VehicleNo = vm.Id
                           LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.DriverId = dm.Id
                           WHERE vm.Id = @VehicleNo
                            ORDER BY dh.TripId DESC
                           OFFSET 1 ROWS;"; // Skip the current trip
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if(connection.State == ConnectionState.Closed) 
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleNo", vehicleNo);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            lastTrip = new
                            {
                                TripId = reader.GetInt32("TripId").ToIntFromNull(),
                                LastTripStartDate = reader.IsDBNull("LastTripStartDate") ? null : reader.GetString("LastTripStartDate"),
                                LastTripEndDate = reader.IsDBNull("LastTripEndDate") ? null : reader.GetString("LastTripEndDate"),
                                LastTripRouteDescr = reader.GetString("Last_Trip_Route_Descr"),
                                OpeningDiesel = reader.GetInt64("Opening_Diesel") == 0 ? 1 : reader.GetInt64("Opening_Diesel"),
                                LastTripVendor = "Test Vendor",
                                LastTripDriver = reader.IsDBNull("LastTripDriver") ? null : reader.GetString("LastTripDriver"),
                                LastTripDriverFatherName = reader.IsDBNull("LastTripDriverFatherName") ? null : reader.GetString("LastTripDriverFatherName"),
                                VehicleNumber = reader.GetString("VehicleNumber")
                            };
                        }
                    }
                }
            }
            return lastTrip;
        }
        public static async Task<object> GetLastTripHistoryByTripIdAsync(string _connectionString, int tripId)
        {
            object lastTrip = null;
            string sql = @"
                            SELECT 
                            dh.TripId,
                            CONVERT(VARCHAR, dh.Trip_Start_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_Start_Date, 108) AS LastTripStartDate,
                            CONVERT(VARCHAR, dh.Trip_End_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_End_Date, 108) AS LastTripEndDate,
                            dh.Last_Trip_Route_Descr,
                            dh.Opening_Diesel,
                            vm.Vehicle_No AS VehicleNumber,
                            dm.Driver_Name AS LastTripDriver,
                            dm.Father_Name AS LastTripDriverFatherName,dh.Bhari_Ka_Average[lastBhariKaAverage]
                            FROM [dbo].[tbl_Diesel_Header] dh
                            INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.VehicleNo = vm.Id
                           LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.DriverId = dm.Id
                           WHERE dh.tripId = @tripId
                            ORDER BY dh.TripId DESC;"; // Skip the current trip
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@tripId", tripId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            lastTrip = new
                            {
                                TripId = reader.GetInt32("TripId").ToIntFromNull(),
                                LastTripStartDate = reader.IsDBNull("LastTripStartDate") ? null : reader.GetString("LastTripStartDate"),
                                LastTripEndDate = reader.IsDBNull("LastTripEndDate") ? null : reader.GetString("LastTripEndDate"),
                                LastTripRouteDescr = reader.GetString("Last_Trip_Route_Descr"),
                                OpeningDiesel = reader.GetInt64("Opening_Diesel") == 0 ? 1 : reader.GetInt64("Opening_Diesel"),
                                LastTripVendor = "Test Vendor",
                                LastBhariKaAverage = reader.GetDecimal("lastBhariKaAverage").To2Decimal(),
                                LastTripDriver = reader.IsDBNull("LastTripDriver") ? null : reader.GetString("LastTripDriver"),
                                LastTripDriverFatherName = reader.IsDBNull("LastTripDriverFatherName") ? null : reader.GetString("LastTripDriverFatherName"),
                                VehicleNumber = reader.GetString("VehicleNumber")
                            };
                        }
                    }
                }
            }
            return lastTrip;
        }
        public static async Task<List<object>> GetDieselFillingListAsync(string _connectionString, int tripId)
        {
            List<object> dieselFillingList = new List<object>();
            string sql = @"
            SELECT df.Diesel_Filling_Date AS DieselFillingDate,
                df.VendorId,
                df.Diesel_Qty AS Litre,
                cm.Code AS VendorName
                FROM [dbo].[tbl_Diesel_Filling] df
                LEFT JOIN [dbo].[tbl_Code_Master] cm ON df.VendorId = cm.Id
            WHERE df.TripId = @TripId;";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TripId", tripId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dieselFillingList.Add(new
                            {
                                DieselFillingDate =Convert.ToDateTime(reader.GetDateTime("DieselFillingDate")).ToString("dd-MM-yyyy"),
                                StrDieselFillingDate =reader.GetDateTime("DieselFillingDate").ToString("dd-MM-yyyy"),
                                VendorId = reader.GetInt32("VendorId"),
                                Litre = reader.GetInt64("Litre"),
                                VendorName = reader.IsDBNull("VendorName") ? null : reader.GetString("VendorName")
                            });
                        }
                    }
                }
            }
            return dieselFillingList;
        }
        public static async Task<List<object>> GetStationListAsync(string _connectionString, int tripId, int vehicleNo)
        {
            List<object> stationList = new List<object>();
            string sql = @"
                            SELECT dl.Route_Id[RouteId],
                            dl.Route_Desc[RouteDesc],dl.Load_Type[LoadType], cm.Id AS LoadTypeId,dl.Distance,dl.Average,dl.Estimated_Diesel,dh.Is_DifferenceAdded,dh.Is_LoadingAdded
                        FROM [dbo].[tbl_Diesel_Line] dl
                        INNER JOIN [dbo].[tbl_Diesel_Header] dh on dl.TripId=dh.TripId
                        LEFT JOIN [dbo].[tbl_Code_Master] cm ON dl.Load_Type = cm.Code AND cm.Code_Type = 'LOADTYPE'
                        LEFT JOIN [dbo].[tbl_Distance_Master] dm ON dl.Route_Id = dm.Id
                        WHERE dl.TripId = @TripId;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@TripId", tripId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stationList.Add(new
                            {
                                VehicleNo = vehicleNo,
                                RouteId = reader.GetInt64("RouteId"),
                                RouteDesc = reader.GetString("RouteDesc"),
                                isDifference = reader.GetBoolean("Is_DifferenceAdded"),
                                isLoadUnload = reader.GetBoolean("Is_LoadingAdded"),
                                LoadType = reader.GetString("LoadType"),
                                LoadTypeId = reader.IsDBNull("LoadTypeId") ? (int?)null : reader.GetInt32("LoadTypeId"),
                                Distance = reader.IsDBNull("Distance") ? (decimal?)null : reader.GetDecimal("Distance"),
                                Average = reader.IsDBNull("Average") ? (decimal?)null : reader.GetDecimal("Average"), 
                                EstimatedDiesel = reader.IsDBNull("Estimated_Diesel") ? (decimal?)null : reader.GetDecimal("Estimated_Diesel"), 
                            });
                        }
                    }
                }
            }
            return stationList;
        }
        public static async Task<List<object>> GetPrevious4TripAverageAsync(string _connectionString, int tripId, string vehicleNo)
        {
            List<object> _lst = new List<object>();
            string sql = @"[dbo].[GetPrevious4Bhari_Ka_Average]";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@CurTripID", tripId);
                    command.Parameters.AddWithValue("@CurVehicleNo", vehicleNo);
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            _lst.Add(row);
                        }
                    }
                }
            }
            return _lst;
        }
        public static async Task<object> GetDieselAverage(string _connectionString, int vehicleNo, string loadType)
        {
            string sql = "";
            string columnName = "";

            switch (loadType?.ToUpperInvariant())
            {
                case "UL_GUJ":
                    columnName = "Ul_Avg";
                    break;
                case "KHALI":
                    columnName = "Khali";
                    break;
                case "NH76":
                    columnName = "Nh";
                    break;
                case "UL_MG":
                    columnName = "Mega_Hw";
                    break;
                case "WAJAN":
                    columnName = "OverLoad";
                    break;
                default:
                    return null;
            }

            if (string.IsNullOrEmpty(columnName))
            {
                return null;
            }

            sql = $@"
            SELECT mam.Id, mam.{columnName} AS value
            FROM  [dbo].[tbl_Vehicle_Master]  vm
            INNER JOIN [dbo].[tbl_Model_Average_Master] mam ON vm.ModelId = mam.Id
            WHERE vm.Id = @VehicleNo;";

            object result = null;


            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    if (connection.State == ConnectionState.Closed)
                        await connection.OpenAsync();
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@VehicleNo", vehicleNo);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                result=new
                                {
                                    Id = reader.GetInt32("Id"),
                                    value = reader.IsDBNull(reader.GetOrdinal("value")) ? (decimal?)null : reader.GetDecimal("value")
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in GetDieselAverage: {ex.Message}");
                return null;
            }

            return result;
        }
        public static async Task<object> GetLastDieselTripHistory(string _connectionString, int vehicleId)
        {
            object dieselInfo = null;
            string sql = @"SELECT TOP 1
                dh.TripId,
                CONVERT(VARCHAR, dh.Trip_Start_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_Start_Date, 108) AS LastTripStartDate,
                CONVERT(VARCHAR, dh.Trip_End_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_End_Date, 108) AS LastTripEndDate,
                CONVERT(VARCHAR, dh.Trip_End_Date, 105) + ' ' + CONVERT(VARCHAR(8), dh.Trip_End_Date, 108) AS NextTripStartDate,
                dh.Last_Trip_Route_Descr,
                ISNULL(dh.Start_Odometer, 1) AS StartOdometer,
                ISNULL(dh.End_Odometer, 1) AS EndOdometer,
                ISNULL(dh.Opening_Diesel, 1) AS OpeningDiesel,
                ISNULL(dh.Closing_Diesel, 1) AS ClosingDiesel,
                vm.Vehicle_No AS VehicleNumber,
                dm.Id[LastTripDriverId],
                dm.Driver_Name AS LastTripDriver,
                dm.Father_Name AS LastTripDriverFatherName,dh.Bhari_Ka_Average[lastBhariKaAverage]
            FROM [dbo].[tbl_Diesel_Header] dh
            INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.VehicleNo = vm.Id
            LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.DriverId = dm.Id
            WHERE dh.Is_Active = 1 AND vm.Id = @VehicleId
            ORDER BY dh.TripId DESC;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@VehicleId", vehicleId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            dieselInfo = new
                            {
                                TripId = reader.GetInt32("TripId"),
                                LastTripStartDate = reader.GetString("LastTripStartDate"),
                                LastTripEndDate = reader.GetString("LastTripEndDate"),
                                NextTripStartDate = reader.GetString("NextTripStartDate"),
                                LastTripRouteDescr = reader.GetString("Last_Trip_Route_Descr"),
                                StartOdometer = reader.GetInt64("StartOdometer"),
                                EndOdometer = reader.GetInt64("EndOdometer"),
                                OpeningDiesel = reader.GetInt64("OpeningDiesel"),
                                ClosingDiesel = reader.GetInt64("ClosingDiesel"),
                                LastBhariKaAverage = reader.GetDecimal("lastBhariKaAverage").To2Decimal(),
                                LastTripVendor = "Test Vendor",
                                LastTripDriverId = reader.IsDBNull("LastTripDriverId") ? 0 : reader.GetInt32("LastTripDriverId"),
                                LastTripDriver = reader.IsDBNull("LastTripDriver") ? null : reader.GetString("LastTripDriver"),
                                LastTripDriverFatherName = reader.IsDBNull("LastTripDriverFatherName") ? null : reader.GetString("LastTripDriverFatherName"),
                                VehicleNumber = reader.GetString("VehicleNumber")
                            };
                        }
                    }
                }
            }
            return dieselInfo;
        }
        public static async Task<List<object>> searchDieselHisabMaster(string _connectionString,int id, int vehicleNo, int driverId, string tripStartDate, string tripEndDate, int startOdometer, int endOdometer, decimal openingDiesel)
        {
            List<object> dieselHeaders = new List<object>();
            string sql = @"[dbo].[DieselHisab_List]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripId", id);
                    command.Parameters.AddWithValue("@VehicleNo", vehicleNo);
                    command.Parameters.AddWithValue("@DriverId", driverId);
                    command.Parameters.AddWithValue("@TripStartDate", string.IsNullOrEmpty(tripStartDate) ? (object)DBNull.Value : tripStartDate);
                    command.Parameters.AddWithValue("@TripEndDate", string.IsNullOrEmpty(tripEndDate) ? (object)DBNull.Value : tripEndDate);
                    command.Parameters.AddWithValue("@StartOdometer", startOdometer <= 0 ? "" : startOdometer);
                    command.Parameters.AddWithValue("@EndOdometer", endOdometer <= 0 ? "" : endOdometer);
                    command.Parameters.AddWithValue("@OpeningDiesel", openingDiesel <= 0 ? "" : openingDiesel);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dieselHeaders.Add(new
                            {
                                TripId = reader.GetInt32("TripId"),
                                VehicleNumber = reader.GetString("VehicleNumber"),
                                DriverId = reader.GetInt32("DriverId"),
                                TripStartDate = reader.GetString("TripStartDate"),
                                TripEndDate = reader.GetString("TripEndDate"),
                                CreationDate = reader.GetString("CreationDate"),
                                UpdateDate = reader.IsDBNull("UpdateDate") ? null : reader.GetString("UpdateDate"),
                                dieselHeaderCreatedByName = reader.IsDBNull("DieselHeaderCreatedByName") ? "" : reader.GetString("DieselHeaderCreatedByName"),
                                dieselHeaderUpdatedByName = reader.IsDBNull("DieselHeaderUpdatedByName") ? "" : reader.GetString("DieselHeaderUpdatedByName"),
                                LastTripRouteDescr = reader.GetString("LastTripRouteDescr"),
                                StartOdometer = reader.GetInt64("Start_Odometer"),
                                EndOdometer = reader.GetInt64("End_Odometer"),
                                OpeningDiesel = reader.GetInt64("Opening_Diesel"),
                                RunningKm = reader.IsDBNull("RunningKm") ? (decimal?)null : reader.GetInt32("RunningKm"),
                                TotalDieselFilled = reader.IsDBNull("TotalDieselFilled") ? (decimal?)null : reader.GetInt64("TotalDieselFilled"),
                                TotalDistanceKM = reader.IsDBNull("TotalDistanceKM") ? (decimal?)null : reader.GetDecimal("TotalDistanceKM"),
                                TotalEstimatedDiesel = reader.IsDBNull("TotalEstimatedDiesel") ? (decimal?)null : reader.GetDecimal("TotalEstimatedDiesel").To2Decimal(),
                                ProfitLoss = reader.IsDBNull("Profit_Loss") ? (decimal?)null : reader.GetDecimal("Profit_Loss").To2Decimal(),
                                PercentLoss = reader.IsDBNull("Percent_Loss") ? (decimal?)null : reader.GetDecimal("Percent_Loss").To2Decimal(),
                                BhariKaAverage = reader.IsDBNull("Bhari_Ka_Average") ? (decimal?)null : reader.GetDecimal("Bhari_Ka_Average").To2Decimal(),
                                DiscountPer = reader.IsDBNull("DiscountPer") ? (decimal?)null : reader.GetDecimal("DiscountPer").To2Decimal(),
                                DiscountValue = reader.IsDBNull("DiscountValue") ? (decimal?)null : reader.GetDecimal("DiscountValue").To2Decimal(),
                                IsActive = reader.GetBoolean("Is_Active"),
                                LastTripVendor = "Test Vendor",
                                DieselHeaderCreationDate = reader.GetDateTime("DieselHeaderCreationDate"),
                                DieselHeaderUpdateDate = reader.IsDBNull("DieselHeaderUpdateDate") ? (DateTime?)null : reader.GetDateTime("DieselHeaderUpdateDate"),
                                DieselHeaderCreatedBy = reader.GetInt32("DieselHeaderCreatedBy"),
                                DieselHeaderUpdatedBy = reader.IsDBNull("DieselHeaderUpdatedBy") ? (int?)null : reader.GetInt32("DieselHeaderUpdatedBy"),
                                DriverName = reader.IsDBNull("Driver_Name") ? null : reader.GetString("Driver_Name"),
                                DriverFatherName = reader.IsDBNull("DriverFatherName") ? null : reader.GetString("DriverFatherName"),
                                ApprovedStatus = reader.IsDBNull("ApprovedStatus") ? null : reader.GetString("ApprovedStatus"),
                                ApprovedBy = reader.IsDBNull("ApprovedBy") ? null : reader.GetString("ApprovedBy"),
                                ApprovedDate = reader.IsDBNull("ApprovedDate") ? null : reader.GetDateTime("ApprovedDate").ToString()
                            });
                        }
                    }
                }
            }
            return dieselHeaders;
        }
        public static async Task<List<object>> searchDieselFilter(string _connectionString, int id, int vehicleNo, int vendorId, string tripStartDate, string tripEndDate)
        {
            List<object> dieselHeaders = new List<object>();
            string sql = @"[dbo].[DieselHisab_DieselList]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@TripId", id);
                    command.Parameters.AddWithValue("@VehicleNo", vehicleNo);
                    command.Parameters.AddWithValue("@vendorId", vendorId);
                    command.Parameters.AddWithValue("@TripStartDate", string.IsNullOrEmpty(tripStartDate) ? (object)DBNull.Value : tripStartDate);
                    command.Parameters.AddWithValue("@TripEndDate", string.IsNullOrEmpty(tripEndDate) ? (object)DBNull.Value : tripEndDate);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            dieselHeaders.Add(new
                            {
                                TripId = reader.GetInt32("TripId"),
                                DieselFillingId = reader.GetInt32("DieselFillingId"),
                                VehicleNumber = reader.GetString("VehicleNumber"),
                                VendorName = reader.IsDBNull("VendorName") ? null : reader.GetString("VendorName"),
                                DriverId = reader.IsDBNull("DriverId")? 0: reader.GetInt32("DriverId"),
                                DieselFillingDate = reader.GetString("Diesel_Filling_Date"),                                
                                DriverName = reader.IsDBNull("Driver_Name") ? null : reader.GetString("Driver_Name"),
                                DriverFatherName = reader.IsDBNull("DriverFatherName") ? null : reader.GetString("DriverFatherName"),
                                DieselQty = reader.IsDBNull("Diesel_Qty") ? 0 : reader.GetInt64("Diesel_Qty")
                            });
                        }
                    }
                }
            }
            return dieselHeaders;
        }

        public static decimal calProfitLoss(decimal openingDiesel, decimal closingDiesel, List<TblDieselFilling> _lstDieselFilling, List<TblDieselLine> _lstDieselLine)
        {
            decimal ProfitLoss = 0;
            try
            {
                decimal sumTotalDiesel = ((openingDiesel + (_lstDieselFilling?.Sum(x => x.DieselQty).To3Decimal() ?? 0)) - closingDiesel);
                decimal sumTotalEstimatedDiesel = _lstDieselLine?.Sum(x => x.EstimatedDiesel).To3Decimal() ?? 0;

                ProfitLoss = sumTotalEstimatedDiesel- sumTotalDiesel ;
            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("calProfitLoss: {0}", ex.Message.ToString()));
            }
            return ProfitLoss;
        }
        public static decimal calPercentLoss(decimal openingDiesel, decimal closingDiesel, List<TblDieselFilling> _lstDieselFilling, List<TblDieselLine> _lstDieselLine)
        {
            decimal PercentLoss = 0; 
            try
            {
                decimal sumTotalDiesel = ((openingDiesel + (_lstDieselFilling?.Sum(x => x.DieselQty).To3Decimal() ?? 0)) - closingDiesel);
                decimal sumTotalEstimatedDiesel = _lstDieselLine?.Sum(x => x.EstimatedDiesel).To3Decimal() ?? 0;
                decimal ProfitLoss = sumTotalEstimatedDiesel - sumTotalDiesel;

                PercentLoss = ((ProfitLoss * 100) / sumTotalEstimatedDiesel).To2Decimal();

            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("calPercentLoss: {0}", ex.Message.ToString()));
            }
            return PercentLoss;
        }
        public static decimal calBhariKaAverage(decimal openingDiesel,decimal closingDiesel,List<TblDieselFilling> _lstDieselFilling,List<TblDieselLine> _lstDieselLine)
        {
            // Bhari ka KM (Total(TotalRunningKm)  - Total khali ka KM) / Bhari Ka Diesel (Total Consumed - Khali ka Diesel)
            decimal BhariKaAverage = 0;
            try
            {
                decimal sumTotalRunningKm = _lstDieselLine?.Sum(x => x.Distance).To3Decimal() ?? 0;
                decimal sumTotalKhaliKaKm = _lstDieselLine?.Where(x => x.LoadType.ToUpper() == "KHALI").Sum(x => x.Distance).To3Decimal() ?? 0;

                decimal sumTotalDiesel = ((openingDiesel + (_lstDieselFilling?.Sum(x => x.DieselQty).To3Decimal() ?? 0)) - closingDiesel);
                decimal sumTotalKhaliKaDiesel = _lstDieselLine?.Where(x => x.LoadType.ToUpper() == "KHALI").Sum(x => x.EstimatedDiesel).To3Decimal() ?? 0;
                decimal sumTotalLoadingUnloadingDiesel = _lstDieselLine?.Where(x => x.RouteDesc.ToUpper() == "Loading/Unloading".ToUpper()).Sum(x => x.EstimatedDiesel).To3Decimal() ?? 0;

                BhariKaAverage = (sumTotalRunningKm - sumTotalKhaliKaKm) / (sumTotalDiesel - sumTotalKhaliKaDiesel- sumTotalLoadingUnloadingDiesel);
            }
            catch(Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("calBhariKaAverage: {0}", ex.Message.ToString()));
            }
            return BhariKaAverage;
        }

        public static decimal calDiscountValue(decimal discountPer, List<TblDieselLine> _lstDieselLine)
        {
            // Bhari ka KM (Total(TotalRunningKm)  - Total khali ka KM) / Bhari Ka Diesel (Total Consumed - Khali ka Diesel)
            decimal discountValue = 0;
            try
            {
                decimal sumTotalEstimatedDiesel = _lstDieselLine?.Sum(x => x.EstimatedDiesel).To3Decimal() ?? 0;
                if (discountPer>0 && sumTotalEstimatedDiesel>0)
                {
                    discountValue = ((sumTotalEstimatedDiesel * discountPer) / 100);
                }
               
            }
            catch (Exception ex)
            {
                Globalsettings.Log(_controllerName, string.Format("calBhariKaAverage: {0}", ex.Message.ToString()));
            }
            return discountValue;
        }

        public static async Task<List<object>> getRouteMaster(string _connectionString)
        {
            List<object> _lst = new List<object>();
            string sql = @"[dbo].[GetRouteNameMaster]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            _lst.Add(row);
                        }
                    }
                }
            }
            return _lst;
        }

        public static async Task<List<object>> getDriverScore(string _connectionString)
        {
            List<object> _lst = new List<object>();
            string sql = @"[dbo].[GetDriverScore]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var row = new Dictionary<string, object>();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            }
                            _lst.Add(row);
                        }
                    }
                }
            }
            return _lst;
        }
    }
}
