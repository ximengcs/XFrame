using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件模块
    /// </summary>
    [CoreModule]
    public class EventModule : SingletonModule<EventModule>
    {
        private List<IEventSystem> m_List;
        private IEventSystem m_Global;

        /// <summary>
        /// 全局事件系统
        /// </summary>
        public IEventSystem Global => m_Global;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Global = new XEventSystem();
            m_List = new List<IEventSystem>();
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);

            foreach (IEventSystem evtSys in m_List)
                evtSys.OnUpdate();
        }

        public IEventSystem NewSys()
        {
            IEventSystem evtSys = new XEventSystem();
            m_List.Add(evtSys);
            return evtSys;
        }

        public void Remove(IEventSystem evtSys)
        {
            m_List.Remove(evtSys);
        }
    }
}
