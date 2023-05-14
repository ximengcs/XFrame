using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 共享组件实体
    /// </summary>
    public class EntityShareCom : ShareCom, IEntityCom
    {
        public IEventSystem Event
        {
            get
            {
                IEntity entity = ((ICom)this).Owner as IEntity;
                return entity.Event;
            }
        }
    }
}
