using XFrame.Collections;
using XFrame.Modules.ID;
using XFrame.Modules.Datas;
using XFrame.Modules.Local;
using XFrame.Modules.Pools;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;
using XFrame.Modules.XType;
using XFrame.Modules.Archives;
using XFrame.Modules.Download;
using XFrame.Modules.Entities;
using XFrame.Modules.Resource;
using XFrame.Modules.Procedure;
using XFrame.Modules.Serialize;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.StateMachine;

namespace XFrame.Core
{
    public static class Entry
    {
        #region Inner Filed
        private static XCore m_Core;
        #endregion

        #region Life Fun
        /// <summary>
        /// 初始化核心模块
        /// </summary>
        public static void Init()
        {
            m_Core = XCore.Create();

            m_Core.Init();
            m_Core.Register<IdModule>();
            m_Core.Register<TypeModule>();
            m_Core.Register<LogModule>();
            m_Core.Register<TimeModule>();
            m_Core.Register<PoolModule>();
            m_Core.Register<FsmModule>();
            m_Core.Register<TaskModule>();
            m_Core.Register<SerializeModule>();
            m_Core.Register<ResModule>();
            m_Core.Register<ArchiveModule>();
            m_Core.Register<DataModule>();
            m_Core.Register<DownloadModule>();
            m_Core.Register<LocalizeModule>();
            m_Core.Register<EntityModule>();
            InnerInitBase();
        }

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            m_Core.Register<ProcedureModule>();
            InnerInitCore();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        public static void Update(float escapeTime)
        {
            m_Core.Update(escapeTime);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void ShutDown()
        {
            m_Core.Destroy();
            m_Core = null;
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public static T Register<T>() where T : IModule
        {
            return m_Core.Register<T>();
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="data">模块初始化数据</param>
        /// <returns>模块实例</returns>
        public static T Register<T>(object data) where T : IModule
        {
            return m_Core.Register<T>(data);
        }

        /// <summary>
        /// 注册模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <typeparam name="ModuleT">模块类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static T RegisterHelper<T, ModuleT>() where T : IModuleHelper where ModuleT : IModule
        {
            return m_Core.RegisterHelper<T, ModuleT>();
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public static T GetModule<T>() where T : IModule
        {
            return m_Core.GetModule<T>();
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static IModuleHelper GetHelper<T>() where T : IModule
        {
            return m_Core.GetHelper<T>();
        }

        /// <summary>
        /// 获取模块主辅助器(第一个注册)
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static T GetMainHelper<T>() where T : IModuleHelper
        {
            return m_Core.GetMainHelper<T>();
        }
        #endregion

        #region Inner Implement
        private static void InnerInitBase()
        {

        }

        private static void InnerInitCore()
        {

        }
        #endregion
    }
}
