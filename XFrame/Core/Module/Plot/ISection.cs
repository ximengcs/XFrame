using XFrame.Core;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事情节
    /// </summary>
    public interface ISection
    {
        /// <summary>
        /// 所属故事
        /// </summary>
        IStory Story { get; }

        /// <summary>
        /// 数据提供器
        /// </summary>
        IDataProvider Data { get; }

        /// <summary>
        /// 情节是否结束
        /// </summary>
        bool IsDone { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="story">故事</param>
        /// <param name="data">数据提供器</param>
        void OnCreate(IStory story, IDataProvider data);

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        void OnInit();

        /// <summary>
        /// 是否可以开始播放
        /// </summary>
        /// <returns>true表示此情节可以开始播放</returns>
        bool CanStart();

        /// <summary>
        /// 开始生命周期
        /// </summary>
        void OnStart();

        /// <summary>
        /// 更新生命周期
        /// </summary>
        void OnUpdate();

        /// <summary>
        /// 完成生命周期
        /// </summary>
        /// <returns>返回true表示已处理完完成后的清理工作</returns>
        bool OnFinish();
    }
}
