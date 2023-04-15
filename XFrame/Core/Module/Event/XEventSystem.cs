using XFrame.Modules.Pools;
using System.Collections.Generic;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件系统
    /// </summary>
    internal class XEventSystem : IEventSystem
    {
        private List<XEvent> m_WorkQueue;
        private IPool<DefaultEvent> m_EventPool;
        private Dictionary<int, XEventHandler> m_Handlers;

        public XEventSystem()
        {
            m_WorkQueue = new List<XEvent>();
            m_Handlers = new Dictionary<int, XEventHandler>();
            m_EventPool = PoolModule.Inst.GetOrNew<DefaultEvent>();
        }

        public void Trigger(int eventId)
        {
            m_EventPool.Require(out DefaultEvent evt);
            evt.SetId(eventId);
            Trigger(evt);
        }

        public void Trigger(XEvent e)
        {
            m_WorkQueue.Add(e);
        }

        public void TriggerNow(int eventId)
        {
            m_EventPool.Require(out DefaultEvent evt);
            evt.SetId(eventId);
            TriggerNow(evt);
        }

        public void TriggerNow(XEvent e)
        {
            if (m_Handlers.TryGetValue(e.Id, out XEventHandler target))
            {
                target?.Invoke(e);
            }
        }

        public void Listen(int eventId, XEventHandler handler)
        {
            if (m_Handlers.TryGetValue(eventId, out XEventHandler target))
                m_Handlers[eventId] += handler;
            else
                m_Handlers.Add(eventId, handler);
        }

        public void Unlisten(int eventId, XEventHandler handler)
        {
            if (m_Handlers.TryGetValue(eventId, out XEventHandler target))
                m_Handlers[eventId] -= handler;
            else
                m_Handlers.Remove(eventId);
        }

        public void Unlisten(int eventId)
        {
            if (m_Handlers.ContainsKey(eventId))
                m_Handlers.Remove(eventId);
        }

        public void Unlisten()
        {
            m_Handlers.Clear();
        }

        void IEventSystem.OnUpdate()
        {
            if (m_WorkQueue == null || m_WorkQueue.Count == 0)
                return;

            foreach (XEvent e in m_WorkQueue)
                TriggerNow(e);
            m_WorkQueue.Clear();
        }
    }
}
