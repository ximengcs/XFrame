using System;
using System.Collections.Generic;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据模块辅助器
    /// </summary>
    public interface IDataHelper
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        internal void OnInit();

        /// <summary>
        /// 添加数据表
        /// </summary>
        /// <param name="json">需要被序列化的数据</param>
        /// <param name="datatype">数据表持有数据类型</param>
        /// <returns>数据表</returns>
        IDataTable Add(string json, Type datatype);

        /// <summary>
        /// 尝试获取一种类型的数据表
        /// </summary>
        /// <param name="datatype">数据表持有数据类型</param>
        /// <param name="list">数据表列表</param>
        /// <returns>是否获取成功</returns>
        bool TryGet(Type datatype, out List<IDataTable> list);
    }
}
