
using XFrame.Core;

namespace XFrame.Modules.Tasks
{
    public interface ITaskModule : IModule, IUpdater
    {
        int ExecCount { get; }

        long TaskTimeout { get; set; }

        T GetOrNew<T>() where T : ITask;

        T Get<T>(string name) where T : ITask;

        ITask Get(string name);

        T GetOrNew<T>(string name) where T : ITask;

        bool Remove(ITask task);

        bool Remove(string name);
    }
}
