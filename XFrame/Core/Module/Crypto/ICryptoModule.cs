
using XFrame.Core;

namespace XFrame.Modules.Crypto
{
    /// <summary>
    /// 数据加密模块
    /// </summary>
    public interface ICryptoModule : IModule
    {
        /// <summary>
        /// 创建加密器
        /// </summary>
        /// <param name="keyStr">密钥</param>
        /// <param name="ivStr">密钥</param>
        /// <returns>加密器</returns>
        ICryptor New(string keyStr, string ivStr);

        /// <summary>
        /// 使用默认密钥创建加密器
        /// </summary>
        /// <returns>加密器</returns>
        ICryptor New();
    }
}
