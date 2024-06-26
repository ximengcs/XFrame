﻿using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using System;
using XFrame.Core;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : Container, IEntity
    {
        public IScene Scene { get; private set; }

        #region Life Fun
        /// <inheritdoc/>
        protected internal override void OnInit()
        {
            base.OnInit();
            Event = m_Module.Domain.GetModule<IEventModule>().NewSys();
        }

        protected override void OnReadyInit()
        {
            base.OnReadyInit();
            ModuleBase module = m_Module as ModuleBase;
            Scene = (IScene)module.GetUseModule<IEntityModule>();
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
            EntitySetting setting = new EntitySetting(type, this);
            setting.DataProvider = onReady;
            return Scene.Create(id, setting);
        }

        protected override IContainer InnerAdd(Type type, OnDataProviderReady onReady)
        {
            EntitySetting setting = new EntitySetting(type, this);
            setting.DataProvider = onReady;
            return Scene.Create(setting);
        }
    }
}
