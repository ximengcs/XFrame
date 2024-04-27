using System;
using XFrame.Modules.Reflection;

namespace XFrame.Core
{
    /// <summary>
    /// 域 (包含多个核心)
    /// </summary>
    public class XDomain
    {
        private XCore[] m_Cores;
        private ITypeModule m_TypeModule;

        /// <summary>
        /// 基础核心
        /// </summary>
        public XCore Base => m_Cores[0];

        /// <summary>
        /// 类型模块
        /// </summary>
        public ITypeModule TypeModule => m_TypeModule;

        /// <summary>
        /// 检索核心
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>核心</returns>
        public XCore this[int index] => m_Cores[index];

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="capacity">容量</param>
        public XDomain(int capacity)
        {
            m_Cores = new XCore[capacity];
            for (int i = 0; i < m_Cores.Length; i++)
                m_Cores[i] = XCore.Create(this);
        }

        /// <summary>
        /// 设置类型模块
        /// </summary>
        /// <param name="type">类型模块</param>
        public void SetTypeModule(Type type)
        {
            m_TypeModule = (ITypeModule)Base.AddModuleFromSystem(type, default, default);
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="id">核心Id</param>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userData">模块初始化参数</param>
        public void AddModule(int id, Type moduleType, int moduleId = default, object userData = null)
        {
            m_Cores[id].AddModule(moduleType, moduleId, userData);
        }

        /// <summary>
        /// 添加模块处理器
        /// </summary>
        /// <param name="handleType">处理目标类型</param>
        /// <param name="handler">模块处理器</param>
        public void AddHandle(Type handleType, IModuleHandler handler)
        {
            foreach (XCore core in m_Cores)
                core.AddHandle(handleType, handler);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        public void RemoveModule(Type moduleType, int moduleId = default)
        {
            foreach (XCore core in m_Cores)
            {
                if (core.RemoveModule(moduleType, moduleId))
                    break;
            }
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="module">模块</param>
        public void RemoveModule(IModule module)
        {
            foreach (XCore core in m_Cores)
            {
                if (core.RemoveModule(module))
                    break;
            }
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns>模块实例</returns>
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

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="moduleId">模块Id</param>
        /// <returns>模块实例</returns>
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

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            for (int i = m_Cores.Length - 1; i >= 0; i--)
                m_Cores[i].Destroy();
            m_Cores = null;
        }
    }
}
