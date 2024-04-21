using System;
using XFrame.Core;
using XFrame.Collections;
using XFrame.Modules.Reflection;
using XFrame.Modules.ID;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器类模块
    /// </summary>
    [CommonModule]
    [XType(typeof(IContainerModule))]
    public partial class ContainerModule : ModuleBase, IContainerModule
    {
        private IContainerHelper m_Helper;
        private XCollection<IContainer> m_Containers;

        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <param name="master">容器拥有者</param>
        /// <returns>容器实例</returns>
        public T New<T>(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null) where T : IContainer
        {
            return (T)InnerNew(typeof(T), Domain.GetModule<IIdModule>().Next(), updateTrusteeship, master, onReady);
        }

        public Container New(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return (Container)InnerNew(typeof(Container), Domain.GetModule<IIdModule>().Next(), updateTrusteeship, master, onReady);
        }

        public IContainer New(Type type, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, Domain.GetModule<IIdModule>().Next(), updateTrusteeship, master, onReady);
        }

        public IContainer New(Type type, int id, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, id, updateTrusteeship, master, onReady);
        }

        private IContainer InnerNew(Type type, int id, bool updateTrusteeship, IContainer master, OnDataProviderReady onReady)
        {
            int poolKey = m_Helper != null ? m_Helper.GetPoolKey(type, id, master) : 0;
            IContainer container = Domain.TypeModule.CreateInstance(type) as IContainer;
            container.OnInit(this, id, master, onReady);
            if (updateTrusteeship)
                m_Containers.Add(container);
            return container;
        }

        /// <summary>
        /// 移除一个容器
        /// </summary>
        /// <param name="container">容器</param>
        public void Remove(IContainer container)
        {
            if (m_Containers.Contains(container))
            {
                m_Containers.Remove(container);
                container.OnDestroy();
            }
        }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Containers = new XCollection<IContainer>(Domain);
            TypeSystem typeSys = Domain.TypeModule.GetOrNew<IContainerHelper>();
            foreach (Type type in typeSys)
            {
                m_Helper = (IContainerHelper)Domain.TypeModule.CreateInstance(type);
                break;
            }
        }

        public void OnUpdate(float escapeTime)
        {
            foreach (IContainer container in m_Containers)
                container.OnUpdate(escapeTime);
        }

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
