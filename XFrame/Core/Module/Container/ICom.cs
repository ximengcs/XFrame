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

        internal void OnInit(int id, IContainer owner, OnComReady onReady);
    }
}
