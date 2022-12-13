
using System;

namespace XFrame.Modules
{
    public interface ITask : ITaskHandler
    {
        bool IsComplete { get; }
        bool IsStart { get; }
        void OnInit();
        void Start();
        ITask Add(ITaskHandler target);
        ITask AddStrategy(ITaskStrategy strategy);
        ITask OnComplete(Action complete);
        void OnUpdate();
    }
}
