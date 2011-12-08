using UrlRepairTool.Logger;

namespace UrlRepairTool.Domain
{
    class Domain
    {
        protected ManualFixFileLog ManualFixLog;

        public Domain() { SetupLoggers(); }

        public void SetupLoggers()
        {
            ManualFixLog = ManualFixFileLog.Instance;
        }
    }
}
