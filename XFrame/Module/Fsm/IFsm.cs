using System;
using XFrame.Core;

namespace XFrame.Modules
{
    public interface IFsm : IFsmBase, IDataProvider
    {
        FsmState Current { get; }
        State GetState<State>() where State : FsmState;
        bool HasState<State>() where State : FsmState;
        bool HasState(Type type);
        void Start<State>() where State : FsmState;
        void Start(Type type);
        void OnInit();
    }
}
