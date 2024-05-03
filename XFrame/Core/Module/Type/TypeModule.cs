using System;
using XFrame.Core;
using System.Reflection;
using XFrame.Collections;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;
using XFrame.Modules.Pools;
using System.Xml.Linq;

namespace XFrame.Modules.Reflection
{
    /// <inheritdoc/>
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
        /// <inheritdoc/>
        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerInit();
        }

        /// <inheritdoc/>
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
            Log.Debug(AppDomain.CurrentDomain.GetHashCode());
            List<Type> tmpList = new List<Type>(1024);
            foreach (Assembly assembly in m_Assemblys)
            {
                Log.Debug($"add {assembly}");
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
        /// <inheritdoc/>
        public T CreateInstance<T>(params object[] args)
        {
            return (T)InnerCreateInstance(typeof(T), args);
        }

        /// <inheritdoc/>
        public object CreateInstance(Type type, params object[] args)
        {
            return InnerCreateInstance(type, args);
        }

        /// <inheritdoc/>
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
                Log.Error(Log.XFrame, $"Create instance failure, {type.FullName}");
            }
            return instance;
        }

        /// <inheritdoc/>
        public void LoadAssembly(byte[] data)
        {
            Assembly.Load(data);
            InnerAssemblyUpdateHandle();
        }

        /// <inheritdoc/>
        public void OnTypeChange(Action handler)
        {
            m_OnTypeChange += handler;
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public TypeSystem GetOrNewWithAttr<T>() where T : Attribute
        {
            return GetOrNewWithAttr(typeof(T));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool HasAttribute<T>(Type classType) where T : Attribute
        {
            return GetAttribute(classType, typeof(T)) != null;
        }

        /// <inheritdoc/>
        public bool HasAttribute(Type classType, Type pType)
        {
            return GetAttribute(classType, pType) != null;
        }

        /// <inheritdoc/>
        public T GetAttribute<T>(Type classType) where T : Attribute
        {
            return (T)GetAttribute(classType, typeof(T));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public TypeSystem GetOrNew<T>() where T : class
        {
            return GetOrNew(typeof(T));
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public Type[] GetAllType()
        {
            return m_Types;
        }
        #endregion
    }
}