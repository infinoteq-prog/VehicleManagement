using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.Data.SqlClient;
using System.Data;
using VMS.Models;

namespace VMS
{
    public static class DieselHisabContext
    {
        public static async Task<object> GetLastTripHistoryAsync(string _connectionString, int vehicleNo)
        {
            object lastTrip = null;
            string sql = @"
                            SELECT 
                            dh.TripId,
                            CONVERT(VARCHAR, dh.Trip_Start_Date, 105) AS LastTripStartDate,
                            CONVERT(VARCHAR, dh.Trip_End_Date, 105) AS LastTripEndDate,
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
        public static async Task<List<object>> GetDieselFillingListAsync(string _connectionString, int tripId)
        {
            List<object> dieselFillingList = new List<object>();
            string sql = @"
            SELECT CONVERT(VARCHAR, df.Diesel_Filling_Date, 105) AS DieselFillingDate,
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
                                DieselFillingDate = reader.GetString("DieselFillingDate"),
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
                            dl.Route_Desc[RouteDesc],dl.Load_Type[LoadType], cm.Id AS LoadTypeId,dm.Distance,dl.Average,dl.Estimated_Diesel
                        FROM [dbo].[tbl_Diesel_Line] dl
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
                                LoadType = reader.GetString("LoadType"),
                                LoadTypeId = reader.IsDBNull("LoadTypeId") ? (int?)null : reader.GetInt32("LoadTypeId"),
                                Distance = reader.IsDBNull("Distance") ? (decimal?)null : reader.GetInt32("Distance"),
                                Average = reader.IsDBNull("Average") ? (decimal?)null : reader.GetDecimal("Average"), 
                                EstimatedDiesel = reader.IsDBNull("Estimated_Diesel") ? (decimal?)null : reader.GetDecimal("Estimated_Diesel"), 
                            });
                        }
                    }
                }
            }
            return stationList;
        }
        public static async Task<object> GetDieselAverage(string _connectionString, int vehicleNo, string loadType)
        {
            string sql = "";
            string columnName = "";

            switch (loadType?.ToUpperInvariant())
            {
                case "UL":
                    columnName = "Ul_Avg";
                    break;
                case "KHALI":
                    columnName = "Khali";
                    break;
                case "NH":
                    columnName = "Nh";
                    break;
                case "MG":
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
                CONVERT(VARCHAR, dh.Trip_Start_Date, 105) AS LastTripStartDate,
                CONVERT(VARCHAR, dh.Trip_End_Date, 105) AS LastTripEndDate,
                dh.Last_Trip_Route_Descr,
                ISNULL(dh.End_Odometer, 1) AS EndOdometer,
                ISNULL(dh.Opening_Diesel, 1) AS OpeningDiesel,
                ISNULL(dh.Closing_Diesel, 1) AS ClosingDiesel,
                vm.Vehicle_No AS VehicleNumber,
                dm.Driver_Name AS LastTripDriver,
                dm.Father_Name AS LastTripDriverFatherName
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
                                LastTripRouteDescr = reader.GetString("Last_Trip_Route_Descr"),
                                EndOdometer = reader.GetInt64("EndOdometer"),
                                OpeningDiesel = reader.GetInt64("OpeningDiesel"),
                                ClosingDiesel = reader.GetInt64("ClosingDiesel"),
                                LastTripVendor = "Test Vendor",
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
                                CreatedBy = reader.IsDBNull("DieselHeaderCreatedByName") ? "" : reader.GetString("DieselHeaderCreatedByName"),
                                UpdatedBy = reader.IsDBNull("DieselHeaderUpdatedByName") ? "" : reader.GetString("DieselHeaderUpdatedByName"),
                                LastTripRouteDescr = reader.GetString("LastTripRouteDescr"),
                                StartOdometer = reader.GetInt64("Start_Odometer"),
                                EndOdometer = reader.GetInt64("End_Odometer"),
                                OpeningDiesel = reader.GetInt64("Opening_Diesel"),
                                RunningKm = reader.IsDBNull("RunningKm") ? (decimal?)null : reader.GetInt32("RunningKm"),
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
    }
}
