using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using UrlRepairTool.Parser;

namespace DatabaseUriRepairTool.Options
{
    public class ProcessOptions
    {
        public void Process()
        {
            var parser = new AnswerOptionParser();

            using (IDbConnection con = GetConnection())
            {
                ProcessTables(con, parser);
            }
        }

        private void ProcessTables(IDbConnection con, AnswerOptionParser parser)
        {
            (new MultipleChoiceAnswerOption(con, parser)).GetStreams();
        }

        private IDbConnection GetConnection()
        {
            var con = new SqlConnection() { ConnectionString = ConfigurationManager.AppSettings["DBConn"] };
            con.Open();
            return con;
        }
    }
}
