
using XFrame.Core;

namespace XFrame.Modules.Event
{
    public interface IEventModule : IModule, IUpdater
    {
        IEventSystem NewSys();

        void Remove(IEventSystem evtSys);
    }
}
