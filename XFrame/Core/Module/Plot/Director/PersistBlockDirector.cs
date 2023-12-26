using XFrame.Modules.Archives;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演类(阻塞式), 数据持久化
    /// </summary>
    [Director(true)]
    public partial class PersistBlockDirector : IDirector, ICanInitialize, IUpdater, ICanDestroy, ICanCreateData
    {
        private const string NAME = "block_director";
        private JsonArchive m_Archive;
        private StoryInfo m_Current;
        private Queue<StoryInfo> m_StoryQueue;

        void ICanInitialize.OnInit()
        {
            m_Current = null;
            m_StoryQueue = new Queue<StoryInfo>();
            m_Archive = XModule.Archive.GetOrNew<JsonArchive>(NAME);
            InnerPlay(PlotUtility.InnerRestoreStories(m_Archive));
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            if (m_Current == null && m_StoryQueue.Count > 0)
                m_Current = m_StoryQueue.Dequeue();
            if (m_Current == null)
                return;
            switch (m_Current.State)
            {
                case StoryState.WaitStart:
                    m_Current.State = StoryState.WaitRunning;
                    break;

                case StoryState.WaitRunning:
                    m_Current.Start();
                    m_Current.State = StoryState.Running;
                    break;

                case StoryState.Running:
                    m_Current.Update();
                    if (m_Current.Story.IsFinish)
                    {
                        m_Current.State = StoryState.Complete;
                        m_Current.Finish();
                    }
                    break;

                case StoryState.Complete:
                    m_Current.Destroy();
                    Remove(m_Current.Story);
                    m_Current = null;
                    break;
            }
        }

        void ICanDestroy.OnDestroy()
        {
            m_Current = null;
            m_StoryQueue = null;
        }

        public void Play(IStory story)
        {
            PlotUtility.InnerRecordStoryState(m_Archive, story);
            InnerPlay(story);
        }

        IPlotDataProvider ICanCreateData.CreateDataProvider(IStory story)
        {
            string saveName = PlotUtility.InnerGetStorySaveName(story.Name);
            JsonArchive archive = XModule.Archive.GetOrNew<JsonArchive>(saveName);
            return new PersistPlotDataProvider(archive);
        }

        private void InnerPlay(IStory story)
        {
            m_StoryQueue.Enqueue(new StoryInfo(story));
        }

        private void InnerPlay(IStory[] stories)
        {
            foreach (IStory story in stories)
                InnerPlay(story);
        }

        public void Play(IStory[] stories)
        {
            foreach (IStory story in stories)
                Play(story);
        }

        public void Remove(IStory story)
        {
            Remove(story.Name);
        }

        public void Remove(string storyName)
        {
            PlotUtility.InnerRemoveStoryState(m_Archive, storyName);
        }
    }
}
