
using XFrame.Collections;

namespace XFrame.Modules.Entities
{
    public interface IEntity: IXItem
    {
        protected internal void OnInit(int id, IScene scene, IEntity parent, EntityData data);
        protected internal void OnUpdate(float elapseTime);
        protected internal void OnDestroy();
    }
}
