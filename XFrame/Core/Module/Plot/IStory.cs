
using System;

namespace XFrame.Modules.Plots
{
    public interface IStory
    {
        string Name { get; }
        bool IsFinish { get; }
        IStory AddSection(Type type);
        internal void OnInit(PlotDataProvider data);
        internal void OnStart();
        internal void OnUpdate();
        internal void OnDestroy();
    }
}
