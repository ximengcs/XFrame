using System;
using XFrame.Collections;
using System.Collections.Generic;

namespace XFrame.Core
{
    /// <summary>
    /// 核心
    /// </summary>
    public partial class XCore
    {
        #region Inner Filed
        private XCollection<IModule> s_Modules;
        private Dictionary<Type, IModuleHelper> s_MainHelper;
        private Dictionary<Type, List<IModuleHelper>> s_Helpers;
        #endregion

        #region Interface
        /// <summary>
        /// 初始化核心
        /// </summary>
        public void Init()
        {
            s_Modules = new XCollection<IModule>();
            s_MainHelper = new Dictionary<Type, IModuleHelper>();
            s_Helpers = new Dictionary<Type, List<IModuleHelper>>();
        }

        /// <summary>
        /// 更新核心
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        public void Update(float escapeTime)
        {
            foreach (IModule manager in s_Modules)
                manager.OnUpdate(escapeTime);
        }

        /// <summary>
        /// 销毁核心
        /// </summary>
        public void Destroy()
        {
            var it = s_Modules.GetBackEnumerator();
            while (it.MoveNext())
                it.Current.OnDestroy();
            s_Modules.Clear();
            s_Modules = null;
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public T Register<T>() where T : IModule
        {
            return InnerAddModule<T>();
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="data">模块初始化数据</param>
        /// <returns>模块实例</returns>
        public T Register<T>(object data) where T : IModule
        {
            return InnerAddModule<T>(data);
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
            if (!s_Helpers.TryGetValue(mType, out List<IModuleHelper> helpers))
            {
                helpers = new List<IModuleHelper>();
                s_Helpers.Add(mType, helpers);
            }
            T helper = Activator.CreateInstance<T>();
            helpers.Add(helper);

            if (!s_MainHelper.ContainsKey(typeof(T)))
                s_MainHelper[typeof(T)] = helper;

            return helper;
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public T GetModule<T>() where T : IModule
        {
            return InnerGetModule<T>();
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public IModuleHelper GetHelper<T>() where T : IModule
        {
            if (s_Helpers.TryGetValue(typeof(T), out List<IModuleHelper> helpers))
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
            if (s_MainHelper.TryGetValue(typeof(T), out IModuleHelper helper))
                return (T)helper;
            return default;
        }
        #endregion

        #region Inner Implement
        private T InnerAddModule<T>(object data = null) where T : IModule
        {
            T module = (T)Activator.CreateInstance(typeof(T));
            module.OnInit(data);
            s_Modules.Add(module);
            return module;
        }

        private T InnerGetModule<T>() where T : IModule
        {
            return s_Modules.Get<T>();
        }
        #endregion
    }
}
