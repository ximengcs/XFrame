using System;
using XFrame.Core;
using XFrame.Modules.Tasks;
using XFrame.Modules.Times;

namespace XFrame.Tasks
{
    /// <summary>
    /// 任务配置器
    /// </summary>
    public class XTaskHelper
    {
        private static ITaskModule m_Module;
        private static ITimeModule m_TimeModule;
        private static XDomain m_Domain;

        /// <summary>
        /// 异常处理
        /// </summary>
        public static Action<Exception> ExceptionHandler;

        /// <summary>
        /// 通用取消绑定器
        /// </summary>
        public static XTaskCancelToken UseToken { get; set; }

        /// <summary>
        /// 通用任务行为
        /// </summary>
        public static XTaskAction UseAction { get; set; }

        /// <summary>
        /// 任务使用时间模块
        /// </summary>
        public static ITimeModule Time => m_TimeModule;

        /// <summary>
        /// 所处域
        /// </summary>
        public static XDomain Domain => m_Domain;

        /// <summary>
        /// 进度最小值
        /// </summary>
        public const int MIN_PROGRESS = 0;

        /// <summary>
        /// 进度最大值
        /// </summary>
        public const int MAX_PROGRESS = 1;

        /// <summary>
        /// 设置域
        /// </summary>
        /// <param name="domain">域</param>
        public static void SetDomain(XDomain domain)
        {
            m_Domain = domain;
            m_Module = domain.GetModule<ITaskModule>();
            m_TimeModule = domain.GetModule<ITimeModule>();
        }

        /// <summary>
        /// 注册可更新任务
        /// </summary>
        /// <param name="task">任务</param>
        public static void Register(IUpdater task)
        {
            m_Module.Register(task);
        }

        /// <summary>
        /// 取消注册可更新任务
        /// </summary>
        /// <param name="task">任务</param>
        public static void UnRegister(IUpdater task)
        {
            m_Module.UnRegister(task);
        }
    }
}