
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace XFrame.Core.NewXCore
{
    public static partial class XDomain_
    {
        private static ICore s_Main;
        private static ConcurrentDictionary<int, ICore> s_Domains;


    }
}
