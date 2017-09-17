using SharedUtils.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedUtils.ExceptionHandling
{
    public abstract class baseExceptionHandler : IExceptionHandler
    {
        public void HandleException(Exception ex)
        {
            HandleException(ex, null);
        }

        public abstract void HandleException(Exception ex, IDictionary<string, string> parameters);

        protected string GetErrorString(Exception theError)
        {
            return GetErrorString(theError, null);
        }
        protected string GetErrorString(Exception theError, IDictionary<string, string> extraData)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine(theError.Message);
            sb.AppendLine(" ");
            if ((extraData != null) && (extraData.Count > 0))
            {
                foreach (KeyValuePair<string, string> kv in extraData)
                {
                    sb.AppendLine(kv.Key + " = " + kv.Value);
                }
            }
            sb.AppendLine("Type = " + theError.GetType().ToString());
            sb.AppendLine("");
            if (!string.IsNullOrEmpty(theError.Source))
            {
                sb.AppendLine(nameof(theError.Source) + "= " + theError.Source);
            }
            if (!string.IsNullOrEmpty(theError.HelpLink))
            {
                sb.AppendLine(nameof(theError.HelpLink) + "= " + theError.HelpLink);
            }

            if (!string.IsNullOrEmpty(theError.StackTrace))
            {
                sb.AppendLine(" ");
                sb.AppendLine(theError.StackTrace);
                sb.AppendLine("*******************");
            }

            sb.AppendLine("");
            //Here we can add some specified error type functionality
            switch (theError)
            {
                case System.Net.WebException wEx:
                    wEx = (System.Net.WebException)theError;
                    sb.AppendLine("Status = " + wEx.Status.ToString());
                    if (wEx.Response != null)
                    {
                        if (wEx.Response.ResponseUri != null)
                        {
                            sb.AppendLine("Response.ResponseUri = " + wEx.Response.ResponseUri.ToString());
                        }
                    }
                    break;
                case System.Reflection.ReflectionTypeLoadException rEx:
                    if ((rEx != null) && (rEx.LoaderExceptions != null) && rEx.LoaderExceptions.Length > 0)
                    {
                        foreach (Exception a in rEx.LoaderExceptions)
                        {
                            string errorString = GetErrorString(a);
                            sb.Append(errorString);
                        }
                    }
                    break;
                case System.IO.FileNotFoundException fEx:
                    fEx = (FileNotFoundException)theError;
                    sb.AppendLine("File Not found is = " + fEx.FileName);
                    break;
                case System.Xml.XmlException xEx:
                    xEx = (System.Xml.XmlException)theError;
                    sb.AppendLine("Line Number = " + xEx.LineNumber.ToString(System.Globalization.CultureInfo.CurrentCulture));
                    sb.AppendLine("Line Position = " + xEx.LinePosition.ToString(System.Globalization.CultureInfo.CurrentCulture));
                    break;
                case System.ArgumentOutOfRangeException aEx:
                    aEx = (System.ArgumentOutOfRangeException)theError;
                    sb.AppendLine("Name of the parameter that caused the exception = " + aEx.ParamName);
                    if ((aEx.ActualValue != null))
                    {
                        sb.AppendLine("Value that caused the error = " + aEx.ActualValue.ToString());
                    }
                    break;
                default:
                    break;
                    //do nothing
            }
            sb.AppendLine("*******************");

            if ((theError.InnerException != null))
            {
                sb.AppendLine("Inner Exception details");
                sb.AppendLine("");
                string errorString = GetErrorString(theError.InnerException);
                sb.Append(errorString);
            }

            return sb.ToString();

        }

    }



}
