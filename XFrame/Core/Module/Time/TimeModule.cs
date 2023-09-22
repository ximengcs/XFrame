using XFrame.Core;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Modules.Tasks;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// 时间模块
    /// </summary>
    [BaseModule]
    [XType(typeof(ITimeModule))]
    public class TimeModule : ModuleBase, ITimeModule
    {
        private float m_Time;
        private float m_EscapeTime;
        private long m_Frame;
        private List<CDTimer> m_AnonymousTimers;
        private Dictionary<string, CDTimer> m_Timers;

        #region Interface
        /// <summary>
        /// 当前时间
        /// </summary>
        public float Time => m_Time;

        /// <summary>
        /// 上帧到此帧逃逸时间
        /// </summary>
        public float EscapeTime => m_EscapeTime;

        public long Frame => m_Frame;
        #endregion

        public CDTimer[] GetTimers()
        {
            CDTimer[] timers = new CDTimer[m_Timers.Count];
            int index = 0;
            foreach (var item in m_Timers)
                timers[index++] = item.Value;
            return timers;
        }

        internal void InnerAddTimer(CDTimer timer)
        {
            if (string.IsNullOrEmpty(timer.Name))
                m_AnonymousTimers.Add(timer);
            else
            {
                if (!m_Timers.ContainsKey(timer.Name))
                    m_Timers.Add(timer.Name, timer);
            }
        }

        internal void InnerRemove(CDTimer timer)
        {
            if (string.IsNullOrEmpty(timer.Name))
            {
                if (m_AnonymousTimers.Contains(timer))
                    m_AnonymousTimers.Remove(timer);
            }
            else
            {
                if (m_Timers.ContainsKey(timer.Name))
                    m_Timers.Remove(timer.Name);
            }
        }

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_AnonymousTimers = new List<CDTimer>();
            m_Timers = new Dictionary<string, CDTimer>();
        }

        public void OnUpdate(float escapeTime)
        {
            m_Time += escapeTime;
            m_EscapeTime = escapeTime;
            m_Frame++;
        }
        #endregion
    }
}
