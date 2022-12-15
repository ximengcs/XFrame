using System;
using XFrame.Modules;
using System.Collections;
using System.Collections.Generic;

namespace XFrame.Core
{
    public partial class TypeModule
    {
        /// <summary>
        /// 类型系统
        /// </summary>
        public class System : IEnumerable<Type>
        {
            private Type m_MainType;
            private List<Type> m_AllTypes;
            private Dictionary<Type, System> m_Classifyes;
            private Dictionary<int, Type> m_KeyTypes;
            private Dictionary<string, Type> m_NameTypes;

            internal System(Type mainType)
            {
                m_MainType = mainType;
                m_AllTypes = new List<Type>();
                m_Classifyes = new Dictionary<Type, System>();
                m_KeyTypes = new Dictionary<int, Type>();
                m_NameTypes = new Dictionary<string, Type>();
            }

            #region Interface
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
                    Log.Error("XFrame", $"Type module add index error.");
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
            public System GetOrNewBySub<T>() where T : class
            {
                Type type = typeof(T);
                if (m_Classifyes.TryGetValue(type, out System module))
                    return module;

                module = new System(type);
                m_Classifyes.Add(type, module);
                foreach (Type subType in m_AllTypes)
                {
                    if (subType.IsSubclassOf(type))
                        module.AddSubClass(subType);
                }

                return module;
            }

            /// <summary>
            /// 获取所有类型
            /// </summary>
            /// <returns>类型集合</returns>
            public Type[] ToArray()
            {
                return m_AllTypes.ToArray();
            }

            /// <summary>
            /// 迭代所有类型
            /// </summary>
            /// <returns>迭代器</returns>
            public IEnumerator<Type> GetEnumerator()
            {
                return m_AllTypes.GetEnumerator();
            }
            #endregion

            internal void AddSubClass(Type type)
            {
                m_AllTypes.Add(type);
                m_NameTypes[type.Name] = type;
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
