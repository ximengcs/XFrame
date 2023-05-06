using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : Container, IEntity
    {
        #region Inner Field
        private IEntity m_Parent;
        private IEventSystem m_EventSys;
        #endregion

        #region Life Fun
        protected override void InnerInitCom(ICom com, int id, OnComReady onReady)
        {
            IEntityCom entityCom = com as IEntityCom;
            if (entityCom == null)
            {
                Log.Error("XFrame", "EntityCom type error");
                return;
            }

            entityCom.OnInit(id, this, (c) => onReady?.Invoke(c));
        }

        void IEntity.OnInit(int id, IEntity parent, OnEntityReady onReady)
        {
            m_EventSys = EventModule.Inst.NewSys();
            object master = InnerCheckOwner(parent);
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
        public IEntity Parent
        {
            get => m_Parent;
            set
            {
                if (m_Parent != value)
                {
                    m_Parent = value;
                    Master = InnerCheckOwner(value);
                    EntityParentChangeEvent e = new EntityParentChangeEvent(m_Parent, value);
                    m_EventSys.Trigger(e);
                }
            }
        }
        #endregion

        #region Inner Implement
        private object InnerCheckOwner(IEntity parent)
        {
            object master;
            if (parent != null)
            {
                m_Parent = parent;
                master = parent.Master;
            }
            else
            {
                m_Parent = null;
                master = this;
            }

            return master;
        }
        #endregion
    }
}
