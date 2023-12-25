using System;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 共享组件基类, 会共享容器数据
    /// </summary>
    public abstract class ShareCom : ICom, ICanSetOwner, ICanInitialize, IUpdater, ICanDestroy
    {
        private IContainer m_Owner;
        private bool m_Active;

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

        public int Id { get; private set; }

        public IContainer Master { get; private set; }

        public IContainer Owner => m_Owner;

        void ICanSetOwner.SetOwner(IContainer owner)
        {
            m_Owner = owner;
        }

        void ICanInitialize.OnInit(int id, IContainer master, OnDataProviderReady onReady)
        {
            Id = id;
            if (master != null && master.Master != null)
                Master = master.Master;
            else
                Master = master;

            onReady?.Invoke(this);
            OnInit();
        }

        protected internal virtual void OnInit() { }

        void IUpdater.OnUpdate(float elapseTime)
        {
            OnUpdate(elapseTime);
        }

        protected internal virtual void OnUpdate(float elapseTime) { }

        void ICanDestroy.OnDestroy()
        {
            OnDestroy();
        }

        protected internal virtual void OnDestroy() { }

        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }

        public T GetCom<T>(int id = 0) where T : ICom
        {
            return m_Owner.GetCom<T>(id);
        }

        public ICom GetCom(Type type, int id = 0)
        {
            return m_Owner.GetCom(type, id);
        }

        public T AddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.AddCom<T>(onReady);
        }

        public ICom AddCom(ICom com)
        {
            return m_Owner.AddCom(com);
        }

        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.AddCom<T>(id, onReady);
        }

        public ICom AddCom(Type type, OnDataProviderReady onReady = null)
        {
            return m_Owner.AddCom(type, onReady);
        }

        public ICom AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return m_Owner.AddCom(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.GetOrAddCom<T>(onReady);
        }

        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.GetOrAddCom<T>(id, onReady);
        }

        public ICom GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            return m_Owner.GetOrAddCom(type, onReady);
        }

        public ICom GetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return m_Owner.GetOrAddCom(type, id, onReady);
        }

        public void RemoveCom<T>(int id = 0) where T : ICom
        {
            m_Owner.RemoveCom<T>(id);
        }

        public void RemoveCom(Type type, int id = 0)
        {
            m_Owner.RemoveCom(type, id);
        }

        public void ClearCom()
        {
            m_Owner.ClearCom();
        }

        public bool HasData<T>()
        {
            return m_Owner.HasData<T>();
        }

        public bool HasData<T>(string name)
        {
            return m_Owner.HasData<T>(name);
        }

        public void SetData<T>(T value)
        {
            m_Owner.SetData(value);
        }

        public T GetData<T>()
        {
            return m_Owner.GetData<T>();
        }

        public void SetData<T>(string name, T value)
        {
            m_Owner.SetData(name, value);
        }

        public T GetData<T>(string name)
        {
            return m_Owner.GetData<T>(name);
        }

        public void ClearData()
        {
            m_Owner.ClearData();
        }

        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Owner.GetEnumerator();
        }

        public void SetIt(XItType type)
        {
            m_Owner.SetIt(type);
        }
    }
}
