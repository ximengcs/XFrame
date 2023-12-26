﻿using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事导演类(非阻塞式), 数据非持久化
    /// </summary>
    [Director]
    public class NonBlockDirector : IDirector, ICanInitialize, IUpdater, ICanDestroy, ICanCreateData
    {
        private XLinkList<StoryInfo> m_Stories;

        void ICanInitialize.OnInit()
        {
            m_Stories = new XLinkList<StoryInfo>();
        }

        void IUpdater.OnUpdate(float escapeTime)
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

        IPlotDataProvider ICanCreateData.CreateDataProvider(IStory story)
        {
            return new PlotDataProvider();
        }

        void ICanDestroy.OnDestroy()
        {
            m_Stories.Clear();
        }

        public void Play(IStory story)
        {
            InnerPlay(story);
        }

        public void Play(IStory[] stories)
        {
            foreach (IStory story in stories)
                Play(story);
        }

        private void InnerPlay(IStory story)
        {
            m_Stories.AddLast(new StoryInfo(story));
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
        }
    }
}
