using System;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.XType;
using XFrame.Modules.Event;
using XFrame.Modules.Config;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 故事模块
    /// </summary>
    [XModule]
    [RequireModule(typeof(EventModule))]
    public class PlotModule : SingletonModule<PlotModule>
    {
        #region Inner Fields
        private IPlotHelper m_Helper;
        private IDirector m_DefaultDirector;
        private Dictionary<Type, IDirector> m_Directors;
        #endregion

        #region Interface
        /// <summary>
        /// 故事处理辅助类
        /// </summary>
        public IPlotHelper Helper => m_Helper;

        /// <summary>
        /// 请求一个新故事
        /// </summary>
        /// <param name="name">故事名</param>
        /// <returns>故事</returns>
        public IStory NewStory(string name = null)
        {
            return new Story(name);
        }
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Directors = new Dictionary<Type, IDirector>();

            TypeSystem typeSys = TypeModule.Inst.GetOrNewWithAttr<DirectorAttribute>();
            foreach (Type type in typeSys)
            {
                IDirector director = (IDirector)Activator.CreateInstance(type);
                DirectorAttribute attr = TypeUtility.GetAttribute<DirectorAttribute>(type);
                if (attr.Default)
                    m_DefaultDirector = director;
                m_Directors.Add(type, director);
                director.OnInit();
            }

            if (!string.IsNullOrEmpty(XConfig.DefaultPlotHelper))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultPlotHelper);
                if (type != null)
                {
                    m_Helper = (IPlotHelper)Activator.CreateInstance(type);
                }
            }

            if (m_Helper == null)
                m_Helper = new DefaultPlotHelper();
            m_Helper.Event.Listen(NewStoryEvent.EventId, InnerNewStoryHandle);
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            foreach (IDirector director in m_Directors.Values)
                director.OnUpdate();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            foreach (IDirector director in m_Directors.Values)
                director.OnDestory();
            m_Helper.Event.Unlisten(NewStoryEvent.EventId, InnerNewStoryHandle);
        }
        #endregion

        #region Inner Implement
        private void InnerNewStoryHandle(XEvent e)
        {
            NewStoryEvent evt = (NewStoryEvent)e;
            IDirector director;
            if (evt.TargetDirector == null || !m_Directors.TryGetValue(evt.TargetDirector, out director))
                director = m_DefaultDirector;
            director.Play(evt.Stories);
        }
        #endregion
    }
}
