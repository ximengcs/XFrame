using System.Collections.Generic;

namespace XFrame.Collections
{
    public enum XItType
    {
        Forward,
        Backward
    }

    public interface IXEnumerable<T>
    {
        IEnumerator<T> GetEnumerator();
        void SetIt(XItType type);
    }
}
