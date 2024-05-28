using XFrame.Core;
using XFrame.Modules.Event;
using XFrame.Modules.Containers;
using XFrame.Collections;

namespace XFrame.Modules.Entities
{
    [XType(typeof(IEntityModule))]
    internal class Scene : ModuleBase, IScene
    {
        private IEntityHelper m_Helper;

        public IEventSystem Event { get; private set; }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            Event = Domain.GetModule<IEventModule>().NewSys();
        }

        public void SetHelper(IEntityHelper helper)
        {
            m_Helper = helper;
        }

        public IEntity Create(int entityId, EntitySetting setting)
        {
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            setting.DataProvider = InnerSetEntityScene;
            IEntity entity = (IEntity)containerModule.Create(entityId, setting);
            if (m_Helper != null)
                m_Helper.EntityOnCreate(entity);
            return entity;
        }

        public IEntity Create(EntitySetting setting)
        {
            IContainerModule containerModule = GetUseModule<IContainerModule>();
            setting.DataProvider = InnerSetEntityScene;
            IEntity entity = (IEntity)containerModule.Create(setting);
            if (m_Helper != null)
                m_Helper.EntityOnCreate(entity);
            return entity;
        }

        private void InnerSetEntityScene(IDataProvider entity)
        {
            entity.SetData<IScene>(this);
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
    }
}
