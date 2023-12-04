using System;
using System.Collections.Generic;
using XFrame.Core;

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

        IStoryHelper Helper { get; }

        IEnumerable<ISection> Sections { get; }

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
        /// <param name="data">数据提供器</param>
        internal void OnInit();

        /// <summary>
        /// 开始生命周期
        /// </summary>
        internal void OnStart();

        /// <summary>
        /// 更新生命周期
        /// </summary>
        internal void OnUpdate();

        internal void OnFinish();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        internal void OnDestroy();
    }
}
