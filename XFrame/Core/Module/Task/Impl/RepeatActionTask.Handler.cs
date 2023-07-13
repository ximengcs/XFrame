
using System;

namespace XFrame.Modules.Tasks
{
    public partial class RepeatActionTask
    {
        public class Handler : ITaskHandler
        {
            public float TimeGap;
            public Func<bool> Act;

            public Handler(float timeGap, Func<bool> act)
            {
                Act = act;
                TimeGap = timeGap;
            }
        }
    }
}
