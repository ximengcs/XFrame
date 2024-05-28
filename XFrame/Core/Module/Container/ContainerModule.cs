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
using XFrame.Modules.Pools;

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

        public void GetAll(List<IContainer> list)
        {
            foreach (var entry in m_Containers)
                list.Add(entry.Value);
        }

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

        public IContainer Create(int id, IContainerSetting setting)
        {
            return InnerNew(setting.Type, id, setting.ModuleUpdate, setting.Master, setting.DataProvider);
        }

        public IContainer Create(IContainerSetting setting)
        {
            int id = GetUseModule<IIdModule>().Next();
            return InnerNew(setting.Type, id, setting.ModuleUpdate, setting.Master, setting.DataProvider);
        }

        private IContainer InnerNew(Type type, int id, bool updateTrusteeship, IContainer master, OnDataProviderReady onReady)
        {
            IContainer container = Domain.TypeModule.CreateInstance(type) as IContainer;
            container.OnInit(this, id, master, onReady);
            if (updateTrusteeship)
                m_Containers.Add(container.Id, container);
            Log.Debug(Log.Container, $"there is container added. {container.GetType().Name} {container.Id}");
            InnerDebugContainers();
            return container;
        }

        /// <inheritdoc/>
        public void Remove(IContainer container)
        {
            Log.Debug(Log.Container, $"there is container removed. {container.GetType().Name} {container.Id}");
            InnerRemoveRecursive(container);
            InnerDebugContainers();
        }

        private void InnerDebugContainers()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            foreach (IContainer child in m_Containers.Values)
            {
                sb.AppendLine($"{(child.Parent != null ? "-" : "·")} {child.GetType().Name} {child.Id} -> {(child.Parent != null ? child.Parent.Id.ToString() : "NULL")}");
                InnerCheckRecursive(child, sb, "\t");
            }
            Log.Debug(Log.Container, sb.ToString());
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
                    Log.Error(Log.Container, $"same container, {child.GetType().Name} {container.GetType().Name}");
                    continue;
                }
                sb.Append(tab);
                sb.AppendLine($"{child.GetType().Name} {child.Id} -> {(child.Parent != null ? child.Parent.Id.ToString() : "NULL")}");
                InnerCheckRecursive(child, sb, tab);
            }
        }

        private void InnerRemoveRecursive(IContainer container)
        {
            CommonPoolObject<List<IContainer>> cache = References.Require<CommonPoolObject<List<IContainer>>>();
            List<IContainer> cacheList;
            if (!cache.Valid)
            {
                cacheList = new List<IContainer>(8);
                cache.Target = cacheList;
            }
            else
            {
                cacheList = cache.Target;
            }

            foreach (IContainer child in container)
                cacheList.Add(child);
            foreach (IContainer child in cacheList)
                InnerRemoveRecursive(child);
            if (m_Containers.ContainsKey(container.Id))
            {
                m_Containers.Remove(container.Id);
                container.OnDestroy();
            }
            cacheList.Clear();
            References.Release(cache);
        }

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Cache = new List<IContainer>();
            m_Containers = new Dictionary<int, IContainer>();
        }

        /// <inheritdoc/>
        public void OnUpdate(double escapeTime)
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
