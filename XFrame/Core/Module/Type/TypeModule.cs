﻿using System;
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
        private Action m_OnTypeChange;
        private Type[] m_Types;
        private Assembly[] m_Assemblys;
        private Dictionary<Type, TypeSystem> m_ClassRegister;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            InnerInit();
            AppDomain.CurrentDomain.AssemblyLoad += InnerAssemblyUpdateHandle;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            m_OnTypeChange = null;
            AppDomain.CurrentDomain.AssemblyLoad -= InnerAssemblyUpdateHandle;
        }

        private void InnerAssemblyUpdateHandle(object sender, AssemblyLoadEventArgs args)
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

        #region Interface
        /// <summary>
        /// 类型改变事件
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
            Type pType = typeof(T);
            TypeSystem module;
            if (m_ClassRegister.TryGetValue(pType, out module))
                return module;

            module = new TypeSystem(pType);
            m_ClassRegister.Add(pType, module);
            foreach (Type subType in m_Types)
            {
                if (TypeUtility.HasAttribute<T>(subType))
                    module.AddSubClass(subType);
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
            Type pType = typeof(T);
            TypeSystem module;
            if (m_ClassRegister.TryGetValue(pType, out module))
                return module;

            module = new TypeSystem(pType);
            m_ClassRegister.Add(pType, module);
            foreach (Type type in m_Types)
            {
                if (pType != type && pType.IsAssignableFrom(type))
                    module.AddSubClass(type);
            }
            return module;
        }

        public Type[] GetAllType()
        {
            return m_Types;
        }
        #endregion
    }
}