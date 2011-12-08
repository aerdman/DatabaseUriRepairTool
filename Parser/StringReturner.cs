
namespace UrlRepairTool.Parser
{
    class StringReturner
    {
        private StringReturner() { }

        public static string ReturnString(string initialString, string newString, bool success)
        {
            return success ? newString : initialString;
        }
    }
}
