
using System;

namespace XFrame.Modules.Tasks
{
    public partial class NextFrameTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public ActionTask Add(Action handler)
        {
            return (ActionTask)Add(new Handler(handler));
        }
    }
}
