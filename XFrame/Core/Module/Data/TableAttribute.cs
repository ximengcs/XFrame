using System;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据表
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 数据表类型Id
        /// </summary>
        public int TableType { get; }

        /// <summary>
        /// 数据表Json类型，此类型需要为泛型类或为空
        /// </summary>
        public Type JsonType { get; }

        /// <summary>
        /// 标记为数据表类型
        /// </summary>
        /// <param name="id">类型Id</param>
        /// <param name="jsonType">Json类型</param>
        public TableAttribute(int id, Type jsonType)
        {
            TableType = id;
            JsonType = jsonType;
        }

        /// <summary>
        /// 标记为数据表类型，Json类型为数据项类型
        /// </summary>
        /// <param name="id">类型Id</param>
        public TableAttribute(int id)
        {
            TableType = id;
            JsonType = default;
        }
    }
}
