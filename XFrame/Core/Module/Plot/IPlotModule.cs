using System;
using XFrame.Core;
using XFrame.Modules.Event;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事模块
    /// </summary>
    public interface IPlotModule : IModule, IUpdater
    {
        /// <summary>
        /// 事件系统
        /// </summary>
        IEventSystem Event { get; }

        /// <summary>
        /// 故事处理辅助类
        /// </summary>
        IPlotHelper Helper { get; }

        /// <summary>
        /// 请求一个新故事
        /// </summary>
        /// <param name="targetDirector">播放导演</param>
        /// <param name="helperType">辅助器类型</param>
        /// <param name="name">故事名</param>
        /// <returns>故事</returns>
        IStory NewStory(Type targetDirector, Type helperType, string name = null);

        /// <summary>
        /// 请求一个新故事
        /// </summary>
        /// <param name="targetDirector">播放导演</param>
        /// <param name="name">故事名</param>
        /// <returns>故事</returns>
        IStory NewStory(Type targetDirector, string name = null);
    }
}
