using System;
using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 通用容器
    /// </summary>
    public partial class Container : IContainer
    {
        private DataProvider m_Data;
        private XCollection<ICom> m_Coms;

        public IContainer Master { get; private set; }
        public int Id { get; private set; }

        #region Container Life Fun
        void IContainer.OnInit(int id, IContainer master, OnDataProviderReady onReady)
        {
            m_Data = new DataProvider();
            m_Coms = new XCollection<ICom>();

            Id = id;
            if (master != null && master.Master != null)
                Master = master.Master;
            else
                Master = master;
            onReady?.Invoke(this);
            OnInit();
        }

        protected internal virtual void OnInit() { }

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

        protected internal virtual void OnUpdate(float elapseTime) { }

        void IContainer.OnDestroy()
        {
            ClearCom();
            OnDestroy();
        }

        protected internal virtual void OnDestroy()
        {
            m_Coms = null;
            m_Data = null;
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

        public ICom AddCom(ICom com)
        {
            return InnerInitCom(com);
        }

        public T AddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            Type type = typeof(T);
            int id = InnerCheckId(type, default);
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public T AddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public ICom AddCom(Type type, OnDataProviderReady onReady = null)
        {
            int id = InnerCheckId(type, default);
            return InnerAdd(type, id, onReady);
        }

        public ICom AddCom(Type type, int id, OnDataProviderReady onReady = null)
        {
            return InnerAdd(type, id, onReady);
        }

        public T GetOrAddCom<T>(OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerGetOrAddCom(typeof(T), default, (com) => onReady?.Invoke((T)com));
        }

        public T GetOrAddCom<T>(int id, OnDataProviderReady onReady = null) where T : ICom
        {
            return (T)InnerGetOrAddCom(typeof(T), id, (com) => onReady?.Invoke((T)com));
        }

        public ICom GetOrAddCom(Type type, OnDataProviderReady onReady = null)
        {
            return InnerGetOrAddCom(type, default, onReady);
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
            ICom newCom = (ICom)XModule.Container.New(type, id, false, this, (db) =>
            {
                InnerInitCom((ICom)db);
                onReady?.Invoke(db);
            });
            return newCom;
        }

        private ICom InnerInitCom(ICom com)
        {
            ICanSetOwner setCom = com as ICanSetOwner;
            if (setCom != null)
                setCom.SetOwner(this);
            com.Active = true;
            m_Coms.Add(com);
            return com;
        }

        private int InnerCheckId(Type type, int id)
        {
            if (m_Coms.Get(type, id) != null)
                id = XModule.Id.Next();
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
        public bool HasData<T>()
        {
            return m_Data.HasData<T>();
        }

        public bool HasData<T>(string name)
        {
            return m_Data.HasData<T>(name);
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
