
using XFrame.Modules.Reflection;

namespace XFrameTest.Common
{
    public class DefaultTypeChecker : ITypeCheckHelper
    {
        public string[] AssemblyList => new string[] { "XFrameTest" };

        public bool CheckType(Type type)
        {
            return true;
        }
    }
}
