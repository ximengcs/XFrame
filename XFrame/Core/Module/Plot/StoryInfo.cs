
namespace XFrame.Modules.Plots
{
    internal class StoryInfo
    {
        public IStory Story;
        public PlotDataBinder Binder;
        public StoryState State;

        public StoryInfo(IStory story, PlotDataBinder binder)
        {
            Story = story;
            Binder = binder;
            State = Binder.Finish ? StoryState.Complete : StoryState.WaitStart;
        }
    }
}
