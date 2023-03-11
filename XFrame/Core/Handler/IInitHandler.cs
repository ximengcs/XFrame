using XFrame.Modules.Tasks;

namespace XFrame.Core
{
    public interface IInitHandler : IEntryHandler
    {
        void EnterHandle();
        ITask BeforeHandle();
        ITask AfterHandle();
    }
}
