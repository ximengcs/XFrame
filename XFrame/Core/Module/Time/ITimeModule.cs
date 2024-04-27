
using XFrame.Core;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// 时间模块
    /// </summary>
    public interface ITimeModule : IModule, IUpdater
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        float Time { get; }

        /// <summary>
        /// 上帧到此帧逃逸时间
        /// </summary>
        float EscapeTime { get; }

        /// <summary>
        /// 当前帧数
        /// </summary>
        long Frame { get; }

        /// <summary>
        /// 获取所有注册的计时器
        /// </summary>
        /// <returns>计时器</returns>
        CDTimer[] GetTimers();
    }
}
