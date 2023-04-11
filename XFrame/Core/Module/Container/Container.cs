using System;
using XFrame.Modules.ID;
using XFrame.Collections;

namespace XFrame.Modules.Containers
{
    internal class Container : IXItem, IContainer
    {
        private object m_Owner;
        private XCollection<ICom> m_Coms;

        public int Id { get; private set; }

        public void OnInit(object owner)
        {
            m_Owner = owner;
            Id = GetHashCode();
            m_Coms = new XCollection<ICom>();
        }

        public void OnUpdate()
        {
            foreach (ICom com in m_Coms)
            {
                if (com.Active)
                    com.OnUpdate();
            }
        }

        public void OnDestroy()
        {
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

        public T Add<T>(int id = 0, object userData = null) where T : ICom
        {
            return (T)InnerAdd(typeof(T), id, userData);
        }

        public ICom Add(Type type, int id = 0, object userData = null)
        {
            return InnerAdd(type, id, userData);
        }

        public T GetOrAdd<T>(int id = 0, object userData = null) where T : ICom
        {
            return (T)GetOrAdd(typeof(T), id, userData);
        }

        public ICom GetOrAdd(Type type, int id = 0, object userData = null)
        {
            ICom com = m_Coms.Get(type, id);
            if (com != null)
                return com;
            else
                return InnerAdd(type, id, userData);
        }

        public void Remove<T>(int id = 0) where T : ICom
        {

        }

        public void Remove(Type type, int id = 0)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Dispatch(Action<ICom> com)
        {
            throw new NotImplementedException();
        }

        private ICom InnerAdd(Type type, int id, object userData)
        {
            if (m_Coms.Get(type, id) != null)
                id = IdModule.Inst.Next();

            ICom newCom = (ICom)Activator.CreateInstance(type);
            newCom.OnInit(id, m_Owner, userData);
            m_Coms.Add(newCom);
            return newCom;
        }
    }
}
