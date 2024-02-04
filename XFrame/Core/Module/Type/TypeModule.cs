using System;
using XFrame.Core;
using System.Reflection;
using XFrame.Collections;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using System.Data;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Reflection
{
    /// <summary>
    /// 类型模块
    /// </summary>
    [XType(typeof(ITypeModule))]
    public partial class TypeModule : ModuleBase, ITypeModule
    {
        #region Inner Fields
        private Action m_OnTypeChange;
        private Type[] m_Types;
        private Assembly[] m_Assemblys;
        private string m_Module;
        private Dictionary<Type, Attribute[]> m_TypesAllAttrs;
        private Dictionary<Type, List<Type>> m_TypesWithAttrs;
        private Dictionary<Type, TypeSystem> m_ClassRegister;
        private Dictionary<Type, ConstructorInfo[]> m_Constructors;
        #endregion

        #region Life Fun
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerInit();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_OnTypeChange = null;
        }
        #endregion

        #region Inner Imeplement
        private void InnerAssemblyUpdateHandle()
        {
            InnerInit();
            m_OnTypeChange?.Invoke();
        }

        private void InnerInit()
        {
            m_Module = nameof(XFrame);
            m_TypesAllAttrs = new Dictionary<Type, Attribute[]>();
            m_TypesWithAttrs = new Dictionary<Type, List<Type>>();
            m_Constructors = new Dictionary<Type, ConstructorInfo[]>();
            m_Assemblys = AppDomain.CurrentDomain.GetAssemblies();
            List<Type> tmpList = new List<Type>(1024);
            foreach (Assembly assembly in m_Assemblys)
            {
                bool find = false;
                AssemblyName aName = assembly.GetName();
                string assemblyName = aName.Name;
                if (assemblyName == m_Module)
                {
                    find = true;
                }
                else if (XConfig.TypeChecker.AssemblyList != null)
                {
                    foreach (string name in XConfig.TypeChecker.AssemblyList)
                    {
                        if (assemblyName == name)
                        {
                            find = true;
                            break;
                        }
                    }
                }

                if (!find)
                    continue;

                foreach (TypeInfo typeInfo in assembly.DefinedTypes)
                {
                    Type type = typeInfo.AsType();
                    if (!XConfig.TypeChecker.CheckType(type))
                        continue;
                    Attribute[] attrs = Attribute.GetCustomAttributes(type);
                    m_TypesAllAttrs.Add(type, attrs);
                    foreach (Attribute attr in attrs)
                    {
                        Type attrType = attr.GetType();
                        if (!m_TypesWithAttrs.TryGetValue(attrType, out List<Type> list))
                        {
                            list = new List<Type>(32);
                            m_TypesWithAttrs.Add(attrType, list);
                        }
                        list.Add(type);
                    }
                    tmpList.Add(type);
                }
            }

            m_Types = tmpList.ToArray();
            if (m_ClassRegister != null)
                m_ClassRegister.Clear();
            else
                m_ClassRegister = new Dictionary<Type, TypeSystem>();
        }
        #endregion

        #region Interface
        public T CreateInstance<T>(params object[] args)
        {
            return (T)InnerCreateInstance(typeof(T), args);
        }

        public object CreateInstance(Type type, params object[] args)
        {
            return InnerCreateInstance(type, args);
        }

        public object CreateInstance(string typeName, params object[] args)
        {
            Type type = GetType(typeName);
            if (type == null)
                return null;
            return InnerCreateInstance(type, args);
        }

        private object InnerCreateInstance(Type type, params object[] args)
        {
            object instance = default;
            if (!m_Constructors.TryGetValue(type, out ConstructorInfo[] ctors))
            {
                ctors = type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                m_Constructors.Add(type, ctors);
            }

            foreach (ConstructorInfo ctor in ctors)
            {
                ParameterInfo[] paramInfos = ctor.GetParameters();
                if (args.Length == paramInfos.Length)
                {
                    int i = 0;
                    while (i < paramInfos.Length)
                    {
                        Type argType = args[i].GetType();
                        Type paramType = paramInfos[i].ParameterType;
                        if (argType != paramType && !paramType.IsAssignableFrom(argType))
                        {
                            break;
                        }
                        i++;
                    }

                    if (i == paramInfos.Length)
                    {
                        instance = ctor.Invoke(args);
                        break;
                    }
                }
            }

            if (instance == null)
            {
                Log.Error("XFrame", $"Create instance failure, {type.FullName}");
            }
            return instance;
        }

        /// <summary>
        /// 加载程序集
        /// </summary>
        /// <param name="data"></param>
        public void LoadAssembly(byte[] data)
        {
            Assembly.Load(data);
            InnerAssemblyUpdateHandle();
        }

        /// <summary>
        /// 程序集改变事件
        /// </summary>
        public void OnTypeChange(Action handler)
        {
            m_OnTypeChange += handler;
        }

        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="name">类型名</param>
        /// <returns>获取到的类型</returns>
        public Type GetType(string name)
        {
            foreach (Assembly assembly in m_Assemblys)
            {
                Type type = assembly.GetType(name);
                if (type != null)
                    return type;
            }
            return default;
        }

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都具有所给定的属性类
        /// </summary>
        /// <typeparam name="T">Attribute属性类</typeparam>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNewWithAttr<T>() where T : Attribute
        {
            return GetOrNewWithAttr(typeof(T));
        }

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都具有所给定的属性类
        /// </summary>
        /// <param name="pType">Attribute属性类</param>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNewWithAttr(Type pType)
        {
            TypeSystem module;
            if (m_ClassRegister.TryGetValue(pType, out module))
                return module;

            module = new TypeSystem(this, pType);
            m_ClassRegister.Add(pType, module);
            foreach (var item in m_TypesWithAttrs)
            {
                if (item.Key.IsSubclassOf(pType) || item.Key == pType)
                {
                    foreach (Type subType in item.Value)
                    {
                        Attribute attr = GetAttribute(subType, pType);
                        if (attr != null)
                        {
                            module.AddSubClass(subType);
                            XAttribute xAttr = attr as XAttribute;
                            if (xAttr != null)
                                module.AddKey(xAttr.Id, subType);
                        }
                    }
                }
            }

            return module;
        }

        public bool HasAttribute<T>(Type classType) where T : Attribute
        {
            return GetAttribute(classType, typeof(T)) != null;
        }

        public bool HasAttribute(Type classType, Type pType)
        {
            return GetAttribute(classType, pType) != null;
        }

        public T GetAttribute<T>(Type classType) where T : Attribute
        {
            return (T)GetAttribute(classType, typeof(T));
        }

        public T[] GetAttributes<T>(Type classType) where T : Attribute
        {
            Type pType = typeof(T);
            List<T> list;
            CommonPoolObject<List<T>> container = null;
            if (References.Available)
            {
                container = References.Require<CommonPoolObject<List<T>>>();
                if (!container.Valid)
                {
                    list = new List<T>(8);
                    container.Target = list;
                }
                else
                {
                    list = container.Target;
                }
            }
            else
            {
                list = new List<T>(8);
            }

            if (m_TypesAllAttrs.TryGetValue(classType, out Attribute[] values))
            {
                foreach (Attribute attr in values)
                {
                    Type attrType = attr.GetType();
                    if (attrType.IsSubclassOf(pType) || attrType == pType)
                        list.Add((T)attr);
                }
            }
            T[] result = list.ToArray();
            if (container != null)
            {
                list.Clear();
                References.Release(container);
            }
            return result;
        }

        public Attribute GetAttribute(Type classType, Type pType)
        {
            if (m_TypesAllAttrs.TryGetValue(classType, out Attribute[] values))
            {
                foreach (Attribute attr in values)
                {
                    Type attrType = attr.GetType();
                    if (attrType.IsSubclassOf(pType) || attrType == pType)
                        return attr;
                }
            }
            return default;
        }

        public Attribute[] GetAttributes(Type classType, Type pType)
        {
            List<Attribute> list;
            CommonPoolObject<List<Attribute>> container = null;
            if (References.Available)
            {
                container = References.Require<CommonPoolObject<List<Attribute>>>();
                if (!container.Valid)
                {
                    list = new List<Attribute>(8);
                    container.Target = list;
                }
                else
                {
                    list = container.Target;
                }
            }
            else
            {
                list = new List<Attribute>(8);
            }

            if (m_TypesAllAttrs.TryGetValue(classType, out Attribute[] values))
            {
                foreach (Attribute attr in values)
                {
                    Type attrType = attr.GetType();
                    if (attrType.IsSubclassOf(pType) || attrType == pType)
                        list.Add(attr);
                }
            }
            Attribute[] result = list.ToArray();
            if (container != null)
            {
                list.Clear();
                References.Release(container);
            }
            return result;
        }

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都是所给定的类型或子类
        /// </summary>
        /// <typeparam name="T">基类</typeparam>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNew<T>() where T : class
        {
            return GetOrNew(typeof(T));
        }

        /// <summary>
        /// 获取(不存在时创建)一个类型系统
        /// 类型都是所给定的类型或子类
        /// </summary>
        /// <param name="baseType">基类</param>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNew(Type baseType)
        {
            TypeSystem module;
            if (m_ClassRegister.TryGetValue(baseType, out module))
                return module;

            module = new TypeSystem(this, baseType);
            m_ClassRegister.Add(baseType, module);
            foreach (Type type in m_Types)
            {
                if (baseType != type && baseType.IsAssignableFrom(type))
                {
                    module.AddSubClass(type);
                    XAttribute attr = GetAttribute<XAttribute>(type);
                    if (attr != null)
                        module.AddKey(attr.Id, type);
                }
            }
            return module;
        }

        /// <summary>
        /// 获取所有类型
        /// </summary>
        /// <returns>类型列表</returns>
        public Type[] GetAllType()
        {
            return m_Types;
        }
        #endregion
    }
}