using System;

namespace XFrame.Modules
{
    public interface ITaskHandler
    {
        Type HandleType { get; }
    }
}
