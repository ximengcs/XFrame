﻿using XFrame.Collections;

namespace XFrame.Modules
{
    public abstract class Entity : IPoolObject, IXItem
    {
        #region Inner Field
        private EntityData m_Data;
        protected Scene m_Scene;
        protected Entity m_Parent;
        protected XCollection<Entity> m_Children;
        #endregion

        #region Life Fun
        internal virtual void OnInternalInit(int id, Scene scene, Entity parent, EntityData data)
        {
            Id = id;
            m_Parent = parent;
            m_Scene = scene;
            if (m_Children == null)
                m_Children = new XCollection<Entity>();
            OnInit(data);
        }

        internal virtual void OnInternalUpdate(float elapseTime)
        {
            OnUpdate(elapseTime);
            foreach (Entity child in m_Children)
                child.OnInternalUpdate(elapseTime);
        }

        internal virtual void OnInternalDestroy(bool onlyRoot)
        {
            foreach (Entity child in m_Children)
                child.OnInternalDestroy(onlyRoot);
            OnDestroy();

            m_Parent = null;
            m_Children = null;
        }

        public void OnCreate()
        {
        }

        public virtual void OnRelease()
        {
            foreach (Entity child in m_Children)
                child.OnRelease();

            m_Data = null;
            m_Scene = null;
            m_Parent = null;
        }

        public virtual void OnDestroyFrom()
        {

        }
        #endregion

        #region Sub Child Life Fun
        protected abstract void OnInit(EntityData data);

        protected abstract void OnUpdate(float elapseTime);

        protected abstract void OnDestroy();
        #endregion

        #region Interface
        public int Id { get; private set; }
        public int TypeId => m_Data.TypeId;
        public Scene Scene => m_Scene;
        public Entity Parent
        {
            get { return m_Parent; }
            private set
            {
                m_Parent = value;
            }
        }

        public Entity Add(Entity entity)
        {
            entity.Parent = this;
            m_Children.Add(entity);
            return entity;
        }

        public T Add<T>(EntityData data) where T : Entity
        {
            T entity = EntityModule.Inst.Create<T>(m_Scene, this, data);
            Add(entity);
            return entity;
        }

        public T Add<T>() where T : Entity
        {
            T entity = EntityModule.Inst.Create<T>(m_Scene, this);
            Add(entity);
            return entity;
        }

        public void Remove(Entity entity)
        {
            m_Children.Remove(entity);
        }

        public void Remove<T>(int entityId) where T : Entity
        {
            Entity entity = m_Children.Get<T>(entityId);
            if (entity != null)
                Remove(entity);
        }

        public T Get<T>() where T : Entity
        {
            return m_Children.Get<T>();
        }

        public T Get<T>(int entityId) where T : Entity
        {
            return m_Children.Get<T>(entityId);
        }

        protected T GetOwner<T>() where T : Entity
        {
            return m_Parent.Get<T>();
        }

        protected T GetOwner<T>(int entityId) where T : Entity
        {
            return m_Parent.Get<T>(entityId);
        }
        #endregion
    }
}
