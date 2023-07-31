using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        private class RepeatHandler : ITaskHandler
        {
            public float TimeGap;
            public Func<bool> Act;

            public RepeatHandler(float timeGap, Func<bool> act)
            {
                Act = act;
                TimeGap = timeGap;
            }
        }
    }
}
