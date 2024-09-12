using XFrame.Core.Threads;

namespace XFrame.Core.NewXCore
{
    public interface ICore
    {
        ITypeScanner TypeScanner { get; }

        Fiber Fiber { get; }
    }
}
