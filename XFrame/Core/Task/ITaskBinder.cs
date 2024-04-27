
namespace XFrame.Tasks
{
    /// <summary>
    /// 任务绑定对象
    /// </summary>
    public interface ITaskBinder
    {
        /// <summary>
        /// 是否已销毁，销毁时任务会被取消
        /// </summary>
        bool IsDisposed { get; }
    }
}
