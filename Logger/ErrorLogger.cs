using System;

namespace UrlRepairTool.Logger
{
    public class ErrorLogger : FileLogger
    {
        private static ErrorLogger _instance;
        public static ErrorLogger Instance
        {
            get
            {
                if (FileName == null)
                    throw new InvalidOperationException("ErrorLog filename must be set");

                return _instance ?? (_instance = new ErrorLogger());
            }
        }

        private ErrorLogger() : base(FileName, LogFileMode) {}

        public void PrintParseErrorMessage(string tableName, int index) 
        {
            Print("Parse error in: " + tableName + ": " + index.ToString());
        }

        public void PrintExceptionMessage(Exception ex)
        {
            if (ex.InnerException != null)
                Print("Exception occurred: " + ex.Message + " Inner exception: " + ex.InnerException.Message);
            else
                Print("Exception occurred: " + ex.Message);
        }
    }
}
