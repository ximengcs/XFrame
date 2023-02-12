using XFrame.Collections;

namespace XFrame.Core
{
    /// <summary>
    /// 模块
    /// </summary>
    public interface IModule : IXItem
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">初始化数据</param>
        protected internal void OnInit(object data);

        /// <summary>
        /// 更新生命周期
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        protected internal void OnUpdate(float escapeTime);

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        protected internal void OnDestroy();
    }
}
