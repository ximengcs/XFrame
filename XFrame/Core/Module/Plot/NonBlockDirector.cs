using XFrame.Collections;
using XFrame.Modules.Archives;

namespace XFrame.Modules.Plots
{
    [Director]
    public class NonBlockDirector : IDirector
    {
        private XLinkList<StoryInfo> m_Stories;

        void IDirector.OnInit()
        {
            m_Stories = new XLinkList<StoryInfo>();
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

        public void Add(IStory story)
        {
            JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>(story.Name);
            PlotDataBinder binder = new PlotDataBinder(archive);
            m_Stories.AddLast(new StoryInfo(story, binder));
            story.OnInit(binder);
        }

        public void Add(IStory[] stories)
        {
            foreach (IStory story in stories)
                Add(story);
        }

        public void Remove(IStory story)
        {
            foreach (XLinkNode<StoryInfo> info in m_Stories)
            {
                if (info.Value.Story == story)
                {
                    info.Delete();
                    break;
                }
            }
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
        }
    }
}
