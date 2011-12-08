using System.Configuration;
using System.IO;
using DatabaseUriRepairTool.Options;
using UrlRepairTool.Logger;

namespace DatabaseUriRepairTool
{
    public static class Program
    {
        private static ErrorLogger ErrorLog;
        private static ManualFixFileLog ManualFixLog;
        private static ChangedLogger ChangedLog;

        public static void Main(string[] args)
        {
            SetupLoggers();
            SetupDebugFile();
            (new ProcessOptions()).Process();
        }

        public static void SetupLoggers()
        {
            ChangedLogger.FileName = ConfigurationManager.AppSettings["ChangedLogPath"];
            ChangedLogger.LogFileMode = FileMode.Create;
            ChangedLog = ChangedLogger.Instance;

            ErrorLogger.FileName = ConfigurationManager.AppSettings["ErrorLogPath"];
            ErrorLogger.LogFileMode = FileMode.Create;
            ErrorLog = ErrorLogger.Instance;

            ManualFixFileLog.FileName = ConfigurationManager.AppSettings["ManualFixLogPath"];
            ManualFixFileLog.LogFileMode = FileMode.Create;
            ManualFixLog = ManualFixFileLog.Instance;
        }

        private static void SetupDebugFile()
        {
            if (File.Exists("optionsWritten.txt"))
                File.Delete("optionsWritten.txt");
        }
    }
    

    
}
