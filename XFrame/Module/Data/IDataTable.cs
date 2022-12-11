using System.Collections.Generic;

namespace XFrame.Modules
{
    public interface IDataTable
    {
    }

    public interface IDataTable<T> : IDataTable, IEnumerable<T> where T : IDataRaw
    {
        int Count { get; }
        T Get();
        T Get(int id);
        List<T> Select(string name, int value);
    }
}
