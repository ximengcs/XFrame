using XFrame.Utility;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.StateMachine;

namespace XFrame.Modules.Procedure
{
    /// <summary>
    /// 流程基类
    /// </summary>
    public abstract class ProcedureBase : FsmState
    {
        private string m_InstName;

        protected internal override void OnInit(IFsm fsm)
        {
            base.OnInit(fsm);
            m_InstName = TypeUtility.GetSimpleName(GetType().Name);
        }

        protected internal override void OnEnter()
        {
            base.OnEnter();
            Log.Debug("Proc", $"Enter {m_InstName} Procedure");
        }

        protected internal override void OnLeave()
        {
            base.OnLeave();
            Log.Debug("Proc", $"Leave {m_InstName} Procedure");
        }
    }
}
