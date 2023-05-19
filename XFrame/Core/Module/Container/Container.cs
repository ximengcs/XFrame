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
    /// </summary>
    public partial class Container : ContainerBase, IContainer
    {
        private DataProvider m_Data;
        private XCollection<ICom> m_Coms;

        public IContainer Master { get; private set; }
        public int Id { get; private set; }

        #region Container Life Fun
        void IContainer.OnInit(int id, IContainer master, OnDataProviderReady onReady)
        {
            if (Status == State.Using)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {Status}, but enter OnInit. hash is {GetHashCode()}");
                return;
            }

            Id = id;
            if (master != null && master.Master != null)
                Master = master.Master;
            else
                Master = master;
            Status = State.Using;
            onReady?.Invoke(this);
            OnInit();
        }

        void IContainer.OnUpdate(float elapseTime)
        {
            if (Status != State.Using)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {Status}, but enter OnUpdate. hash is {GetHashCode()}");
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
            if (Status == State.Disposed)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {Status}, but enter OnDestroy again. hash is {GetHashCode()}");
                return;
            }

            m_Coms.SetIt(XItType.Backward);
            foreach (ICom com in m_Coms)
                com.OnDestroy();
            OnDestroy();
            Status = State.Disposed;
        }
        #endregion

        #region Pool Life Fun
        int IPoolObject.PoolKey => 0;

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
            Status = State.NotInit;
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
            return InnerGetOrAddCom(type, id, onReady);
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
                IPool pool = PoolModule.Inst.GetOrNew(type);
                pool.Release(com);
            }
        }

        private ICom InnerGetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
            {
                ContainerBase container = com as ContainerBase;
                if (container != null && container.Status == State.NotInit)
                {
                    InnerInitCom(com, id, onReady);
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
            InnerInitCom(com, id, onReady);
            m_Coms.Add(com);
            return com;
        }

        private void InnerInitCom(ICom com, int id, OnDataProviderReady onReady)
        {
            com.Owner = this;
            com.Active = true;
            if (Master != null)
                com.OnInit(id, Master, onReady);
            else
                com.OnInit(id, this, onReady);
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
