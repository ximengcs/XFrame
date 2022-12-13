
namespace XFrame.Modules
{
    public interface IResHandler : ITaskHandler
    {
        object Data { get; }
        bool IsDone { get; }
    }
}
