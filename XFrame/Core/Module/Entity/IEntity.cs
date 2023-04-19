using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    public interface IEntity: IContainer
    {
        IEventSystem Event { get; }

        protected internal void OnInit(int id, IEntity parent, OnEntityReady onData);
    }
}
