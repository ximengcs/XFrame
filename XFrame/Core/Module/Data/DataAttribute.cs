using System;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据表
    /// </summary>
    public class DataAttribute : Attribute
    {
        /// <summary>
        /// 数据表类型
        /// </summary>
        public DataType Type { get; }

        public DataAttribute(DataType type)
        {
            Type = type;
        }
    }

    /// <summary>
    /// 数据表类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 列表
        /// </summary>
        List,

        /// <summary>
        /// 对象
        /// </summary>
        Object
    }
}
