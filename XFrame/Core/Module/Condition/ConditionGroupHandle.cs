using System;
using XFrame.Modules.Event;
using System.Collections.Generic;

namespace XFrame.Modules.Conditions
{
    public class ConditionGroupHandle
    {
        private bool m_Complete;
        private IConditionHelper m_Helper;
        private ConditionSetting m_Setting;
        private List<ConditionHandle> m_AllInfos;
        private Action<ConditionGroupHandle> m_CompleteEvent;
        private Dictionary<int, List<ConditionHandle>> m_NotInfos;

        public string Name => m_Setting.Name;
        public bool Complete => m_Complete;
        public ConditionSetting Setting => m_Setting;
        public List<ConditionHandle> AllInfo => m_AllInfos;
        public Dictionary<int, List<ConditionHandle>> NotInfo => m_NotInfos;

        internal ConditionGroupHandle(ConditionSetting setting, IConditionHelper helper, Action<ConditionGroupHandle> completeCallback = null)
        {
            m_Setting = setting;
            m_Complete = false;
            m_Helper = helper;
            m_CompleteEvent = completeCallback;
            m_AllInfos = new List<ConditionHandle>();
            m_NotInfos = new Dictionary<int, List<ConditionHandle>>();

            if (m_Helper != null && m_Helper.CheckFinish(setting.Name))
            {
                m_Complete = true;
            }
            else
            {
                var list = setting.Condition.Value;
                foreach (var node in list)
                {
                    ConditionHandle info = new ConditionHandle(node.Value);
                    m_AllInfos.Add(info);
                    if (!ConditionModule.Inst.InnerCheckFinish(info))
                    {
                        if (!m_NotInfos.TryGetValue(info.Target, out List<ConditionHandle> conds))
                        {
                            conds = new List<ConditionHandle>();
                            m_NotInfos.Add(info.Target, conds);
                        }
                        conds.Add(info);
                    }
                }
                ConditionModule.Inst.Event.Listen(ConditionEvent.EventId, InnerTriggerHandler);
            }

            InnerCheckComplete();
        }

        private void InnerTriggerHandler(XEvent e)
        {
            ConditionEvent evt = (ConditionEvent)e;
            if (m_NotInfos.TryGetValue(evt.Target, out List<ConditionHandle> handles))
            {
                for (int i = handles.Count - 1; i >= 0; i--)
                {
                    ConditionHandle handle = handles[i];
                    if (ConditionModule.Inst.InnerCheckCompare(handle, evt.Param))
                        handles.RemoveAt(i);
                }
                if (handles.Count == 0)
                    m_NotInfos.Remove(evt.Target);
                InnerCheckComplete();
            }
        }

        private void InnerCheckComplete()
        {
            if (m_NotInfos.Count == 0 || m_Complete)
            {
                m_Complete = true;
                m_Helper?.MarkFinish(m_Setting.Name);
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
