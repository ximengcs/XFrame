using System;
using System.Reflection;
using System.Collections;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Datas
{
    internal class DataTable<T> : IDataTable<T> where T : IDataRaw
    {
        private Type m_Type;
        private List<T> m_List;
        private Dictionary<int, T> m_Datas;
        private Dictionary<string, FieldInfo> m_Fields;
        private const string UNIQUE_KEY = "Id";
        private int m_MinId = -1;

        public DataTable(List<T> data)
        {
            m_Type = typeof(T);
            m_List = data;
            m_Fields = new Dictionary<string, FieldInfo>();
            m_Datas = new Dictionary<int, T>();

            FieldInfo info = m_Type.GetField(UNIQUE_KEY);
            if (info != null)
            {
                m_Fields.Add(UNIQUE_KEY, info);
                foreach (T item in data)
                {
                    int id = (int)info.GetValue(item);
                    if (id == 0)
                        continue;

                    if (m_MinId == -1 || id < m_MinId)
                        m_MinId = id;

                    if (m_Datas.ContainsKey(id))
                    {
                        Log.Warning("XFrame", $"DataTable Error {id} Multiple");
                        continue;
                    }
                    m_Datas.Add(id, item);
                }
            }
        }

        public int Count { get { return m_List.Count; } }

        public T Get()
        {
            return Get(m_MinId);
        }

        public T Get(int id)
        {
            if (m_Datas.ContainsKey(id))
            {
                return m_Datas[id];
            }
            else
            {
                Log.Error("XFrame", $"DataTable Error {m_Type.Name} {id}");
                return default;
            }
        }

        public List<T> Select(string name, int value)
        {
            FieldInfo field;
            if (!m_Fields.TryGetValue(name, out field))
            {
                field = m_Type.GetField(name);
                m_Fields.Add(name, field);
            }

            List<T> result = new List<T>();
            foreach (T item in m_Datas.Values)
            {
                object v = field.GetValue(item);
                if (v != null && (int)v == value)
                    result.Add(item);
            }

            return result;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_List.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
