using XFrame.Core;
using XFrame.Core.Binder;

namespace XFrame.Modules.Plots
{
    internal interface IPlotDataProvider : IDataProvider
    {
        ValueBinder<bool> Finish { get; set; }

        bool CheckSectionFinish(int index);

        void SetSectionFinish(int index, bool finish);
    }
}
