using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.Data.SqlClient;
using System.Data;
using VMS.Models;

namespace VMS
{
    public static class DriverHisabContext
    {
        public static async Task<object> GetLastTripHistoryBySettlementNoAsync(string _connectionString, int SettlementNo)
        {
            object lastTrip = null;
            string sql = @" SELECT 
                             dh.Settlement_No,
                             CONVERT(VARCHAR, dh.Trip_Start_Date, 105) AS LastTripStartDate,
                             CONVERT(VARCHAR, dh.Trip_End_Date, 105) AS LastTripEndDate,
                             dh.Route_Description,
                             dh.Opening_Balance,
                             vm.Vehicle_No AS VehicleNumber,
                             dm.Driver_Name AS LastTripDriver,
                             dm.Father_Name AS LastTripDriverFatherName
                             FROM [dbo].[tbl_Driver_Hisab_Header]dh
                             INNER JOIN [dbo].[tbl_Vehicle_Master] vm ON dh.Vehicle_No = vm.Id
                            LEFT JOIN [dbo].[tbl_Driver_Master] dm ON dh.Driver_Id = dm.Id
                            WHERE dh.Settlement_No = @SettlementNo
                             ORDER BY dh.Settlement_No DESC;"; // Skip the current trip
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@SettlementNo", SettlementNo);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            lastTrip = new
                            {
                                SettlementNo = reader.GetInt32("Settlement_No").ToIntFromNull(),
                                LastTripStartDate = reader.IsDBNull("LastTripStartDate") ? null : reader.GetString("LastTripStartDate"),
                                LastTripEndDate = reader.IsDBNull("LastTripEndDate") ? null : reader.GetString("LastTripEndDate"),
                                LastTripRouteDescr = reader.GetString("Route_Description"),
                                OpeningBalance = reader.GetDecimal("Opening_Balance") == 0 ? 1 : reader.GetDecimal("Opening_Balance"),
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

        public static async Task<List<object>> GetExpenseListAsync(string _connectionString, int settlementId)
        {
            List<object> stationList = new List<object>();
            string sql = @"select dh.Expense_Code,cm.Code_Type,cm.Code,
                            cm.Description,dh.Dr_amt,dh.Cr_Amt 
                            from [dbo].[tbl_Driver_Hisab_Lines] dh
                            inner join [dbo].[tbl_Code_Master] cm on dh.Expense_Code=cm.ID
                            where dh.Settlement_No=@settlementId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@settlementId", settlementId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            stationList.Add(new
                            {
                                SettlementNo = settlementId,
                                ExpenseCode = reader.GetString("Expense_Code"),
                                ExpenseType = reader.GetString("Code_Type"),
                                Code = reader.GetString("Code"),
                                Description = reader.GetString("Description"),
                                DrAmt = reader.GetDecimal("Dr_amt"),
                                CrAmt = reader.GetDecimal("Cr_Amt"),
                            });
                        }
                    }
                }
            }
            return stationList;
        }
    }
}
