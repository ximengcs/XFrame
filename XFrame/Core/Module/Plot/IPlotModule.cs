
using XFrame.Core;

namespace XFrame.Modules.Plots
{
    public interface IPlotModule : IModule, IUpdater
    {
        IPlotHelper Helper { get; }

        IStory NewStory(string name = null);
    }
}
