
namespace XFrame.Core
{
    /// <summary>
    /// 更新处理器
    /// </summary>
    public interface IUpdater
    {
        /// <summary>
        /// 更新生命周期
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        void OnUpdate(double escapeTime);
    }
}
