
using XFrame.Core;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件模块
    /// </summary>
    public interface IEventModule : IModule, IUpdater
    {
        /// <summary>
        /// 创建一个事件系统
        /// </summary>
        /// <returns>事件系统</returns>
        IEventSystem NewSys();

        /// <summary>
        /// 移除一个事件系统 
        /// </summary>
        /// <param name="evtSys">事件系统</param>
        void Remove(IEventSystem evtSys);
    }
}
