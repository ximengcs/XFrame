using System;
using System.Reflection;

namespace XFrame.Modules.Tasks
{
    public partial class TaskBase
    {
        private class StrategyInfo
        {
            public ITaskStrategy Inst;
            public Type HandleType;
            public MethodInfo HandleMethod;

            public bool IsSub(StrategyInfo info)
            {
                return HandleType.IsAssignableFrom(info.HandleType);
            }

            public bool IsSub(Type type)
            {
                return HandleType.IsAssignableFrom(type);
            }
        }
    }
}
