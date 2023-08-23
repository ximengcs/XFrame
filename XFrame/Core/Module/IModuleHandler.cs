using System;

namespace XFrame.Core
{
    public interface IModuleHandler
    {
        Type Target { get; }
        void Handle(IModule module, object data);
    }
}
