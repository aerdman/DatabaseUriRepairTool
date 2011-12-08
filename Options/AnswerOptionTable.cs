using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using Dapper;
using UrlRepairTool.Parser;

namespace DatabaseUriRepairTool.Options
{
    public class AnswerOptionTable
    {
        public string TableName { get; set; }
        public string IndexLabel { get; set; }
        public string TextLabel { get; set; }

        public List<int> IdList
        {
            get { return m_IdList; }
        }

        private readonly List<int> m_IdList = new List<int>();
        private IDbConnection m_connection;
        private AnswerOptionParser m_parser;

        public AnswerOptionTable(IDbConnection connection, AnswerOptionParser parser)
        {
            m_connection = connection;
            m_parser = parser;
        }

        public void GetStreams()
        {
            const int increment = 1000;
            int retval,
                bottom = 0,
                top = bottom + increment;
            StringBuilder sb = new StringBuilder();

            do
            {
                var query =
                    sb.Append("SELECT ").Append(IndexLabel).Append(" as ID,").Append(TextLabel).Append(" as Text FROM ")
                        .Append(TableName).Append(" WHERE ").Append(IndexLabel).Append(" between @lowerId and @upperId").ToString();
                var indexes = m_connection.Query<AnswerOptionRec>(query, new { lowerId = bottom, upperId = top }, null, false).ToList();
                ParseStreams(indexes);
                Console.WriteLine(String.Format("{0} - {1} {2}", TableName, IndexLabel, top));
                sb.Clear();

                retval = indexes.Count;
                bottom += increment;
                top += increment;
            } while (retval > 0);
        }

        public void UpdateStream(string text, int ID)
        {
            // apostrophes must be escaped
            text = text.Replace("'", "''");
#if DEBUG
            debugFileLog(text, ID);
#else
            StringBuilder sb = new StringBuilder();
            m_connection.Execute(
                sb.Append("UPDATE ").Append(TableName).Append(" SET DateModified=GETDATE(), ").Append(TextLabel).Append("='")
                .Append(text).Append("' WHERE ").Append(IndexLabel).Append("=@ID").ToString(), new {@ID = ID});
#endif
        }

        public void ParseStreams(List<AnswerOptionRec> indexes)
        {
            foreach (AnswerOptionRec rec in indexes)
            {
                rec.TableName = TableName;
                string option;

                if (m_parser.ParseOption(out option, rec.Text, rec.TableName, rec.ID))
                {
                    UpdateStream(option, rec.ID);
                }
            }
        }

        private void debugFileLog(string text, int id)
        {
            using (var sw = new StreamWriter("optionsWritten.txt", true))
            {
                sw.WriteLine(String.Format("{0} - {1}: {2}", TableName, id, text));
            }
        }
    }
}
