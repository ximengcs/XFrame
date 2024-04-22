using System;
using System.Collections;
using System.Collections.Generic;
using XFrame.Core.Binder;

namespace XFrame.Collections
{
    public class XList<T> :
        ICollection<T>,
        IEnumerable<T>,
        IEnumerable,
        IList<T>,
        IReadOnlyCollection<T>,
        IReadOnlyList<T>,
        ICollection,
        IList,
        IChangeableValue
    {
        private List<T> m_Container;
        private Action m_ChangeHandler;

        event Action IChangeableValue.OnValueChange
        {
            add => m_ChangeHandler += value;
            remove => m_ChangeHandler -= value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_Container.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)m_Container).GetEnumerator();
        }

        public void Add(T item)
        {
            m_Container.Add(item);
            m_ChangeHandler?.Invoke();
        }

        public int Add(object value)
        {
            int count = ((IList)m_Container).Add(value);
            m_ChangeHandler?.Invoke();
            return count;
        }

        public bool Replace(T value, T newValue)
        {
            int index = IndexOf(value);
            if (index != -1)
            {
                m_Container[index] = newValue;
                m_ChangeHandler?.Invoke();
            }
            return index != -1;
        }

        void IList.Clear()
        {
            m_Container.Clear();
            m_ChangeHandler?.Invoke();
        }

        public bool Contains(object value)
        {
            return ((IList)m_Container).Contains(value);
        }

        public int IndexOf(object value)
        {
            return ((IList)m_Container).IndexOf(value);
        }

        public void Insert(int index, object value)
        {
            ((IList)m_Container).Insert(index, value);
            m_ChangeHandler?.Invoke();
        }

        public void Remove(object value)
        {
            ((IList)m_Container).Remove(value);
            m_ChangeHandler?.Invoke();
        }

        void IList.RemoveAt(int index)
        {
            m_Container.RemoveAt(index);
            m_ChangeHandler?.Invoke();
        }

        public bool IsFixedSize => ((IList)m_Container).IsFixedSize;

        void ICollection<T>.Clear()
        {
            m_Container.Clear();
            m_ChangeHandler?.Invoke();
        }

        public bool Contains(T item)
        {
            return m_Container.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_Container.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            bool success = m_Container.Remove(item);
            m_ChangeHandler?.Invoke();
            return success;
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)m_Container).CopyTo(array, index);
        }

        public int Count => m_Container.Count;

        public bool IsSynchronized => ((ICollection)m_Container).IsSynchronized;

        public object SyncRoot => ((ICollection)m_Container).SyncRoot;

        public bool IsReadOnly => ((ICollection<T>)m_Container).IsReadOnly;

        object IList.this[int index]
        {
            get => ((IList)m_Container)[index];
            set
            {
                ((IList)m_Container)[index] = value;
                m_ChangeHandler?.Invoke();
            }
        }

        public int IndexOf(T item)
        {
            return m_Container.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_Container.Insert(index, item);
            m_ChangeHandler?.Invoke();
        }

        void IList<T>.RemoveAt(int index)
        {
            m_Container.RemoveAt(index);
            m_ChangeHandler?.Invoke();
        }

        public T this[int index]
        {
            get => m_Container[index];
            set
            {
                m_Container[index] = value;
                m_ChangeHandler?.Invoke();
            }
        }
    }
}