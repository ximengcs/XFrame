
namespace XFrame.Core
{
    internal interface IModuleCanInitialize
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">初始化数据</param>
        /// <param name="configCallback">初始化之前可进行模块配置的回调</param>
        void OnInit(object data, ModuleConfigAction configCallback = null);
    }
}
