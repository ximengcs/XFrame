﻿using System;
using XFrame.Collections;
using XFrame.Modules.Pools;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 共享组件基类, 会共享容器数据
    /// </summary>
    public abstract class ShareCom : ContainerBase, ICom
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
        IContainer ICom.Owner { get; set; }
        public object Master { get; private set; }

        void IContainer.OnInit(int id, object master, OnDataProviderReady onReady)
        {
            if (Status == Container.State.Using)
            {
                Log.Warning("XFrame", $"container {GetType().Name} state is {Status}, but enter OnInit.");
                return;
            }

            Id = id;
            Master = master;
            Status = Container.State.Using;
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

        void IPoolObject.OnRequest()
        {
            OnReleaseFromPool();
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

        public ICom AddCom(ICom com, int id = 0, OnDataProviderReady onReady = null)
        {
            return m_Owner.AddCom(com, id, onReady);
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
