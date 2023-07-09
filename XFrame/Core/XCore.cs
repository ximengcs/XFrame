using System;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.XType;
using System.Diagnostics;
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    /// <summary>
    /// 核心
    /// </summary>
    public partial class XCore
    {
        #region Inner Filed
        private bool m_IsStart;
        private XCollection<IModule> m_Modules;
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
        /// 更新核心
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        public void Update(float escapeTime)
        {
            foreach (IModule manager in m_Modules)
                manager.OnUpdate(escapeTime);
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

        public IModule Register(IModule module)
        {
            return InnerInitModule(module, null);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public T Register<T>() where T : IModule
        {
            return (T)InnerAddModule(typeof(T), default);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns>模块实例</returns>
        public IModule Register(Type moduleType)
        {
            return InnerAddModule(moduleType, default);
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="data">模块初始化数据</param>
        /// <returns>模块实例</returns>
        public T Register<T>(object data) where T : IModule
        {
            return (T)InnerAddModule(typeof(T), data);
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
            T helper = TypeModule.Inst.CreateInstance<T>();
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
        public T GetModule<T>() where T : IModule
        {
            return (T)InnerGetModule(typeof(T));
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <returns>模块实例</returns>
        public IModule GetModule(Type moduleType)
        {
            return m_Modules.Get(moduleType);
        }

        public bool HasModule(Type moduleType)
        {
            return GetModule(moduleType) != null;
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

        private IModule InnerAddModule(Type moduleType, object data)
        {
            Stopwatch sw = Stopwatch.StartNew();
            IModule module;
            if (TypeModule.Inst != null)
                module = (IModule)TypeModule.Inst.CreateInstance(moduleType);
            else
                module = (IModule)Activator.CreateInstance(moduleType);

            InnerInitModule(module, data);
            sw.Stop();
            Log.EnqueueWaitQueue("XFrame", $"Add module {moduleType.Name} time {sw.ElapsedMilliseconds} ms.");
            return module;
        }

        private IModule InnerInitModule(IModule module, object data)
        {
            module.OnInit(data);
            m_Modules.Add(module);
            return module;
        }

        private IModule InnerGetModule(Type moduleType)
        {
            return m_Modules.Get(moduleType);
        }
        #endregion
    }
}
