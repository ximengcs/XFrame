using XFrame.Core;
using XFrame.Tasks;

namespace XFrame.Modules.Tasks
{
    public interface ITaskModule : IModule, IUpdater
    {
        int ExecCount { get; }

        long TaskTimeout { get; set; }

        void Register(IUpdater task);

        void UnRegister(IUpdater task);
    }
}
