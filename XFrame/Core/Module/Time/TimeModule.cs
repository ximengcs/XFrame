using XFrame.Core;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// 时间模块
    /// </summary>
    [BaseModule]
    public class TimeModule : SingletonModule<TimeModule>
    {
        #region Interface
        /// <summary>
        /// 当前时间
        /// </summary>
        public float Time { get; private set; }

        /// <summary>
        /// 上帧到此帧逃逸时间
        /// </summary>
        public float EscapeTime { get; private set; }
        #endregion

        #region Life Fun
        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            Time += escapeTime;
            EscapeTime = escapeTime;
        }
        #endregion
    }
}
