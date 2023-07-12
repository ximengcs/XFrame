using System;
using XFrame.Core;
using XFrame.Modules.Diagnotics;

namespace XFrame.Modules.Conditions
{
    /// <summary>
    /// 条件句柄(单个条件)
    /// <para>
    /// 在初始化时会调用<see cref="IConditionCompare.CheckFinish(ConditionHandle)"/>检查条件完成状态
    /// </para>
    /// <para>
    /// 当触发<see cref="ConditionEvent"/>事件时，会调用<see cref="IConditionCompare.Check(ConditionHandle, object)"/>检查完成条件，第二个参数为事件参数
    /// </para>
    /// <para>
    /// 需要实现类<see cref="IConditionCompare"/>去执行<see cref="Trigger(object, object)"/>来触发更新<see cref="OnComplete(Action{ConditionHandle})"/>事件
    /// </para>
    /// </summary>
    public class ConditionHandle : DataProvider, IConditionHandle
    {
        private int m_Target;
        private UniversalParser m_Param;
        private IDataProvider m_Data;
        private ConditionGroupHandle m_Group;
        private Action<ConditionHandle> m_OnComplete;
        private Action<object, object> m_UpdateEvent;
        private bool m_Complete;
        private object m_Value;

        private bool m_HelperInstance;
        private IConditionCompare m_Helper;

        /// <summary>
        /// 条件目标
        /// <para>
        /// <see cref="ConditionEvent.Target"/> 触发的目标会根据此值匹配句柄实例
        /// </para>
        /// <para>
        /// <see cref="IConditionCompare.Target"/> 具体的实现类会匹配到此值
        /// </para>
        /// </summary>
        public int Target => m_Target;

        /// <summary>
        /// 条件需要达成的目标参数，如数量等
        /// </summary>
        public UniversalParser Param => m_Param;

        /// <summary>
        /// 条件句柄所有条件组
        /// </summary>
        public IConditionGroupHandle Group => m_Group;

        /// <summary>
        /// 条件句柄数据提供器
        /// </summary>
        public IDataProvider Data => m_Data;

        internal ConditionHandle(ConditionGroupHandle group, PairParser<IntParser, UniversalParser> parser)
        {
            m_Group = group;
            Pair<IntParser, UniversalParser> pair = parser;
            m_Target = pair.Key;
            m_Param = pair.Value;
            m_Data = new DataProvider();
            m_Complete = false;
        }

        internal void SetHelper(bool helperInstance, IConditionCompare helper)
        {
            m_HelperInstance = helperInstance;
            m_Helper = helper;
        }

        internal void MarkComplete()
        {
            m_Complete = true;
            m_OnComplete?.Invoke(this);
            m_OnComplete = null;
        }

        internal bool InnerCheckComplete()
        {
            if (m_Helper == null)
            {
                Log.Error("Condition", $"Target {Target} compare is null");
                return false;
            }

            return m_Helper.CheckFinish(this);
        }

        internal bool InnerCheckComplete(object param)
        {
            if (m_Helper == null)
            {
                Log.Error("Condition", $"Target {Target} compare is null");
                return false;
            }
            if (m_HelperInstance)
                m_Helper.OnEventTrigger(param);
            return m_Helper.Check(this, param);
        }

        /// <summary>
        /// 调用此方法触发条件句柄的更新(通过<see cref="OnComplete(Action{ConditionHandle})注册的事件"/>)事件，
        /// 一般通过<see cref="IConditionCompare"/>实现类来触发。
        /// </summary>
        /// <param name="oldValue">旧值</param>
        /// <param name="newValue">新值</param>
        public void Trigger(object oldValue, object newValue)
        {
            m_Value = newValue;
            m_UpdateEvent?.Invoke(oldValue, newValue);
        }

        /// <summary>
        /// 条件更新事件，若提前触发了更新事件，则会立即触发一次更新，并使用上次的值执行回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void OnUpdate(Action<object, object> callback)
        {
            if (m_Value != null)
                callback?.Invoke(m_Value, m_Value);
            m_UpdateEvent += callback;
        }

        /// <summary>
        /// 条件完成事件，当条件已经完成时，会立刻执行回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void OnComplete(Action<ConditionHandle> callback)
        {
            if (m_Complete)
            {
                callback?.Invoke(this);
            }
            else
            {
                m_OnComplete += callback;
            }
        }
    }
}
