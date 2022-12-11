using System;
using System.Collections.Generic;
using System.Reflection;
using XFrame.Utility;

namespace XFrame.Core
{
    public partial class TypeModule : SingleModule<TypeModule>
    {
        private Assembly[] m_Assemblys;
        private Type[] m_Types;
        private Dictionary<Type, Set> m_ClassRegister;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_ClassRegister = new Dictionary<Type, Set>();
            m_Assemblys = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> types = new List<Type>(128);
            foreach (Assembly assembly in m_Assemblys)
                types.AddRange(assembly.GetTypes());
            m_Types = types.ToArray();
        }

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

        public Set Get<T>() where T : class
        {
            Type pType = typeof(T);
            if (m_ClassRegister.TryGetValue(pType, out Set module))
                return module;
            else
                return default;
        }

        public Set RegisterWithAtr<T>() where T : Attribute
        {
            Type pType = typeof(T);
            Set module;
            if (!m_ClassRegister.TryGetValue(pType, out module))
            {
                module = new Set(pType);
                m_ClassRegister.Add(pType, module);
            }

            foreach (Type subType in m_Types)
            {
                if (TypeUtility.HasAttribute<T>(subType))
                    module.AddSubClass(subType);
            }
            return module;
        }

        public Set Register<T>() where T : class
        {
            Type pType = typeof(T);
            Set module;
            if (!m_ClassRegister.TryGetValue(pType, out module))
            {
                module = new Set(pType);
                m_ClassRegister.Add(pType, module);
            }

            foreach (Type type in m_Types)
            {
                if (type.IsSubclassOf(pType))
                    module.AddSubClass(type);
            }
            return module;
        }
    }
}
