using System;
using System.Collections.Generic;

namespace XFrame.Core
{
    public class DataProvider : IDataProvider
    {
        private Dictionary<Type, object> m_MainDatas;
        private Dictionary<Type, Dictionary<string, object>> m_Datas;

        public DataProvider()
        {
            m_MainDatas = new Dictionary<Type, object>();
            m_Datas = new Dictionary<Type, Dictionary<string, object>>();
        }

        public T GetData<T>() where T : class
        {
            if (m_MainDatas.TryGetValue(typeof(T), out object value))
                return value as T;
            else
                return default;
        }

        public T GetData<T>(string name) where T : class
        {
            if (m_Datas.TryGetValue(typeof(T), out Dictionary<string, object> values))
            {
                if (values.TryGetValue(name, out object value))
                    return value as T;
            }

            return default;
        }

        public void SetData<T>(T value) where T : class
        {
            m_MainDatas[typeof(T)] = value;
        }

        public void SetData<T>(string name, T value) where T : class
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

        public void Dispose()
        {
            m_MainDatas.Clear();
            m_Datas.Clear();
            m_MainDatas = null;
            m_Datas = null;
        }
    }
}
