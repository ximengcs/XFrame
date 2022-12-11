
namespace XFrame.Modules
{
    public abstract class FsmState
    {
        protected IFsm m_Fsm;

        protected internal virtual void OnInit(IFsm fsm)
        {
            m_Fsm = fsm;
        }

        protected internal virtual void OnEnter()
        {

        }

        protected internal virtual void OnUpdate()
        {

        }

        protected internal virtual void OnLeave()
        {

        }

        protected internal virtual void OnDestroy()
        {
            m_Fsm = null;
        }

        protected internal void ChangeState<State>() where State : FsmState
        {
            Fsm realFsm = m_Fsm as Fsm;
            realFsm.ChangeState(typeof(State));
        }
    }

    
}
