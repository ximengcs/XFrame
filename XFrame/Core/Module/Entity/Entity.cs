using System;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.Containers;
using System.Collections.Generic;
using XFrame.Modules.Event;
using XFrame.Modules.ID;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public abstract class Entity : IEntity
    {
        #region Inner Field
        private int m_Id;
        private int m_TypeId;
        private Entity m_Parent;
        private Container m_Container;
        private IEventSystem m_EventSys;
        #endregion

        #region Life Fun
        void IEntity.OnInit(int id, IEntity parent, OnEntityReady onData)
        {
            m_Id = id;
            m_Parent = parent != null ? (Entity)parent : null;
            m_Container = new Container();
            m_Container.OnInit(this);
            m_EventSys = EventModule.Inst.NewSys();
            onData?.Invoke(this);
            m_TypeId = GetData<int>(nameof(m_TypeId));
            OnInit();
        }

        void IEntity.OnUpdate(float elapseTime)
        {
            foreach (ICom com in m_Container)
                com.OnUpdate();
            OnUpdate(elapseTime);
        }

        void IEntity.OnDestroy()
        {
            OnDestroy();
            foreach (ICom com in m_Container)
                com.OnDestroy();
            m_Container.Dispose();
        }
        #endregion

        #region Pool Life Fun
        void IPoolObject.OnCreate()
        {
            OnCreate();
        }

        void IPoolObject.OnRelease()
        {
            m_Id = default;
            m_Parent = null;
            OnRelease();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
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
        protected abstract void OnDestroyFromPool();
        #endregion

        #region Sub Child Life Fun
        /// <summary>
        /// 实体初始化生命周期
        /// </summary>
        /// <param name="data">实体数据，数据在销毁时不会被回收或释放</param>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// 实体更新生命周期
        /// </summary>
        /// <param name="elapseTime">逃逸时间</param>
        protected virtual void OnUpdate(float elapseTime)
        {
        }

        /// <summary>
        /// 实体销毁生命周期
        /// 这个方法无论是释放或者真正销毁时都会被调用
        /// </summary>
        protected virtual void OnDestroy()
        {
        }
        #endregion

        #region Interface
        /// <summary>
        /// 实体事件系统
        /// </summary>
        public IEventSystem Event => m_EventSys;

        /// <summary>
        /// 实体Id
        /// </summary>
        public int Id => m_Id;

        /// <summary>
        /// 实体类型Id
        /// </summary>
        public int TypeId => m_TypeId;

        /// <summary>
        /// 实体父节点
        /// </summary>
        public Entity Parent
        {
            get => m_Parent;
            set
            {
                if (m_Parent != value)
                {
                    EntityParentChangeEvent e = new EntityParentChangeEvent(m_Parent, value);
                    m_Parent = value;
                    m_EventSys.Trigger(e);
                }
            }
        }
        #endregion

        private ICom InnerGetOrAdd(Type type, int comId, OnContainerReady onReady)
        {
            ICom com = m_Container.Get(type, comId);
            if (com != null)
                return com;
            return InnerAddCom(type, comId, onReady);
        }

        private ICom InnerAddCom(Type type, int comId, OnContainerReady onReady)
        {
            ICom entityCom = (ICom)Activator.CreateInstance(type);
            m_Container.Add(entityCom, comId, onReady);
            return entityCom;
        }

        #region IContainer Implement
        public ICom Get(Type type, int id = 0)
        {
            return m_Container.Get(type, id);
        }

        public T Add<T>(OnContainerReady onReady = null) where T : ICom
        {
            return (T)InnerAddCom(typeof(T), IdModule.Inst.Next(), onReady);
        }

        public T Add<T>(int id, OnContainerReady onReady = null) where T : ICom
        {
            return (T)InnerAddCom(typeof(T), id, onReady);
        }

        public ICom Add(Type type, OnContainerReady onReady = null)
        {
            return InnerAddCom(type, IdModule.Inst.Next(), onReady);
        }

        public ICom Add(Type type, int id = 0, OnContainerReady onReady = null)
        {
            return InnerAddCom(type, id, onReady);
        }

        public T GetOrAdd<T>(OnContainerReady onReady = null) where T : ICom
        {
            return (T)InnerGetOrAdd(typeof(T), IdModule.Inst.Next(), onReady);
        }

        public T GetOrAdd<T>(int id = 0, OnContainerReady onReady = null) where T : ICom
        {
            return (T)InnerGetOrAdd(typeof(T), id, onReady);
        }

        public ICom GetOrAdd(Type type, OnContainerReady onReady = null)
        {
            return InnerGetOrAdd(type, IdModule.Inst.Next(), onReady);
        }

        public ICom GetOrAdd(Type type, int id = 0, OnContainerReady onReady = null)
        {
            return InnerGetOrAdd(type, id, onReady);
        }

        public void Remove(Type type, int id = 0)
        {
            m_Container.Remove(type, id);
        }

        public void Clear()
        {
            m_Container.Clear();
        }

        public void Dispatch(OnContainerReady handle)
        {
            m_Container.Dispatch(handle);
        }

        public void SetData<T>(T value)
        {
            m_Container.SetData(value);
        }

        public T GetData<T>()
        {
            return m_Container.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Container.SetData(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Container.GetData<T>(name);
        }

        public void Dispose()
        {
            m_Container.Dispose();
        }

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Container.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Container.SetIt(type);
        }

        public T Get<T>(int id = 0) where T : ICom
        {
            return m_Container.Get<T>(id);
        }

        public void Remove<T>(int id = 0) where T : ICom
        {
            m_Container.Remove<T>(id);
        }

        public ICom Add(ICom com, int id = 0, OnContainerReady onReady = null)
        {
            IEntity entity = (IEntity)com;
            if (entity != null)
            {
                return m_Container.Add(com, id, onReady);
            }
            return default;
        }
        #endregion
    }
}
