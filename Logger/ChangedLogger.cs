using System;

namespace UrlRepairTool.Logger
{
    public class ChangedLogger : FileLogger
    {
        private static ChangedLogger _instance;
        public static ChangedLogger Instance
        {
            get
            {
                if(String.IsNullOrWhiteSpace(FileName))
                    throw new InvalidOperationException("ChangedLogger.FileName must be set");

                return _instance ?? (_instance = new ChangedLogger());
            }
        }

        private ChangedLogger() : base(FileName, LogFileMode) { InsertInitialHeaderForCsv(); }

        public void PrintChangedFileMessage(string oldValue, string newValue, string tableName, int index)
        {
            Print("\"" + oldValue + "\"," + 
                "\"" + newValue + "\"," +
                "\"" + tableName + "\"," +
                "\"" + index.ToString() + "\",");
        }

        public void InsertInitialHeaderForCsv()
        {
            Print("Old Uri, New Uri, Table Name, Index");
        }
    }
}
