using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Core;

namespace XFrame.Modules.Event
{
    /// <inheritdoc/>
    [CoreModule]
    [XType(typeof(IEventModule))]
    public class EventModule : ModuleBase, IEventModule
    {
        private List<XEventSystem> m_List;
         
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_List = new List<XEventSystem>();
        }

        /// <inheritdoc/>
        public void OnUpdate(double escapeTime)
        {
            for (int i = m_List.Count - 1; i >= 0; i--)
                m_List[i].OnUpdate();
        }

        /// <inheritdoc/>
        public IEventSystem NewSys()
        {
            XEventSystem evtSys = new XEventSystem();
            m_List.Add(evtSys);
            return evtSys;
        }

        /// <inheritdoc/>
        public void Remove(IEventSystem evtSys)
        {
            m_List.Remove((XEventSystem)evtSys);
        }
    }
}
