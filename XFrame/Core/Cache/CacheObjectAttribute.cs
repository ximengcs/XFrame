
using System;

namespace XFrame.Core.Caches
{
    public class CacheObjectAttribute : Attribute
    {
        public Type Target { get; }
        public int CacheCount { get; }

        public CacheObjectAttribute(Type targetType, int cacheCount = 1)
        {
            Target = targetType;
            CacheCount = cacheCount;
        }
    }
}
