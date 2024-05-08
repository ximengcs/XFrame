using XFrame.Modules.Event;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    /// <summary>
    /// 实体
    /// </summary>
    public interface IEntity : IContainer
    {
        /// <summary>
        /// 实体事件系统
        /// </summary>
        IEventSystem Event { get; }

        /// <summary>
        /// 根实体
        /// </summary>
        new IEntity Master { get; }

        /// <summary>
        /// 父实体
        /// </summary>
        new IEntity Parent { get; }

    }
}
