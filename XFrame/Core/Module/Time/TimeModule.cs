using XFrame.Core;

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

        #region Life Fun
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
