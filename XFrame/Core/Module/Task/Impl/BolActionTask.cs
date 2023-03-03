using System;

namespace XFrame.Modules.Tasks
{
    public partial class BolActionTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public BolActionTask Add(Func<bool> handler)
        {
            return (BolActionTask)Add(new Handler(handler));
        }
    }
}
