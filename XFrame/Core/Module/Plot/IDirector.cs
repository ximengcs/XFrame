namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演
    /// </summary>
    public interface IDirector
    {
        /// <summary>
        /// 播放一组故事
        /// </summary>
        /// <param name="stories">故事列表</param>
        void Play(IStory[] stories);

        /// <summary>
        /// 播放一个故事
        /// </summary>
        /// <param name="story">故事</param>
        void Play(IStory story);

        /// <summary>
        /// 移除一个故事
        /// </summary>
        /// <param name="story">故事</param>
        void Remove(IStory story);

        /// <summary>
        /// 移除一个故事 
        /// </summary>
        /// <param name="storyName">故事名</param>
        void Remove(string storyName);

        /// <summary>
        /// 创建一个数据提供器
        /// </summary>
        /// <returns>数据提供器</returns>
        internal IPlotDataProvider CreateDataProvider(IStory story);

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        internal void OnInit();

        /// <summary>
        /// 更新生命周期
        /// </summary>
        internal void OnUpdate();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        internal void OnDestory();
    }
}
