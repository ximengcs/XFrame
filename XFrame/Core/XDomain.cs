using System;
using XFrame.Modules.Reflection;

namespace XFrame.Core
{
    public class XDomain
    {
        private XCore[] m_Cores;
        private ITypeModule m_TypeModule;

        public XCore Base => m_Cores[0];

        public ITypeModule TypeModule => m_TypeModule;

        public XCore this[int index] => m_Cores[index];

        public XDomain(int capacity)
        {
            m_Cores = new XCore[capacity];
            for (int i = 0; i < m_Cores.Length; i++)
                m_Cores[i] = XCore.Create(this);
        }

        public void SetTypeModule(Type type)
        {
            m_TypeModule = (ITypeModule)Base.AddModuleFromSystem(type, default, default);
        }

        public void AddModule(int id, Type moduleType, int moduleId = default, object userData = null)
        {
            m_Cores[id].AddModule(moduleType, moduleId, userData);
        }

        public void AddHandle(Type handleType, IModuleHandler handler)
        {
            foreach (XCore core in m_Cores)
                core.AddHandle(handleType, handler);
        }

        public void RemoveModule(Type moduleType, int moduleId = default)
        {
            foreach (XCore core in m_Cores)
            {
                if (core.RemoveModule(moduleType, moduleId))
                    break;
            }
        }

        public void RemoveModule(IModule module)
        {
            foreach (XCore core in m_Cores)
            {
                if (core.RemoveModule(module))
                    break;
            }
        }

        public IModule GetModule(Type moduleType, int moduleId = default)
        {
            foreach (XCore core in m_Cores)
            {
                IModule module = core.GetModule(moduleType, moduleId);
                if (module != null)
                    return module;
            }
            return null;
        }

        public T GetModule<T>(int moduleId = default) where T : IModule
        {
            foreach (XCore core in m_Cores)
            {
                T module = core.GetModule<T>(moduleId);
                if (module != null)
                    return module;
            }
            return default;
        }

        public void Destroy()
        {
            for (int i = m_Cores.Length - 1; i >= 0; i--)
                m_Cores[i].Destroy();
            m_Cores = null;
        }
    }
}
