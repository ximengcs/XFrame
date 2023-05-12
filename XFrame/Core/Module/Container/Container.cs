using System;
using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 通用容器
    /// 可继承去实现所需生命周期处理
    /// </summary>
    public partial class Container : IContainer
    {
        private State m_State;
        private DataProvider m_Data;
        private XCollection<ICom> m_Coms;

        public object Master { get; private set; }
        public int Id { get; private set; }

        #region Container Life Fun
        void IContainer.OnInit(int id, object master, OnDataProviderReady onReady)
        {
            if (m_State == State.Using)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {m_State}, but enter OnInit.");
                return;
            }

            Id = id;
            Master = master;
            m_State = State.Using;
            onReady?.Invoke(this);
            OnInit();
        }

        void IContainer.OnUpdate(float elapseTime)
        {
            if (m_State != State.Using)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {m_State}, but enter OnUpdate.");
                return;
            }

            m_Coms.SetIt(XItType.Forward);
            foreach (ICom com in m_Coms)
            {
                if (com.Active)
                    com.OnUpdate(elapseTime);
            }
            OnUpdate(elapseTime);
        }

        void IContainer.OnDestroy()
        {
            if (m_State == State.Disposed)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {m_State}, but enter OnDestroy agin.");
                return;
            }

            m_Coms.SetIt(XItType.Backward);
            foreach (ICom com in m_Coms)
                com.OnDestroy();
            OnDestroy();
            m_State = State.Disposed;
        }
        #endregion

        #region Pool Life Fun
        void IPoolObject.OnCreate()
        {
            m_Data = new DataProvider();
            m_Coms = new XCollection<ICom>();
            OnCreateFromPool();
        }

        void IPoolObject.OnRequest()
        {
            foreach (ICom com in m_Coms)
                com.OnRequest();
            OnRequestFromPool();
            m_State = State.NotInit;
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
            foreach (ICom com in m_Coms)
                com.OnRelease();
            m_Data.ClearData();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
            foreach (ICom com in m_Coms)
                com.OnDelete();
            m_Data = null;
            m_Coms = null;
        }
        #endregion

        #region Sub Interface
        protected internal virtual void OnInit() { }
        protected internal virtual void OnUpdate(float elapseTime) { }
        protected internal virtual void OnDestroy() { }
        protected internal virtual void OnCreateFromPool() { }
        protected internal virtual void OnRequestFromPool() { }
        protected internal virtual void OnDestroyFromPool() { }
        protected internal virtual void OnReleaseFromPool() { }
        #endregion

        #region Container Interface
        public T GetCom<T>(int id = 0) where T : ICom
        {
            return (T)InnerGetCom(typeof(T), id);
        }

        public ICom GetCom(Type type, int id = 0)
        {
            return InnerGetCom(type, id);
        }

        public ICom AddCom(ICom com, int id = default, OnDataProviderReady onReady = null)
        {
            return InnerAdd(com, id, onReady);
        }

        public T AddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), default, (com) => onReady?.Invoke((T)com));
        }

        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public ICom AddCom(Type type, OnDataProviderReady onReady = null)
        {
            return InnerAdd(type, default, onReady);
        }

        public ICom AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return InnerAdd(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)GetOrAddCom(typeof(T), default, (com) => onReady?.Invoke((T)com));
        }

        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)GetOrAddCom(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public ICom GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            return GetOrAddCom(type, default, onReady);
        }

        public ICom GetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
            {
                onReady?.Invoke(com);
                return com;
            }
            else
                return InnerAdd(type, id, onReady);
        }

        public void RemoveCom<T>(int id = 0) where T : ICom
        {
            InnerRemove(typeof(T), id);
        }

        public void RemoveCom(Type type, int id = 0)
        {
            InnerRemove(type, id);
        }

        public void ClearCom()
        {
            foreach (ICom com in m_Coms)
            {
                com.OnDestroy();
            }
            m_Coms.Clear();
        }
        #endregion

        #region Inner Implement
        private void InnerRemove(Type type, int id)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
            {
                m_Coms.Remove(com);
                com.OnDestroy();
            }
        }

        private ICom InnerGetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
            {
                Container container = com as Container;
                if (container != null && container.m_State == State.NotInit)
                {
                    com.Owner = this;
                    com.OnInit(id, Master, onReady);
                }
                return com;
            }
            else
            {
                return InnerAdd(type, id, onReady);
            }
        }

        private ICom InnerAdd(Type type, int id, OnDataProviderReady onReady)
        {
            id = InnerCheckId(type, id);
            IPool pool = PoolModule.Inst.GetOrNew(type);
            IPoolObject obj = pool.Require();
            ICom newCom = (ICom)obj;
            return InnerAdd(newCom, id, onReady);
        }

        private ICom InnerAdd(ICom com, int id, OnDataProviderReady onReady)
        {
            id = InnerCheckId(com.GetType(), id);
            com.Owner = this;
            com.OnInit(id, Master, onReady);
            m_Coms.Add(com);
            return com;
        }

        private int InnerCheckId(Type type, int id)
        {
            if (m_Coms.Get(type, id) != null)
                id = IdModule.Inst.Next();
            return id;
        }

        private ICom InnerGetCom(Type type, int id)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                foreach (ICom com in m_Coms)
                {
                    Type comType = com.GetType();
                    if (type.IsAssignableFrom(comType) && com.Id == id)
                        return com;
                }
            }
            else
            {
                return m_Coms.Get(type, id);
            }
            return default;
        }
        #endregion

        #region Data Interface
        public void SetData<T>(T value)
        {
            m_Data.SetData<T>(value);
        }

        public T GetData<T>()
        {
            return m_Data.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Data.SetData<T>(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Data.GetData<T>(name);
        }

        public void ClearData()
        {
            m_Data.ClearData();
        }
        #endregion

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Coms.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Coms.SetIt(type);
        }
    }
}
