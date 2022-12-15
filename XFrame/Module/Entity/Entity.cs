using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : IPoolObject, IXItem
    {
        #region Inner Field
        private EntityData m_Data;
        protected Scene m_Scene;
        protected Entity m_Parent;
        protected XCollection<Entity> m_Children;
        #endregion

        #region Life Fun
        internal virtual void OnInternalInit(int id, Scene scene, Entity parent, EntityData data)
        {
            Id = id;
            m_Parent = parent;
            m_Scene = scene;
            if (m_Children == null)
                m_Children = new XCollection<Entity>();
            OnInit(data);
        }

        internal virtual void OnInternalUpdate(float elapseTime)
        {
            OnUpdate(elapseTime);
            foreach (Entity child in m_Children)
                child.OnInternalUpdate(elapseTime);
        }

        internal virtual void OnInternalDestroy()
        {
            foreach (Entity child in m_Children)
                child.OnInternalDestroy();
            OnDestroy();
        }
        #endregion

        #region Pool Life Fun
        public virtual void OnCreate()
        {

        }

        public virtual void OnRelease()
        {
            Id = default;
            m_Data = null;
            m_Scene = null;
            m_Parent = null;
        }

        public virtual void OnDestroyFrom()
        {

        }
        #endregion

        #region Sub Child Life Fun
        /// <summary>
        /// 实体初始化生命周期
        /// </summary>
        /// <param name="data">实体数据，数据在销毁时不会被回收或释放</param>
        protected abstract void OnInit(EntityData data);

        /// <summary>
        /// 实体更新生命周期
        /// </summary>
        /// <param name="elapseTime">逃逸时间</param>
        protected abstract void OnUpdate(float elapseTime);

        /// <summary>
        /// 实体销毁生命周期
        /// 这个方法无论是释放或者真正销毁时都会被调用
        /// </summary>
        protected abstract void OnDestroy();

        /// <summary>
        /// 实体销毁生命周期
        /// 这个方法只有在真正被销毁时才会被调用，如果是释放到对象池中时不会被调用
        /// </summary>
        protected abstract void OnDelete();
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
