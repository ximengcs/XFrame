using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : Container, IEntity
    {
        #region Life Fun
        protected internal override void OnInit()
        {
            base.OnInit();
            Event = EventModule.Inst.NewSys();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 实体事件系统
        /// </summary>
        public IEventSystem Event { get; private set; }
        #endregion
    }
}
