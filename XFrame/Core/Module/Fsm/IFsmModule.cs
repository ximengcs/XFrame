
using System;
using XFrame.Core;

namespace XFrame.Modules.StateMachine
{
    public interface IFsmModule : IModule
    {
        IFsm GetOrNew(string name, params Type[] states);

        IFsm GetOrNew(params Type[] states);

        IFsm<T> GetOrNew<T>(string name, T owner, params Type[] states);

        IFsm<T> GetOrNew<T>(T owner, params Type[] states);

        void Remove(string name);

        void Remove(IFsmBase fsm);
    }
}
