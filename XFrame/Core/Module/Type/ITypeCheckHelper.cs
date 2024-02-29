using System;

namespace XFrame.Modules.Reflection
{
    public interface ITypeCheckHelper
    {
        string[] AssemblyList { get; }

        bool CheckType(Type type);
    }
}
