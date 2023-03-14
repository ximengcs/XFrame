using System;
using System.Reflection;

namespace XFrame.Modules.Tasks
{
    public partial class TaskBase
    {
        private class StrategyInfo
        {
            private ITaskStrategy m_Inst;
            private Func<ITask, float> m_HandleMethod;
            private Delegate m_UseMethod;
            private Action m_FinishMethod;

            public Type HandleType;

            public StrategyInfo(ITaskStrategy strategy, Type baseType)
            {
                m_Inst = strategy;
                Type type = strategy.GetType();
                Type interfaceType = type.GetInterface(baseType.FullName);
                HandleType = interfaceType.GetGenericArguments()[0];

                MethodInfo handleMethod = type.GetMethod("OnHandle");
                MethodInfo useMethod = type.GetMethod("OnUse");
                MethodInfo finishMethod = type.GetMethod("OnFinish");

                m_HandleMethod = (Func<ITask, float>)handleMethod.CreateDelegate(typeof(Func<ITask, float>), m_Inst);

                Type usePramType = typeof(Action<>);
                usePramType = usePramType.MakeGenericType(HandleType);
                m_UseMethod = useMethod.CreateDelegate(usePramType, m_Inst);

                m_FinishMethod = (Action)finishMethod.CreateDelegate(typeof(Action), m_Inst);
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
                m_UseMethod?.DynamicInvoke(handler);
            }

            public float Handle(ITask from)
            {
                return m_HandleMethod(from);
            }

            public void Finish()
            {
                m_FinishMethod();
            }
        }
    }
}
