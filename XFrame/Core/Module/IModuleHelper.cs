
namespace XFrame.Core
{
    /// <summary>
    /// 模块辅助器
    /// </summary>
    public interface IModuleHelper
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        protected internal void OnInit();

        /// <summary>
        /// 销毁生命周期
        /// </summary>
        protected internal void OnDestroy();

        /// <summary>
        /// 模块被创建生命周期
        /// </summary>
        /// <param name="module">模块</param>
        protected internal void OnModuleCreate(IModule module);

        /// <summary>
        /// 模块初始化生命周期
        /// </summary>
        /// <param name="module">被初始化的模块</param>
        protected internal void OnModuleInit(IModule module);

        /// <summary>
        /// 模块开始运行生命周期
        /// </summary>
        /// <param name="module">模块</param>
        protected internal void OnModuleStart(IModule module);

        /// <summary>
        /// 模块销毁生命周期
        /// </summary>
        /// <param name="module">被销毁的模块</param>
        protected internal void OnModuleDestroy(IModule module);
    }
}
