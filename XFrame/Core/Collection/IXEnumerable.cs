using System.Collections.Generic;

namespace XFrame.Collections
{
    /// <summary>
    /// 迭代类型
    /// </summary>
    public enum XItType
    {
        /// <summary>
        /// 前向迭代
        /// </summary>
        Forward,
        
        /// <summary>
        /// 后向迭代
        /// </summary>
        Backward
    }

    /// <summary>
    /// 可迭代类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IXEnumerable<T>
    {
        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        IEnumerator<T> GetEnumerator();

        /// <summary>
        /// 设置迭代类型
        /// </summary>
        /// <param name="type"></param>
        void SetIt(XItType type);
    }
}
