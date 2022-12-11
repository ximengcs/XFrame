
using XFrame.Core;

namespace XFrame.Modules
{
    public abstract class FsmState<T>
    {
        protected IGenericFsm<T> m_Fsm;

        protected internal virtual void OnInit(IGenericFsm<T> fsm)
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

        protected internal void ChangeState<State>() where State : FsmState<T>
        {
            GenericFsm<T> realFsm = m_Fsm as GenericFsm<T>;
            realFsm.ChangeState<State>();
        }
    }
}
