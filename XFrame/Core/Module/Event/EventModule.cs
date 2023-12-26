using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件模块
    /// </summary>
    [CoreModule]
    [XType(typeof(IEventModule))]
    public class EventModule : ModuleBase, IEventModule, IUpdater
    {
        private List<XEventSystem> m_List;
         
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_List = new List<XEventSystem>();
        }

        public void OnUpdate(float escapeTime)
        {
            for (int i = m_List.Count - 1; i >= 0; i--)
                m_List[i].OnUpdate();
        }

        /// <summary>
        /// 创建一个事件系统
        /// </summary>
        /// <returns>事件系统</returns>
        public IEventSystem NewSys()
        {
            XEventSystem evtSys = new XEventSystem();
            m_List.Add(evtSys);
            return evtSys;
        }

        /// <summary>
        /// 移除一个事件系统 
        /// </summary>
        /// <param name="evtSys">事件系统</param>
        public void Remove(IEventSystem evtSys)
        {
            m_List.Remove((XEventSystem)evtSys);
        }
    }
}
