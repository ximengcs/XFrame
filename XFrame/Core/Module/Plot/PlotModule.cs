using System;
using XFrame.Core;
using XFrame.Modules.Reflection;
using XFrame.Modules.Event;
using XFrame.Modules.Config;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事模块
    /// </summary>
    [CommonModule]
    [RequireModule(typeof(EventModule))]
    [XType(typeof(IPlotModule))]
    public class PlotModule : ModuleBase, IPlotModule
    {
        #region Inner Fields
        private IPlotHelper m_Helper;
        private IDirector m_DefaultDirector;
        private Dictionary<Type, IStoryHelper> m_StoryHelpers;
        private Dictionary<Type, IDirector> m_Directors;
        #endregion

        #region Interface
        public IEventSystem Event { get; private set; }

        /// <summary>
        /// 故事处理辅助类
        /// </summary>
        public IPlotHelper Helper => m_Helper;

        /// <summary>
        /// 请求一个新故事
        /// </summary>
        /// <param name="name">故事名</param>
        /// <returns>故事</returns>
        public IStory NewStory(Type targetDirectorType, Type helperType, string name = null)
        {
            Story story = null;
            if (m_Directors.TryGetValue(targetDirectorType, out IDirector director))
            {
                IStoryHelper storyHelper = null;
                if (helperType != null)
                    m_StoryHelpers.TryGetValue(helperType, out storyHelper);
                story = new Story(director, storyHelper, name);
            }
            return story;
        }

        public IStory NewStory(Type targetDirectorType, string name = null)
        {
            return NewStory(targetDirectorType, null, name);
        }
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_StoryHelpers = new Dictionary<Type, IStoryHelper>();
            m_Directors = new Dictionary<Type, IDirector>();
            Event = Domain.GetModule<IEventModule>().NewSys();

            TypeSystem typeSys = Domain.TypeModule.GetOrNewWithAttr<DirectorAttribute>();
            foreach (Type type in typeSys)
            {
                IDirector director = (IDirector)Domain.TypeModule.CreateInstance(type);
                DirectorAttribute attr = Domain.TypeModule.GetAttribute<DirectorAttribute>(type);
                if (attr.Default)
                    m_DefaultDirector = director;
                m_Directors.Add(type, director);
                director.OnInit(this);
            }

            typeSys = Domain.TypeModule.GetOrNew<IStoryHelper>();
            foreach (Type type in typeSys)
            {
                IStoryHelper director = (IStoryHelper)Domain.TypeModule.CreateInstance(type);
                m_StoryHelpers.Add(type, director);
            }

            if (!string.IsNullOrEmpty(XConfig.DefaultPlotHelper))
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultPlotHelper);
                if (type != null)
                {
                    m_Helper = (IPlotHelper)Domain.TypeModule.CreateInstance(type);
                }
            }

            if (m_Helper == null)
                m_Helper = new DefaultPlotHelper();
            Event.Listen(NewStoryEvent.EventId, InnerNewStoryHandle);
        }

        public void OnUpdate(float escapeTime)
        {
            foreach (IDirector director in m_Directors.Values)
                director.OnUpdate();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (IDirector director in m_Directors.Values)
                director.OnDestory();
            Event.Unlisten(NewStoryEvent.EventId, InnerNewStoryHandle);
        }
        #endregion

        #region Inner Implement
        private void InnerNewStoryHandle(XEvent e)
        {
            NewStoryEvent evt = (NewStoryEvent)e;
            foreach (IStory story in evt.Stories)
            {
                story.Director.Play(story);
            }
        }
        #endregion
    }
}
