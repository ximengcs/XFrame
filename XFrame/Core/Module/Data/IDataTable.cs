using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据表
    /// </summary>
    public interface IDataTable
    {
        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data">数据</param>
        internal void OnInit(object data);
    }

    /// <summary>
    /// 数据表
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDataTable<T> : IDataTable, IXEnumerable<T> where T : IDataRaw
    {
        /// <summary>
        /// 数据个数
        /// </summary>
        int Count { get; }

        /// <summary>
        /// 获取第一项数据
        /// </summary>
        /// <returns>获取到的数据</returns>
        T Get();

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="id">数据Id</param>
        /// <returns>获取到的数据</returns>
        T Get(int id);

        /// <summary>
        /// 检索数据
        /// </summary>
        /// <param name="name">数据项名</param>
        /// <param name="value">符合的值</param>
        /// <param name="target">结果存放列表</param>
        /// <returns>检索到的数据项</returns>
        int Select(string name, object value, List<T> target);
    }
}
