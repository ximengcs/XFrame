using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Entities
{
    public interface IEntity: IXItem, IPoolObject
    {
        protected internal void OnInit(int id, IScene scene, IEntity parent, EntityData data);
        protected internal void OnUpdate(float elapseTime);
        protected internal void OnDestroy();
    }
}
