﻿using System;
using System.Collections.Generic;

namespace XFrame.Collections
{
    /// <summary>
    /// XItem的集合, 可以通过Id快速读取元素
    /// </summary>
    /// <typeparam name="T">持有的数据类型</typeparam>
    public partial class XCollection<T> : IXEnumerable<T> where T : IXItem
    {
        #region Const Fields
        private const int DEFAULT_CAPACITY = 16;
        #endregion

        #region Inner Fields
        private Dictionary<Type, Dictionary<int, T>> m_WithTypes;
        private Dictionary<Type, T> m_Mains;
        private XLinkList<T> m_Elements;
        private Dictionary<int, XLinkNode<T>> m_NodeMap;
        #endregion

        #region Constructor
        /// <summary>
        /// 构造集合
        /// </summary>
        public XCollection(int startCapacity = DEFAULT_CAPACITY)
        {
            m_WithTypes = new Dictionary<Type, Dictionary<int, T>>(startCapacity);
            m_Mains = new Dictionary<Type, T>(startCapacity);
            m_Elements = new XLinkList<T>(false);
            m_NodeMap = new Dictionary<int, XLinkNode<T>>(startCapacity);
        }
        #endregion

        #region Interface
        /// <summary>
        /// 元素数量
        /// </summary>
        public int Count => m_Elements.Count;

        /// <summary>
        /// 添加一个元素 时间复杂度O(1)
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

            XLinkNode<T> node = m_Elements.AddLast(entity);
            m_NodeMap.Add(node.GetHashCode(), node);

            entities.Add(entity.Id, entity);
            if (!m_Mains.ContainsKey(type))
                m_Mains.Add(type, entity);
        }

        /// <summary>
        /// 移除一个元素 时间复杂度O(1)
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

            int hash = item.GetHashCode();
            if (m_NodeMap.TryGetValue(hash, out XLinkNode<T> node))
            {
                node.Delete();
                m_NodeMap.Remove(hash);
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
            m_NodeMap.Clear();
            m_Elements.Clear();
        }

        /// <summary>
        /// 是否包含某项元素 时间复杂度O(1)
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
        /// 获取给定类型的第一个添加到集合中的元素
        /// </summary>
        /// <param name="elementType">类型</param>
        /// <returns>获取到的元素</returns>
        public T Get(Type elementType)
        {
            if (m_Mains.TryGetValue(elementType, out T entity))
                return entity;
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

        public T Get(Type elementType, int entityId)
        {
            if (m_WithTypes.TryGetValue(elementType, out Dictionary<int, T> entities))
                if (entities.TryGetValue(entityId, out T entity))
                    return entity;
            return default;
        }
        #endregion

        #region IXEnumerable Interface
        public void SetIt(XItType type)
        {
            m_Elements.SetIt(type);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(m_Elements);
        }
        #endregion
    }
}
