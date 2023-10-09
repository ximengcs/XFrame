using System;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.Reflection;
using System.Diagnostics;
using XFrame.Modules.Diagnotics;

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
        private XCollection<IModule> m_Modules;
        private Dictionary<Type, ModuleHandle> m_ModulesWithEvents;
        private Dictionary<Type, IModuleHelper> m_MainHelper;
        private Dictionary<Type, List<IModuleHelper>> m_Helpers;
        #endregion

        #region Interface
        /// <summary>
        /// 初始化核心
        /// </summary>
        public void Init()
        {
            m_IsStart = false;
            m_Modules = new XCollection<IModule>();
            m_MainHelper = new Dictionary<Type, IModuleHelper>();
            m_Helpers = new Dictionary<Type, List<IModuleHelper>>();
            m_ModulesWithEvents = new Dictionary<Type, ModuleHandle>();
        }

        public void AddHandle(Type handleType, IModuleHandler handler)
        {
            if (!m_ModulesWithEvents.ContainsKey(handleType))
            {
                ModuleHandle handle = new ModuleHandle(handler, new List<IModule>());
                m_ModulesWithEvents[handleType] = handle;
            }
        }

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
            foreach (IModule manager in m_Modules)
                manager.OnStart();
        }

        /// <summary>
        /// 销毁核心
        /// </summary>
        public void Destroy()
        {
            m_Modules.SetIt(XItType.Backward);
            foreach (IModule manager in m_Modules)
                manager.OnDestroy();
            m_Modules.Clear();
            m_Modules = null;
        }

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
        /// <returns>模块实例</returns>
        public IModule Register(Type moduleType, int moduleId)
        {
            return InnerAddModule(moduleType, moduleId, default);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns>模块实例</returns>
        public IModule Register(Type moduleType, int moduleId, object userData)
        {
            return InnerAddModule(moduleType, moduleId, userData);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="data">模块初始化数据</param>
        /// <returns>模块实例</returns>
        public T Register<T>(int moduleId, object userData) where T : IModule
        {
            return (T)InnerAddModule(typeof(T), moduleId, userData);
        }

        /// <summary>
        /// 注册模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <typeparam name="ModuleT">模块类型</typeparam>
        /// <returns>辅助器实例</returns>
        public T RegisterHelper<T, ModuleT>() where T : IModuleHelper where ModuleT : IModule
        {
            Type mType = typeof(ModuleT);
            if (!m_Helpers.TryGetValue(mType, out List<IModuleHelper> helpers))
            {
                helpers = new List<IModuleHelper>();
                m_Helpers.Add(mType, helpers);
            }
            T helper = ModuleUtility.Type.CreateInstance<T>();
            helpers.Add(helper);

            if (!m_MainHelper.ContainsKey(typeof(T)))
                m_MainHelper[typeof(T)] = helper;

            return helper;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public T GetModule<T>(int moduleId = default) where T : IModule
        {
            return (T)InnerGetModule(typeof(T), moduleId);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns>模块实例</returns>
        public IModule GetModule(Type moduleType, int moduleId = default)
        {
            return InnerGetModule(moduleType, moduleId);
        }

        public bool HasModule(Type moduleType, int moduleId = default)
        {
            return InnerGetModule(moduleType, moduleId) != null;
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public IModuleHelper GetHelper<T>() where T : IModule
        {
            if (m_Helpers.TryGetValue(typeof(T), out List<IModuleHelper> helpers))
                return helpers[0];
            return default;
        }

        /// <summary>
        /// 获取模块主辅助器(第一个注册)
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public T GetMainHelper<T>() where T : IModuleHelper
        {
            if (m_MainHelper.TryGetValue(typeof(T), out IModuleHelper helper))
                return (T)helper;
            return default;
        }
        #endregion

        #region Inner Implement
        private XCore() { }

        private IModule InnerAddModule(Type moduleType, int moduleId, object data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IModule module = (IModule)ModuleUtility.Type.CreateInstance(moduleType);
            InnerInitModule(module, moduleId, data);
            sw.Stop();
            Log.Debug("XFrame", $"Add module {moduleType.Name} time {sw.ElapsedMilliseconds} ms.");
            return module;
        }

        private IModule InnerAddModuleFromSystem(Type moduleType, int moduleId, object data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IModule module = (IModule)Activator.CreateInstance(moduleType);
            InnerInitModule(module, moduleId, data);
            sw.Stop();
            Log.Debug("XFrame", $"Add module {moduleType.Name} time {sw.ElapsedMilliseconds} ms.");
            return module;
        }

        private IModule InnerInitModule(IModule module, int moduleId, object data)
        {
            ModuleBase baseClass = module as ModuleBase;
            if (baseClass != null)
                baseClass.Id = moduleId;
            module.OnInit(data);
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
        #endregion
    }
}
