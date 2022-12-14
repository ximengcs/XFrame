using System.Collections;
using System.Collections.Generic;

namespace XFrame.Modules
{
    internal class ConfigTable<T> : IDataTable<T> where T : IDataRaw
    {
        private List<T> m_UnUse;
        private T m_Data;
        public int Count => 1;

        public ConfigTable(T data)
        {
            m_Data = data;
            m_UnUse = new List<T>(1) { data };
        }

        public T Get(int id)
        {
            return m_Data;
        }

        public T Get()
        {
            return Get(0);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_UnUse.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_UnUse.GetEnumerator();
        }

        public List<T> Select(string name, int value)
        {
            return m_UnUse;
        }
    }
}
