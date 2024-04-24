
namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事辅助器
    /// </summary>
    public interface IStoryHelper
    {
        /// <summary>
        /// 故事被创建
        /// </summary>
        /// <param name="story">故事</param>
        void OnStoryInit(IStory story);

        /// <summary>
        /// 故事开始
        /// </summary>
        /// <param name="story">故事</param>
        void OnStoryStart(IStory story);

        /// <summary>
        /// 故事更新
        /// </summary>
        /// <param name="story">故事</param>
        void OnStoryUpdate(IStory story);

        /// <summary>
        /// 故事完成
        /// </summary>
        /// <param name="story">故事</param>
        void OnStoryFinish(IStory story);

        /// <summary>
        /// 故事销毁
        /// </summary>
        /// <param name="story">故事</param>
        void OnStoryDestory(IStory story);
    }
}
