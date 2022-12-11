
namespace XFrame.Modules
{
    public partial class Director
    {
        private class StoryInfo
        {
            public Story Story;
            public PlotDataBinder m_Binder;
            public StoryState State;

            public StoryInfo(IStory story, PlotDataBinder binder)
            {
                Story = (Story)story;
                m_Binder = binder;
                State = m_Binder.Finish ? StoryState.Complete : StoryState.WaitStart;

                InnerOnOpen();
            }

            public void End()
            {
                m_Binder.Finish.Value = true;
            }

            private void InnerOnOpen()
            {
                State = StoryState.WaitRunning;
            }
        }
    }
}
