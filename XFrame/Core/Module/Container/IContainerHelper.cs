
using System;

namespace XFrame.Modules.Containers
{
    public interface IContainerHelper
    {
        int GetPoolKey(Type type, int id, IContainer master);
    }
}
