using XFrame.Modules.Archives;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演类(阻塞式)
    /// </summary>
    [Director(true)]
    public partial class BlockDirector : IDirector
    {
        private StoryInfo m_Current;
        private Queue<StoryInfo> m_StoryQueue;

        void IDirector.OnInit()
        {
            m_Current = null;
            m_StoryQueue = new Queue<StoryInfo>();
        }

        void IDirector.OnUpdate()
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
                    m_Current.Story.OnStart();
                    m_Current.State = StoryState.Running;
                    break;

                case StoryState.Running:
                    m_Current.Story.OnUpdate();
                    if (m_Current.Story.IsFinish)
                        m_Current.State = StoryState.Complete;
                    break;

                case StoryState.Complete:
                    m_Current.Story.OnDestroy();
                    m_Current = null;
                    break;
            }
        }

        void IDirector.OnDestory()
        {

        }

        public void Play(IStory story)
        {
            JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>(story.Name);
            PlotDataProvider binder = new PlotDataProvider(archive);
            m_StoryQueue.Enqueue(new StoryInfo(story, binder));
            story.OnInit(binder);
        }

        public void Play(IStory[] stories)
        {
            foreach (IStory story in stories)
                Play(story);
        }

        public void Remove(IStory story)
        {

        }

        public void Remove(string storyName)
        {

        }
    }
}
