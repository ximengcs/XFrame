
using System;
using System.Collections.Generic;
using XFrame.Core;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池模块
    /// </summary>
    public interface IPoolModule : IModule
    {
        /// <summary>
        /// 获取所有对象池的集合
        /// </summary>
        IEnumerable<IPool> AllPool { get; }

        /// <summary>
        /// 创建或获取一个对象池
        /// </summary>
        /// <typeparam name="T">对象池持有类型</typeparam>
        /// <param name="helper">对象池使用辅助器</param>
        /// <returns>对象池</returns>
        IPool<T> GetOrNew<T>(IPoolHelper helper = null) where T : IPoolObject;

        /// <summary>
        /// 创建或获取一个对象池
        /// </summary>
        /// <param name="objType">对象池持有数据类型</param>
        /// <param name="helper">对象池使用辅助器</param>
        /// <returns>对象池</returns>
        IPool GetOrNew(Type objType, IPoolHelper helper = null);
    }
}
