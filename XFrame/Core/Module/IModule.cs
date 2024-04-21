using XFrame.Collections;

namespace XFrame.Core
{
    /// <summary>
    /// 模块 
    /// </summary>
    public interface IModule : IXItem
    {
        XDomain Domain { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">初始化数据</param>
        /// <param name="configCallback">初始化之前可进行模块配置的回调</param>
        protected internal void OnInit(object data, ModuleConfigAction configCallback = null);

        /// <summary>
        /// 开始运行生命周期
        /// </summary>
        protected internal void OnStart();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        protected internal void OnDestroy();
    }
}
