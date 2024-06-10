
namespace XFrame.Modules.Diagnotics
{
    /// <summary>
    /// 可配置Log等级
    /// </summary>
    public interface ICanConfigLog
    {
        /// <summary>
        /// Log等级
        /// </summary>
        LogLevel LogLv { get; set; }
    }
}
