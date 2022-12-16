using System;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Modules.Archives;
using XFrame.Modules.Datas;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Download;
using XFrame.Modules.Entities;
using XFrame.Modules.ID;
using XFrame.Modules.Local;
using XFrame.Modules.Pools;
using XFrame.Modules.Procedure;
using XFrame.Modules.Resource;
using XFrame.Modules.Serialize;
using XFrame.Modules.StateMachine;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using XFrame.Modules.XType;

namespace XFrame.Core
{
    public static class Entry
    {
        #region Inner Filed
        private static XCollection<IModule> s_Modules;
        private static Dictionary<Type, IModuleHelper> s_MainHelper;
        private static Dictionary<Type, List<IModuleHelper>> s_Helpers;
        #endregion

        #region Life Fun
        /// <summary>
        /// 初始化核心模块
        /// </summary>
        public static void Init()
        {
            s_Modules = new XCollection<IModule>();
            s_MainHelper = new Dictionary<Type, IModuleHelper>();
            s_Helpers = new Dictionary<Type, List<IModuleHelper>>();

            InnerAddModule<IdModule>();
            InnerAddModule<TypeModule>();
            InnerAddModule<LogModule>();
            InnerAddModule<TimeModule>();
            InnerAddModule<PoolModule>();
            InnerAddModule<FsmModule>();
            InnerAddModule<TaskModule>();
            InnerAddModule<SerializeModule>();
            InnerAddModule<ResModule>();
            InnerAddModule<ArchiveModule>();
            InnerAddModule<DataModule>();
            InnerAddModule<DownloadModule>();
            InnerAddModule<LocalizeModule>();
            InnerAddModule<EntityModule>();
            InnerInitBase();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            InnerAddModule<ProcedureModule>();
            InnerInitCore();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        public static void Update(float escapeTime)
        {
            foreach (IModule manager in s_Modules)
                manager.OnUpdate(escapeTime);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void ShutDown()
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
        public static T Register<T>() where T : IModule
        {
            return InnerAddModule<T>();
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="data">模块初始化数据</param>
        /// <returns>模块实例</returns>
        public static T Register<T>(object data) where T : IModule
        {
            return InnerAddModule<T>(data);
        }

        /// <summary>
        /// 注册模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <typeparam name="ModuleT">模块类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static T RegisterHelper<T, ModuleT>() where T : IModuleHelper where ModuleT : IModule
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
        public static T GetModule<T>() where T : IModule
        {
            return InnerGetManager<T>();
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static IModuleHelper GetHelper<T>() where T : IModule
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
        public static T GetMainHelper<T>() where T : IModuleHelper
        {
            if (s_MainHelper.TryGetValue(typeof(T), out IModuleHelper helper))
                return (T)helper;
            return default;
        }
        #endregion

        #region Inner Implement
        private static void InnerInitBase()
        {
            PoolModule.Inst.GetOrNew<IXItem>(32);
        }

        private static void InnerInitCore()
        {

        }

        private static T InnerAddModule<T>(object data = null) where T : IModule
        {
            T module = Activator.CreateInstance<T>();
            module.OnInit(data);
            s_Modules.Add(module);
            return module;
        }

        private static T InnerGetManager<T>() where T : IModule
        {
            return s_Modules.Get<T>();
        }
        #endregion
    }
}
