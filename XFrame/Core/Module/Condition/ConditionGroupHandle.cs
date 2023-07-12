using System;
using XFrame.Modules.Event;
using System.Collections.Generic;
using XFrame.Modules.XType;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件组句柄，一个实例的条件控制句柄，一个实例可以有多个条件。
    /// <para>
    /// </para>
    /// </summary>
    internal class ConditionGroupHandle : IConditionGroupHandle
    {
        private bool m_Complete;
        private IConditionHelper m_Helper;
        private ConditionSetting m_Setting;
        private List<IConditionHandle> m_AllInfos;
        private Action<IConditionGroupHandle> m_CompleteEvent;
        private Dictionary<int, List<IConditionHandle>> m_NotInfos;

        /// <summary>
        /// 条件组名称
        /// </summary>
        public string Name => m_Setting.Name;

        /// <summary>
        /// 条件是否完成
        /// </summary>
        public bool Complete => m_Complete;

        /// <summary>
        /// 条件配置
        /// </summary>
        public ConditionSetting Setting => m_Setting;

        /// <summary>
        /// 组内所有的条件
        /// </summary>
        public List<IConditionHandle> AllInfo => m_AllInfos;

        /// <summary>
        /// 组内还未达成的条件
        /// </summary>
        public Dictionary<int, List<IConditionHandle>> NotInfo => m_NotInfos;

        internal ConditionGroupHandle(ConditionSetting setting, IConditionHelper helper, Action<IConditionGroupHandle> completeCallback = null)
        {
            m_Setting = setting;
            m_Complete = false;
            m_Helper = helper;
            m_CompleteEvent = completeCallback;
            m_AllInfos = new List<IConditionHandle>();
            m_NotInfos = new Dictionary<int, List<IConditionHandle>>();

            var list = setting.Condition.Value;
            foreach (var node in list)
            {
                ConditionHandle handle = new ConditionHandle(this, node.Value);
                int target = handle.Target;
                IConditionCompare compare;
                bool isInstanceHelper = setting.ConditionIsInstance(target);
                if (isInstanceHelper)
                {
                    Type compType = ConditionModule.Inst.GetCompareType(target);
                    compare = (IConditionCompare)TypeModule.Inst.CreateInstance(compType);
                }
                else
                {
                    compare = ConditionModule.Inst.GetCompare(target);
                }
                handle.SetHelper(isInstanceHelper, compare);
                m_AllInfos.Add(handle);
                if (!handle.InnerCheckComplete())
                {
                    if (!m_NotInfos.TryGetValue(handle.Target, out List<IConditionHandle> conds))
                    {
                        conds = new List<IConditionHandle>();
                        m_NotInfos.Add(handle.Target, conds);
                    }
                    conds.Add(handle);
                }
            }

            if (m_Helper != null && m_Helper.CheckFinish(this))
            {
                m_Complete = true;
                m_CompleteEvent?.Invoke(this);
                m_CompleteEvent = null;
            }
            else
            {
                InnerCheckComplete();
            }
        }

        internal void InnerTrigger(int target, object param)
        {
            if (m_NotInfos.TryGetValue(target, out List<IConditionHandle> handles))
            {
                for (int i = handles.Count - 1; i >= 0; i--)
                {
                    ConditionHandle handle = (ConditionHandle)handles[i];
                    if (handle.InnerCheckComplete(param))
                    {
                        handle.MarkComplete();
                        handles.RemoveAt(i);
                    }
                }
                if (handles.Count == 0)
                    m_NotInfos.Remove(target);
                InnerCheckComplete();
            }
        }

        private void InnerCheckComplete()
        {
            if (m_NotInfos.Count == 0)
            {
                m_Complete = true;
                m_Helper?.MarkFinish(this);
                m_CompleteEvent?.Invoke(this);
                m_CompleteEvent = null;
            }
        }

        /// <summary>
        /// 释放条件实例
        /// </summary>
        internal void Dispose()
        {

        }

        /// <summary>
        /// 注册完成回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void OnComplete(Action<IConditionGroupHandle> callback)
        {
            if (m_Complete)
                callback?.Invoke(this);
            else
                m_CompleteEvent += callback;
        }
    }
}
