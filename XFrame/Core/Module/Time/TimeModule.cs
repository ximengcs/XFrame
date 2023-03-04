using XFrame.Core;

namespace XFrame.Modules.Times
{
    /// <summary>
    /// 时间模块
    /// </summary>
    [CoreModule]
    public class TimeModule : SingletonModule<TimeModule>
    {
        /// <summary>
        /// 当前时间
        /// </summary>
        public float Time { get; private set; }

        public float EscapeTime { get; private set; }

        protected override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            Time += escapeTime;
            EscapeTime = escapeTime;
        }
    }
}
