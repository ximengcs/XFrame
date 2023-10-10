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
        public IEventSystem Event { get; private set; }

        protected internal override void OnInit()
        {
            base.OnInit();
            Event = XModule.Event.NewSys();
        }
    }
}
