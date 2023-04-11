using System;

namespace XFrame.Modules.Containers
{
    public abstract class Com : ICom
    {
        protected object m_Owner;
        protected object m_UserData;

        public bool Active { get; set; }

        public int Id { get; private set; }

        void ICom.OnInit(int id, object owner, object userData)
        {
            m_Owner = owner;
            m_UserData = userData;
            OnInit();
        }

        void ICom.OnUpdate()
        {
            OnUpdate();
        }

        void ICom.OnDestroy()
        {
            OnDestroy();
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate() { }
        protected virtual void OnDestroy() { }

        public T Add<T>() where T : ICom
        {
            throw new NotImplementedException();
        }

        public ICom Add(Type type, int id = 0)
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

        public T Get<T>(int id = 0) where T : ICom
        {
            throw new NotImplementedException();
        }

        public ICom Get(Type type, int id = 0)
        {
            throw new NotImplementedException();
        }

        public T GetOrAdd<T>(int id = 0) where T : ICom
        {
            throw new NotImplementedException();
        }

        public ICom GetOrAdd(Type type, int id = 0)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>() where T : ICom
        {
            throw new NotImplementedException();
        }

        public void Remove(Type type)
        {
            throw new NotImplementedException();
        }

        public void Remove(int id)
        {
            throw new NotImplementedException();
        }

    }
}
