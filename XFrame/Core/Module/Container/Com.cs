using System;
using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    public abstract class Com : ICom
    {
        private Container m_Container;
        private IDataProvider m_Data;
        private bool m_Active;

        protected object m_Owner;

        public bool Active
        {
            get { return m_Active; }
            set
            {
                if (m_Active != value)
                {
                    m_Active = value;
                    if (m_Active)
                        OnActive();
                    else
                        OnInactive();
                }
            }
        }

        public IDataProvider ShareData => m_Container;

        public int Id { get; private set; }

        void ICom.OnInit(IContainer container, int id, object owner)
        {
            Id = id;
            Active = false;
            OnInactive();
            m_Container = (Container)container;
            m_Owner = owner;
            m_Data = new DataProvider();
            OnInit();
        }

        void ICom.OnAwake()
        {
            Active = true;
            OnAwake();
        }

        void ICom.OnUpdate()
        {
            if (Active)
                OnUpdate();
        }

        void ICom.OnDestroy()
        {
            Active = false;
            OnDestroy();
        }

        protected virtual void OnInit() { }
        protected virtual void OnAwake() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnDestroy() { }
        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }

        public T Get<T>(int id = 0) where T : ICom
        {
            return m_Container.Get<T>(id);
        }

        public ICom Get(Type type, int id = 0)
        {
            return m_Container.Get(type, id);
        }

        public T Add<T>(Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.Add<T>(default, comInitComplete);
        }

        public T Add<T>(int id, Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.Add<T>(id, comInitComplete);
        }

        public ICom Add(Type type, Action<ICom> comInitComplete = null)
        {
            return m_Container.Add(type, default, comInitComplete);
        }

        public ICom Add(Type type, int id, Action<ICom> comInitComplete = null)
        {
            return m_Container.Add(type, id, comInitComplete);
        }

        public T GetOrAdd<T>(Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.GetOrAdd<T>(default, comInitComplete);
        }

        public T GetOrAdd<T>(int id, Action<ICom> comInitComplete = null) where T : ICom
        {
            return m_Container.GetOrAdd<T>(id, comInitComplete);
        }

        public ICom GetOrAdd(Type type, Action<ICom> comInitComplete = null)
        {
            return m_Container.GetOrAdd(type, default, comInitComplete);
        }

        public ICom GetOrAdd(Type type, int id, Action<ICom> comInitComplete = null)
        {
            return m_Container.GetOrAdd(type, id, comInitComplete);
        }

        public void Remove<T>(int id = 0) where T : ICom
        {
            m_Container.Remove<T>(id);
        }

        public void Remove(Type type, int id = 0)
        {
            m_Container.Remove(type, id);
        }

        public void Clear()
        {
            m_Container.Clear();
        }

        public void Dispatch(Action<ICom> handle)
        {
            m_Container.Dispatch(handle);
        }

        public void SetData<T>(T value)
        {
            m_Data.SetData(value);
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
        }

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Container.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Container.SetIt(type);
        }
    }
}
