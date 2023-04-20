using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : Container, IEntity
    {
        #region Inner Field
        private Entity m_Parent;
        private IEventSystem m_EventSys;
        #endregion

        #region Life Fun
        void IEntity.OnInit(int id, IEntity parent, OnEntityReady onReady)
        {
            object master;
            if (parent != null)
            {
                m_Parent = (Entity)parent;
                master = m_Parent.Master;
            }
            else
            {
                m_Parent = null;
                master = this;
            }

            m_EventSys = EventModule.Inst.NewSys();

            IContainer thisContainer = this;
            thisContainer.OnInit(id, master, (c) => onReady?.Invoke(this));
        }
        #endregion

        #region Interface
        /// <summary>
        /// 实体事件系统
        /// </summary>
        public IEventSystem Event => m_EventSys;

        /// <summary>
        /// 实体父节点
        /// </summary>
        public Entity Parent
        {
            get => m_Parent;
            set
            {
                if (m_Parent != value)
                {
                    EntityParentChangeEvent e = new EntityParentChangeEvent(m_Parent, value);
                    m_Parent = value;
                    m_EventSys.Trigger(e);
                }
            }
        }
        #endregion
    }
}
