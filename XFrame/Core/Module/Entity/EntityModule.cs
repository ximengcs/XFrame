using System;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.XType;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体模块
    /// 只有根实体才会接受实体模块的更新生命周期
    /// </summary>
    [CoreModule]
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
                entity.OnDestroy();
                entity.OnDelete();
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
            TypeSystem module = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>();

            foreach (Type type in module)
            {
                EntityPropAttribute atr = TypeUtility.GetAttribute<EntityPropAttribute>(type);
                if (atr != null)
                    module.AddKey(atr.Type, type);
            }
        }

        /// <summary>
        /// 创建无实体数据的根实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>创建的实体</returns>
        public T Create<T>(OnEntityReady<T> onReady = null) where T : class, IEntity
        {
            return InnerCreate(typeof(T), default, (entity) => onReady?.Invoke((T)entity), true) as T;
        }

        public IEntity Create(Type type, OnEntityReady onReady = null)
        {
            return InnerCreate(type, default, onReady, true);
        }

        /// <summary>
        /// 创建无实体数据的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="parent">父实体</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IEntity parent, OnEntityReady<T> onReady = null) where T : class, IEntity
        {
            return InnerCreate(typeof(T), parent, (entity) => onReady?.Invoke((T)entity), true) as T;
        }

        public IEntity Create(Type type, IEntity parent, OnEntityReady onReady = null)
        {
            return InnerCreate(type, parent, onReady, true);
        }

        /// <summary>
        /// 创建有实体数据的根实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="typeId">实体类型</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(int typeId, OnEntityReady<T> onReady = null) where T : class, IEntity
        {
            return (T)Create(typeof(T), typeId, (entity) => onReady?.Invoke((T)entity));
        }

        public IEntity Create(Type baseType, int typeId, OnEntityReady onReady = null)
        {
            Type type = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub(baseType)
                .GetKey(typeId);
            if (type == null)
            {
                Log.Debug("XFrame", $"Entity {baseType.Name} not register.");
                return default;
            }
            return InnerCreate(type, default, onReady, true);
        }

        /// <summary>
        /// 创建有实体数据的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="parent">父实体</param>
        /// <param name="typeId">实体数据</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IEntity parent, int typeId, OnEntityReady<T> onReady = null) where T : class, IEntity
        {
            return (T)Create(typeof(T), parent, typeId, (entity) => onReady?.Invoke((T)entity));
        }

        public IEntity Create(Type baseType, IEntity parent, int typeId, OnEntityReady onReady = null)
        {
            Type type = TypeModule.Inst
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub(baseType)
                .GetKey(typeId);
            return InnerCreate(type, parent, onReady, true);
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
        private IEntity InnerCreate(Type entityType, IEntity parent, OnEntityReady onReady, bool fromPool)
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

            entity.OnInit(IdModule.Inst.Next(), parent, onReady);
            if (parent == null)
                m_Entities.Add(entity);
            return entity;
        }
        #endregion
    }
}
