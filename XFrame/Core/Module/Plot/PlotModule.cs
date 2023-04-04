using System;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.XType;
using XFrame.Modules.Event;
using XFrame.Modules.Config;
using System.Collections.Generic;

namespace XFrame.Modules.Plots
{
    [XModule]
    public class PlotModule : SingletonModule<PlotModule>
    {
        private IPlotHelper m_Helper;
        private IDirector m_DefaultDirector;
        private Dictionary<Type, IDirector> m_Directors;

        public IPlotHelper Helper => m_Helper;

        public IStory NewStory(string name = null)
        {
            return new Story(name);
        }

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
            m_Helper.OnNewStory.Listen(NewStoryEvent.EventId, InnerNewStoryHandle);
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
            m_Helper.OnNewStory.Unlisten(NewStoryEvent.EventId, InnerNewStoryHandle);
        }

        private void InnerNewStoryHandle(XEvent e)
        {
            NewStoryEvent evt = (NewStoryEvent)e;
            IDirector director;
            if (evt.TargetDirector == null || !m_Directors.TryGetValue(evt.TargetDirector, out director))
                director = m_DefaultDirector;
            director.Add(evt.Stories);
        }
    }
}
