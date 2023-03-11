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

        public static XCore Create(params Type[] modules)
        {
            XCore core = new XCore();
            core.Init();
            foreach (Type moduleType in modules)
                core.InnerAddModule(moduleType, default);
            return core;
        }

        public static XCore Create(Type[] modules, object[] datas = null)
        {
            XCore core = new XCore();
            core.Init();
            for (int i = 0; i < modules.Length; i++)
            {
                Type moduleType = modules[i];
                object data = i < datas.Length ? datas[i] : default;
                core.InnerAddModule(moduleType, data);
            }
            return core;
        }
    }
}
