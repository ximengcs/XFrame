using System;
using XFrame.Core;
using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    public interface IPlotModule : IModule, IUpdater
    {
        IEventSystem Event { get; }

        IPlotHelper Helper { get; }

        IStory NewStory(Type targetDirector, Type helperType, string name = null);
        IStory NewStory(Type targetDirector, string name = null);
    }
}
