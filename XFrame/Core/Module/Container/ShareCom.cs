using System;
using XFrame.Collections;
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

        /// <summary>
        /// 所属容器模块
        /// </summary>
        protected IContainerModule m_Module;

        /// <summary>
        /// 激活状态
        /// </summary>
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

        /// <inheritdoc/>
        public int Id { get; private set; }

        /// <inheritdoc/>
        public IContainer Master { get; private set; }

        IContainer ICom.Owner
        {
            get => m_Owner;
            set => m_Owner = value;
        }

        /// <summary>
        /// 父容器
        /// </summary>
        protected IContainer Owner => m_Owner;

        void IContainer.OnInit(IContainerModule module, int id, IContainer master, OnDataProviderReady onReady)
        {
            Id = id;
            m_Module = module;
            if (master != null && master.Master != null)
                Master = master.Master;
            else
                Master = master;

            onReady?.Invoke(this);
            OnInit();
        }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected internal virtual void OnInit() { }

        void IContainer.OnUpdate(float elapseTime)
        {
            OnUpdate(elapseTime);
        }

        /// <summary>
        /// 更新生命周期
        /// </summary>
        /// <param name="elapseTime">逃逸时间</param>
        protected internal virtual void OnUpdate(float elapseTime) { }

        void IContainer.OnDestroy()
        {
            OnDestroy();
        }

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        protected internal virtual void OnDestroy() { }

        /// <summary>
        /// 激活生命周期
        /// </summary>
        protected virtual void OnActive() { }

        /// <summary>
        /// 失活生命周期
        /// </summary>
        protected virtual void OnInactive() { }

        /// <inheritdoc/>
        public T GetCom<T>(int id = 0) where T : ICom
        {
            return m_Owner.GetCom<T>(id);
        }

        /// <inheritdoc/>
        public ICom GetCom(Type type, int id = 0)
        {
            return m_Owner.GetCom(type, id);
        }

        /// <inheritdoc/>
        public T AddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.AddCom<T>(onReady);
        }

        /// <inheritdoc/>
        public ICom AddCom(ICom com)
        {
            return m_Owner.AddCom(com);
        }

        /// <inheritdoc/>
        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.AddCom<T>(id, onReady);
        }

        /// <inheritdoc/>
        public ICom AddCom(Type type, OnDataProviderReady onReady = null)
        {
            return m_Owner.AddCom(type, onReady);
        }

        /// <inheritdoc/>
        public ICom AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return m_Owner.AddCom(type, id, onReady);
        }

        /// <inheritdoc/>
        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.GetOrAddCom<T>(onReady);
        }

        /// <inheritdoc/>
        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return m_Owner.GetOrAddCom<T>(id, onReady);
        }

        /// <inheritdoc/>
        public ICom GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            return m_Owner.GetOrAddCom(type, onReady);
        }

        /// <inheritdoc/>
        public ICom GetOrAddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return m_Owner.GetOrAddCom(type, id, onReady);
        }

        /// <inheritdoc/>
        public void RemoveCom<T>(int id = 0) where T : ICom
        {
            m_Owner.RemoveCom<T>(id);
        }

        /// <inheritdoc/>
        public void RemoveCom(Type type, int id = 0)
        {
            m_Owner.RemoveCom(type, id);
        }

        /// <inheritdoc/>
        public void ClearCom()
        {
            m_Owner.ClearCom();
        }

        /// <inheritdoc/>
        public bool HasData<T>()
        {
            return m_Owner.HasData<T>();
        }

        /// <inheritdoc/>
        public bool HasData<T>(string name)
        {
            return m_Owner.HasData<T>(name);
        }

        /// <inheritdoc/>
        public void SetData<T>(T value)
        {
            m_Owner.SetData(value);
        }

        /// <inheritdoc/>
        public T GetData<T>()
        {
            return m_Owner.GetData<T>();
        }

        /// <inheritdoc/>
        public void SetData<T>(string name, T value)
        {
            m_Owner.SetData(name, value);
        }

        /// <inheritdoc/>
        public T GetData<T>(string name)
        {
            return m_Owner.GetData<T>(name);
        }

        /// <inheritdoc/>
        public void ClearData()
        {
            m_Owner.ClearData();
        }

        /// <inheritdoc/>
        public IEnumerator<ICom> GetEnumerator()
        {
            return m_Owner.GetEnumerator();
        }

        /// <inheritdoc/>
        public void SetIt(XItType type)
        {
            m_Owner.SetIt(type);
        }
    }
}
