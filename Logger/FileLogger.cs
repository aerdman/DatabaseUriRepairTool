using System;
using System.Diagnostics;
using System.IO;

namespace UrlRepairTool.Logger
{
    public class FileLogger : IDisposable
    {
        protected TextWriterTraceListener Listener { get; set; }

        private ErrorLogger _errorLog;
        protected ErrorLogger ErrorLog { get { return _errorLog; } set { _errorLog = value; } }
        private ManualFixFileLog _manualFixLog;
        protected ManualFixFileLog ManualFixLog { get { return _manualFixLog; } set { _manualFixLog = value; } }
        private ChangedLogger _changedLog;
        protected ChangedLogger ChangedLog { get { return _changedLog; } set { _changedLog = value; } }

        public static string FileName { get; set; }
        public static FileMode LogFileMode { get; set; }

        public FileLogger() { SetupLoggers(); }

        // All loggers FileNames and FileModes should be set up prior to this
        public void SetupLoggers() 
        {
            _errorLog = ErrorLogger.Instance;
            _manualFixLog = ManualFixFileLog.Instance;
            _changedLog = ChangedLogger.Instance;
        }

        public FileLogger(string fileName, FileMode fileMode)
        {
            var fileSteam = new FileStream(fileName, fileMode);
            Listener = new TextWriterTraceListener(fileSteam);
        }

        public void Print(string message)
        {
            Listener.WriteLine(message);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if(Listener != null)
                Listener.Dispose();
        }
    }
}
