﻿using System;
using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.ID;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 通用容器
    /// </summary>
    public partial class Container : IContainer
    {
        protected bool m_IsActive;
        private DataProvider m_Data;
        private XCollection<IContainer> m_Coms;

        /// <summary>
        /// 容器所属模块
        /// </summary>
        protected IContainerModule m_Module;

        /// <inheritdoc/>
        public IContainer Master { get; private set; }

        public IContainer Parent { get; private set; }

        public bool Active => m_IsActive;

        public virtual void SetActive(bool active, bool recursive = true)
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
                foreach (IContainer com in m_Coms)
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
            m_Coms = new XCollection<IContainer>(m_Module.Domain);

            Id = id;
            m_IsActive = true;
            if (master != null && master.Master != null)
                Master = master.Master;
            else
                Master = master;
            Parent = master;

            if (Parent != null)
            {
                Parent.AddCom(Parent);
            }

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
        public T GetCom<T>(int id = 0) where T : IContainer
        {
            return (T)InnerGetCom(typeof(T), id);
        }

        /// <inheritdoc/>
        public IContainer GetCom(Type type, int id = 0)
        {
            return InnerGetCom(type, id);
        }

        /// <inheritdoc/>
        public IContainer AddCom(IContainer com)
        {
            return InnerInitCom(com);
        }

        /// <inheritdoc/>
        public T AddCom<T>(OnDataProviderReady onReady = null) where T : IContainer
        {
            Type type = typeof(T);
            int id = InnerCheckId(type, default);
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : IContainer
        {
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public IContainer AddCom(Type type, OnDataProviderReady onReady = null)
        {
            int id = InnerCheckId(type, default);
            return InnerAdd(type, id, onReady);
        }

        /// <inheritdoc/>
        public IContainer AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return InnerAdd(type, id, onReady);
        }

        /// <inheritdoc/>
        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : IContainer
        {
            int id = InnerCheckId(typeof(T), default);
            return (T)InnerGetOrAddCom(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : IContainer
        {
            return (T)InnerGetOrAddCom(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        /// <inheritdoc/>
        public IContainer GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            int id = InnerCheckId(type, default);
            return InnerGetOrAddCom(type, id, onReady);
        }

        /// <inheritdoc/>
        public IContainer GetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return InnerGetOrAddCom(type, id, onReady);
        }

        /// <inheritdoc/>
        public void RemoveCom<T>(int id = 0) where T : IContainer
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
            m_Coms.Clear();
        }
        #endregion

        #region Inner Implement
        private void InnerRemove(Type type, int id)
        {
            IContainer com = m_Coms.Get(type, id);
            if (com != null)
            {
                m_Coms.Remove(com);
                com.OnDestroy();
            }
        }

        private IContainer InnerGetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            IContainer com = InnerGetCom(type, id);
            if (com != null)
                return com;
            else
                return InnerAdd(type, id, onReady);
        }

        private IContainer InnerAdd(Type type, int id, OnDataProviderReady onReady)
        {
            IContainer newCom = m_Module.New(type, id, true, this, onReady);
            return newCom;
        }

        private IContainer InnerInitCom(IContainer com)
        {
            Log.Debug($"Inner init com {Id} child {com.Id}");
            m_Coms.Add(com);
            Container container = com as Container;
            if (container != null)
                container.Parent = this;
            return com;
        }

        private int InnerCheckId(Type type, int id)
        {
            //if (m_Coms.Get(type, id) != null)
            //    id = m_Module.Domain.GetModule<IIdModule>().Next();
            return m_Module.Domain.GetModule<IIdModule>().Next();
        }

        private IContainer InnerGetCom(Type type, int id)
        {
            if (type.IsInterface || type.IsAbstract)
            {
                foreach (IContainer com in m_Coms)
                {
                    Type comType = com.GetType();
                    if (type.IsAssignableFrom(comType) && com.Id == id)
                        return com;
                }
            }
            else
            {
                if (id == default)
                    return m_Coms.Get(type);
                else
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
        public IEnumerator<IContainer> GetEnumerator()
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
