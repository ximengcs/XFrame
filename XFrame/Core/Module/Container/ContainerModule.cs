using XFrame.Core;
using XFrame.Modules.ID;
using XFrame.Collections;
using XFrame.Modules.Pools;
using System;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器类模块
    /// </summary>
    [XModule]
    public partial class ContainerModule : SingletonModule<ContainerModule>
    {
        private XCollection<IContainer> m_Containers;

        /// <summary>
        /// 请求一个新的容器
        /// </summary>
        /// <param name="owner">容器拥有者</param>
        /// <returns>容器实例</returns>
        public T New<T>(object owner = null, OnDataProviderReady onReady = null) where T : IContainer
        {
            return (T)InnerNew(typeof(T), owner, onReady);
        }

        public Container New(object owner = null, OnDataProviderReady onReady = null)
        {
            return (Container)InnerNew(typeof(Container), owner, onReady);
        }

        public IContainer New(Type type, object owner = null, OnDataProviderReady onReady = null)
        {
            return InnerNew(type, owner, onReady);
        }

        private IContainer InnerNew(Type type, object owner, OnDataProviderReady onReady)
        {
            IPool pool = PoolModule.Inst.GetOrNew(type);
            IPoolObject obj = pool.Require();
            IContainer container = obj as IContainer;
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
                IPool pool = PoolModule.Inst.GetOrNew(container.GetType());
                pool.Release(container);
            }
        }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Containers = new XCollection<IContainer>();
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
                container.OnDestroy();
                IPool pool = PoolModule.Inst.GetOrNew(container.GetType());
                pool.Release(container);
            }
            m_Containers.Clear();
        }
    }
}
