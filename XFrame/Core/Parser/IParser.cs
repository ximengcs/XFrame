
namespace XFrame.Core
{
    public interface IParser
    {
        object Value { get; }

        object Parse(string pattern);
    }

    public interface IParser<T> : IParser
    {
        new T Value { get; }

        new T Parse(string pattern);
    }
}
