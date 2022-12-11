using XFrame.Core;

namespace XFrame.Modules
{
    public interface IGenericFsm<T> : IFsmBase, IDataProvider
    {
        T Owner { get; }
        FsmState<T> Current { get; }
        State GetState<State>() where State : FsmState<T>;
        bool HasState<State>() where State : FsmState<T>;
        void Start<State>() where State : FsmState<T>;
        void OnInit(T owner);
    }
}
