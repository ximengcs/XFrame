using System;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池
    /// </summary>
    public interface IPool : IDisposable
    {
        /// <summary>
        /// 对象池持有类型
        /// </summary>
        Type ObjectType { get; }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="obj">获取到的对象</param>
        /// <returns>是否是新创建的对象</returns>
        bool Require(out IPoolObject obj);

        /// <summary>
        /// 释放一个对象 
        /// </summary>
        /// <param name="obj">待释放的对象</param>
        void Release(IPoolObject obj);
    }

    /// <summary>
    /// 对象池
    /// </summary>
    /// <typeparam name="T">持有对象类型</typeparam>
    public interface IPool<T> : IPool where T : IPoolObject
    {
        /// <summary>
        /// 释放一个对象
        /// </summary>
        /// <param name="obj">要释放的对象</param>
        void Release(T obj);

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="obj">请求的对象</param>
        /// <returns>是否是新创建的对象，返回false表示从对象池中创建</returns>
        bool Require(out T obj);
    }
}
