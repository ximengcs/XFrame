using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : IEntity, IPoolObject
    {
        #region Inner Field
        private EntityData m_Data;
        protected Scene m_Scene;
        protected Entity m_Parent;
        protected XCollection<Entity> m_Children;
        #endregion

        #region Life Fun
        void IEntity.OnInit(int id, IScene scene, IEntity parent, EntityData data)
        {
            Id = id;
            m_Parent = parent as Entity;
            m_Scene = scene as Scene;
            if (m_Children == null)
                m_Children = new XCollection<Entity>();
            OnInit(data);
        }

        void IEntity.OnUpdate(float elapseTime)
        {
            OnUpdate(elapseTime);
        }

        void IEntity.OnDestroy()
        {
            OnDestroy();
        }
        #endregion

        #region Pool Life Fun
        void IPoolObject.OnCreate()
        {
            OnCreate();
        }

        void IPoolObject.OnRelease()
        {
            Id = default;
            m_Data = null;
            m_Scene = null;
            m_Parent = null;
            OnRelease();
        }

        void IPoolObject.OnDestroyForever()
        {
            OnDestroyForever();
        }

        /// <summary>
        /// 实体创建生命周期，new或从对象池中获取到的对象都会被调用
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// 实体释放生命周期, 释放到对象池中时被调用
        /// </summary>
        protected abstract void OnRelease();

        /// <summary>
        /// 实体永久销毁生命周期，当对象池满时会触发此生命周期
        /// </summary>
        protected abstract void OnDestroyForever();
        #endregion

        #region Sub Child Life Fun
        /// <summary>
        /// 实体初始化生命周期
        /// </summary>
        /// <param name="data">实体数据，数据在销毁时不会被回收或释放</param>
        protected virtual void OnInit(EntityData data)
        {

        }

        /// <summary>
        /// 实体更新生命周期
        /// </summary>
        /// <param name="elapseTime">逃逸时间</param>
        protected virtual void OnUpdate(float elapseTime)
        {
            foreach (Entity child in m_Children)
                child.OnUpdate(elapseTime);
        }

        /// <summary>
        /// 实体销毁生命周期
        /// 这个方法无论是释放或者真正销毁时都会被调用
        /// </summary>
        protected virtual void OnDestroy()
        {
            foreach (Entity child in m_Children)
                child.OnDestroy();
        }
        #endregion

        #region Interface
        /// <summary>
        /// 实体Id
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 实体类型Id
        /// </summary>
        public int TypeId => m_Data.TypeId;

        /// <summary>
        /// 实体所属场景
        /// </summary>
        public Scene Scene => m_Scene;

        /// <summary>
        /// 实体父节点
        /// </summary>
        public Entity Parent => m_Parent;

        /// <summary>
        /// 添加一个实体到孩子列表中
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="data">实体数据</param>
        /// <returns>创建的实体</returns>
        public T Add<T>(EntityData data) where T : Entity
        {
            T entity = EntityModule.Inst.InnerCreate<T>(m_Scene, this, data);
            InnerAdd(entity);
            return entity;
        }

        /// <summary>
        /// 添加一个实体到孩子列表中
        /// 注意实体从对象池中创建或释放时孩子的对象池生命周期方法不会被调用
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>创建的实体</returns>
        public T Add<T>() where T : Entity
        {
            T entity = EntityModule.Inst.InnerCreate<T>(m_Scene, this);
            InnerAdd(entity);
            return entity;
        }

        /// <summary>
        /// 从孩子列表中移除实体
        /// </summary>
        /// <param name="entity">待移除的实体</param>
        public void Remove(Entity entity)
        {
            m_Children.Remove(entity);
        }

        /// <summary>
        /// 从孩子列表中移除实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityId">实体Id</param>
        public void Remove<T>(int entityId) where T : Entity
        {
            Entity entity = m_Children.Get<T>(entityId);
            if (entity != null)
                Remove(entity);
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>获取到的实体</returns>
        public T Get<T>() where T : Entity
        {
            return m_Children.Get<T>();
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityId">实体Id</param>
        /// <returns>获取到的实体</returns>
        public T Get<T>(int entityId) where T : Entity
        {
            return m_Children.Get<T>(entityId);
        }

        /// <summary>
        /// 从父实体中获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <returns>获取到的实体</returns>
        protected T GetOwner<T>() where T : Entity
        {
            if (m_Parent == null)
                return null;
            return m_Parent.Get<T>();
        }

        /// <summary>
        /// 从父实体中获取实体
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="entityId">实体Id</param>
        /// <returns>获取到的实体</returns>
        protected T GetOwner<T>(int entityId) where T : Entity
        {
            if (m_Parent == null)
                return null;
            return m_Parent.Get<T>(entityId);
        }
        #endregion

        #region Inner Implement
        private Entity InnerAdd(Entity entity)
        {
            entity.m_Parent = this;
            m_Children.Add(entity);
            return entity;
        }
        #endregion
    }
}
