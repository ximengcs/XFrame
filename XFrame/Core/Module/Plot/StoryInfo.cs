
namespace XFrame.Modules.Plots
{
    internal class StoryInfo
    {
        public Story Story;
        public PlotDataBinder m_Binder;
        public StoryState State;

        public StoryInfo(IStory story, PlotDataBinder binder)
        {
            Story = (Story)story;
            m_Binder = binder;
            State = m_Binder.Finish ? StoryState.Complete : StoryState.WaitStart;
        }
    }
}
