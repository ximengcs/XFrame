using XFrame.Modules.Tasks;

namespace XFrame.Core
{
    public interface IStartHandler : IEntryHandler
    {
        ITask BeforeHandle();
        ITask AfterHandle();
    }
}
