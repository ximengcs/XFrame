using System;
using XFrame.Collections;

namespace XFrame.Modules.Pools
{
    /// <summary>
    /// 对象池
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// 对象池持有类型
        /// </summary>
        Type ObjectType { get; }

        IPoolHelper Helper { get; }

        /// <summary>
        /// 获取一个对象
        /// </summary>
        /// <param name="obj">获取到的对象</param>
        IPoolObject Require(int poolKey = default, object userData = default);

        /// <summary>
        /// 释放一个对象 
        /// </summary>
        /// <param name="obj">待释放的对象</param>
        void Release(IPoolObject obj);

        /// <summary>
        /// 生成池对象
        /// </summary>
        /// <param name="count">生成数量</param>
        XLinkList<IPoolObject> Spawn(int poolKey = default, int count = 1, object userData = default);

        /// <summary>
        /// 清除所有池化对象
        /// </summary>
        void ClearObject();
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
        new T Require(int poolKey = default, object userData = default);
    }
}
