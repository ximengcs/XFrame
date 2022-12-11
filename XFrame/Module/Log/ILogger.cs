
namespace XFrame.Modules
{
    public interface ILogger
    {
        void Debug(params object[] content);
        void Warning(params object[] content);
        void Error(params object[] content);
        void Fatal(params object[] content);
    }
}
