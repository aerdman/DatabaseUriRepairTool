using System.Data;
using UrlRepairTool.Parser;

namespace DatabaseUriRepairTool.Options
{
    public class MultipleChoiceAnswerOption : AnswerOptionTable
    {
        public MultipleChoiceAnswerOption(IDbConnection connection, AnswerOptionParser parser)
            : base(connection, parser)
        {
            TableName = "EDU_MultipleChoiceAnswerOption";
            IndexLabel = "MultipleChoiceAnswerOptionID";
            TextLabel = "OptionText";
        }
    }
}
