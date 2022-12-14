
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
        void OnModuleCreate(IModule module);

        /// <summary>
        /// 模块更新生命周期
        /// </summary>
        void OnModuleUpdate();

        /// <summary>
        /// 模块销毁生命周期
        /// </summary>
        void OnModuleDestroy();
    }
}
