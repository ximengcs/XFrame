
using System;
using XFrame.Modules.Times;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class NextFrameHandler : ITaskHandler
        {
            public long Frame;
            public Action Act;

            public NextFrameHandler(Action act)
            {
                Act = act;
            }
        }
    }
}
