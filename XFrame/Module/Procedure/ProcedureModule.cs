using System;
using XFrame.Core;

namespace XFrame.Modules
{
    /// <summary>
    /// 流程模块
    /// </summary>
    public class ProcedureModule : SingletonModule<ProcedureModule>
    {
        private static string ENTRANCE = "MainProcedure";
        private TypeModule.Set m_Procedures;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            m_Procedures = TypeModule.Inst.Register<ProcedureBase>();
            if (m_Procedures.TryGetByName(ENTRANCE, out Type entrance))
            {
                Log.Debug("XFrame", "Enter Main Procedure");
                FsmModule.Inst.GetOrNew(m_Procedures.ToArray()).Start(entrance);
            }
            else
                Log.Error("XFrame", $"Main Procedure do not define");
        }
    }
}
