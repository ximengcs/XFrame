using System;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public class DataTableHelper
    {
        private Dictionary<int, List<IDataTable>> m_Tables;

        public DataTableHelper()
        {
            m_Tables = new Dictionary<int, List<IDataTable>>();
        }

        public bool TryGetList<T>(out List<IDataTable> list) where T : IDataRaw
        {
            return m_Tables.TryGetValue(typeof(IDataTable<T>).GetHashCode(), out list);
        }

        public DT AddTable<T, DT>(string json) where DT : IDataTable<T> where T : IDataRaw
        {
            List<T> data = SerializeModule.Inst.DeserializeJsonToObject<List<T>>(json);
            Type type = typeof(DT);
            DT table = (DT)Activator.CreateInstance(type, data);
            int code = typeof(IDataTable<T>).GetHashCode();
            List<IDataTable> list;
            if (!m_Tables.TryGetValue(code, out list))
            {
                list = new List<IDataTable>();
                m_Tables[code] = list;
            }
            list.Add(table);
            return table;
        }

        public DT AddConfigTable<T, DT>(string json) where DT : IDataTable<T> where T : IDataRaw
        {
            T data = SerializeModule.Inst.DeserializeJsonToObject<T>(json);
            Type type = typeof(DT);
            DT table = (DT)Activator.CreateInstance(type, data);
            int code = typeof(IDataTable<T>).GetHashCode();
            List<IDataTable> list;
            if (!m_Tables.TryGetValue(code, out list))
            {
                list = new List<IDataTable>();
                m_Tables[code] = list;
            }
            list.Add(table);
            return table;
        }
    }
}
