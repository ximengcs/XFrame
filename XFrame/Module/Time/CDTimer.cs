using System.Collections.Generic;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// CD计时器
    /// 需要TimeModule支持
    /// </summary>
    public class CDTimer
    {
        private Dictionary<int, CDInfo> m_Times;

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        public CDTimer()
        {
            m_Times = new Dictionary<int, CDInfo>();
        }

        /// <summary>
        /// 开始记录一个CD
        /// </summary>
        /// <param name="key">CD键(使用此键查看CD状态)</param>
        /// <param name="cd">cd时间</param>
        public void Record(int key, float cd)
        {
            CDInfo info = new CDInfo();
            info.CD = cd;
            info.EndTime = TimeModule.Inst.Time;
            m_Times[key] = info;
        }

        /// <summary>
        /// 重置一个cd, 调用后重置CD时间
        /// </summary>
        /// <param name="key">CD键</param>
        public void Reset(int key)
        {
            if (m_Times.TryGetValue(key, out CDInfo info))
                info.Reset();
        }

        /// <summary>
        /// 检查一个CD的状态
        /// </summary>
        /// <param name="key">CD键</param>
        /// <param name="reset">如果检查到的状态为到期，是否重置CD时间</param>
        /// <returns>true表示到期，false表示未到CD时间</returns>
        public bool Check(int key, bool reset = false)
        {
            if (m_Times.TryGetValue(key, out CDInfo info))
            {
                if (info.Due)
                {
                    if (reset)
                        info.Reset();
                    return true;
                }
            }

            return false;
        }

        private class CDInfo
        {
            public float EndTime;
            public float CD;

            public bool Due
            {
                get { return TimeModule.Inst.Time >= EndTime; }
            }

            public void Reset()
            {
                EndTime = TimeModule.Inst.Time + CD;
            }
        }
    }
}