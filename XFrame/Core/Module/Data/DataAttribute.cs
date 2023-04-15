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

        public DataAttribute(int tableType)
        {
            TableType = tableType;
        }
    }
}
