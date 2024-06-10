using System;

namespace XFrame.Core
{
    /// <summary>
    /// 数据提供者
    /// </summary>
    public interface IDataProvider
    {
        /// <summary>
        /// 是否含有数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>true为含有</returns>
        bool HasData<T>();

        /// <summary>
        /// 是否含有数据
        /// </summary>
        /// <param name="name">数据名</param>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>true为含有</returns>
        bool HasData<T>(string name);

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="value">数据</param>
        void SetData<T>(T value);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <returns>获取到的数据</returns>
        T GetData<T>();

        /// <summary>
        /// 设置数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">数据标识</param>
        /// <param name="value">数据</param>
        void SetData<T>(string name, T value);

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="name">数据标识</param>
        /// <returns>获取到的数据</returns>
        T GetData<T>(string name);

        /// <summary>
        /// 清除数据
        /// </summary>
        void ClearData();
    }
}
