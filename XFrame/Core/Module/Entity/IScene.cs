
using XFrame.Core.Threads;

namespace XFrame.Modules.Entities
{
    public interface IScene : IEntityModule
    {
        Fiber Fiber { get; }
    }
}
