using System;
using System.Collections.ObjectModel;
using System.Configuration;

namespace UrlRepairTool.Data
{
    public static class ContentServerData
    {
        public static string ContentServerPath
        {
            get
            {
                return ConfigurationManager.AppSettings["RootContentServerPath"];
            }
        }

        public static Uri BaseUri
        {
            get
            {
                var baseUri = new Uri(ConfigurationManager.AppSettings["BaseUri"]);
                return baseUri;
            }
        }

        public static ReadOnlyCollection<string> BaseShares
        {
            get
            {
                var items = ConfigurationManager.AppSettings["ValidShares"].Split(',');

                for(var i = 0; i < items.Length; i++) items[i] = items[i].Trim();

                var readOnlyItems = new ReadOnlyCollection<string>(items);
                return readOnlyItems;
            }
        }
    }
}
