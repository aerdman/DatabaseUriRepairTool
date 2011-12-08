using System;

namespace UrlRepairTool.Logger
{
    // If anyone wonders why I didn't just make PrintManualFixInfo public and just pass a string
    // which seems simpler, it is because when debugging through the millions of links sometimes
    // I only wanted to see one kind of link and would turn off the Print statement while debugging.
    // I could turn off all of one kind of message throughout the app this way, even though for
    // some they only happen once. 
    public class ManualFixFileLog : FileLogger
    {
        private static ManualFixFileLog _instance;
        public static ManualFixFileLog Instance
        {
            get
            {
                if (FileName == null)
                    throw new InvalidOperationException("ManualFixLog file name must not be null");

                return _instance ?? (_instance = new ManualFixFileLog());
            }
        }

        private ManualFixFileLog() : base(FileName, LogFileMode)
        {
            InsertInitialHeaderForCsv();
        }

        private void PrintManualFixInfo(string path, string tableName, int index, string reason)
        {
            Print("\"" + path + "\"," +
                  "\"" + tableName + "\"," + 
                  "\"" + index.ToString() + "\"," +
                  "\"" + reason + "\",");
        }

        public void PrintNonAaiNonAventaHttpPathMessage(string path, string tableName, int index)
        {
            PrintManualFixInfo(path, tableName, index, "Non-AAI or Aventa HTTP Path");
        }

        public void PrintNonSupportedSchemeMessage(string path, string tableName, int index)
        {
            PrintManualFixInfo(path, tableName, index, "Non-supported scheme");
        }

        public void PrintUnableToParseToValidUriMessage(string path, string tableName, int index)
        {
            PrintManualFixInfo(path, tableName, index, "Unable to parse to valid URI");
        }

        public void PrintFileNotFoundMessage(string path, string tableName, int index)
        {
            PrintManualFixInfo(path, tableName, index, "file not found");
        }

        public void PrintBadPathOrImproperTerms(string path, string tableName, int index)
        {
            PrintManualFixInfo(path, tableName, index, "Bad path or improper terms");
        }

        public void PrintAventaSavedIncorrectly(string path, string tableName, int index)
        {
            PrintManualFixInfo(path, tableName, index, "Aventa image saved incorrectly");
        }

        public  void PrintAventaPathFound(string path, string tableName, int index)        
        {
            PrintManualFixInfo(path, tableName, index, "Aventa path found.");
        }

        public void InsertInitialHeaderForCsv()
        {
            Print("Path, tableName, Index, Reason");
        }
    }
}
