using System;
using System.Collections.Generic;

namespace XFrame.Core
{
    /// <summary>
    /// 简单数据提供者
    /// </summary>
    public class DataProvider : IDataProvider
    {
        #region Inner Fileds
        private Dictionary<Type, object> m_MainDatas;
        private Dictionary<Type, Dictionary<string, object>> m_Datas;
        #endregion

        #region Constructor
        /// <summary>
        /// 构造器
        /// </summary>
        public DataProvider()
        {
            m_MainDatas = new Dictionary<Type, object>();
            m_Datas = new Dictionary<Type, Dictionary<string, object>>();
        }
        #endregion

        #region IDataProvider Interface
        public bool HasData<T>()
        {
            return m_MainDatas.ContainsKey(typeof(T));
        }

        public bool HasData<T>(string name)
        {
            if (m_Datas.TryGetValue(typeof(T), out Dictionary<string, object> values))
            {
                return values.ContainsKey(name);
            }
            return false;
        }

        public T GetData<T>()
        {
            if (m_MainDatas.TryGetValue(typeof(T), out object value))
                return (T)value;
            else
                return default;
        }

        public T GetData<T>(string name)
        {
            if (m_Datas.TryGetValue(typeof(T), out Dictionary<string, object> values))
            {
                if (values.TryGetValue(name, out object value))
                    return (T)value;
            }

            return default;
        }

        public void SetData<T>(T value)
        {
            m_MainDatas[typeof(T)] = value;
        }

        public void SetData<T>(string name, T value)
        {
            Type type = typeof(T);
            Dictionary<string, object> values;
            if (!m_Datas.TryGetValue(type, out values))
            {
                values = new Dictionary<string, object>();
                m_Datas.Add(type, values);
            }

            values[name] = value;
        }

        public void ClearData()
        {
            m_MainDatas.Clear();
            m_Datas.Clear();
        }
        #endregion
    }
}
