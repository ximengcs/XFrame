using System;

namespace XFrame.Core
{
    public partial class XCore
    {
        public static XCore Create()
        {
            XCore core = new XCore();
            core.Init();
            return core;
        }

        public static XCore Create(Type coreType, params Type[] modules)
        {
            XCore core = new XCore();
            core.Init();
            core.InnerAddModuleFromSystem(coreType, default, default);
            foreach (Type moduleType in modules)
                core.InnerAddModule(moduleType, default, default);
            return core;
        }

        public static XCore Create(Type coreType, Type[] modules, object[] datas = null)
        {
            XCore core = new XCore();
            core.Init();
            core.InnerAddModuleFromSystem(coreType, default, default);
            for (int i = 0; i < modules.Length; i++)
            {
                Type moduleType = modules[i];
                object data = i < datas.Length ? datas[i] : default;
                core.InnerAddModule(moduleType, default, data);
            }
            return core;
        }
    }
}
