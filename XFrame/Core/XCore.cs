using System;
using System.Diagnostics;
using XFrame.Collections;
using XFrame.Modules.Diagnotics;
using System.Collections.Generic;

namespace XFrame.Core
{
    internal struct ModuleHandle
    {
        public IModuleHandler Handler;
        public List<IModule> Modules;

        public ModuleHandle(IModuleHandler handler, List<IModule> modules)
        {
            Handler = handler;
            Modules = modules;
        }
    }

    /// <summary>
    /// 核心
    /// </summary>
    public partial class XCore
    {
        #region Inner Filed
        private bool m_IsStart;
        private XDomain m_Domain;
        private XCollection<IModule> m_Modules;
        private Dictionary<Type, ModuleHandle> m_ModulesWithEvents;
        private Dictionary<Type, List<IModuleHelper>> m_Helpers;
        #endregion

        #region Interface
        /// <summary>
        /// 初始化核心
        /// </summary>
        public void Init(XDomain domain)
        {
            m_IsStart = false;
            m_Domain = domain;
            m_Modules = new XCollection<IModule>(m_Domain);
            m_Helpers = new Dictionary<Type, List<IModuleHelper>>();
            m_ModulesWithEvents = new Dictionary<Type, ModuleHandle>();
        }

        /// <summary>
        /// 添加模块处理器
        /// </summary>
        /// <param name="handleType">处理目标类型</param>
        /// <param name="handler">处理器</param>
        public void AddHandle(Type handleType, IModuleHandler handler)
        {
            if (!m_ModulesWithEvents.ContainsKey(handleType))
            {
                ModuleHandle handle = new ModuleHandle(handler, new List<IModule>());
                m_ModulesWithEvents[handleType] = handle;
            }
        }

        /// <summary>
        /// 触发模块处理器
        /// </summary>
        /// <param name="handlerType">处理目标类型</param>
        /// <param name="data">参数</param>
        public void Trigger(Type handlerType, object data)
        {
            if (m_ModulesWithEvents.TryGetValue(handlerType, out ModuleHandle handle))
            {
                foreach (IModule module in handle.Modules)
                {
                    handle.Handler.Handle(module, data);
                }
            }
        }

        /// <summary>
        /// 启动核心
        /// </summary>
        public void Start()
        {
            if (m_IsStart)
                return;
            m_IsStart = true;
            foreach (IModule module in m_Modules)
            {
                module.OnStart();
                if (m_Helpers.TryGetValue(module.GetType(), out List<IModuleHelper> helpers))
                {
                    foreach (IModuleHelper helper in helpers)
                    {
                        helper.OnModuleStart(module);
                    }
                }
            }
        }

        /// <summary>
        /// 销毁核心
        /// </summary>
        public void Destroy()
        {
            m_Modules.SetIt(XItType.Backward);
            foreach (IModule module in m_Modules)
            {
                module.OnDestroy();
                if (m_Helpers.TryGetValue(module.GetType(), out List<IModuleHelper> helpers))
                {
                    foreach (IModuleHelper helper in helpers)
                    {
                        helper.OnModuleDestroy(module);
                    }
                }
            }

            foreach (List<IModuleHelper> list in m_Helpers.Values)
            {
                foreach (IModuleHelper helper in list)
                {
                    helper.OnDestroy();
                }
            }
            m_Helpers.Clear();
            m_Helpers = null;
            m_Modules.Clear();
            m_Modules = null;
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="module">模块</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userData">模块初始化参数</param>
        /// <returns></returns>
        public IModule Register(IModule module, int moduleId, object userData)
        {
            return InnerInitModule(module, moduleId, userData);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public T Register<T>(int moduleId) where T : IModule
        {
            return (T)InnerAddModule(typeof(T), moduleId, default);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns>模块实例</returns>
        public IModule Register(Type moduleType, int moduleId)
        {
            return InnerAddModule(moduleType, moduleId, default);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userData">模块初始化参数</param>
        /// <returns>模块实例</returns>
        public IModule Register(Type moduleType, int moduleId, object userData)
        {
            return InnerAddModule(moduleType, moduleId, userData);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userData">模块初始化数据</param>
        /// <returns>模块实例</returns>
        public T Register<T>(int moduleId, object userData) where T : IModule
        {
            return (T)InnerAddModule(typeof(T), moduleId, userData);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="moduleId">模块Id</param>
        /// <returns>模块实例</returns>
        public T GetModule<T>(int moduleId = default) where T : IModule
        {
            return (T)InnerGetModule(typeof(T), moduleId);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns>模块实例</returns>
        public IModule GetModule(Type moduleType, int moduleId = default)
        {
            return InnerGetModule(moduleType, moduleId);
        }

        /// <summary>
        /// 检查是否存在模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns></returns>
        public bool HasModule(Type moduleType, int moduleId = default)
        {
            return InnerGetModule(moduleType, moduleId) != null;
        }

        /// <summary>
        /// 添加模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="userData">模块初始化参数</param>
        /// <returns>模块实例</returns>
        public IModule AddModule(Type moduleType, int moduleId = default, object userData = null)
        {
            return InnerAddModule(moduleType, moduleId, userData);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="module">模块</param>
        /// <returns>是否成功</returns>
        public bool RemoveModule(IModule module)
        {
            return m_Modules.Remove(module);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="moduleId">模块Id</param>
        /// <returns>是否成功</returns>
        public bool RemoveModule<T>(int moduleId = default) where T : IModule
        {
            return InnerRemoveModule(typeof(T), moduleId);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <returns>是否成功</returns>
        public bool RemoveModule(Type moduleType, int moduleId = default)
        {
            return InnerRemoveModule(moduleType, moduleId);
        }

        /// <summary>
        /// 注册模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <typeparam name="ModuleT">模块类型</typeparam>
        /// <returns>辅助器实例</returns>
        public T RegisterHelper<T, ModuleT>() where T : IModuleHelper where ModuleT : IModule
        {
            return (T)RegisterHelper(typeof(T), typeof(ModuleT));
        }

        /// <summary>
        /// 注册模块辅助器
        /// </summary>
        /// <param name="helperType">辅助器类型</param>
        /// <param name="mType">模块类型</param>
        /// <returns>辅助器实例</returns>
        public IModuleHelper RegisterHelper(Type helperType, Type mType)
        {
            if (!m_Helpers.TryGetValue(mType, out List<IModuleHelper> helpers))
            {
                helpers = new List<IModuleHelper>();
                m_Helpers.Add(mType, helpers);
            }
            IModuleHelper helper = (IModuleHelper)m_Domain.TypeModule.CreateInstance(helperType);
            helper.OnInit();
            helpers.Add(helper);

            return helper;
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public T[] GetHelpers<T>() where T : IModule
        {
            if (m_Helpers.TryGetValue(typeof(T), out List<IModuleHelper> helpers))
            {
                T[] result = new T[helpers.Count];
                for (int i = 0; i < result.Length; i++)
                    result[i] = (T)helpers[i];
                return result;
            }
            return default;
        }

        /// <summary>
        /// 获取所有处理目标类型的辅助器
        /// </summary>
        /// <param name="mType">目标类型</param>
        /// <returns>辅助器列表</returns>
        public IModuleHelper[] GetHelpers(Type mType)
        {
            if (m_Helpers.TryGetValue(mType, out List<IModuleHelper> helpers))
                return helpers.ToArray();
            return default;
        }

        /// <summary>
        /// 获取模块主辅助器(第一个注册)
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public T GetMainHelper<T>() where T : IModuleHelper
        {
            if (m_Helpers.TryGetValue(typeof(T), out List<IModuleHelper> helpers))
            {
                if (helpers.Count > 0)
                    return (T)helpers[0];
            }
            return default;
        }

        /// <summary>
        /// 获取主辅助器（第一个添加）
        /// </summary>
        /// <param name="mType">处理目标类型</param>
        /// <returns></returns>
        public IModuleHelper GetMainHelper(Type mType)
        {
            if (m_Helpers.TryGetValue(mType, out List<IModuleHelper> helpers))
            {
                if (helpers.Count > 0)
                    return helpers[0];
            }
            return default;
        }
        #endregion

        #region Inner Implement
        private XCore() { }

        private IModule InnerAddModule(Type moduleType, int moduleId, object data)
        {
            long now = DateTime.Now.Ticks;
            IModule module = (IModule)m_Domain.TypeModule.CreateInstance(moduleType);
            InnerInitModule(module, moduleId, data);
            now = DateTime.Now.Ticks - now;
            Log.Debug(Log.XFrame, $"Add module {moduleType.Name} time {now / TimeSpan.TicksPerMillisecond} ms.");
            return module;
        }

        /// <summary>
        /// 添加模块(从系统类<see cref="Activator"/>初始化)
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        /// <param name="data">初始化参数</param>
        /// <returns>模块实例</returns>
        public IModule AddModuleFromSystem(Type moduleType, int moduleId, object data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IModule module = (IModule)Activator.CreateInstance(moduleType);
            InnerInitModule(module, moduleId, data);
            sw.Stop();
            Log.Debug(Log.XFrame, $"Add module {moduleType.Name} time {sw.ElapsedMilliseconds} ms.");
            return module;
        }

        private IModule InnerInitModule(IModule module, int moduleId, object data)
        {
            if (m_Helpers.TryGetValue(module.GetType(), out List<IModuleHelper> helpers))
            {
                foreach (IModuleHelper helper in helpers)
                {
                    helper.OnModuleCreate(module);
                }
            }

            ModuleBase baseClass = module as ModuleBase;
            if (baseClass != null)
            {
                baseClass.Id = moduleId;
                baseClass.Domain = m_Domain;
            }
            module.OnInit(data);
            if (m_IsStart)
                module.OnStart();

            if (helpers != null)
            {
                foreach (IModuleHelper helper in helpers)
                {
                    helper.OnModuleInit(module);
                }
            }

            m_Modules.Add(module);

            Type moduleType = module.GetType();
            foreach (var entry in m_ModulesWithEvents)
            {
                if (entry.Key.IsAssignableFrom(moduleType))
                {
                    entry.Value.Modules.Add(module);
                }
            }

            return module;
        }

        private IModule InnerGetModule(Type moduleType, int moduleId)
        {
            return m_Modules.Get(moduleType, moduleId);
        }

        private bool InnerRemoveModule(Type moduleType, int moduleId)
        {
            IModule module = m_Modules.Get(moduleType, moduleId);
            if (module != null)
            {
                module.OnDestroy();
                return m_Modules.Remove(module);
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
