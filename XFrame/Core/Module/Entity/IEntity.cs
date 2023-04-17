using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    public interface IEntity: IContainer, IXItem, IPoolObject
    {
        protected internal void OnInit(int id, IEntity parent, OnEntityReady onData);
        protected internal void OnUpdate(float elapseTime);
        protected internal void OnDestroy();
    }
}
