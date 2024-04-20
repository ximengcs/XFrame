
namespace XFrame.Tasks
{
    internal interface ICancelTask
    {
        XTaskCancelToken Token { get; }

        ITaskBinder Binder { get; }
        
        void SetState(XTaskState state);
    }
}
