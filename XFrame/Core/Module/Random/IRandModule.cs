
using System;
using XFrame.Core;

namespace XFrame.Modules.Rand
{
    /// <summary>
    /// 随机模块
    /// </summary>
    public interface IRandModule : IModule
    {
        /// <summary>
        /// 随机产生 <paramref name="num"/> 长度的字符串
        /// </summary>
        /// <param name="num">字符串长度</param>
        /// <returns>字符串</returns>
        string RandString(int num = 8);

        /// <summary>
        /// 随机产生 <paramref name="num"/> 长度的路径
        /// </summary>
        /// <param name="num">字符串长度</param>
        /// <returns>字符串</returns>
        string RandPath(int num = 8);

        /// <summary>
        /// 随机一个枚举
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="exlusion">剔除列表</param>
        /// <returns>枚举</returns>
        T RandEnum<T>(params T[] exlusion) where T : Enum;
    }
}
