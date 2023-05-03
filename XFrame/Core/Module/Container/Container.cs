using System;
using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 通用容器
    /// 可继承去实现所需生命周期处理
    /// </summary>
    public class Container : IContainer
    {
        private object m_Master;
        private DataProvider m_Data;
        private XCollection<ICom> m_Coms;

        public object Master
        {
            get => m_Master;
            protected set => m_Master = value;
        }

        public int Id { get; private set; }

        void IContainer.OnInit(int id, object master, OnContainerReady onReady)
        {
            Id = id;
            m_Master = master;
            m_Data = new DataProvider();
            m_Coms = new XCollection<ICom>();
            onReady?.Invoke(this);
            OnInit();
        }

        void IContainer.OnUpdate(float elapseTime)
        {
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
            m_Coms.SetIt(XItType.Backward);
            foreach (ICom com in m_Coms)
                com.OnDestroy();
            OnDestroy();
            m_Coms = null;
        }

        void IPoolObject.OnCreate()
        {
            OnCreateFromPool();
        }

        void IPoolObject.OnRelease()
        {
            OnReleaseFromPool();
        }

        void IPoolObject.OnDelete()
        {
            OnDestroyFromPool();
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate(float elapseTime) { }
        protected virtual void OnDestroy() { }
        protected virtual void OnCreateFromPool() { }
        protected virtual void OnDestroyFromPool() { }
        protected virtual void OnReleaseFromPool() { }

        public T GetCom<T>(int id = 0) where T : ICom
        {
            return (T)InnerGetCom(typeof(T), id);
        }

        public ICom GetCom(Type type, int id = 0)
        {
            return InnerGetCom(type, id);
        }

        public ICom AddCom(ICom com, int id = default, OnComReady onReady = null)
        {
            return InnerAdd(com, id, onReady);
        }

        public T AddCom<T>(OnComReady<T> onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), default, (com) => onReady?.Invoke((T)com));
        }

        public T AddCom<T>(int id, OnComReady<T> onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public ICom AddCom(Type type, OnComReady onReady = null)
        {
            return InnerAdd(type, default, onReady);
        }

        public ICom AddCom(Type type, int id, OnComReady onReady = null)
        {
            return InnerAdd(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnComReady<T> onReady = null) where T : ICom
        {
            return (T)GetOrAddCom(typeof(T), default, (com) => onReady?.Invoke((T)com));
        }

        public T GetOrAddCom<T>(int id, OnComReady<T> onReady = null) where T : ICom
        {
            return (T)GetOrAddCom(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public ICom GetOrAddCom(Type type, OnComReady onReady = null)
        {
            return GetOrAddCom(type, default, onReady);
        }

        public ICom GetOrAddCom(Type type, int id, OnComReady onReady = null)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
                return com;
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
            m_Coms.Clear();
        }

        public void DispatchCom(OnComReady handle)
        {
            if (handle == null)
                return;

            m_Coms.SetIt(XItType.Forward);
            foreach (ICom com in m_Coms)
                handle.Invoke(com);
        }

        private void InnerRemove(Type type, int id)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
            {
                m_Coms.Remove(com);
                com.Dispose();
                com.OnDestroy();
            }
        }

        private ICom InnerAdd(Type type, int id, OnComReady onReady)
        {
            if (m_Coms.Get(type, id) != null)
                id = IdModule.Inst.Next();

            ICom newCom = (ICom)Activator.CreateInstance(type);
            return InnerAdd(newCom, id, onReady);
        }

        private ICom InnerAdd(ICom com, int id, OnComReady onReady)
        {
            com.OnInit(id, this, onReady);
            m_Coms.Add(com);
            return com;
        }

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

        public void Dispose()
        {
            m_Data.Dispose();
            ClearCom();
        }

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Coms.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Coms.SetIt(type);
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
    }
}
