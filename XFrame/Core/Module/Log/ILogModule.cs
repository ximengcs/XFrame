
using XFrame.Core;

namespace XFrame.Modules.Diagnotics
{
    public interface ILogModule : IModule
    {
        void AddLogger<T>() where T : ILogger;

        T GetLogger<T>() where T : ILogger;

        void Debug(params object[] content);

        void Warning(params object[] content);

        void Error(params object[] content);

        void Fatal(params object[] content);
    }
}
