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

        IEntity Parent { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <param name="parent">父实体</param>
        /// <param name="onData">实体就绪事件</param>
        void OnInit(int id, IEntity parent, OnEntityReady onData);
    }
}
