using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Collections;
using XFrame.Core.Threads;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Entities
{
    [XType(typeof(IEntityModule))]
    internal class Scene : ModuleBase, IScene, IFiberUpdate
    {
        private IEntityHelper m_Helper;
        private Fiber m_Fiber;
        private List<IContainer> m_Containers;

        public IEventSystem Event { get; private set; }

        public Fiber Fiber => m_Fiber;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Fiber = data as Fiber;
            m_Fiber.RegisterUpdater(this);
            m_Containers = new List<IContainer>();
            Event = Domain.GetModule<IEventModule>().NewSys();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Domain.GetModule<IEventModule>().Remove(Event);
            m_Fiber.UnRegisterUpdater(this);
            m_Fiber.Dispose();
            m_Fiber = null;
            m_Containers = null;
            Event = null;
        }

        public void SetHelper(IEntityHelper helper)
        {
            m_Helper = helper;
        }

        public IEntity Create(int entityId, EntitySetting setting)
        {
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            setting.ModuleUpdate = false;
            IEntity entity = (IEntity)containerModule.Create(entityId, setting);
            if (m_Helper != null)
                m_Helper.EntityOnCreate(entity);
            return entity;
        }

        public IEntity Create(EntitySetting setting)
        {
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            setting.ModuleUpdate = false;
            IEntity entity = (IEntity)containerModule.Create(setting);
            if (m_Helper != null)
                m_Helper.EntityOnCreate(entity);
            return entity;
        }

        public void Destroy(IEntity entity)
        {
            if (entity == null)
                return;
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            containerModule.Remove(entity);
            m_Helper.EntityOnDestroy(entity);
        }

        public IEntity Get(int entityId)
        {
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            return (IEntity)containerModule.Get(entityId);
        }

        public void OnUpdate(double escapeTime)
        {
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            m_Containers.Clear();
            containerModule.GetAll(m_Containers);
            foreach (IContainer container in m_Containers)
                container.OnUpdate(escapeTime);
        }
    }
}
