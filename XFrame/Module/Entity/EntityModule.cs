using System;
using XFrame.Core;
using XFrame.Utility;
using System.Collections.Generic;
using XFrame.Collections;

namespace XFrame.Modules
{
    public class EntityModule : SingleModule<EntityModule>
    {
        #region Inner Field
        private XCollection<Entity> m_Entities;
        #endregion

        #region Life Fun
        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Entities = new XCollection<Entity>();
            TypeModule.Inst.RegisterWithAtr<EntityPropAttribute>();
        }

        public override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            foreach (Entity entity in m_Entities)
                entity.OnInternalUpdate(escapeTime);
        }
        #endregion

        #region Interface
        public void RegisterEntity<T>() where T : Entity
        {
            TypeModule.Set module = TypeModule.Inst.Get<EntityPropAttribute>().ClassifyBySub<T>();
            List<Type> types = new List<Type>(module);
            if (TypeUtility.HasAttribute<EntityPropAttribute>(module.Main))
                types.Add(module.Main);

            foreach (Type type in types)
            {
                EntityPropAttribute atr = TypeUtility.GetAttribute<EntityPropAttribute>(type);
                module.AddIndex(atr.Type, type);
            }
        }

        public T Create<T>(Scene scene) where T : Entity
        {
            return InnerCreate(typeof(T), scene, default, default) as T;
        }

        public T Create<T>(Scene scene, Entity parent) where T : Entity
        {
            return InnerCreate(typeof(T), scene, parent, default) as T;
        }

        public T Create<T>(Scene scene, EntityData data) where T : Entity
        {
            Type type = TypeModule.Inst.Get<EntityPropAttribute>().GetBySub<T>().GetIndex(data.TypeId);
            T entity = InnerCreate(type, scene, default, data) as T;
            return entity;
        }

        public T Create<T>(Scene scene, Entity parent, EntityData data) where T : Entity
        {
            Type type = TypeModule.Inst.Get<EntityPropAttribute>().GetBySub<T>().GetIndex(data.TypeId);
            return InnerCreate(type, scene, parent, data) as T;
        }

        public void Destroy(Entity entity)
        {
            IPoolSystem<Entity> poolSystem = PoolModule.Inst.GetOrNew<Entity>(entity.GetType());
            IPool<Entity> pool = poolSystem.Require();
            pool.Release(entity);
            poolSystem.Release(pool);
        }

        private Entity InnerCreate(Type entityType, Scene scene, Entity parent, EntityData data)
        {
            Entity entity;
            PoolModule.Inst.GetOrNew<Entity>(entityType).Require().Require(out entity);
            entity.OnInternalInit(IdModule.Inst.Next(), scene, parent, data);
            if (parent == null)
                m_Entities.Add(entity);
            return entity;
        }
        #endregion
    }
}
