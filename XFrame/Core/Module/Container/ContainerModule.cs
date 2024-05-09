﻿using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.ID;
using XFrame.Modules.Entities;
using System.Collections.Generic;
using XFrame.Modules.Caches;
using System.ComponentModel;
using XFrame.Modules.Diagnotics;
using System.Text;

namespace XFrame.Modules.Containers
{
    /// <inheritdoc/>
    [CommonModule]
    [XType(typeof(IContainerModule))]
    public partial class ContainerModule : ModuleBase, IContainerModule
    {
        //private XCollection<IContainer> m_Containers;
        private Dictionary<int, IContainer> m_Containers;
        private List<IContainer> m_Cache;

        /// <inheritdoc/>
        public IContainer Get(int id)
        {
            if (m_Containers.TryGetValue(id, out IContainer container))
            {
                return container;
            }
            else
            {
                return null;
            }
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
                m_Containers.Add(container.Id, container);
            Log.Debug($"there is container added.");
            InnerDebugContainers();
            return container;
        }

        /// <inheritdoc/>
        public void Remove(IContainer container)
        {
            InnerRemoveRecursive(container);
            Log.Debug($"there is container removed.");
            InnerDebugContainers();
        }

        private void InnerDebugContainers()
        {
            StringBuilder sb = new StringBuilder();
            foreach (IContainer child in m_Containers.Values)
            {
                sb.AppendLine($"- {child.GetType().Name} {child.Id}");
                InnerCheckRecursive(child, sb, "\t");
            }
            Log.Debug(sb.ToString());
        }

        private void InnerCheckRecursive(IContainer container, StringBuilder sb, string tab)
        {
            if (container is ShareContainer)
                return;
            tab += '\t';
            foreach (IContainer child in container)
            {
                if (child == container)
                {
                    Log.Error($"same container, {child.GetType().Name} {container.GetType().Name}");
                    continue;
                }
                sb.Append(tab);
                sb.AppendLine($"{child.GetType().Name} {child.Id}");
                InnerCheckRecursive(child, sb, tab);
            }
        }

        private void InnerRemoveRecursive(IContainer container)
        {
            foreach (IContainer child in container)
                InnerRemoveRecursive(child);
            if (m_Containers.ContainsKey(container.Id))
            {
                Log.Debug($"contians {container.GetType().Name} {container.Id}, will remove");
                m_Containers.Remove(container.Id);
                container.OnDestroy();
            }
            else
            {
                Log.Debug($"cannot contians {container.GetType().Name} {container.Id}");
            }
        }

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Cache = new List<IContainer>();
            m_Containers = new Dictionary<int, IContainer>();
        }

        /// <inheritdoc/>
        public void OnUpdate(float escapeTime)
        {
            m_Cache.Clear();
            m_Cache.AddRange(m_Containers.Values);
            foreach (IContainer container in m_Cache)
            {
                if (container.Active)
                    container.OnUpdate(escapeTime);
            }
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_Cache.Clear();
            m_Cache.AddRange(m_Containers.Values);
            var it = new ListExt.BackwardIt<IContainer>(m_Cache);
            while (it.MoveNext())
            {
                IContainer container = it.Current;
                if (container == null)
                    continue;
                m_Containers.Remove(container.Id);
                container.OnDestroy();
            }
            m_Containers.Clear();
        }
    }
}
