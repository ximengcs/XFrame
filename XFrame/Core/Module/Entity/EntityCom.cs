using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体组件
    /// </summary>
    public abstract class EntityCom : Entity, ICom
    {
        protected int m_ComId;
        protected object m_Owner;

        public bool Active { get; set; }

        /// <summary>
        /// 组件共享数据,(顶层实体下所有组件共享数据)
        /// </summary>
        public IDataProvider ShareData { get; }

        void ICom.OnInit(IContainer container, int id, object owner, OnContainerReady onReady)
        {
            IEntity entity = this;
            IEntity parent = (IEntity)owner;
            entity.OnInit(IdModule.Inst.Next(), parent, (e) =>
            {
                m_ComId = id;
                m_Owner = owner;
                onReady?.Invoke(this);
            });
        }

        void ICom.OnDestroy()
        {
            m_ComId = default;
            m_Owner = null;
        }

        void ICom.OnUpdate()
        {

        }
    }
}
