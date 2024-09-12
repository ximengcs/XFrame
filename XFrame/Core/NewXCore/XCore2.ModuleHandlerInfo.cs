using System.Collections.Generic;

namespace XFrame.Core.NewXCore
{
    internal partial class XCore2
    {
        private struct ModuleHandlerInfo
        {
            public IModuleHandler Handler;
            public List<IModule> Modules;

            public ModuleHandlerInfo(IModuleHandler handler, List<IModule> modules)
            {
                Handler = handler;
                Modules = modules;
            }
        }
    }
}
