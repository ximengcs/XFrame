
namespace XFrame.Modules.Plots
{
    public interface IStoryHelper
    {
        void OnStoryInit(IStory story);
        void OnStoryStart(IStory story);
        void OnStoryUpdate(IStory story);
        void OnStoryFinish(IStory story);
        void OnStoryDestory(IStory story);
    }
}
