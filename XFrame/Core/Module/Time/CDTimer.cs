﻿using System.Collections.Generic;
using XFrame.Modules.Pools;
using XFrame.Modules.Rand;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// CD计时器
    /// </summary>
    public partial class CDTimer : IPoolObject
    {
        private string m_Name;
        private IUpdater m_Updater;
        private Dictionary<int, CDInfo> m_Times;
        private TimeModule m_TimeModule;

        /// <inheritdoc/>
        public string Name => m_Name;

        int IPoolObject.PoolKey => default;
        /// <inheritdoc/>
        public string MarkName { get; set; }

        IPool IPoolObject.InPool { get; set; }

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        private CDTimer()
        {
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

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        /// <returns>时间更新器</returns>
        public static CDTimer Create()
        {
            CDTimer timer = References.Require<CDTimer>();
            timer.m_Times = new Dictionary<int, CDInfo>();
            timer.m_Updater = GetDeaultUpdater(timer.m_TimeModule);
            return timer;
        }

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        /// <param name="name">计时器名</param>
        /// <returns>时间更新器</returns>
        public static CDTimer Create(string name)
        {
            CDTimer timer = References.Require<CDTimer>();
            timer.m_Name = name;
            timer.m_Times = new Dictionary<int, CDInfo>();
            timer.m_Updater = GetDeaultUpdater(timer.m_TimeModule);
            return timer;
        }

        /// <summary>
        /// 构造CD计时器
        /// </summary>
        /// <param name="name">计时器名</param>
        /// <param name="updater">更新器</param>
        /// <returns>时间更新器</returns>
        public static CDTimer Create(string name, IUpdater updater)
        {
            CDTimer timer = References.Require<CDTimer>();
            timer.m_Name = name;
            timer.m_Updater = updater;
            timer.m_Times = new Dictionary<int, CDInfo>();
            return timer;
        }

        /// <summary>
        /// 设置更新器
        /// </summary>
        /// <param name="updater"></param>
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

        /// <summary>
        /// 记录默认键的CD
        /// </summary>
        /// <param name="cd">cd时间</param>
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

        /// <summary>
        /// 重置CD
        /// </summary>
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

        /// <summary>
        /// 检查一个CD的状态
        /// </summary>
        /// <param name="reset">如果检查到的状态为到期，是否重置CD时间</param>
        /// <returns>true表示到期，false表示未到CD时间</returns>
        public bool Check(bool reset = false)
        {
            return Check(default, reset);
        }

        /// <summary>
        /// 检查CD的时间状态
        /// </summary>
        /// <param name="key">CD键</param>
        /// <returns>时间</returns>
        public double CheckTime(int key)
        {
            if (m_Times.TryGetValue(key, out CDInfo info))
            {
                return info.Suplus;
            }

            return -1;
        }

        /// <summary>
        /// 检查默认键的CD时间状态
        /// </summary>
        /// <returns>时间</returns>
        public double CheckTime()
        {
            return CheckTime(default);
        }

        void IPoolObject.OnCreate()
        {
            IPoolObject poolObj = this;
            m_TimeModule = (TimeModule)poolObj.InPool.Module.Domain.GetModule<ITimeModule>();
        }

        void IPoolObject.OnRequest()
        {
            m_Name = m_TimeModule.Domain.GetModule<IRandModule>().RandString();
            m_TimeModule.InnerAddTimer(this);
        }

        void IPoolObject.OnRelease()
        {
            m_TimeModule.InnerRemove(this);
            m_Name = null;
            m_Times.Clear();
            m_Updater = null;
        }

        void IPoolObject.OnDelete()
        {

        }

        private class CDInfo
        {
            private IUpdater m_Updater;
            public double EndTime;
            public float CD;

            public CDInfo(IUpdater updater)
            {
                m_Updater = updater;
            }

            public double Suplus
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
