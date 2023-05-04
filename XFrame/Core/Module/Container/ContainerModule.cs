using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器类模块
    /// </summary>
    [XModule]
    public partial class ContainerModule : SingletonModule<ContainerModule>
    {
        private IPool<Container> m_Pool;
        private XCollection<IContainer> m_Containers;

        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <param name="owner">容器拥有者</param>
        /// <returns>容器实例</returns>
        public IContainer New(object owner = null, OnContainerReady onReady = null)
        {
            m_Pool.Require(out Container c);
            IContainer container = c;
            container.OnInit(IdModule.Inst.Next(), owner, onReady);
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
                container.OnDestroy();
                m_Containers.Remove(container);
                m_Pool.Release(container);
            }
        }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Containers = new XCollection<IContainer>();
            m_Pool = PoolModule.Inst.GetOrNew<Container>();
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
                container.OnDestroy();
            m_Containers.Clear();
        }
    }
}
