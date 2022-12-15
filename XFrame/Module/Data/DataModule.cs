using XFrame.Core;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Datas
{
    /// <summary>
    /// 数据模块
    /// </summary>
    public class DataModule : SingletonModule<DataModule>
    {
        #region Life Fun
        private DataTableHelper m_Helper;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Helper = new DataTableHelper();
        }
        #endregion

        #region Interface
        public IDataTable<T> Add<T>(string json) where T : IDataRaw
        {
            return m_Helper.AddTable<T, DataTable<T>>(json);
        }

        public IDataTable<T> AddConfig<T>(string json) where T : IDataRaw
        {
            return m_Helper.AddConfigTable<T, ConfigTable<T>>(json);
        }

        public IDataTable<T> Get<T>() where T : IDataRaw
        {
            if (m_Helper.TryGetList<T>(out List<IDataTable> datas))
            {
                return (IDataTable<T>)datas[default];
            }
            else
            {
                Log.Error("XFrame", $"DataManager Get Error {typeof(T).Name}");
                return default;
            }
        }

        public IDataTable<T> Get<T>(int tableId) where T : IDataRaw
        {
            if (m_Helper.TryGetList<T>(out List<IDataTable> datas))
            {
                return (IDataTable<T>)datas[tableId - 1];
            }
            else
            {
                Log.Error("XFrame", $"DataManager Get Error {typeof(T).Name}");
                return default;
            }
        }

        public T GetOne<T>() where T : IDataRaw
        {
            return GetOne<T>(1);
        }

        public T GetOne<T>(int tableId) where T : IDataRaw
        {
            IDataTable<T> datas = Get<T>(tableId);
            return datas.Get();
        }

        public T GetItem<T>(int tableId) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>();
            if (table != null)
                return table.Get(tableId);
            else
            {
                Log.Error("XFrame", $"DataManager GetItem Error {typeof(DataTable<T>).Name} {tableId}");
                return default;
            }
        }

        public T GetItem<T>(int tableId, int itemId) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>(tableId);
            if (table != null)
                return table.Get(itemId);
            else
            {
                Log.Debug($"DataManager GetItem Error {typeof(DataTable<T>).Name} {itemId}");
                return default;
            }
        }
        #endregion
    }
}