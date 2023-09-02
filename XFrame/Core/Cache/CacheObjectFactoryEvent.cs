using System;
using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Core.Caches
{
    public class CacheObjectFactoryEvent : XEvent
    {
        private static int s_EventId;

        public static int EventId
        {
            get
            {
                if (s_EventId == 0)
                    s_EventId = typeof(CacheObjectFactoryEvent).GetHashCode();
                return s_EventId;
            }
        }

        public Type Target { get; private set; }

        public static CacheObjectFactoryEvent Create(Type target)
        {
            CacheObjectFactoryEvent e = References.Require<CacheObjectFactoryEvent>();
            e.Target = target;
            return e;
        }
    }
}
