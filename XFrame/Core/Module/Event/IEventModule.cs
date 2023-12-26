
using XFrame.Core;

namespace XFrame.Modules.Event
{
    public interface IEventModule : IModule
    {
        IEventSystem NewSys();

        void Remove(IEventSystem evtSys);
    }
}
