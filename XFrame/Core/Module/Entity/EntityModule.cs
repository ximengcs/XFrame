using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Reflection;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Containers;
using XFrame.Modules.Event;

namespace XFrame.Modules.Entities
{
    /// <inheritdoc/>
    [CoreModule]
    [XType(typeof(IEntityModule))]
    public class EntityModule : ModuleBase, IEntityModule
    {
        #region Inner Field
        private IEventSystem m_Event;
        private XCollection<IEntity> m_Entities;
        #endregion

        #region Life Fun
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Entities = new XCollection<IEntity>(Domain);
            m_Event = Domain.GetModule<IEventModule>().NewSys();
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_Entities.Clear();
        }
        #endregion

        #region Interface
        public IEventSystem Event => m_Event;

        /// <inheritdoc/>
        public void RegisterEntity<T>() where T : class, IEntity
        {
            TypeSystem module = Domain.TypeModule
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>();

            foreach (Type type in module)
            {
                EntityPropAttribute atr = Domain.TypeModule.GetAttribute<EntityPropAttribute>(type);
                if (atr != null)
                    module.AddKey(atr.Type, type);
            }
        }

        /// <inheritdoc/>
        public IEntity Get(int id)
        {
            return (IEntity)Domain.GetModule<IContainerModule>().Get(id);
        }

        public IEntity Create(Type entityType, int entityId, int typeId, OnDataProviderReady onReady = null)
        {
            return InnerCreate(entityType, entityId, typeId, onReady);
        }

        /// <inheritdoc/>
        public T Create<T>(OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return InnerCreate(typeof(T), default, onReady) as T;
        }

        /// <inheritdoc/>
        public IEntity Create(Type type, OnDataProviderReady onReady = null)
        {
            return InnerCreate(type, default, onReady);
        }

        /// <inheritdoc/>
        public T Create<T>(IEntity parent, OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return InnerCreate(typeof(T), parent, onReady) as T;
        }

        /// <inheritdoc/>
        public IEntity Create(Type type, IEntity parent, OnDataProviderReady onReady = null)
        {
            return InnerCreate(type, parent, onReady);
        }

        /// <inheritdoc/>
        public T Create<T>(int typeId, OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return (T)Create(typeof(T), typeId, onReady);
        }

        /// <inheritdoc/>
        public IEntity Create(Type baseType, int typeId, OnDataProviderReady onReady = null)
        {
            Type type = Domain.TypeModule
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub(baseType)
                .GetKey(typeId);
            if (type == null)
            {
                Log.Debug(Log.XFrame, $"Entity {baseType.Name} not register.");
                return default;
            }
            return InnerCreate(type, default, onReady);
        }

        /// <inheritdoc/>
        public T Create<T>(IEntity parent, int typeId, OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return (T)Create(typeof(T), parent, typeId, onReady);
        }

        /// <inheritdoc/>
        public IEntity Create(Type baseType, IEntity parent, int typeId, OnDataProviderReady onReady = null)
        {
            Type type = Domain.TypeModule
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub(baseType)
                .GetKey(typeId);
            return InnerCreate(type, parent, onReady);
        }

        /// <inheritdoc/>
        public void Destroy(IEntity entity)
        {
            if (entity == null)
                return;

            Domain.GetModule<IContainerModule>().Remove(entity);
            m_Entities.Remove(entity);
            Event.TriggerNow(EntityDestroyEvent.Create(entity));
        }
        #endregion

        #region Inernal Implement
        private IEntity InnerCreate(Type entityType, int entityId, int typeId, OnDataProviderReady onReady)
        {
            IEntity entity = (IEntity)Domain.GetModule<IContainerModule>().New(entityType, entityId, true, null, onReady);
            Event.TriggerNow(EntityCreateEvent.Create(entity));
            return entity;
        }

        private IEntity InnerCreate(Type entityType, IEntity parent, OnDataProviderReady onReady)
        {
            IEntity entity = (IEntity)Domain.GetModule<IContainerModule>().New(entityType, true, parent, onReady);
            Event.TriggerNow(EntityCreateEvent.Create(entity));
            return entity;
        }
        #endregion
    }
}
