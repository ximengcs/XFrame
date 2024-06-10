using System;
using XFrame.Core;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 情节故事
    /// </summary>
    public interface IStory : IDataProvider
    {
        /// <summary>
        /// 所属导演类
        /// </summary>
        IDirector Director { get; }

        /// <summary>
        /// 故事辅助器
        /// </summary>
        IStoryHelper Helper { get; }

        /// <summary>
        /// 故事情节
        /// </summary>
        IEnumerable<ISection> Sections { get; }

        /// <summary>
        /// 故事情节数量
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 故事名称
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 故事是否完成
        /// </summary>
        bool IsFinish { get; set; }

        /// <summary>
        /// 添加一个故事情节
        /// </summary>
        /// <param name="type">故事情节实现类</param>
        /// <returns>故事</returns>
        ISection AddSection(Type type);

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        internal void OnInit();

        /// <summary>
        /// 开始生命周期
        /// </summary>
        internal void OnStart();

        /// <summary>
        /// 更新生命周期
        /// </summary>
        internal void OnUpdate();

        /// <summary>
        /// 完成生命周期
        /// </summary>
        internal void OnFinish();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        internal void OnDestroy();
    }
}
