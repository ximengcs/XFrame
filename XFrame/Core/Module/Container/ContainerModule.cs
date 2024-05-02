using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.ID;
using XFrame.Modules.Entities;

namespace XFrame.Modules.Containers
{
    /// <inheritdoc/>
    [CommonModule]
    [XType(typeof(IContainerModule))]
    public partial class ContainerModule : ModuleBase, IContainerModule
    {
        private XCollection<IContainer> m_Containers;

        /// <inheritdoc/>
        public IContainer Get(int id)
        {
            return m_Containers.Get<IEntity>(id);
        }

        /// <inheritdoc/>
        public T New<T>(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null) where T : IContainer
        {
            return (T)InnerNew(typeof(T), Domain.GetModule<IIdModule>().Next(), updateTrusteeship, master, onReady);
        }

        /// <inheritdoc/>
        public Container New(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return (Container)InnerNew(typeof(Container), Domain.GetModule<IIdModule>().Next(), updateTrusteeship, master, onReady);
        }

        /// <inheritdoc/>
        public IContainer New(Type type, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, Domain.GetModule<IIdModule>().Next(), updateTrusteeship, master, onReady);
        }

        /// <inheritdoc/>
        public IContainer New(Type type, int id, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, id, updateTrusteeship, master, onReady);
        }

        private IContainer InnerNew(Type type, int id, bool updateTrusteeship, IContainer master, OnDataProviderReady onReady)
        {
            IContainer container = Domain.TypeModule.CreateInstance(type) as IContainer;
            container.OnInit(this, id, master, onReady);
            if (updateTrusteeship)
                m_Containers.Add(container);
            return container;
        }

        /// <inheritdoc/>
        public void Remove(IContainer container)
        {
            if (m_Containers.Contains(container))
            {
                m_Containers.Remove(container);
                container.OnDestroy();
            }
        }

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Containers = new XCollection<IContainer>(Domain);
        }

        /// <inheritdoc/>
        public void OnUpdate(float escapeTime)
        {
            foreach (IContainer container in m_Containers)
            {
                if (container.Active)
                    container.OnUpdate(escapeTime);
            }
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (IContainer container in m_Containers)
            {
                if (container == null)
                    continue;
                m_Containers.Remove(container);
                container.OnDestroy();
            }
            m_Containers.Clear();
        }
    }
}
