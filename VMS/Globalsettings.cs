using DocumentFormat.OpenXml.Office2010.PowerPoint;
using Microsoft.Data.SqlClient;
using System.Data;
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
    }
}
