using System;
using XFrame.Collections;

namespace XFrame.Modules.Tasks
{
    public partial class TaskBase
    {
        private class ExecInfo
        {
            private StrategyInfo m_Strategy;
            private ITaskHandler m_Handler;
            private Type m_HandType;

            public void Init(ITaskHandler handler, XNode<StrategyInfo> nodes)
            {
                m_Handler = handler;
                m_HandType = handler.GetType();

                StrategyInfo max = null;
                int level = 0;

                nodes.ForEachAll((n) =>
                {
                    StrategyInfo info = n.Value;
                    if (m_HandType == info.HandleType)
                    {
                        m_Strategy = info;
                        max = null;
                        return false;
                    }
                    else if (info.IsSub(m_HandType))
                    {
                        if (max == null || n.Level > level)
                        {
                            max = info;
                            level = n.Level;
                        }
                    }
                    return true;
                });

                if (max != null)
                    m_Strategy = max;

                m_Strategy.Inst.OnUse();
            }

            public float Exec(ITask from)
            {
                return (float)m_Strategy.HandleMethod.Invoke(m_Strategy.Inst, new object[] { from, m_Handler });
            }

        }
    }
}
