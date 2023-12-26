using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Modules.Datas
{
    [Table(TableType.Object)]
    internal class ConfigTable<T> : IDataTable, IDataTable<T>, ICanInitialize where T : IDataRaw
    {
        private T m_Data;

        public int Count => 1;

        void ICanInitialize.OnInit(object data)
        {
            m_Data = (T)data;
        }

        public T Get(int id)
        {
            return m_Data;
        }

        public T GetByIndex(int index)
        {
            return m_Data;
        }

        public T Get()
        {
            return m_Data;
        }

        public int Select(string name, object value, List<T> target)
        {
            target.Add(m_Data);
            return Count;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SingleValueEnumerator<T>(m_Data);
        }

        public void SetIt(XItType type)
        {

        }

        public override string ToString()
        {
            return m_Data.ToString();
        }
    }
}
