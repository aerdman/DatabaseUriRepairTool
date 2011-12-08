using System;
using System.Globalization;
using UrlRepairTool.Data;
using UrlRepairTool.Domain;
using UrlRepairTool.Logger;

namespace UrlRepairTool.Parser
{
    class UriAnalyzer
    {
        protected ManualFixFileLog ManualFixLog;

        public UriAnalyzer() { SetupLoggers(); }

        // Loggers should already be initialized by this point
        public void SetupLoggers()
        {
            ManualFixLog = ManualFixFileLog.Instance;
        }

        public string AnalyzeUri(string uriString, string tableName, int index, out bool success)
        {
            if (!String.IsNullOrWhiteSpace(uriString) && Uri.IsWellFormedUriString(uriString, UriKind.RelativeOrAbsolute))
            {
                Uri uri;
                if (Uri.TryCreate(uriString, UriKind.Absolute, out uri))
                {
                    var result = AnalyzeAbsoluteUri(uri, tableName, index, out success);
                    return StringReturner.ReturnString(uriString, result, success);
                }
                else
                {
                    var result = AnalyzeRelativeUri(uri, uriString, tableName, index, out success);
                    return StringReturner.ReturnString(uriString, result, success);
                }
            }

            success = false;
            ManualFixLog.PrintUnableToParseToValidUriMessage(uriString, tableName, index);

            return uriString;
        }

        string AnalyzeAbsoluteUri(Uri uri, string tableName, int index, out bool success)
        {
            if (uri.Scheme.ToLower(CultureInfo.CurrentCulture) == "http")
            {
                if (DomainUtility.IsValidDomainFrom(uri, "advancedacademics.com"))
                {
                    var newUrl = (new AaiDomain()).AnalyzeAndRepair(uri, tableName, index, out success);
                    return StringReturner.ReturnString(uri.OriginalString, newUrl, success);
                }
                if (DomainUtility.IsValidDomainFrom(uri, "aventalearning.com"))
                {
                    //var newUrl = (new AventaDomain()).AnalyzeAndRepair(uri, tableName, index, out success);
                    //return StringReturner.ReturnString(uri.OriginalString, newUrl, success);
                    ManualFixLog.PrintAventaPathFound(uri.OriginalString, tableName, index);
                }

                success = false;
                ManualFixLog.PrintNonAaiNonAventaHttpPathMessage(uri.OriginalString, tableName, index);
                return uri.OriginalString;
            }

            success = false;
            ManualFixLog.PrintNonSupportedSchemeMessage(uri.OriginalString, tableName, index);
            return uri.OriginalString;
        }

        string AnalyzeRelativeUri(Uri uri, string originalString, string tableName, int index, out bool success)
        {
            //// Get base URI (if starts with / it might be a share, if not try from
            //// parent file path without last leaf
            //var baseUri = originalString.StartsWith(@"/", StringComparison.CurrentCulture) ? ContentServerData.BaseUri : new Uri(parentFileName.Substring(0, parentFileName.LastIndexOf("\\", StringComparison.CurrentCulture)));

            // Combine relative URI with base. If it fails, log for manual fix and return
            if (Uri.TryCreate(ContentServerData.BaseUri, new Uri(originalString, UriKind.Relative), out uri))
            {
                var newUrl = (new AaiDomain()).AnalyzeAndRepair(uri, tableName, index, out success);
                return StringReturner.ReturnString(originalString, newUrl, success);
            }

            success = false;
            ManualFixLog.PrintUnableToParseToValidUriMessage(originalString, tableName, index);
            return originalString;
        }
    }
}
