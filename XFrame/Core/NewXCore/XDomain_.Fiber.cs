
using System.Collections.Concurrent;
using System.Collections.Generic;
using XFrame.Core.Threads;

namespace XFrame.Core.NewXCore
{
    public static partial class XDomain_
    {
        private static ConcurrentDictionary<int, Fiber> s_Fibers;
    }
}
