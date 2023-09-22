
using System;
using XFrame.Core;

namespace XFrame.Modules.Rand
{
    public interface IRandModule : IModule
    {
        string RandString(int num = 8);

        string RandPath(int num = 8);

        T RandEnum<T>(params T[] exlusion) where T : Enum;
    }
}
