using XFrame.Collections;
using XFrame.Modules.Archives;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演类(非阻塞式)
    /// </summary>
    [Director]
    public class NonBlockDirector : IDirector
    {
        private const string NAME = "nonblock_director";
        private JsonArchive m_Archive;
        private XLinkList<StoryInfo> m_Stories;

        void IDirector.OnInit()
        {
            m_Stories = new XLinkList<StoryInfo>();
            m_Archive = ArchiveModule.Inst.GetOrNew<JsonArchive>(NAME);
            InnerPlay(PlotUtility.InnerRestoreStories(m_Archive));
        }

        void IDirector.OnUpdate()
        {
            foreach (XLinkNode<StoryInfo> story in m_Stories)
            {
                StoryInfo item = story.Value;
                switch (item.State)
                {
                    case StoryState.WaitStart:
                        item.State = StoryState.WaitRunning;
                        break;

                    case StoryState.WaitRunning:
                        item.Story.OnStart();
                        item.State = StoryState.Running;
                        break;

                    case StoryState.Running:
                        item.Story.OnUpdate();
                        if (item.Story.IsFinish)
                            item.State = StoryState.Complete;
                        break;

                    case StoryState.Complete:
                        item.Story.OnDestroy();
                        story.Delete();
                        break;
                }
            }
        }

        void IDirector.OnDestory()
        {
            m_Stories.Clear();
        }

        public void Play(IStory story)
        {
            PlotUtility.InnerRecordStoryState(m_Archive, story);
            InnerPlay(story);
        }

        public void Play(IStory[] stories)
        {
            foreach (IStory story in stories)
                Play(story);
        }

        private void InnerPlay(IStory story)
        {
            string saveName = PlotUtility.InnerGetStorySaveName(story.Name);
            JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>(saveName);
            PlotDataProvider binder = new PlotDataProvider(archive);
            m_Stories.AddLast(new StoryInfo(story, binder));
            story.OnInit(binder);
        }

        private void InnerPlay(IStory[] stories)
        {
            foreach (IStory story in stories)
                InnerPlay(story);
        }

        public void Remove(IStory story)
        {
            Remove(story.Name);
        }

        public void Remove(string storyName)
        {
            foreach (XLinkNode<StoryInfo> info in m_Stories)
            {
                if (info.Value.Story.Name == storyName)
                {
                    info.Delete();
                    break;
                }
            }
            PlotUtility.InnerRemoveStoryState(m_Archive, storyName);
        }
    }
}
