using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 共享组件实体
    /// </summary>
    public class ShareEntity : ShareContainer, IEntity
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        public IEventSystem Event => Master.Event;

        public new IEntity Master => ((ShareContainer)this).Master as IEntity;

        public new IEntity Parent => ((ShareContainer)this).Parent as IEntity;
    }
}
