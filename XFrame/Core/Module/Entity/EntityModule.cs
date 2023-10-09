using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Reflection;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Containers;
using XFrame.Modules.Download;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体模块
    /// 只有根实体才会接受实体模块的更新生命周期
    /// </summary>
    [CoreModule]
    [XType(typeof(IEntityModule))]
    public class EntityModule : ModuleBase, IEntityModule
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

        protected override void OnDestroy()
        {
            base.OnDestroy(); 
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
            TypeSystem module = ModuleUtility.Type
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub<T>();

            foreach (Type type in module)
            {
                EntityPropAttribute atr = ModuleUtility.Type.GetAttribute<EntityPropAttribute>(type);
                if (atr != null)
                    module.AddKey(atr.Type, type);
            }
        }

        /// <summary>
        /// 创建无实体数据的根实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>创建的实体</returns>
        public T Create<T>(OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return InnerCreate(typeof(T), default, onReady) as T;
        }

        public IEntity Create(Type type, OnDataProviderReady onReady = null)
        {
            return InnerCreate(type, default, onReady);
        }

        /// <summary>
        /// 创建无实体数据的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="parent">父实体</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IEntity parent, OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return InnerCreate(typeof(T), parent, onReady) as T;
        }

        public IEntity Create(Type type, IEntity parent, OnDataProviderReady onReady = null)
        {
            return InnerCreate(type, parent, onReady);
        }

        /// <summary>
        /// 创建有实体数据的根实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="typeId">实体类型</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(int typeId, OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return (T)Create(typeof(T), typeId, onReady);
        }

        public IEntity Create(Type baseType, int typeId, OnDataProviderReady onReady = null)
        {
            Type type = ModuleUtility.Type
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub(baseType)
                .GetKey(typeId);
            if (type == null)
            {
                Log.Debug("XFrame", $"Entity {baseType.Name} not register.");
                return default;
            }
            return InnerCreate(type, default, onReady);
        }

        /// <summary>
        /// 创建有实体数据的实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="parent">父实体</param>
        /// <param name="typeId">实体数据</param>
        /// <returns>创建的实体</returns>
        public T Create<T>(IEntity parent, int typeId, OnDataProviderReady onReady = null) where T : class, IEntity
        {
            return (T)Create(typeof(T), parent, typeId, onReady);
        }

        public IEntity Create(Type baseType, IEntity parent, int typeId, OnDataProviderReady onReady = null)
        {
            Type type = ModuleUtility.Type
                .GetOrNewWithAttr<EntityPropAttribute>()
                .GetOrNewBySub(baseType)
                .GetKey(typeId);
            return InnerCreate(type, parent, onReady);
        }

        /// <summary>
        /// 销毁一个实体
        /// </summary>
        /// <param name="entity">需要销毁的实体</param>
        public void Destroy(IEntity entity)
        {
            if (entity == null)
                return;

            ModuleUtility.Container.Remove(entity);
            m_Entities.Remove(entity);
        }
        #endregion

        #region Inernal Implement
        private IEntity InnerCreate(Type entityType, IEntity parent, OnDataProviderReady onReady)
        {
            return (IEntity)ModuleUtility.Container.New(entityType, true, parent, onReady);
        }
        #endregion
    }
}
