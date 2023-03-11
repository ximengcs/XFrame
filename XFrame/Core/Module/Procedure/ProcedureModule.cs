using System;
using XFrame.Core;
using XFrame.Utility;
using XFrame.Modules.XType;
using XFrame.Modules.Config;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.StateMachine;

namespace XFrame.Modules.Procedure
{
    /// <summary>
    /// 流程模块
    /// </summary>
    [XModule]
    public class ProcedureModule : SingletonModule<ProcedureModule>
    {
        private TypeSystem m_Procedures;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            m_Procedures = TypeModule.Inst.GetOrNew<ProcedureBase>();
        }

        protected override void OnStart()
        {
            base.OnStart();
            string entrance = XConfig.Entrance;
            if (!string.IsNullOrEmpty(entrance) && m_Procedures.TryGetByName(entrance, out Type type))
            {
                Log.Debug("XFrame", $"Enter {TypeUtility.GetSimpleName(entrance)} Procedure");
                FsmModule.Inst.GetOrNew(m_Procedures.ToArray()).Start(type);
            }
            else
            {
                Log.Error("XFrame", $"{TypeUtility.GetSimpleName(entrance)} Procedure do not define");
            }
        }
    }
}
