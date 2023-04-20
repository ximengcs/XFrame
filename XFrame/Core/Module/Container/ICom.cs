using XFrame.Modules.Entities;

namespace XFrame.Modules.Containers
{
    /// <summary>
    /// 容器组件
    /// </summary>
    public interface ICom : IContainer
    {
        /// <summary>
        /// 是否处于激活状态
        /// </summary>
        bool Active { get; set; }

        /// <summary>
        /// 组件拥有者
        /// </summary>
        IContainer Owner { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="id">组件Id</param>
        /// <param name="owner">组件容器</param>
        /// <param name="onReady">组件就绪事件</param>
        internal void OnInit(int id, IContainer owner, OnComReady onReady);
    }
}
