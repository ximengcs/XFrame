using System;

namespace XFrame.Modules
{
    public interface ITaskStrategy
    {
        Type HandleType { get; }
        bool Handle(ITask from, ITaskHandler target);
    }
}
