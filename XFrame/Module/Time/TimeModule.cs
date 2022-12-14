using XFrame.Core;

namespace XFrame.Modules
{
    public class TimeModule : SingletonModule<TimeModule>
    {
        public float Time { get; private set; }

        public override void OnUpdate(float escapeTime)
        {
            base.OnUpdate(escapeTime);
            Time += escapeTime;
        }
    }
}
