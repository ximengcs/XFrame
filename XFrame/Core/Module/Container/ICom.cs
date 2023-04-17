using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器组件
    /// </summary>
    public interface ICom : IXItem, IContainer
    {
        /// <summary>
        /// 是否处于激活状态
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// 组件共享数据提供器
        /// </summary>
        IDataProvider ShareData { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="container">所属容器</param>
        /// <param name="id">组件Id</param>
        /// <param name="owner">组件拥有者</param>
        internal void OnInit(IContainer container, int id, object owner, OnContainerReady onReady);

        /// <summary>
        /// 更新生命周期
        /// </summary>
        internal void OnUpdate();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        internal void OnDestroy();
    }
}
