using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.Data.SqlClient;
using System.Data;
using VMS.Models;

namespace VMS
{
    public static class ServiceDueMasterContext
    {
        public static async Task<List<object>> searchServiceDueMaster(string _connectionString, int id, int vehicleNo,  string dueDateFrom, string dueDateTo)
        {
            List<object> serviceDue = new List<object>();
            string sql = @"[dbo].[ServiceDue_List]";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@VehicleNo", vehicleNo);
                    command.Parameters.AddWithValue("@DueDateFrom", string.IsNullOrEmpty(dueDateFrom) ? (object)DBNull.Value : dueDateFrom);
                    command.Parameters.AddWithValue("@DueDateTo", string.IsNullOrEmpty(dueDateTo) ? (object)DBNull.Value : dueDateTo);


                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            serviceDue.Add(new 
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                                VehicleNumber = reader.IsDBNull(reader.GetOrdinal("VehicleNumber")) ? "N/A" : reader.GetString(reader.GetOrdinal("VehicleNumber")),

                                PurchaseDate = reader.IsDBNull(reader.GetOrdinal("Purchase_Date")) ? string.Empty : reader.GetDateTime(reader.GetOrdinal("Purchase_Date")).ToString("yyyy-MM-dd"),
                                ServiceCode = reader.IsDBNull(reader.GetOrdinal("Service_Code")) ? string.Empty : reader.GetString(reader.GetOrdinal("Service_Code")),
                                IntervalKm = reader.GetInt32(reader.GetOrdinal("Interval_Km")),
                                IntervalMonth = reader.GetInt32(reader.GetOrdinal("Interval_Month")),
                                DueDate = reader.IsDBNull(reader.GetOrdinal("Due_Date")) ? string.Empty : reader.GetDateTime(reader.GetOrdinal("Due_Date")).ToString("yyyy-MM-dd"),
                                PartCost = reader.GetDecimal(reader.GetOrdinal("Parts_Cost")),
                                LabourCost = reader.GetDecimal(reader.GetOrdinal("Labour_Cost")),
                                TotalCost = reader.GetDecimal(reader.GetOrdinal("Total_Cost")),
                                Workshop = reader.IsDBNull(reader.GetOrdinal("Workshop")) ? string.Empty : reader.GetString(reader.GetOrdinal("Workshop")),
                                Remarks = reader.IsDBNull(reader.GetOrdinal("Remarks")) ? string.Empty : reader.GetString(reader.GetOrdinal("Remarks")),
                                IsActive = reader.GetBoolean(reader.GetOrdinal("Is_Active")),
                                CreationDate = reader.IsDBNull(reader.GetOrdinal("Creation_Date")) ? string.Empty : reader.GetDateTime(reader.GetOrdinal("Creation_Date")).ToString("yyyy-MM-dd HH:mm:ss"),
                                CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedByUserName")) ? "N/A" : reader.GetString(reader.GetOrdinal("CreatedByUserName")),
                                UpdateDate = reader.IsDBNull(reader.GetOrdinal("Update_Date")) ? string.Empty : reader.GetDateTime(reader.GetOrdinal("Update_Date")).ToString("yyyy-MM-dd HH:mm:ss"),
                                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedByUserName")) ? "N/A" : reader.GetString(reader.GetOrdinal("UpdatedByUserName"))
                            });
                        }
                    }
                }
            }
            return serviceDue;
        }

    }
}
