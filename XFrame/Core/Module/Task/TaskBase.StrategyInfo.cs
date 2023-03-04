using System;
using System.Reflection;

namespace XFrame.Modules.Tasks
{
    public partial class TaskBase
    {
        private class StrategyInfo
        {
            private ITaskStrategy m_Inst;
            private MethodInfo m_HandleMethod;
            private MethodInfo m_UseMethod;
            private MethodInfo m_FinishMethod;

            public Type HandleType;

            public StrategyInfo(ITaskStrategy strategy, Type baseType)
            {
                m_Inst = strategy;
                Type type = strategy.GetType();
                Type interfaceType = type.GetInterface(baseType.FullName);
                HandleType = interfaceType.GetGenericArguments()[0];

                m_HandleMethod = type.GetMethod("OnHandle");
                m_UseMethod = type.GetMethod("OnUse");
                m_FinishMethod = type.GetMethod("OnFinish");
            }

            public bool IsSub(StrategyInfo info)
            {
                return HandleType.IsAssignableFrom(info.HandleType);
            }

            public bool IsSub(Type type)
            {
                return HandleType.IsAssignableFrom(type);
            }

            public void Use(object handler)
            {
                m_UseMethod.Invoke(m_Inst, new object[] { handler });
            }

            public float Handle(ITask from)
            {
                return (float)m_HandleMethod.Invoke(m_Inst, new object[] { from });
            }

            public void Finish()
            {
                m_FinishMethod.Invoke(m_Inst, null);
            }
        }
    }
}
