using System;
using XFrame.Core;
using XFrame.Modules.Event;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Conditions
{
    public class ConditionGroupHandle
    {
        private string m_Name;
        private bool m_Complete;
        private Action<ConditionGroupHandle> m_CompleteEvent;
        private List<ConditionHandle> m_AllInfos;
        private Dictionary<int, ConditionHandle> m_NotInfos;

        public string Name => m_Name;
        public List<ConditionHandle> AllInfo => m_AllInfos;
        public Dictionary<int, ConditionHandle> NotInfo => m_NotInfos;

        internal ConditionGroupHandle(string name, ArrayParser<PairParser<IntParser, UniversalParser>> parser, Action<ConditionGroupHandle> completeCallback = null)
        {
            m_Name = name;
            m_Complete = false;
            m_CompleteEvent = completeCallback;
            m_AllInfos = new List<ConditionHandle>();
            m_NotInfos = new Dictionary<int, ConditionHandle>();

            var list = parser.Value;
            foreach (var node in list)
            {
                ConditionHandle info = new ConditionHandle(node.Value);
                m_AllInfos.Add(info);
                if (!ConditionModule.Inst.InnerCheckFinish(info))
                {
                    m_NotInfos.Add(info.Target, info);
                }
            }
            ConditionModule.Inst.Event.Listen(ConditionEvent.EventId, InnerTriggerHandler);
            InnerCheckComplete();
        }

        private void InnerTriggerHandler(XEvent e)
        {
            ConditionEvent evt = (ConditionEvent)e;
            if (m_NotInfos.TryGetValue(evt.Target, out ConditionHandle handle))
            {
                if (ConditionModule.Inst.InnerCheckCompare(handle, evt.Param))
                {
                    m_NotInfos.Remove(evt.Target);
                    InnerCheckComplete();
                }
            }
        }

        private void InnerCheckComplete()
        {
            if (m_NotInfos.Count == 0)
            {
                m_Complete = true;
                m_CompleteEvent?.Invoke(this);
                m_CompleteEvent = null;
            }
        }

        public void Dispose()
        {
            ConditionModule.Inst.Event.Unlisten(ConditionEvent.EventId, InnerTriggerHandler);
        }

        public void OnComplete(Action<ConditionGroupHandle> callback)
        {
            if (m_Complete)
                callback?.Invoke(this);
            else
                m_CompleteEvent += callback;
        }
    }
}
