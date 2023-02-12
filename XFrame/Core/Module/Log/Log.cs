
namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// Log
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// 调试信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Debug(params object[] content)
        {
            LogModule.Inst.Debug(content);
        }

        /// <summary>
        /// 警告信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Warning(params object[] content)
        {
            LogModule.Inst.Warning(content);
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Error(params object[] content)
        {
            LogModule.Inst.Error(content);
        }

        /// <summary>
        /// 致命错误信息
        /// </summary>
        /// <param name="content">信息</param>
        public static void Fatal(params object[] content)
        {
            LogModule.Inst.Fatal(content);
        }
    }
}
