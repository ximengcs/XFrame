using XFrame.Utility;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public partial class Director : IDirector
    {
        private StoryInfo m_CurBlock;
        private List<StoryInfo> m_Stories;
        private Queue<StoryInfo> m_BlockQueue;
        private List<StoryInfo> m_NonBlockQueue;

        public Director()
        {
            m_Stories = new List<StoryInfo>();
            m_BlockQueue = new Queue<StoryInfo>();
            m_NonBlockQueue = new List<StoryInfo>();
        }

        public void OnUpdate()
        {
            for (int i = m_Stories.Count - 1; i >= 0; i--)
            {
                StoryInfo info = m_Stories[i];
                if (info.State == StoryState.WaitRunning)
                {
                    InnerStoryReady(info);
                    m_Stories.RemoveAt(i);
                }
            }

            if (m_CurBlock == null && m_BlockQueue.Count > 0)
                m_CurBlock = m_BlockQueue.Dequeue();

            if (InnerUpdateStory(m_CurBlock))
                m_CurBlock = null;
            for (int i = m_NonBlockQueue.Count - 1; i >= 0; i--)
            {
                if (InnerUpdateStory(m_NonBlockQueue[i]))
                    m_NonBlockQueue.RemoveAt(i);
            }
        }

        public void OnDestory()
        {

        }

        public void Add(IStory story)
        {
            JsonArchive archive = ArchiveModule.Inst.GetOrNew<JsonArchive>(story.Name);
            PlotDataBinder binder = new PlotDataBinder(archive);
            m_Stories.Add(new StoryInfo(story, binder));
        }

        private bool InnerUpdateStory(StoryInfo info)
        {
            if (info == null)
                return true;

            switch (info.State)
            {
                case StoryState.WaitRunning:
                    info.Story.OnStart();
                    info.State = StoryState.Running;
                    break;

                case StoryState.Running:
                    info.Story.OnUpdate();
                    if (info.Story.IsFinish)
                        info.State = StoryState.Complete;
                    break;

                case StoryState.Complete:
                    info.End();
                    return true;
            }

            return false;
        }

        private void InnerStoryReady(StoryInfo info)
        {
            info.Story.OnInit(info.m_Binder);
            if (TypeUtility.HasAttribute<HangUpableAttribute>(info.Story))
                m_NonBlockQueue.Add(info);
            else
                m_BlockQueue.Enqueue(info);
        }
    }
}
