
using System;
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class NextFrameTask
    {
        private class Handler : ITaskHandler
        {
            public long Frame;
            public Action Act;

            public Handler(Action act)
            {
                Act = act;
                Frame = TimeModule.Inst.Frame;
            }
        }
    }
}
