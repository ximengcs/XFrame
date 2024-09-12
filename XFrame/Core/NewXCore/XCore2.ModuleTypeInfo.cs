using System;
using System.Collections.Generic;

namespace XFrame.Core.NewXCore
{
    internal partial class XCore2
    {
        private struct ModuleTypeInfo
        {
            public IModule MainModule;
            public Dictionary<int, IModule> Modules;
            public List<IModuleHelper> Helpers;

            public static ModuleTypeInfo Create()
            {
                ModuleTypeInfo info = new ModuleTypeInfo();
                info.MainModule = null;
                info.Modules = new Dictionary<int, IModule>();
                info.Helpers = new List<IModuleHelper>();
                return info;
            }

            public void AddModule(IModule module)
            {
                if (MainModule == null)
                    MainModule = module;
                Modules.Add(module.Id, module);
            }

            public void RemoveModule(IModule module)
            {
                if (MainModule == module)
                    MainModule = null;
                Modules.Remove(module.Id);
            }

            public void TriggerHelperOnCreate(IModule module)
            {
                foreach (IModuleHelper helper in Helpers)
                    helper.OnModuleCreate(module);
            }

            public void TriggerHelperOnInit(IModule module)
            {
                foreach (IModuleHelper helper in Helpers)
                    helper.OnModuleInit(module);
            }

            public void TriggerHelperOnStart(IModule module)
            {
                foreach (IModuleHelper helper in Helpers)
                    helper.OnModuleStart(module);
            }

            public void TriggerHelperOnDestroy(IModule module)
            {
                foreach (IModuleHelper helper in Helpers)
                    helper.OnModuleDestroy(module);
            }
        }
    }
}
