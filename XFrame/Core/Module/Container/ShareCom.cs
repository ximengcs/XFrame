using System;
using XFrame.Collections;
using XFrame.Modules.Pools;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 共享组件基类, 会共享容器数据
    /// </summary>
    public abstract class ShareCom : ICom
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
        public IContainer Owner => m_Owner;
        public object Master => m_Owner.Master;

        void ICom.OnInit(int id, IContainer owner, OnComReady onReady)
        {
            m_Owner = owner;
            IContainer thisContainer = this;
            thisContainer.OnInit(id, m_Owner.Master, (c) =>
            {
                onReady?.Invoke(this);
                Active = true;
            });
        }

        void IContainer.OnInit(int id, object master, OnContainerReady onReady)
        {
            Id = id;
            onReady?.Invoke(this);
            OnInit();
        }

        void IContainer.OnUpdate(float elapseTime)
        {
            OnUpdate(elapseTime);
        }

        void IContainer.OnDestroy()
        {
            OnDestroy();
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

        protected virtual void OnActive() { }
        protected virtual void OnInactive() { }
        protected virtual void OnInit() { }
        protected virtual void OnUpdate(float elapseTime) { }
        protected virtual void OnDestroy() { }
        protected virtual void OnCreateFromPool() { }
        protected virtual void OnDestroyFromPool() { }
        protected virtual void OnReleaseFromPool() { }

        public T GetCom<T>(int id = 0) where T : ICom
        {
            return m_Owner.GetCom<T>(id);
        }

        public ICom GetCom(Type type, int id = 0)
        {
            return m_Owner.GetCom(type, id);
        }

        public T AddCom<T>(OnComReady<T> onReady = null) where T : ICom
        {
            return m_Owner.AddCom(onReady);
        }

        public ICom AddCom(ICom com, int id = 0, OnComReady onReady = null)
        {
            return m_Owner.AddCom(com, id, onReady);
        }

        public T AddCom<T>(int id, OnComReady<T> onReady = null) where T : ICom
        {
            return m_Owner.AddCom(id, onReady);
        }

        public ICom AddCom(Type type, OnComReady onReady = null)
        {
            return m_Owner.AddCom(type, onReady);
        }

        public ICom AddCom(Type type, int id, OnComReady onReady = null)
        {
            return m_Owner.AddCom(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnComReady<T> onReady = null) where T : ICom
        {
            return m_Owner.GetOrAddCom(onReady);
        }

        public T GetOrAddCom<T>(int id, OnComReady<T> onReady = null) where T : ICom
        {
            return m_Owner.GetOrAddCom(id, onReady);
        }

        public ICom GetOrAddCom(Type type, OnComReady onReady = null)
        {
            return m_Owner.GetOrAddCom(type, onReady);
        }

        public ICom GetOrAddCom(Type type, int id, OnComReady onReady = null)
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

        public void DispatchCom(OnComReady handle)
        {
            m_Owner.DispatchCom(handle);
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

        public void Dispose()
        {
            m_Owner.Dispose();
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
