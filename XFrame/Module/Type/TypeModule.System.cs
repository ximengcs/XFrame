using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using XFrame.Modules;

namespace XFrame.Core
{
    public partial class TypeModule
    {
        public class Set : IEnumerable<Type>
        {
            private Type m_MainType;
            private List<Type> m_AllTypes;
            private Dictionary<Type, Set> m_Classifyes;
            private Dictionary<int, Type> m_Indexes;
            private Dictionary<string, Type> m_NameTypes;

            public Type Main => m_MainType;

            public Set(Type mainType)
            {
                m_MainType = mainType;
                m_AllTypes = new List<Type>();
                m_Classifyes = new Dictionary<Type, Set>();
                m_Indexes = new Dictionary<int, Type>();
                m_NameTypes = new Dictionary<string, Type>();
            }

            public void AddIndex(int index, Type type)
            {
                if ((!m_AllTypes.Contains(type) && m_MainType != type) || m_Indexes.ContainsKey(index))
                {
                    Log.Error("XFrame", $"Type module add index error.");
                    return;
                }
                m_Indexes[index] = type;
            }

            public Type GetIndex(int index)
            {
                if (m_Indexes.TryGetValue(index, out Type type))
                    return type;
                else
                    return default;
            }

            public Type GetByName(string name)
            {
                if (m_NameTypes.TryGetValue(name, out Type type))
                    return type;
                else
                    return default;
            }

            public bool TryGetByName(string name, out Type type)
            {
                return m_NameTypes.TryGetValue(name, out type);
            }

            public Set GetBySub<T>() where T : class
            {
                Type type = typeof(T);
                if (m_Classifyes.TryGetValue(type, out Set module))
                    return module;
                else
                    return default;
            }

            public Set ClassifyBySub<T>() where T : class
            {
                Type type = typeof(T);
                if (!m_Classifyes.TryGetValue(type, out Set module))
                {
                    module = new Set(type);
                    m_Classifyes.Add(type, module);
                }

                foreach (Type subType in m_AllTypes)
                {
                    if (subType.IsSubclassOf(type))
                        module.AddSubClass(subType);
                }

                return module;
            }

            public void AddSubClass(Type type)
            {
                m_AllTypes.Add(type);
                m_NameTypes[type.Name] = type;
            }

            public Type[] ToArray()
            {
                return m_AllTypes.ToArray();
            }

            public IEnumerator<Type> GetEnumerator()
            {
                return m_AllTypes.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
