
namespace XFrame.Modules.Plots
{
    internal class StoryInfo
    {
        public IStory Story;
        public IStoryHelper Helper;
        public StoryState State;

        public StoryInfo(IStory story)
        {
            Story = story;
            Helper = story.Helper;
            State = story.IsFinish ? StoryState.Complete : StoryState.WaitStart;

            Story.OnInit();
            Helper?.OnStoryInit(Story);
        }

        public void Start()
        {
            Story.OnStart();
            Helper?.OnStoryStart(Story);
        }

        public void Update()
        {
            Story.OnUpdate();
            Helper?.OnStoryUpdate(Story);
        }

        public void Finish()
        {
            Story.OnFinish();
            Helper?.OnStoryFinish(Story);
        }

        public void Destroy()
        {
            Story.OnDestroy();
            Helper?.OnStoryDestory(Story);
        }
    }
}
