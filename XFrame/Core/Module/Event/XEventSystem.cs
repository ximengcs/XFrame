using XFrame.Modules.Pools;
using System.Collections.Generic;
using System;

namespace XFrame.Modules.Event
{
    /// <summary>
    /// 事件系统
    /// </summary>
    internal class XEventSystem : IEventSystem
    {
        private class HandlerInfo : IPoolObject
        {
            public XEventHandler Handler1;
            public XEventHandler2 Handler2;

            public bool Empty => Handler1 == null && Handler2 == null;

            int IPoolObject.PoolKey => default;

            public void Do(XEvent e)
            {
                Handler1?.Invoke(e);

                if (Handler2 != null)
                {
                    var list = Handler2.GetInvocationList();
                    foreach (Delegate dele in list)
                    {
                        XEventHandler2 handler = (XEventHandler2)dele;
                        if (handler(e))
                            Handler2 -= handler;
                    }
                }
            }

            public void Add(XEventHandler handler1)
            {
                Handler1 += handler1;
            }

            public void Add(XEventHandler2 handler2)
            {
                Handler2 += handler2;
            }

            public void Remove(XEventHandler handler1)
            {
                Handler1 -= handler1;
            }
            public void Remove(XEventHandler2 handler2)
            {
                Handler2 -= handler2;
            }

            void IPoolObject.OnCreate()
            {

            }

            void IPoolObject.OnRequest()
            {
                Handler1 = null;
                Handler2 = null;
            }

            void IPoolObject.OnRelease()
            {
                Handler1 = null;
                Handler2 = null;
            }

            void IPoolObject.OnDelete()
            {

            }
        }

        private List<XEvent> m_WorkQueue;
        private List<XEvent> m_UpdateQueue;
        private Dictionary<int, HandlerInfo> m_Handlers;

        public XEventSystem()
        {
            m_WorkQueue = new List<XEvent>();
            m_UpdateQueue = new List<XEvent>();
            m_Handlers = new Dictionary<int, HandlerInfo>();
        }

        public void Trigger(int eventId)
        {
            Trigger(DefaultEvent.Create(eventId));
        }

        public void Trigger(XEvent e)
        {
            m_WorkQueue.Add(e);
        }

        public void TriggerNow(int eventId)
        {
            TriggerNow(DefaultEvent.Create(eventId));
        }

        public void TriggerNow(XEvent e)
        {
            if (m_Handlers.TryGetValue(e.Id, out HandlerInfo target))
            {
                target.Do(e);
            }
            References.Release(e);
        }

        public void Listen(int eventId, XEventHandler handler)
        {
            InnerGetInfo(eventId).Add(handler);
        }

        public void Listen(int eventId, XEventHandler2 handler)
        {
            InnerGetInfo(eventId).Add(handler);
        }

        private HandlerInfo InnerGetInfo(int eventId)
        {
            if (!m_Handlers.TryGetValue(eventId, out HandlerInfo target))
            {
                target = References.Require<HandlerInfo>();
                m_Handlers.Add(eventId, target);
            }
            return target;
        }

        public void Unlisten(int eventId, XEventHandler handler)
        {
            if (m_Handlers.TryGetValue(eventId, out HandlerInfo target))
            {
                target.Remove(handler);
                InnerCheckRelease(eventId, target);
            }
        }

        public void Unlisten(int eventId, XEventHandler2 handler)
        {
            if (m_Handlers.TryGetValue(eventId, out HandlerInfo target))
            {
                target.Remove(handler);
                InnerCheckRelease(eventId, target);
            }
        }

        private void InnerCheckRelease(int eventId, HandlerInfo info)
        {
            if (info.Empty)
            {
                m_Handlers.Remove(eventId);
                References.Release(info);
            }
        }

        public void Unlisten(int eventId)
        {
            if (m_Handlers.TryGetValue(eventId, out HandlerInfo info))
            {
                m_Handlers.Remove(eventId);
                References.Release(info);
            }
        }

        public void Unlisten()
        {
            foreach (var item in m_Handlers)
                References.Release(item.Value);
            m_Handlers.Clear();
        }

        public void OnUpdate()
        {
            if (m_WorkQueue == null || m_WorkQueue.Count == 0)
                return;

            m_UpdateQueue.AddRange(m_WorkQueue);
            foreach (XEvent e in m_UpdateQueue)
                TriggerNow(e);
            m_WorkQueue.Clear();
            m_UpdateQueue.Clear();
        }
    }
}
