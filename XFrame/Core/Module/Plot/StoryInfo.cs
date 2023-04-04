
namespace XFrame.Modules.Plots
{
    internal class StoryInfo
    {
        public IStory Story;
        public PlotDataProvider Binder;
        public StoryState State;

        public StoryInfo(IStory story, PlotDataProvider binder)
        {
            Story = story;
            Binder = binder;
            State = Binder.Finish ? StoryState.Complete : StoryState.WaitStart;
        }
    }
}
