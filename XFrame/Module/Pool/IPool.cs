using System;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public interface IPool : IEnumerable<IPoolObject>
    {
        /// <summary>
        /// 释放一个对象
        /// </summary>
        /// <param name="obj">要释放的对象</param>
        void Release(IPoolObject obj);

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">请求的对象</param>
        /// <returns>是否是新对象</returns>
        bool Require<T>(out T obj) where T : IPoolObject;

        bool Require(Type type, out IPoolObject obj);
    }
}
