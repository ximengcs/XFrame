using System;
using System.Collections.Generic;
using XFrame.Modules.Reflection;

namespace XFrame.Core
{
    /// <summary>
    /// 域 (包含多个核心)
    /// </summary>
    public partial class XDomain
    {
        private List<CoreInfo> m_Cores;
        private ITypeModule m_TypeModule;

        /// <summary>
        /// 基础核心
        /// </summary>
        public XCore Base => m_Cores[0].Inst;

        public int Count => m_Cores.Count;

        /// <summary>
        /// 类型模块
        /// </summary>
        public ITypeModule TypeModule => m_TypeModule;

        /// <summary>
        /// 检索核心
        /// </summary>
        /// <param name="index">下标</param>
        /// <returns>核心</returns>
        public XCore this[int index] => m_Cores[index].Inst;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="capacity">容量</param>
        public XDomain(int capacity)
        {
            m_Cores = new List<CoreInfo>(capacity);
            for (int i = 0; i < capacity; i++)
                AddCore(true, true);
        }

        public XCore AddCore(bool trigger, bool destory)
        {
            XCore core = XCore.Create(this);
            CoreInfo info = new CoreInfo()
            {
                Inst = core,
                Index = m_Cores.Count,
                Trigger = trigger,
                Destory = destory
            };
            m_Cores.Add(info);
            return core;
        }

        /// <summary>
        /// 设置类型模块
        /// </summary>
        /// <param name="type">类型模块</param>
        public void SetTypeModule(Type type)
        {
            m_TypeModule = (ITypeModule)Base.AddModuleFromSystem(type, default, default);
        }

        public void Trigger(int index, Type type, object data)
        {
            CoreInfo info = m_Cores[index];
            if (info.Trigger)
                info.Inst.Trigger(type, data);
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
            m_Cores[id].Inst.AddModule(moduleType, moduleId, userData);
        }

        /// <summary>
        /// 添加模块处理器
        /// </summary>
        /// <param name="handleType">处理目标类型</param>
        /// <param name="handler">模块处理器</param>
        public void AddHandle(Type handleType, IModuleHandler handler)
        {
            foreach (CoreInfo core in m_Cores)
                core.Inst.AddHandle(handleType, handler);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        public void RemoveModule(Type moduleType, int moduleId = default)
        {
            foreach (CoreInfo core in m_Cores)
            {
                if (core.Inst.RemoveModule(moduleType, moduleId))
                    break;
            }
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="module">模块</param>
        public void RemoveModule(IModule module)
        {
            foreach (CoreInfo core in m_Cores)
            {
                if (core.Inst.RemoveModule(module))
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
            foreach (CoreInfo core in m_Cores)
            {
                IModule module = core.Inst.GetModule(moduleType, moduleId);
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
            foreach (CoreInfo core in m_Cores)
            {
                T module = core.Inst.GetModule<T>(moduleId);
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
            for (int i = m_Cores.Count - 1; i >= 0; i--)
            {
                CoreInfo info = m_Cores[i];
                if (info.Destory)
                    info.Inst.Destroy();
            }
            m_Cores = null;
        }
    }
}
