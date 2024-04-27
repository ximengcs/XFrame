using System;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Modules.Reflection
{
    /// <summary>
    /// 类型系统
    /// </summary>
    public class TypeSystem : IXEnumerable<Type>
    {
        #region Inner Fields
        private XItType m_ItType;
        private Type m_MainType;
        private ITypeModule m_TypeModule;
        private List<Type> m_AllTypes;
        private Dictionary<Type, TypeSystem> m_Classifyes;
        private Dictionary<int, Type> m_KeyTypes;
        private Dictionary<string, Type> m_NameTypes;
        #endregion

        #region Inner Imeplement
        internal TypeSystem(ITypeModule typeModule, Type mainType)
        {
            m_TypeModule = typeModule;
            m_MainType = mainType;
            m_AllTypes = new List<Type>();
            m_Classifyes = new Dictionary<Type, TypeSystem>();
            m_KeyTypes = new Dictionary<int, Type>();
            m_NameTypes = new Dictionary<string, Type>();
        }

        internal void AddSubClass(Type type)
        {
            m_AllTypes.Add(type);
            m_NameTypes[type.FullName] = type;
        }
        #endregion

        #region Interface
        /// <summary>
        /// 类型数量
        /// </summary>
        public int Count => m_AllTypes.Count;

        /// <summary>
        /// 主类
        /// </summary>
        public Type Main => m_MainType;

        /// <summary>
        /// 以key键标记一个类型
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="type">要标记的类型</param>
        public void AddKey(int key, Type type)
        {
            if ((!m_AllTypes.Contains(type) && m_MainType != type) || m_KeyTypes.ContainsKey(key))
            {
                Log.Error("XFrame", $"Type {type.Name} module add index error.");
                return;
            }
            m_KeyTypes[key] = type;
        }

        /// <summary>
        /// 获取一个被标记的类型
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>获取到的类型</returns>
        public Type GetKey(int key)
        {
            if (m_KeyTypes.TryGetValue(key, out Type type))
                return type;
            else
                return default;
        }

        /// <summary>
        /// 通过名字获取类型
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Type GetByName(string name)
        {
            if (m_NameTypes.TryGetValue(name, out Type type))
                return type;
            else
                return default;
        }

        /// <summary>
        /// 尝试通过名字获取类型
        /// </summary>
        /// <param name="name">类型名</param>
        /// <param name="type">类型</param>
        /// <returns>是否获取成功</returns>
        public bool TryGetByName(string name, out Type type)
        {
            return m_NameTypes.TryGetValue(name, out type);
        }

        /// <summary>
        /// 获取(不存在时创建)子类类型系统
        /// </summary>
        /// <typeparam name="T">基类</typeparam>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNewBySub<T>() where T : class
        {
            return GetOrNewBySub(typeof(T));
        }

        /// <summary>
        /// 获取(不存在时创建)子类类型系统
        /// </summary>
        /// <param name="type">基类</param>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNewBySub(Type type)
        {
            if (m_Classifyes.TryGetValue(type, out TypeSystem module))
                return module;

            module = new TypeSystem(m_TypeModule, type);
            m_Classifyes.Add(type, module);
            foreach (Type subType in m_AllTypes)
            {
                if (type.IsAssignableFrom(subType))
                {
                    XAttribute xAttr = m_TypeModule.GetAttribute<XAttribute>(subType);
                    module.AddSubClass(subType);
                    if (xAttr != null)
                        module.AddKey(xAttr.Id, subType);
                }
            }

            return module;
        }

        /// <summary>
        /// 迭代所有类型
        /// </summary>
        /// <returns>迭代器</returns>
        public IEnumerator<Type> GetEnumerator()
        {
            switch (m_ItType)
            {
                case XItType.Forward: return new ListExt.ForwardIt<Type>(m_AllTypes);
                case XItType.Backward: return new ListExt.BackwardIt<Type>(m_AllTypes);
                default: return default;
            }
        }

        /// <inheritdoc/>
        public void SetIt(XItType type)
        {
            m_ItType = type;
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns></returns>
        public Type[] ToArray()
        {
            return m_AllTypes.ToArray();
        }
        #endregion
    }
}
