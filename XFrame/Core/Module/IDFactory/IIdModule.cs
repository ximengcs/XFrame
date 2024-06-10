
using XFrame.Core;

namespace XFrame.Modules.ID
{
    /// <summary>
    /// Id模块
    /// </summary>
    public interface IIdModule : IModule
    {
        /// <summary>
        /// 生成一个Id
        /// </summary>
        /// <returns>生成的Id</returns>
        int Next();
    }
}
