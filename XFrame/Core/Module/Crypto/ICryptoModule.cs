
using XFrame.Core;

namespace XFrame.Modules.Crypto
{
    public interface ICryptoModule : IModule
    {
        ICryptor New(string keyStr, string ivStr);
        ICryptor New();
    }
}
