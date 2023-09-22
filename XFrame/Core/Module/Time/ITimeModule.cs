
using XFrame.Core;

namespace XFrame.Modules.Times
{
    public interface ITimeModule : IModule, IUpdater
    {
        float Time { get; }

        float EscapeTime { get; }

        long Frame { get; }

        CDTimer[] GetTimers();
    }
}
