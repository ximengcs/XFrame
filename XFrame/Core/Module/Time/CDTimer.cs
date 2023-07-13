﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// CD计时器
    /// </summary>
    public partial class CDTimer
    {
        private IUpdater m_Updater;
        private Dictionary<int, CDInfo> m_Times;

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        /// <param name="updater">时间更新器</param>
        public CDTimer(IUpdater updater)
        {
            m_Updater = updater;
            m_Times = new Dictionary<int, CDInfo>();
        }

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        public CDTimer()
        {
            m_Updater = Default;
            m_Times = new Dictionary<int, CDInfo>();
        }

        /// <summary>
        /// 开始记录一个CD
        /// </summary>
        /// <param name="key">CD键(使用此键查看CD状态)</param>
        /// <param name="cd">cd时间</param>
        public void Record(int key, float cd)
        {
            CDInfo info = new CDInfo(m_Updater);
            info.CD = cd;
            info.EndTime = m_Updater.Time;
            m_Times[key] = info;
        }

        public void Record(float cd)
        {
            Record(default, cd);
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

        public void Reset()
        {
            Reset(default);
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

        public bool Check(bool reset = false)
        {
            return Check(default, reset);
        }

        public float CheckTime(int key)
        {
            if (m_Times.TryGetValue(key, out CDInfo info))
            {
                return info.Suplus;
            }

            return -1;
        }

        public float CheckTime()
        {
            return CheckTime(default);
        }

        private class CDInfo
        {
            private IUpdater m_Updater;
            public float EndTime;
            public float CD;

            public CDInfo(IUpdater updater)
            {
                m_Updater = updater;
            }

            public float Suplus
            {
                get { return EndTime - m_Updater.Time; }
            }

            public bool Due
            {
                get { return m_Updater.Time >= EndTime; }
            }

            public void Reset()
            {
                EndTime = m_Updater.Time + CD;
            }
        }
    }
}
