using System;
using XFrame.Core;
using XFrame.Modules.Reflection;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Collections;

namespace XFrame.Modules.Datas
{
    /// <inheritdoc/>
    [CommonModule]
    [XType(typeof(IDataModule))]
    public class DataModule : ModuleBase, IDataModule
    {
        #region Life Fun
        private IDataHelper m_Helper;

        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);

            if (!string.IsNullOrEmpty(XConfig.DefaultDataTableHelper))
            {
                Type type = Domain.TypeModule.GetType(XConfig.DefaultDataTableHelper);
                if (type != null)
                    m_Helper = (IDataHelper)Domain.TypeModule.CreateInstance(type);
            }

            if (m_Helper == null)
            {
                m_Helper = new DefaultDataHelper(this);
                m_Helper.OnInit();
            }

            TypeSystem typeSys = Domain.TypeModule.GetOrNewWithAttr<TableAttribute>();
            foreach (Type type in typeSys)
                Register(type);
        }
        #endregion

        #region Interface
        /// <inheritdoc/>
        public void Register(Type tableType)
        {
            m_Helper.AddTableType(tableType);
        }

        /// <inheritdoc/>
        public IDataTable<T> Add<T>(string json, int textType = default) where T : IDataRaw
        {
            return (IDataTable<T>)m_Helper.Add(json, typeof(T), textType);
        }

        /// <inheritdoc/>
        public IDataTable<T> Get<T>() where T : IDataRaw
        {
            if (m_Helper.TryGet(typeof(T), out List<IDataTable> datas))
            {
                return (IDataTable<T>)datas[default];
            }
            else
            {
                Log.Error(Log.XFrame, $"DataManager Get Error {typeof(T).Name}");
                return default;
            }
        }

        /// <inheritdoc/>
        public IDataTable<T> Get<T>(int tableIndex) where T : IDataRaw
        {
            if (m_Helper.TryGet(typeof(T), out List<IDataTable> datas))
            {
                return (IDataTable<T>)datas[tableIndex];
            }
            else
            {
                Log.Error(Log.XFrame, $"DataManager Get Error {typeof(T).Name}");
                return default;
            }
        }

        /// <inheritdoc/>
        public T GetOne<T>() where T : IDataRaw
        {
            return GetOne<T>(default);
        }

        /// <inheritdoc/>
        public T GetOne<T>(int tableIndex) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>(tableIndex);
            return table.Get();
        }

        /// <inheritdoc/>
        public T GetItem<T>(int itemId) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>();
            if (table != null)
                return table.Get(itemId);
            else
            {
                Log.Error(Log.XFrame, $"DataManager GetItem Error {typeof(DataTable<T>).Name} {itemId}");
                return default;
            }
        }

        /// <inheritdoc/>
        public T GetItem<T>(int tableIndex, int itemId) where T : IDataRaw
        {
            IDataTable<T> table = Get<T>(tableIndex);
            if (table != null)
                return table.Get(itemId);
            else
            {
                Log.Debug(Log.XFrame, $"DataManager GetItem Error {typeof(DataTable<T>).Name} {itemId}");
                return default;
            }
        }
        #endregion
    }
}