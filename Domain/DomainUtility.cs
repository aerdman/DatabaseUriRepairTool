using System;
using System.Globalization;

namespace UrlRepairTool.Domain
{
    class DomainUtility
    {
        private DomainUtility() {}

        public static bool IsValidDomainFrom(Uri uri, string domainName)
        {
            return uri.Host.ToLower(CultureInfo.CurrentCulture).Contains(domainName);
        }
    }
}
