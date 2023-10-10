
namespace XFrame.Core
{
    /// <summary>
    /// 模块辅助器
    /// </summary>
    public interface IModuleHelper
    {
        protected internal void OnInit();
        protected internal void OnDestroy();

        protected internal void OnModuleCreate(IModule module);

        /// <summary>
        /// 模块创建生命周期
        /// </summary>
        /// <param name="module">被创建的模块</param>
        protected internal void OnModuleInit(IModule module);

        protected internal void OnModuleStart(IModule module);

        /// <summary>
        /// 模块销毁生命周期
        /// </summary>
        protected internal void OnModuleDestroy(IModule module);
    }
}
