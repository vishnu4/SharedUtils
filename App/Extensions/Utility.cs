using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SharedUtils.Extensions
{
    public static class Utiliy
    {

        public static string GetDescription(this Enum element)
        {
            Type type = element.GetType();
            MemberInfo[] memberinfo = type.GetTypeInfo().GetMember(element.ToString());
            if (memberinfo != null && memberinfo.Any())
            {
                IEnumerable<System.Attribute> attributes = memberinfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes != null && attributes.Any())
                {
                    return ((DescriptionAttribute)attributes.FirstOrDefault()).Description;
                }
            }
            return element.ToString();
        }

        public static Guid ToGUID(this object guidValue)
        {
            Guid g = Guid.Empty;
            if (guidValue != null && guidValue.ToString().Length > 0)
            {
                if (Guid.TryParse(guidValue.ToString(), out g))
                {
                    //we're good
                }
                else
                {
                    g = Guid.Empty;
                }
            }
            return g;
        }

        #region "Strings"

        public static string ScrubHtml(this string value)
        {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        public static string ToFormattedString(this object stringValue, bool treatZeroAsEmpty)
        {
            if ((stringValue == null) | (object.ReferenceEquals(stringValue, DBNull.Value)))
            {
                return string.Empty;
            }
            else
            {
                if (stringValue.ToString().Trim().Length > 0)
                {
                    //treating zeros as nulls
                    if ((object.ReferenceEquals(stringValue.GetType(), typeof(int))) && stringValue.ToString().Trim() == "0" && treatZeroAsEmpty)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return stringValue.ToString().Trim();
                    }
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public static string ToFormattedString(this object stringValue)
        {
            return ToFormattedString(stringValue, true);
        }
        #endregion

        public static bool IsNumeric(this object value)
        {
            bool isNum = Double.TryParse(Convert.ToString(value), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out double retNum);
            return isNum;
        }

        #region "boolean"

        public static bool? ToNullableBoolean(this object boolValue)
        {
            if ((boolValue != null) && !object.ReferenceEquals(boolValue, DBNull.Value) && boolValue.ToString().Length > 0)
            {
                bool convHappened = bool.TryParse(ToFormattedString(boolValue), out bool ret);
                if (convHappened)
                {
                    return ret;
                }
            }
            return null;
        }

        #endregion

        #region "Double"
        //https://stackoverflow.com/questions/485175/is-it-safe-to-check-floating-point-values-for-equality-to-0-in-c-net
        private const double staticprecision = 0.0000001;
        public static bool AlmostEquals(this double double1, double double2, double precision)
        {
            return (Math.Abs(double1 - double2) <= precision);
        }

        public static bool AlmostEquals(this double double1, double double2)
        {
            return AlmostEquals(double1, double2, staticprecision);
        }

        public static double ToDouble(this object value)
        {
            return ToDouble(value, 0);
        }

        public static double ToDouble(this object value, int defaultValue)
        {
            double? ret = value.ToNullableDouble();
            if (ret.HasValue)
            {
                return ret.Value;
            }
            return defaultValue;
        }

        public static double? ToNullableDouble(this object value)
        {
            return value.ToNullableDouble(System.Globalization.CultureInfo.CurrentCulture);
        }
        public static double? ToNullableDouble(this object value, System.Globalization.CultureInfo culture)
        {
            Nullable<double> theReturn = null;
            if ((value != null) && (IsNumeric(value)))
            {
                //theReturn = CDbl(doubleValue.ToString)
                string doubleStr = null;
                if (((value) is double))
                {
                    doubleStr = ((double)value).ToString(culture);
                }
                else if (((value) is decimal))
                {
                    doubleStr = ((decimal)value).ToString(culture);
                    //ElseIf (TypeOf (doubleValue) Is String) Then
                    //doubleStr = DirectCast(doubleValue, String).ToString(culture)
                }
                else
                {
                    doubleStr = value.ToString();
                }
                if (double.TryParse(doubleStr, System.Globalization.NumberStyles.Any, culture, out double tmp))
                {
                    theReturn = tmp;
                }
            }
            return theReturn;
        }
        
        #endregion

        #region "Integer"
        public static int ToInteger(this object value)
        {
            return ToInteger(value, 0);
        }

        public static int ToInteger(this object value, int defaultValue)
        {
            int? ret = value.ToNullableInteger();
            if (ret.HasValue)
            {
                return ret.Value;
            }
            return defaultValue;
        }

        public static int? ToNullableInteger(this object value)
        {
            return value.ToNullableInteger(System.Globalization.NumberFormatInfo.CurrentInfo);
        }

        public static int? ToNullableInteger(this object intValue, System.Globalization.NumberFormatInfo nmFrmInfo)
        {
            Nullable<int> theReturn = null;
            if (intValue != null)
            {
                //if (IsNumeric(intValue) | intValue.ToString() == "0")
                //{
                //theReturn = CDbl(doubleValue.ToString)

                //first see if it is a float
                if (double.TryParse(intValue.ToString(), System.Globalization.NumberStyles.Any, nmFrmInfo, out double dbl))
                {
                    theReturn = Convert.ToInt32(Math.Floor(dbl));
                    return theReturn;
                }

                if (int.TryParse(intValue.ToString(), System.Globalization.NumberStyles.Any, nmFrmInfo, out int tmp))
                {
                    theReturn = tmp;
                }
                // }
            }
            return theReturn;
        }

        #endregion

        #region "Date"

        public static double ToUnixTimestamp(this DateTime value)
        {
            return value.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        public static DateTime FromUnixTimestamp(this double timestamp,DateTimeKind kind)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, kind);
            return origin.AddSeconds(timestamp);
        }
        public static DateTime FromUnixTimestamp(this double timestamp)
        {
            return timestamp.FromUnixTimestamp(DateTimeKind.Unspecified);
        }

        public static DateTime? ToNullableDateTime(this object value)
        {
            return value.ToNullableDateTime(System.Globalization.CultureInfo.CurrentCulture);
        }
        public static DateTime? ToNullableDateTime(this object value, System.Globalization.CultureInfo cultureType)
        {
            return value.ToNullableDateTime(cultureType, DateTimeKind.Utc);
        }
        public static DateTime? ToNullableDateTime(this object datevalue, System.Globalization.CultureInfo cultureType, DateTimeKind dateKind)
        {
            Nullable<DateTime> theDate = default(Nullable<DateTime>);
            if ((datevalue != null) && (!object.ReferenceEquals(datevalue, DBNull.Value)) && (datevalue.ToString().Length > 0))
            {
                //i want to use assumeuniversal, but that doesn't seem to do anything
                if (DateTime.TryParse(datevalue.ToString(), cultureType, System.Globalization.DateTimeStyles.AdjustToUniversal, out DateTime dDate))
                {
                    if (dDate <= System.DateTime.MinValue | dDate > System.DateTime.MaxValue)
                    {
                        theDate = null;
                    }
                    else if (dDate == Convert.ToDateTime("1/1/0001"))
                    {
                        theDate = null;
                    }
                    else
                    {
                        theDate = DateTime.SpecifyKind(dDate, dateKind);
                    }
                }
                else
                {
                    //try with a format
                    string[] format = { "yyyyMMdd" };
                    if ((DateTime.TryParseExact(datevalue.ToString(), format, cultureType, System.Globalization.DateTimeStyles.None, out dDate)))
                    {
                        theDate = DateTime.SpecifyKind(dDate, dateKind);
                    }
                    else
                    {
                        theDate = null;
                    }

                }
            }
            else
            {
                theDate = null;
            }
            return theDate;
        }

        #endregion

        #region "Files"

        public static void Empty(this System.IO.DirectoryInfo directory)
        {
            foreach (System.IO.FileInfo file in directory.GetFiles()) file.Delete();
            foreach (System.IO.DirectoryInfo subDirectory in directory.GetDirectories()) subDirectory.Delete(true);
        }

        #endregion

    }
}
