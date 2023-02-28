using System;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using System.Collections.Generic;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体模块
    /// 只有根实体才会接受实体模块的更新生命周期
    /// </summary>
    public class EntityModule : SingletonModule<EntityModule>
    {
        #region Inner Field
        private XCollection<IEntity> m_Entities;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Entities = new XCollection<IEntity>();
            TypeModule.Inst.GetOrNewWithAttr<EntityPropAttribute>();
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            foreach (IEntity entity in m_Entities)
                entity.OnUpdate(escapeTime);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            foreach (IEntity entity in m_Entities)
            {
                IPool pool = PoolModule.Inst.GetOrNew(entity.GetType());
                entity.OnDestroy();
                pool.Release(entity);
            }
            m_Entities.Clear();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 注册实体，创建实体前需要注册实体
        /// </summary>
        /// <typeparam name="T">实体基类或实体类</typeparam>
        public void RegisterEntity<T>() where T : class, IEntity
        {
            TypeModule.System module = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>();
            List<Type> types = new List<Type>(module);
            if (TypeUtility.HasAttribute<EntityPropAttribute>(module.Main))
                types.Add(module.Main);

            foreach (Type type in types)
            {
                EntityPropAttribute atr = TypeUtility.GetAttribute<EntityPropAttribute>(type);
                module.AddKey(atr.Type, type);
            }
        }

        /// <summary>
        /// 创建无实体数据的根实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="scene">实体所属场景</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IScene scene) where T : class, IEntity
        {
            return InnerCreate(typeof(T), scene, default, default, true) as T;
        }

        /// <summary>
        /// 创建无实体数据的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="scene">实体所属场景</param>
        /// <param name="parent">父实体</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IScene scene, IEntity parent) where T : class, IEntity
        {
            return InnerCreate(typeof(T), scene, parent, default, true) as T;
        }

        /// <summary>
        /// 创建有实体数据的根实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="scene">实体所属场景</param>
        /// <param name="data">实体数据</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IScene scene, EntityData data) where T : class, IEntity
        {
            Type type = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>()
                .GetKey(data.TypeId);
            T entity = InnerCreate(type, scene, default, data, true) as T;
            return entity;
        }

        /// <summary>
        /// 创建有实体数据的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="scene">实体所属场景</param>
        /// <param name="parent">父实体</param>
        /// <param name="data">实体数据</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IScene scene, IEntity parent, EntityData data) where T : class, IEntity
        {
            Type type = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>()
                .GetKey(data.TypeId);
            return InnerCreate(type, scene, parent, data, true) as T;
        }

        /// <summary>
        /// 销毁一个实体
        /// </summary>
        /// <param name="entity">需要销毁的实体</param>
        public void Destroy(IEntity entity)
        {
            IPool pool = PoolModule.Inst.GetOrNew(entity.GetType());
            pool.Release(entity);
            entity.OnDestroy();
            m_Entities.Remove(entity);
        }
        #endregion

        #region Inernal Implement
        internal T InnerCreate<T>(IScene scene, IEntity parent, EntityData data) where T : class, IEntity
        {
            Type type = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>()
                .GetKey(data.TypeId);
            return InnerCreate(type, scene, parent, data, false) as T;
        }

        internal T InnerCreate<T>(IScene scene, IEntity parent) where T : class, IEntity
        {
            return InnerCreate(typeof(T), scene, parent, default, false) as T;
        }

        private IEntity InnerCreate(Type entityType, IScene scene, IEntity parent, EntityData data, bool fromPool)
        {
            IEntity entity;

            if (fromPool)
            {
                IPool pool = PoolModule.Inst.GetOrNew(entityType);
                pool.Require(out IPoolObject obj);
                entity = obj as IEntity;
            }
            else
            {
                entity = Activator.CreateInstance(entityType) as IEntity;
            }

            entity.OnInit(IdModule.Inst.Next(), scene, parent, data);
            if (parent == null)
                m_Entities.Add(entity);
            return entity;
        }
        #endregion
    }
}
