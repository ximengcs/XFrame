﻿using System;
using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.ID;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 通用容器
    /// </summary>
    public partial class Container : IContainer
    {
        private bool m_IsActive;
        private DataProvider m_Data;
        private XCollection<ICom> m_Coms;

        /// <summary>
        /// 容器所属模块
        /// </summary>
        protected IContainerModule m_Module;

        /// <inheritdoc/>
        public IContainer Master { get; private set; }

        public IContainer Parent { get; private set; }

        public bool Active => m_IsActive;

        public void SetActive(bool active, bool recursive = true)
        {
            if (active != m_IsActive)
            {
                m_IsActive = active;
                if (m_IsActive)
                    OnActive();
                else
                    OnInactive();
            }

            if (recursive)
            {
                foreach (ICom com in m_Coms)
                {
                    com.SetActive(active, recursive);
                }
            }
        }


        /// <summary>
        /// 激活生命周期
        /// </summary>
        protected virtual void OnActive() { }

        /// <summary>
        /// 失活生命周期
        /// </summary>
        protected virtual void OnInactive() { }


        /// <summary>
        /// 容器Id
        /// </summary>
        public int Id { get; private set; }

        #region Container Life Fun
        void IContainer.OnInit(IContainerModule module, int id, IContainer master, OnDataProviderReady onReady)
        {
            m_Module = module;
            m_Data = new DataProvider();
            m_Coms = new XCollection<ICom>(m_Module.Domain);

            Id = id;
            m_IsActive = true;
            if (master != null && master.Master != null)
                Master = master.Master;
            else
                Master = master;
            Parent = master;

            onReady?.Invoke(this);
            OnInit();
        }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected internal virtual void OnInit() { }

        void IContainer.OnUpdate(float elapseTime)
        {
            //m_Coms.SetIt(XItType.Forward);
            //foreach (ICom com in m_Coms)
            //{
            //    if (com.Active)
            //        com.OnUpdate(elapseTime);
            //}
            //OnUpdate(elapseTime);
        }

        /// <summary>
        /// 更新生命周期
        /// </summary>
        /// <param name="elapseTime">逃逸时间</param>
        protected internal virtual void OnUpdate(float elapseTime) { }

        void IContainer.OnDestroy()
        {
            ClearCom();
            OnDestroy();
        }

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        protected internal virtual void OnDestroy()
        {
            m_Coms = null;
            m_Data = null;
        }
        #endregion

        #region Container Interface
        /// <inheritdoc/>
        public T GetCom<T>(int id = 0) where T : ICom
        {
            return (T)InnerGetCom(typeof(T), id);
        }

        /// <inheritdoc/>
        public ICom GetCom(Type type, int id = 0)
        {
            return InnerGetCom(type, id);
        }

        /// <inheritdoc/>
        public ICom AddCom(ICom com)
        {
            return InnerInitCom(com);
        }

        /// <inheritdoc/>
        public T AddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            Type type = typeof(T);
            int id = InnerCheckId(type, default);
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public ICom AddCom(Type type, OnDataProviderReady onReady = null)
        {
            int id = InnerCheckId(type, default);
            return InnerAdd(type, id, onReady);
        }

        /// <inheritdoc/>
        public ICom AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return InnerAdd(type, id, onReady);
        }

        /// <inheritdoc/>
        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerGetOrAddCom(typeof(T), default, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerGetOrAddCom(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public ICom GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            return InnerGetOrAddCom(type, default, onReady);
        }

        /// <inheritdoc/>
        public ICom GetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return InnerGetOrAddCom(type, id, onReady);
        }

        /// <inheritdoc/>
        public void RemoveCom<T>(int id = 0) where T : ICom
        {
            InnerRemove(typeof(T), id);
        }

        /// <inheritdoc/>
        public void RemoveCom(Type type, int id = 0)
        {
            InnerRemove(type, id);
        }

        /// <inheritdoc/>
        public void ClearCom()
        {
            m_Coms.SetIt(XItType.Backward);
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
            ICom com = InnerGetCom(type, id);
            if (com != null)
                return com;
            else
                return InnerAdd(type, id, onReady);
        }

        private ICom InnerAdd(Type type, int id, OnDataProviderReady onReady)
        {
            ICom newCom = (ICom)m_Module.New(type, id, true, this, (db) =>
            {
                InnerInitCom((ICom)db);
                onReady?.Invoke(db);
            });
            return newCom;
        }

        private ICom InnerInitCom(ICom com)
        {
            m_Coms.Add(com);
            return com;
        }

        private int InnerCheckId(Type type, int id)
        {
            //if (m_Coms.Get(type, id) != null)
            //    id = m_Module.Domain.GetModule<IIdModule>().Next();
            return m_Module.Domain.GetModule<IIdModule>().Next();
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
        /// <inheritdoc/>
        public bool HasData<T>()
        {
            return m_Data.HasData<T>();
        }

        /// <inheritdoc/>
        public bool HasData<T>(string name)
        {
            return m_Data.HasData<T>(name);
        }

        /// <inheritdoc/>
        public void SetData<T>(T value)
        {
            m_Data.SetData<T>(value);
        }

        /// <inheritdoc/>
        public T GetData<T>()
        {
            return m_Data.GetData<T>();
        }

        /// <inheritdoc/>
        public void SetData<T>(string name, T value)
        {
            m_Data.SetData<T>(name, value);
        }

        /// <inheritdoc/>
        public T GetData<T>(string name)
        {
            return m_Data.GetData<T>(name);
        }

        /// <inheritdoc/>
        public void ClearData()
        {
            m_Data.ClearData();
        }
        #endregion

        /// <inheritdoc/>
        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Coms.GetEnumerator();
        }

        /// <inheritdoc/>
        public void SetIt(XItType type)
        {
            m_Coms.SetIt(type);
        }
    }
}
