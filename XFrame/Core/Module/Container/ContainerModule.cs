using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;
using System;
using XFrame.Modules.XType;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器类模块
    /// </summary>
    [XModule]
    public partial class ContainerModule : SingletonModule<ContainerModule>
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
            return (T)InnerNew(typeof(T), IdModule.Inst.Next(), updateTrusteeship, master, onReady);
        }

        public Container New(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return (Container)InnerNew(typeof(Container), IdModule.Inst.Next(), updateTrusteeship, master, onReady);
        }

        public IContainer New(Type type, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, IdModule.Inst.Next(), updateTrusteeship, master, onReady);
        }

        public IContainer New(Type type, int id, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, id, updateTrusteeship, master, onReady);
        }

        private IContainer InnerNew(Type type, int id, bool updateTrusteeship, IContainer master, OnDataProviderReady onReady)
        {
            IPool pool = PoolModule.Inst.GetOrNew(type);
            int poolKey = m_Helper != null ? m_Helper.GetPoolKey(type, id, master) : 0;
            IPoolObject obj = pool.Require(poolKey);
            IContainer container = obj as IContainer;
            container.OnInit(id, master, onReady);
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
                IPool pool = PoolModule.Inst.GetOrNew(container.GetType());
                pool.Release(container);
            }
        }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Containers = new XCollection<IContainer>();
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IContainerHelper>();
            foreach (Type type in typeSys)
            {
                m_Helper = (IContainerHelper)TypeModule.Inst.CreateInstance(type);
                break;
            }
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
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
                IPool pool = PoolModule.Inst.GetOrNew(container.GetType());
                pool.Release(container);
            }
            m_Containers.Clear();
        }
    }
}
