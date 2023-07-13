using System;

namespace XFrame.Modules.Tasks
{
    public partial class RepeatActionTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public RepeatActionTask Add(float timeGap, Func<bool> handler)
        {
            return (RepeatActionTask)Add(new Handler(timeGap, handler));
        }
    }
}
