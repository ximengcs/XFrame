
using System;

namespace XFrame.Modules.Caches
{
    /// <summary>
    /// Cache对象工厂
    /// </summary>
    public class CacheObjectAttribute : Attribute
    {
        /// <summary>
        /// 生产对象的类型
        /// </summary>
        public Type Target { get; }

        /// <summary>
        /// 缓存数量
        /// </summary>
        public int CacheCount { get; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="targetType">对象类型</param>
        /// <param name="cacheCount">缓存数量</param>
        public CacheObjectAttribute(Type targetType, int cacheCount = 1)
        {
            Target = targetType;
            CacheCount = cacheCount;
        }
    }
}
