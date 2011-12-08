using System;
using System.Globalization;
using System.IO;
using System.Linq;
using HtmlAgilityPack;
using UrlRepairTool.Logger;

namespace UrlRepairTool.Parser
{
    public class AnswerOptionParser
    {
        private ErrorLogger _errorLog;
        private ChangedLogger _changedLog;

        public AnswerOptionParser() { SetupLoggers(); }

        // All loggers FileNames and FileModes should be set up prior to this
        public void SetupLoggers()
        {
            _errorLog = ErrorLogger.Instance;
            _changedLog = ChangedLogger.Instance;
        }

        public bool ParseOption(out string option, string input, string tablename, int index)
        {
            var doc = new HtmlDocument();
            bool changed = false;
            option = "";

            try
            {
                doc.LoadHtml(input);
                doc.OptionFixNestedTags = true;

                if (doc.DocumentNode == null) return false;
                var htmlNodes = doc.DocumentNode.SelectNodes("//img | //a");

                if (htmlNodes == null) return false;

                if (doc.ParseErrors != null && doc.ParseErrors.ToList().Count > 0)
                {
                    _errorLog.PrintParseErrorMessage(tablename, index);
                    return false;
                }

                foreach (
                    var node in
                        htmlNodes.Where(
                            node =>
                            (node.Name == "img") ? node.Attributes["src"] != null : node.Attributes["href"] != null))
                {
                    bool success;

                    var oldValue = (node.Name == "img")
                                       ? node.Attributes["src"].Value.ToLower(CultureInfo.CurrentCulture)
                                       : node.Attributes["href"].Value.ToLower(CultureInfo.CurrentCulture);
                    var newValue = (new UriAnalyzer()).AnalyzeUri(oldValue, tablename, index, out success);

                    if (!success) continue;

                    if (node.Name == "img")
                        node.Attributes["src"].Value = newValue;
                    else
                        node.Attributes["href"].Value = newValue;

                    if (oldValue != newValue)
                    {
                        _changedLog.PrintChangedFileMessage(oldValue, newValue, tablename, index);
                        changed = true;
                    }
                }
                if (changed)
                {
                    StringWriter sw  = new StringWriter(); 
                    doc.Save(sw);
                    option = sw.ToString();
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Instance.PrintExceptionMessage(ex);
            }
            return changed;
        }
    }
}
