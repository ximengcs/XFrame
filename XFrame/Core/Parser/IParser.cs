
namespace XFrame.Core
{
    public interface IParser<T>
    {
        T Value { get; }

        T Parse(string pattern);
    }
}
