using System;

namespace XFrame.Modules.Pools
{
    public interface IPoolHelper
    {
        IPoolObject Factory(Type type);
    }
}
