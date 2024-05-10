using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using System;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : Container, IEntity
    {
        #region Life Fun
        /// <inheritdoc/>
        protected internal override void OnInit()
        {
            base.OnInit();
            Event = m_Module.Domain.GetModule<IEventModule>().NewSys();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 实体事件系统
        /// </summary>
        public IEventSystem Event { get; private set; }

        public new IEntity Master => ((Container)this).Master as IEntity;

        public new IEntity Parent => ((Container)this).Parent as IEntity;
        #endregion

        protected override IContainer InnerAdd(Type type, int id, OnDataProviderReady onReady)
        {
            return m_Module.Domain.GetModule<IEntityModule>().Create(this, type, id, onReady);
        }

        protected override IContainer InnerAdd(Type type, OnDataProviderReady onReady)
        {
            return m_Module.Domain.GetModule<IEntityModule>().Create(type, this, onReady);
        }
    }
}
