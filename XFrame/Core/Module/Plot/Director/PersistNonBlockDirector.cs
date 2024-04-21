using XFrame.Collections;
using XFrame.Core;
using XFrame.Modules.Archives;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演类(非阻塞式), 数据持久化
    /// </summary>
    [Director]
    public class PersistNonBlockDirector : IDirector
    {
        private const string NAME = "nonblock_director";
        private JsonArchive m_Archive;
        private XLinkList<StoryInfo> m_Stories;
        private IPlotModule m_Module;

        public IPlotModule Module => m_Module;

        void IDirector.OnInit(IPlotModule module)
        {
            m_Module = module;
            m_Stories = new XLinkList<StoryInfo>();
            m_Archive = m_Module.Domain.GetModule<ArchiveModule>().GetOrNew<JsonArchive>(NAME);
            InnerPlay(PlotUtility.InnerRestoreStories(m_Module, m_Archive));
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
                        item.Start();
                        item.State = StoryState.Running;
                        break;

                    case StoryState.Running:
                        item.Update();
                        if (item.Story.IsFinish)
                        {
                            item.State = StoryState.Complete;
                            item.Finish();
                        }
                        break;

                    case StoryState.Complete:
                        item.Destroy();
                        story.Delete();
                        Remove(item.Story);
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

        IPlotDataProvider IDirector.CreateDataProvider(IStory story)
        {
            string saveName = PlotUtility.InnerGetStorySaveName(story.Name);
            IArchiveModule module = m_Module.Domain.GetModule<ArchiveModule>();
            JsonArchive archive = module.GetOrNew<JsonArchive>(saveName);
            return new PersistPlotDataProvider(m_Module, archive);
        }

        private void InnerPlay(IStory story)
        {
            m_Stories.AddLast(new StoryInfo(story));
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
