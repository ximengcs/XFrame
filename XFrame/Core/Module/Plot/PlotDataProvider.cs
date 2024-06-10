
using XFrame.Core;
using XFrame.Core.Binder;

namespace XFrame.Modules.Plots
{
    internal class PlotDataProvider : DataProvider, IPlotDataProvider
    {
        private bool m_Finish;
        public ValueBinder<bool> Finish { get; set; }

        public PlotDataProvider()
        {
            Finish = new ValueBinder<bool>(() => m_Finish, (v) => m_Finish = v);
        }

        public bool CheckSectionFinish(int index)
        {
            string key = $"section{index}";
            return GetData<bool>(key);
        }

        public void SetSectionFinish(int index, bool finish)
        {
            string key = $"section{index}";
            SetData(key, finish);
        }
    }
}
