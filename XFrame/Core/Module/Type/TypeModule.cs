using System;
using XFrame.Core;
using XFrame.Utility;
using System.Reflection;
using System.Collections.Generic;

namespace XFrame.Modules.XType
{
    /// <summary>
    /// 类型模块
    /// </summary>
    public partial class TypeModule : SingletonModule<TypeModule>
    {
        #region Inner Fields
        private Action m_OnTypeChange;
        private Type[] m_Types;
        private Assembly[] m_Assemblys;
        private Dictionary<Type, TypeSystem> m_ClassRegister;
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
            List<Type> types = new List<Type>(128);
            m_Assemblys = AppDomain.CurrentDomain.GetAssemblies();
            foreach (Assembly assembly in m_Assemblys)
                types.AddRange(assembly.GetTypes());

            if (m_Types != null && m_Types.Length == types.Count)
                return;

            m_Types = types.ToArray();
            if (m_ClassRegister != null)
                m_ClassRegister.Clear();
            else
                m_ClassRegister = new Dictionary<Type, TypeSystem>();
        }
        #endregion

        #region Interface
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

            module = new TypeSystem(pType);
            m_ClassRegister.Add(pType, module);
            foreach (Type subType in m_Types)
            {
                Attribute attr = TypeUtility.GetAttribute(subType, pType);
                if (attr != null)
                {
                    module.AddSubClass(subType);
                    XAttribute xAttr = attr as XAttribute;
                    if (xAttr != null)
                        module.AddKey(xAttr.Id, subType);
                }
            }
            return module;
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
        /// <param name="pType">基类</param>
        /// <returns>获取到的类型系统</returns>
        public TypeSystem GetOrNew(Type pType)
        {
            TypeSystem module;
            if (m_ClassRegister.TryGetValue(pType, out module))
                return module;

            module = new TypeSystem(pType);
            m_ClassRegister.Add(pType, module);
            foreach (Type type in m_Types)
            {
                if (pType != type && pType.IsAssignableFrom(type))
                {
                    module.AddSubClass(type);
                    XAttribute attr = TypeUtility.GetAttribute<XAttribute>(type);
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
