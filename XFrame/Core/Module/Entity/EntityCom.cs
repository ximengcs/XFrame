using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Core;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体组件
    /// </summary>
    public abstract class EntityCom : Com, IEntityCom
    {
        /// <summary>
        /// 时间系统
        /// </summary>
        public IEventSystem Event { get; private set; }

        /// <inheritdoc/>
        protected internal override void OnInit()
        {
            base.OnInit();
            Event = m_Module.Domain.GetModule<IEventModule>().NewSys();
        }
    }
}
