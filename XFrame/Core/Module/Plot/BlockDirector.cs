using XFrame.Modules.Archives;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
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

            InnerUpdateStory();
        }

        void IDirector.OnDestory()
        {

        }

        public void Add(IStory story)
        {
            JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>(story.Name);
            PlotDataBinder binder = new PlotDataBinder(archive);
            m_StoryQueue.Enqueue(new StoryInfo(story, binder));
        }

        private bool InnerUpdateStory()
        {
            switch (m_Current.State)
            {
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
                    return true;
            }

            return false;
        }
    }
}
