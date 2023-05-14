using XFrame.Modules.Event;
using XFrame.Modules.Containers;

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
            Event = EventModule.Inst.NewSys();
        }
    }
}
