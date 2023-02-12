using System;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池系统
    /// 可以从对象池系统中获取对象池
    /// </summary>
    /// <typeparam name="T">对象池持有类型, 可为基类或接口</typeparam>
    public interface IPoolSystem<T> where T : IPoolObject
    {
        /// <summary>
        /// 获取一个对象池
        /// 从对象池系统的缓存中获取一个对象池，如果没有则创建一个默认容量的对象池
        /// </summary>
        /// <returns>获取到的对象池</returns>
        IPool<PoolObjType> Require<PoolObjType>() where PoolObjType : T;

        /// <summary>
        /// 获取一个对象池
        /// </summary>
        /// <param name="poolObjType">对象池持有类型</param>
        /// <returns>获取到的对象池</returns>
        IPool Require(Type poolObjType);

        /// <summary>
        /// 获取一个对象池
        /// 从对象池系统的缓存中获取给定容量的对象池，如果没有给定的容量，则创建一个给定容量的对象池
        /// </summary>
        /// <param name="capacity">对象池容量</param>
        /// <returns>获取到的对象池</returns>
        IPool<PoolType> Require<PoolType>(int capacity) where PoolType : T;

        /// <summary>
        /// 获取一个对象池
        /// </summary>
        /// <param name="poolObjType">对象池持有对象类型</param>
        /// <param name="capacity">对象池容量</param>
        /// <returns>获取到的对象池</returns>
        IPool Require(Type poolObjType, int capacity);

        /// <summary>
        /// 释放一个对象池
        /// </summary>
        /// <param name="pool">要释放的对象池</param>
        void Release(IPool pool);
    }
}
