using System;

namespace XFrame.Modules.Tasks
{
    public partial class ProActionTask : TaskBase
    {
        protected override void OnInit()
        {
            AddStrategy(new Strategy());
        }

        public ProActionTask Add(Func<float> handler)
        {
            return (ProActionTask)Add(new Handler(handler));
        }
    }
}
