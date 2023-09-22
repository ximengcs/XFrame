
using System;
using XFrame.Core;
using XFrame.Modules.StateMachine;

namespace XFrame.Modules.Procedure
{
    public interface IProcedureModule : IModule
    {
        IFsm Fsm { get; }

        void Redirect(string name);

        void Redirect(Type type);

        void Add(Type type);

        void Add<T>() where T : ProcedureBase;
    }
}
