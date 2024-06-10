
using System;
using XFrame.Core;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据模块
    /// </summary>
    public interface IDataModule : IModule
    {
        /// <summary>
        /// 注册数据表类型
        /// </summary>
        /// <param name="tableType">数据表类型</param>
        void Register(Type tableType);

        /// <summary>
        /// 添加数据表
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="json">需要被序列化的数据</param>
        /// <param name="textType">数据类型</param>
        /// <returns>数据表接口实例</returns>
        IDataTable<T> Add<T>(string json, int textType = default) where T : IDataRaw;

        /// <summary>
        /// 获取数据表(第一个添加的数据表)
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <returns>数据表接口实例</returns>
        IDataTable<T> Get<T>() where T : IDataRaw;

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="tableIndex">数据表位置(当有多个同类型的数据表时，用此位置可区分)</param>
        /// <returns>数据表接口实例</returns>
        IDataTable<T> Get<T>(int tableIndex) where T : IDataRaw;

        /// <summary>
        /// 获取数据表默认项数据(第一个添加的数据表)
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <returns>数据</returns>
        T GetOne<T>() where T : IDataRaw;

        /// <summary>
        /// 获取数据表默认项数据
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="tableIndex">数据表位置(当有多个同类型的数据表时，用此位置可区分)</param>
        /// <returns>数据</returns>
        T GetOne<T>(int tableIndex) where T : IDataRaw;

        /// <summary>
        /// 获取数据表数据项
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="itemId">数据项Id</param>
        /// <returns>数据</returns>
        T GetItem<T>(int itemId) where T : IDataRaw;

        /// <summary>
        /// 获取数据表数据项
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="tableIndex">数据表位置(当有多个同类型的数据表时，用此位置可区分)</param>
        /// <param name="itemId">数据项Id</param>
        /// <returns>数据</returns>
        T GetItem<T>(int tableIndex, int itemId) where T : IDataRaw;
    }
}
