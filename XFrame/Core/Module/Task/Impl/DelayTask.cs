using System;

namespace XFrame.Modules.Tasks
{
    public partial class DelayTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public DelayTask Add(float delayTime, Action callback)
        {
            return (DelayTask)Add(new Handler(delayTime, callback));
        }
    }
}
