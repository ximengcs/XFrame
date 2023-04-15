using System;
using XFrame.Core;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据模块
    /// </summary>
    [XModule]
    public class DataModule : SingletonModule<DataModule>
    {
        #region Life Fun
        private IDataHelper m_Helper;

        protected override void OnInit(object data)
        {
            base.OnInit(data);

            if (!string.IsNullOrEmpty(XConfig.DefaultDataTableHelper))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultDataTableHelper);
                if (type != null)
                    m_Helper = (IDataHelper)Activator.CreateInstance(type);
            }

            if (m_Helper == null)
            {
                m_Helper = new DefaultDataHelper();
                m_Helper.OnInit();
            }

            TypeSystem typeSys = TypeModule.Inst.GetOrNewWithAttr<TableAttribute>();
            foreach (Type type in typeSys)
                Register(type);
        }
        #endregion

        #region Interface
        /// <summary>
        /// 注册数据表类型
        /// </summary>
        /// <param name="tableType">数据表类型</param>
        public void Register(Type tableType)
        {
            m_Helper.AddTableType(tableType);
        }

        /// <summary>
        /// 添加数据表
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="json">需要被序列化的数据</param>
        /// <returns>数据表接口实例</returns>
        public IDataTable<T> Add<T>(string json) where T : IDataRaw
        {
            return (IDataTable<T>)m_Helper.Add(json, typeof(T));
        }

        /// <summary>
        /// 获取数据表(第一个添加的数据表)
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <returns>数据表接口实例</returns>
        public IDataTable<T> Get<T>() where T : IDataRaw
        {
            if (m_Helper.TryGet(typeof(T), out List<IDataTable> datas))
            {
                return (IDataTable<T>)datas[default];
            }
            else
            {
                Log.Error("XFrame", $"DataManager Get Error {typeof(T).Name}");
                return default;
            }
        }

        /// <summary>
        /// 获取数据表
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="tableIndex">数据表位置(当有多个同类型的数据表时，用此位置可区分)</param>
        /// <returns>数据表接口实例</returns>
        public IDataTable<T> Get<T>(int tableIndex) where T : IDataRaw
        {
            if (m_Helper.TryGet(typeof(T), out List<IDataTable> datas))
            {
                return (IDataTable<T>)datas[tableIndex];
            }
            else
            {
                Log.Error("XFrame", $"DataManager Get Error {typeof(T).Name}");
                return default;
            }
        }

        /// <summary>
        /// 获取数据表默认项数据(第一个添加的数据表)
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <returns>数据</returns>
        public T GetOne<T>() where T : IDataRaw
        {
            return GetOne<T>(default);
        }

        /// <summary>
        /// 获取数据表默认项数据
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="tableIndex">数据表位置(当有多个同类型的数据表时，用此位置可区分)</param>
        /// <returns>数据</returns>
        public T GetOne<T>(int tableIndex) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>(tableIndex);
            return table.Get();
        }

        /// <summary>
        /// 获取数据表数据项
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="itemId">数据项Id</param>
        /// <returns>数据</returns>
        public T GetItem<T>(int itemId) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>();
            if (table != null)
                return table.Get(itemId);
            else
            {
                Log.Error("XFrame", $"DataManager GetItem Error {typeof(DataTable<T>).Name} {itemId}");
                return default;
            }
        }

        /// <summary>
        /// 获取数据表数据项
        /// </summary>
        /// <typeparam name="T">数据表持有数据类型</typeparam>
        /// <param name="tableIndex">数据表位置(当有多个同类型的数据表时，用此位置可区分)</param>
        /// <param name="itemId">数据项Id</param>
        /// <returns>数据</returns>
        public T GetItem<T>(int tableIndex, int itemId) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>(tableIndex);
            if (table != null)
                return table.Get(itemId);
            else
            {
                Log.Debug("XFrame", $"DataManager GetItem Error {typeof(DataTable<T>).Name} {itemId}");
                return default;
            }
        }
        #endregion
    }
}