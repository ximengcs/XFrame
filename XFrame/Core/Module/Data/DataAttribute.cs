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
        public int TableType { get; }

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="tableType">数据表类型</param>
        public DataAttribute(int tableType)
        {
            TableType = tableType;
        }
    }
}
