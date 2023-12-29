using XFrame.Core;
using XFrame.Modules.Pools;
using System.Collections.Generic;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// CD计时器
    /// </summary>
    public partial class CDTimer : PoolObjectBase, IPoolObject
    {
        private string m_Name;
        private IUpdater m_Updater;
        private Dictionary<int, CDInfo> m_Times;

        public string Name => m_Name;

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        private CDTimer()
        {
            m_Updater = Default;
            m_Times = new Dictionary<int, CDInfo>();
        }

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        /// <param name="updater">时间更新器</param>
        public static CDTimer Create(IUpdater updater)
        {
            CDTimer timer = References.Require<CDTimer>();
            timer.m_Updater = updater;
            timer.m_Times = new Dictionary<int, CDInfo>();
            return timer;
        }

        public static CDTimer Create()
        {
            CDTimer timer = References.Require<CDTimer>();
            TimeModule timeModule = (TimeModule)XModule.Time;
            timeModule.InnerAddTimer(timer);
            return timer;
        }

        public static CDTimer Create(string name)
        {
            CDTimer timer = References.Require<CDTimer>();
            timer.m_Name = name;
            TimeModule timeModule = (TimeModule)XModule.Time;
            timeModule.InnerAddTimer(timer);
            return timer;
        }

        public static CDTimer Create(string name, IUpdater updater)
        {
            CDTimer timer = References.Require<CDTimer>();
            timer.m_Name = name;
            timer.m_Updater = updater;
            timer.m_Times = new Dictionary<int, CDInfo>();
            TimeModule timeModule = (TimeModule)XModule.Time;
            timeModule.InnerAddTimer(timer);
            return timer;
        }

        public void SetUpdater(IUpdater updater)
        {
            m_Updater = updater;
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

        protected override void OnRequestFromPool()
        {
            base.OnRequestFromPool();
            PoolKey = 0;
            m_Name = XModule.Rand.RandString();
            m_Updater = Default;
            TimeModule timeModule = (TimeModule)XModule.Time;
            timeModule.InnerAddTimer(this);
        }

        protected override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            TimeModule timeModule = (TimeModule)XModule.Time;
            timeModule.InnerRemove(this);
            m_Name = null;
            m_Times.Clear();
            m_Updater = null;
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
