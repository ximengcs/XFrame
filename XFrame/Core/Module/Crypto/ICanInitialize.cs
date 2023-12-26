
namespace XFrame.Modules.Crypto
{
    internal interface ICanInitialize
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="keyStr">密钥</param>
        /// <param name="ivStr">密钥</param>
        void OnInit(string keyStr, string ivStr);
    }
}
