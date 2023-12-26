using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演类(阻塞式), 数据非持久化
    /// </summary>
    [Director(true)]
    public partial class BlockDirector : IDirector, ICanInitialize, IUpdater, ICanDestroy, ICanCreateData
    {
        private StoryInfo m_Current;
        private List<StoryInfo> m_StoryQueue;

        void ICanInitialize.OnInit()
        {
            m_Current = null;
            m_StoryQueue = new List<StoryInfo>();
        }

        void IUpdater.OnUpdate(float escapeTime)
        {
            if (m_Current == null && m_StoryQueue.Count > 0)
            {
                m_Current = m_StoryQueue[0];
                m_StoryQueue.RemoveAt(0);
            }
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
                    InnerCompleteCurrent();
                    break;
            }
        }

        IPlotDataProvider ICanCreateData.CreateDataProvider(IStory story)
        {
            return new PlotDataProvider();
        }

        private void InnerCompleteCurrent()
        {
            m_Current.Destroy();
            Remove(m_Current.Story);
            m_Current = null;
        }

        void ICanDestroy.OnDestroy()
        {
            m_Current = null;
            m_StoryQueue = null;
        }

        public void Play(IStory story)
        {
            InnerPlay(story);
        }

        private void InnerPlay(IStory story)
        {
            m_StoryQueue.Add(new StoryInfo(story));
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
            if (m_Current.Story.Name == storyName)
            {
                InnerCompleteCurrent();
                return;
            }

            for (int i = 0; i < m_StoryQueue.Count; i++)
            {
                if (m_StoryQueue[i].Story.Name == storyName)
                {
                    m_StoryQueue.RemoveAt(i);
                    return;
                }
            }
        }
    }
}
