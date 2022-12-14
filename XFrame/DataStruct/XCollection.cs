using System;
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Collections
{
    /// <summary>
    /// XItem的集合, 可以通过Id快速读取元素
    /// </summary>
    /// <typeparam name="T">持有的数据类型</typeparam>
    public partial class XCollection<T> : ICollection<T> where T : IXItem
    {
        private Dictionary<Type, Dictionary<int, T>> m_WithTypes;
        private Dictionary<Type, T> m_Mains;
        private List<T> m_Elements;

        /// <summary>
        /// 元素数量
        /// </summary>
        public int Count => m_Elements.Count;

        /// <summary>
        /// 是否只读，总是返回false
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        /// 构造集合
        /// </summary>
        public XCollection()
        {
            m_WithTypes = new Dictionary<Type, Dictionary<int, T>>();
            m_Mains = new Dictionary<Type, T>();
            m_Elements = new List<T>();
        }

        /// <summary>
        /// 添加一个元素
        /// </summary>
        /// <param name="entity">要添加的元素</param>
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

        /// <summary>
        /// 移除一个元素
        /// </summary>
        /// <param name="item">要移除的元素</param>
        /// <returns>是否移除成功</returns>
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

        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            m_WithTypes.Clear();
            m_Mains.Clear();
            m_Elements.Clear();
        }

        /// <summary>
        /// 是否包含某项元素
        /// </summary>
        /// <param name="item">检查的元素</param>
        /// <returns>true表是包含</returns>
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

        /// <summary>
        /// 拷贝元素到另一个数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            int count = array.Length;
            for (int i = arrayIndex; i < count; i++)
                Add(array[i]);
        }

        /// <summary>
        /// 获取给定类型的第一个添加到集合中的元素
        /// </summary>
        /// <typeparam name="TEntity">类型</typeparam>
        /// <returns>获取到的元素</returns>
        public TEntity Get<TEntity>() where TEntity : T
        {
            if (m_Mains.TryGetValue(typeof(TEntity), out T entity))
                return (TEntity)entity;
            else
                return default;
        }

        /// <summary>
        /// 获取指定id和给定类型的元素 
        /// </summary>
        /// <typeparam name="TEntity">需要获取的类型</typeparam>
        /// <param name="entityId">元素Id</param>
        /// <returns>获取到的元素</returns>
        public TEntity Get<TEntity>(int entityId) where TEntity : T
        {
            if (m_WithTypes.TryGetValue(typeof(TEntity), out Dictionary<int, T> entities))
                if (entities.TryGetValue(entityId, out T entity))
                    return (TEntity)entity;
            return default;
        }

        /// <summary>
        /// 获取正向迭代器
        /// </summary>
        /// <returns>正向迭代器</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return m_Elements.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 获取反向迭代器
        /// </summary>
        /// <returns>反向迭代器</returns>
        public IEnumerator<T> GetBackEnumerator()
        {
            return new BackEnumerator(m_Elements);
        }
    }
}
