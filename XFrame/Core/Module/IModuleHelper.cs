
namespace XFrame.Core
{
    /// <summary>
    /// 模块辅助器
    /// </summary>
    public interface IModuleHelper
    {
        /// <summary>
        /// 模块创建生命周期
        /// </summary>
        /// <param name="module">被创建的模块</param>
        protected internal void OnModuleCreate(IModule module);

        protected internal void OnModuleHandle(IModule module, IModuleHandler handler);

        /// <summary>
        /// 模块销毁生命周期
        /// </summary>
        protected internal void OnModuleDestroy();
    }
}
