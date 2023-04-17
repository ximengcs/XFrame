using System;
using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    internal class Container : IXItem, IContainer
    {
        private object m_Owner;
        private DataProvider m_Data;
        private XCollection<ICom> m_Coms;

        public int Id { get; private set; }

        public void OnInit(object owner)
        {
            m_Owner = owner;
            Id = GetHashCode();
            m_Data = new DataProvider();
            m_Coms = new XCollection<ICom>();
        }

        public void OnUpdate()
        {
            m_Coms.SetIt(XItType.Forward);
            foreach (ICom com in m_Coms)
            {
                if (com.Active)
                    com.OnUpdate();
            }
        }

        public void OnDestroy()
        {
            m_Coms.SetIt(XItType.Backward);
            foreach (ICom com in m_Coms)
                com.OnDestroy();
            m_Coms = null;
        }

        public T Get<T>(int id = 0) where T : ICom
        {
            return m_Coms.Get<T>(id);
        }

        public ICom Get(Type type, int id = 0)
        {
            return m_Coms.Get(type, id);
        }

        public ICom Add(ICom com, int id = default, OnContainerReady onReady = null)
        {
            return InnerAdd(com, id, onReady);
        }

        public T Add<T>(OnContainerReady onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), default, onReady);
        }

        public T Add<T>(int id, OnContainerReady onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), id, onReady);
        }

        public ICom Add(Type type, OnContainerReady onReady = null)
        {
            return InnerAdd(type, default, onReady);
        }

        public ICom Add(Type type, int id, OnContainerReady onReady = null)
        {
            return InnerAdd(type, id, onReady);
        }

        public T GetOrAdd<T>(OnContainerReady onReady = null) where T : ICom
        {
            return (T)GetOrAdd(typeof(T), default, onReady);
        }

        public T GetOrAdd<T>(int id, OnContainerReady onReady = null) where T : ICom
        {
            return (T)GetOrAdd(typeof(T), id, onReady);
        }

        public ICom GetOrAdd(Type type, OnContainerReady onReady = null)
        {
            return GetOrAdd(type, default, onReady);
        }

        public ICom GetOrAdd(Type type, int id, OnContainerReady onReady = null)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
                return com;
            else
                return InnerAdd(type, id, onReady);
        }

        public void Remove<T>(int id = 0) where T : ICom
        {
            InnerRemove(typeof(T), id);
        }

        public void Remove(Type type, int id = 0)
        {
            InnerRemove(type, id);
        }

        public void Clear()
        {
            m_Coms.Clear();
        }

        public void Dispatch(OnContainerReady handle)
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

        private ICom InnerAdd(Type type, int id, OnContainerReady onReady)
        {
            if (m_Coms.Get(type, id) != null)
                id = IdModule.Inst.Next();

            ICom newCom = (ICom)Activator.CreateInstance(type);
            return InnerAdd(newCom, id, onReady);
        }

        private ICom InnerAdd(ICom com, int id, OnContainerReady onReady)
        {
            com.OnInit(this, id, m_Owner, onReady);
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
            Clear();
        }

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
