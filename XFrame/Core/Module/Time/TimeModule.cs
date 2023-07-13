using XFrame.Core;
using System.Collections.Generic;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// 时间模块
    /// </summary>
    [BaseModule]
    public class TimeModule : SingletonModule<TimeModule>
    {
        private float m_Time;
        private float m_EscapeTime;
        private int m_Frame;
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

        public int Frame => m_Frame;
        #endregion

        public CDTimer[] GetTimers()
        {
            CDTimer[] timers = new CDTimer[m_Timers.Count];
            int index = 0;
            foreach (var item in m_Timers)
                timers[index++] = item.Value;
            return timers;
        }

        public CDTimer NewTimer()
        {
            CDTimer timer = CDTimer.Create();
            m_AnonymousTimers.Add(timer);
            return timer;
        }

        public void Remove(CDTimer timer)
        {
            if (m_AnonymousTimers.Contains(timer))
            {
                m_AnonymousTimers.Remove(timer);
                References.Release(timer);
            }
        }

        public CDTimer NewTimer(string name)
        {
            CDTimer timer = CDTimer.Create(name);
            m_Timers.Add(name, timer);
            return timer;
        }

        public void RemoveTimer(string name)
        {
            if (m_Timers.TryGetValue(name, out CDTimer timer))
            {
                m_Timers.Remove(name);
                References.Release(timer);
            }
        }

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_AnonymousTimers = new List<CDTimer>();
            m_Timers = new Dictionary<string, CDTimer>();
        }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            m_Time += escapeTime;
            m_EscapeTime = escapeTime;
            m_Frame++;
        }
        #endregion
    }
}
