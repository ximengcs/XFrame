
using System;
using XFrame.Core;

namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// Log模块
    /// </summary>
    public interface ILogModule : IModule
    {
        /// <summary>
        /// 添加Log辅助器
        /// </summary>
        /// <typeparam name="T">Log辅助器类型</typeparam>
        void AddLogger<T>() where T : ILogger;

        /// <summary>
        /// 获取Log辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>获取到的实例</returns>
        T GetLogger<T>() where T : ILogger;

        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="content">信息</param>
        void Debug(params object[] content);

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="content">信息</param>
        void Warning(params object[] content);

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">信息</param>
        void Error(params object[] content);

        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="content">信息</param>
        void Fatal(params object[] content);

        /// <summary>
        /// 异常错误
        /// </summary>
        /// <param name="e">异常</param>
        void Exception(Exception e);
    }
}
