using System;

namespace XFrame.Modules.Tasks
{
    public partial class ActionTask
    {
        public class ProgressHandler : ITaskHandler
        {
            public Func<float> Act;

            public ProgressHandler(Func<float> act)
            {
                Act = act;
            }
        }
    }
}
