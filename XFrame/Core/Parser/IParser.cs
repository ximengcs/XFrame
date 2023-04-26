
namespace XFrame.Core
{
    /// <summary>
    /// 解析器
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// 值
        /// </summary>
        object Value { get; }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="pattern">源数据</param>
        /// <returns>结果</returns>
        object Parse(string pattern);
    }

    public interface IParser<T> : IParser
    {
        new T Value { get; }

        new T Parse(string pattern);
    }
}
