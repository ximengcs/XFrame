using System;
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    public partial class XCollection<T> : ICollection<T> where T : IXItem
    {
        private Dictionary<Type, Dictionary<int, T>> m_WithTypes;
        private Dictionary<Type, T> m_Mains;
        private List<T> m_Elements;

        public int Count => m_Elements.Count;

        public bool IsReadOnly => false;

        public XCollection()
        {
            m_WithTypes = new Dictionary<Type, Dictionary<int, T>>();
            m_Mains = new Dictionary<Type, T>();
            m_Elements = new List<T>();
        }

        public void Add(T entity)
        {
            Type type = entity.GetType();
            Dictionary<int, T> entities;
            if (!m_WithTypes.TryGetValue(type, out entities))
            {
                entities = new Dictionary<int, T>();
                m_WithTypes.Add(type, entities);
            }

            m_Elements.Add(entity);
            entities.Add(entity.Id, entity);
            if (!m_Mains.ContainsKey(type))
                m_Mains.Add(type, entity);
        }

        public bool Remove(T item)
        {
            bool success = false;
            Type type = item.GetType();
            if (m_WithTypes.TryGetValue(type, out Dictionary<int, T> entities))
                success = entities.Remove(item.Id);
            if (m_Mains.Remove(type))
            {
                if (!success)
                    success = true;
            }
            m_Elements.Remove(item);
            return success;
        }

        public void Clear()
        {
            m_WithTypes.Clear();
            m_Mains.Clear();
            m_Elements.Clear();
        }

        public bool Contains(T item)
        {
            Type type = item.GetType();
            if (m_Mains.ContainsKey(type))
                return true;
            if (m_WithTypes.TryGetValue(type, out Dictionary<int, T> entities))
            {
                if (entities.ContainsKey(item.Id))
                    return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int count = array.Length;
            for (int i = arrayIndex; i < count; i++)
                Add(array[i]);
        }

        public TEntity Get<TEntity>() where TEntity : T
        {
            if (m_Mains.TryGetValue(typeof(TEntity), out T entity))
                return (TEntity)entity;
            else
                return default;
        }

        public TEntity Get<TEntity>(int entityId) where TEntity : T
        {
            if (m_WithTypes.TryGetValue(typeof(TEntity), out Dictionary<int, T> entities))
                if (entities.TryGetValue(entityId, out T entity))
                    return (TEntity)entity;
            return default;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetBackEnumerator()
        {
            return new BackEnumerator(m_Elements);
        }
    }
}
