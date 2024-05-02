using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 共享组件实体
    /// </summary>
    public class EntityShareCom : ShareCom, IEntityCom
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventSystem Event
        {
            get
            {
                IEntity entity = Master as IEntity;
                return entity.Event;
            }
        }
    }
}
