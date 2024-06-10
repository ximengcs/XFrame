using System;

namespace XFrame.Modules.Reflection
{
    /// <summary>
    /// 类型检查辅助器
    /// </summary>
    public interface ITypeCheckHelper
    {
        /// <summary>
        /// 程序集列表
        /// </summary>
        string[] AssemblyList { get; }

        /// <summary>
        /// 检查类型
        /// </summary>
        /// <param name="type">目标类型</param>
        /// <returns>true表示类型通过检查</returns>
        bool CheckType(Type type);
    }
}
