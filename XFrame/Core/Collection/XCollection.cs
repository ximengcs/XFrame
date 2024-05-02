using System;
using System.Collections.Generic;
using XFrame.Modules.Reflection;
using System.Reflection;
using XFrame.Core;

namespace XFrame.Collections
{
    /// <summary>
    /// XItem的集合, 可以通过Id快速读取元素
    /// </summary>
    /// <typeparam name="T">持有的数据类型</typeparam>
    public partial class XCollection<T> : IXEnumerable<T> where T : IXItem
    {
        #region Const Fields
        /// <summary>
        /// 默认容量
        /// </summary>
        public const int DEFAULT_CAPACITY = 16;
        #endregion

        #region Inner Fields
        private XDomain m_Domain;
        private Dictionary<int, T> m_WithTypes;
        private Dictionary<Type, T> m_Mains;
        private XLinkList<T> m_Elements;
        private Dictionary<int, XLinkNode<T>> m_NodeMap;
        #endregion

        #region Constructor
        /// <summary>
        /// 构造集合
        /// </summary>
        public XCollection(XDomain domain, int startCapacity = DEFAULT_CAPACITY)
        {
            m_Domain = domain;
            m_WithTypes = new Dictionary<int, T>(startCapacity);
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

        private Type InnerGetMapType(Type type)
        {
            if (type == typeof(ITypeModule))
                return type;

            XTypeAttribute attr;
            ITypeModule typeModule = m_Domain.TypeModule;
            if (typeModule != null)
                attr = typeModule.GetAttribute<XTypeAttribute>(type);
            else
                attr = type.GetCustomAttribute<XTypeAttribute>();
            return attr != null ? attr.Type : type;
        }

        /// <summary>
        /// 添加一个元素 时间复杂度O(1)
        /// </summary>
        /// <param name="entity">要添加的元素</param>
        public void Add(T entity)
        {
            Type type = entity.GetType();
            Type xType = InnerGetMapType(type);
            Dictionary<int, T> entities;


            //if (!m_WithTypes.TryGetValue(xType, out entities))
            //{
            //    entities = new Dictionary<int, T>();
            //    m_WithTypes.Add(xType, entities);
            //}

            XLinkNode<T> node = m_Elements.AddLast(entity);
            m_NodeMap.Add(node.GetHashCode(), node);

            m_WithTypes.Add(entity.Id, entity);
            //entities.Add(entity.Id, entity);
            if (!m_Mains.ContainsKey(xType))
                m_Mains.Add(xType, entity);
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
            Type xType = InnerGetMapType(type);
            if (m_WithTypes.ContainsKey(item.Id))
                m_WithTypes.Remove(item.Id);
            //if (m_WithTypes.TryGetValue(xType, out Dictionary<int, T> entities))
            //    success = entities.Remove(item.Id);
            if (m_Mains.Remove(xType))
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
            Type xType = InnerGetMapType(type);
            if (m_Mains.ContainsKey(xType))
                return true;
            if (m_WithTypes.ContainsKey(item.Id))
                return true;
            //if (m_WithTypes.TryGetValue(xType, out Dictionary<int, T> entities))
            //{
            //    if (entities.ContainsKey(item.Id))
            //        return true;
            //}
            return false;
        }

        /// <summary>
        /// 获取给定类型的第一个添加到集合中的元素
        /// </summary>
        /// <typeparam name="TEntity">类型</typeparam>
        /// <returns>获取到的元素</returns>
        public TEntity Get<TEntity>() where TEntity : T
        {
            Type xType = InnerGetMapType(typeof(TEntity));
            if (m_Mains.TryGetValue(xType, out T entity))
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
            Type xType = InnerGetMapType(elementType);
            if (m_Mains.TryGetValue(xType, out T entity))
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
            //Type xType = InnerGetMapType(typeof(TEntity));
            if (m_WithTypes.TryGetValue(entityId, out T entity))
            {
                return (TEntity)entity;
            }
            //if (m_WithTypes.TryGetValue(xType, out Dictionary<int, T> entities))
            //    if (entities.TryGetValue(entityId, out T entity))
            //        return (TEntity)entity;
            return default;
        }

        /// <summary>
        /// 获取指定id和给定类型的元素 
        /// </summary>
        /// <param name="elementType">需要获取的类型</param>
        /// <param name="entityId">元素Id</param>
        /// <returns>获取到的元素</returns>
        public T Get(Type elementType, int entityId)
        {
            //Type xType = InnerGetMapType(elementType);
            if (m_WithTypes.TryGetValue(entityId, out T entity))
            {
                return entity;
            }
            //if (m_WithTypes.TryGetValue(xType, out Dictionary<int, T> entities))
            //    if (entities.TryGetValue(entityId, out T entity))
            //        return entity;
            return default;
        }
        #endregion

        #region IXEnumerable Interface
        /// <summary>
        /// 设置迭代器类型
        /// </summary>
        /// <param name="type">迭代器类型</param>
        public void SetIt(XItType type)
        {
            m_Elements.SetIt(type);
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(m_Elements);
        }
        #endregion
    }
}
