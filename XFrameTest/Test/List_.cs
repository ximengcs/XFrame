using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFrame.Core.Binder;

namespace XFrameTest.Test
{
    public class List_<T> : IList<T>, IChangeableValue
    {
        private List<T> list;

        public T this[int index] { get => list[index]; set => list[index] = value; }

        public int Count => list.Count;

        public bool IsReadOnly => false;

        public event Action OnChange;

        public List_()
        {
            list = new List<T>();
        }

        public void Add(T item)
        {
            list.Add(item);
            OnChange?.Invoke();
        }

        public void Clear()
        {
            list.Clear();
            OnChange?.Invoke();
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            list.Insert(index, item);
            OnChange?.Invoke();
        }

        public bool Remove(T item)
        {
            bool success = list.Remove(item);
            OnChange?.Invoke();
            return success;
        }

        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
            OnChange?.Invoke();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
                sb.Append(item + " ");
            return sb.ToString();
        }
    }
}
