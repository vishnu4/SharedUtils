//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Security;
//using System.Text;
//using System.Threading.Tasks;

//namespace SharedUtils.ExceptionHandling
//{
//    public class EventExceptionHandler : baseExceptionHandler
//    {
//        public override void HandleException(Exception ex, IDictionary<string, string> parameters)
//        {
//            WriteToEventLog(ex, System.Diagnostics.EventLogEntryType.Error, "HAL");
//        }


//        private void WriteToEventLog(Exception theError, System.Diagnostics.EventLogEntryType errType, string appName)
//        {
//            try
//            {
//                PermissionSet permissionSet = new PermissionSet(System.Security.Permissions.PermissionState.None);
//                permissionSet.AddPermission(new EventLogPermission(System.Security.Permissions.PermissionState.Unrestricted));
//                bool isGranted = permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
//                if (isGranted)
//                {
//                    //*************************************
//                    //If SecurityManager.IsGranted(New EventLogPermission(Permissions.PermissionState.Unrestricted)) Then
//                    if (!EventLog.SourceExists(appName))
//                    {
//                        EventLog.CreateEventSource(appName, "Application");
//                    }
//                    EventLog.WriteEntry(appName, GetErrorString(theError), errType);

//                    //Dim errLog As New EventLog("Application")
//                    //errLog.Source = My.Application.Info.ProductName
//                    //errLog.WriteEntry(GetErrorString(theError), errType)
//                }
//            }
//            catch //(Exception ex)
//            {
//                //do nothing, can't write to event log
//            }
//        }

//    }
//}
