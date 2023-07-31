using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class BoolHandler : ITaskHandler
        {
            public Func<bool> Act;

            public BoolHandler(Func<bool> act)
            {
                Act = act;
            }
        }
    }
}
