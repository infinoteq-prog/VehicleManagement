using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using VMS.Models;

namespace VMS
{
    public static class Extensions
    {

        public static string ToStringFromNull(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return "";
            }
            else if (obj == null)
            {
                return "";
            }
            else
            {
                try
                {
                    return obj.ToString().Trim();
                }
                catch
                {
                    return "";
                }
            }
        }
        public static bool ToboolFromNull(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return false;
            }
            else if (obj == null)
            {
                return false;
            }
            else
            {
                try
                {
                    if ((bool)obj)
                        return true;
                    else
                        return false;
                }
                catch
                {
                    return false;
                }
            }
        }

        public static int ToIntFromNull(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return 0;
            }
            else if (obj == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    return Convert.ToInt32(obj);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static decimal To2Decimal(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return 0;
            }
            else if (obj == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    return Math.Round(Convert.ToDecimal(obj.ToString().Replace("\r", "").Replace("\a", "").Trim()), 2);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static bool ContainsAny(this string haystack, params string[] needles)
        {
            foreach (string needle in needles)
            {
                if (haystack.ToUpper().Contains(needle.ToStringFromNull().ToUpper()))
                    return true;
            }

            return false;
        }

        public static decimal To3Decimal(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return 0;
            }
            else if (obj == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    return Math.Round(Convert.ToDecimal(obj.ToString().Replace("\r", "").Replace("\a", "").Trim()), 3);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static decimal To1Decimal(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return 0;
            }
            else if (obj == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    return Math.Round(Convert.ToDecimal(obj.ToString().Replace("\r", "").Replace("\a", "").Trim()), 1);
                }
                catch
                {
                    return 0;
                }
            }
        }
        public static decimal To2Decimal8(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return 0;
            }
            else if (obj == null)
            {
                return 0;
            }
            else
            {
                try
                {
                    return Math.Round(Convert.ToDecimal(obj.ToString().Replace("\r", "").Replace("\a", "").Trim()), 8);
                }
                catch
                {
                    return 0;
                }
            }
        }

        public static string ToRemoveNull(this object obj)
        {
            if (obj == System.DBNull.Value)
            {
                return "";
            }
            else if (obj == null)
            {
                return "";
            }
            else
            {
                try
                {
                    if (obj.ToString().Trim().ToLower() == "n/a" || obj.ToString().Trim().ToLower() == "tbc" || obj.ToString().Trim().ToLower() == "nil"
                        || obj.ToString().Trim().ToLower() == "var" || obj.ToString().Trim().ToLower() == "na" || obj.ToString().Trim().ToLower() == "calm")
                    {
                        return "";
                    }
                    else
                    {
                        return obj.ToString().Trim();
                    }
                    //return obj.ToString().Trim().Replace("n/a","").Replace("N/A", "").Replace("tbc", "").Replace("TBC", "").Replace("nil", "").Replace("NIL", "").Replace("var", "").Replace("VAR", "").Replace("NA", "").Replace("na", "").Replace("calm", "").Replace("CALM", "");
                }
                catch
                {
                    return "";
                }
            }
        }
    }
    public static class Globalsettings
    {
        public static void Log(string controllerName, string message)
        {
            try
            {
                // Determine the base directory of the application.
                // This is a robust way to get the path where the application assembly is located.
                string basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

                // Define the path for the 'Logs' folder.
                string logFolderPath = Path.Combine(basePath, "Logs");

                // Create the 'Logs' directory if it doesn't already exist.
                // Directory.CreateDirectory handles cases where the directory already exists.
                Directory.CreateDirectory(logFolderPath);

                // Generate the log file name: ControllerName_YYYYMMDD.txt
                // This ensures a new file is created daily for each controller.
                string fileName = $"{controllerName}_{DateTime.Now:yyyyMMdd}.txt";

                // Combine the log folder path with the file name to get the full file path.
                string filePath = Path.Combine(logFolderPath, fileName);

                // Format the log entry with a timestamp (HH:mm:ss.fff for hours, minutes, seconds, milliseconds).
                string logEntry = $"{DateTime.Now:HH:mm:ss.fff} - {message}";

                // Append the formatted log entry to the file.
                // File.AppendAllText creates the file if it doesn't exist, otherwise it appends to it.
                File.AppendAllText(filePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                // In a real application, you would log this exception to a console,
                // an error monitoring system, or a separate critical error log.
                // For simplicity, we are just printing to console here.
                Console.WriteLine($"ERROR: Failed to write log for {controllerName}. Exception: {ex.Message}");
            }
        }
    }
}
