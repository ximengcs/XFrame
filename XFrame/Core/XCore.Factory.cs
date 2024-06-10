using System;

namespace XFrame.Core
{
    public partial class XCore
    {
        /// <summary>
        /// 创建一个核心
        /// </summary>
        /// <param name="domain">域</param>
        /// <returns>核心实例</returns>
        public static XCore Create(XDomain domain)
        {
            XCore core = new XCore();
            core.Init(domain);
            return core;
        }

        /// <summary>
        /// 创建一个核心
        /// </summary>
        /// <param name="domain">域</param>
        /// <param name="coreType">添加的核心模块类型</param>
        /// <param name="modules">其他类型模块列表</param>
        /// <returns>核心模块</returns>
        public static XCore Create(XDomain domain, Type coreType, params Type[] modules)
        {
            XCore core = new XCore();
            core.Init(domain);
            core.AddModuleFromSystem(coreType, default, default);
            foreach (Type moduleType in modules)
                core.InnerAddModule(moduleType, default, default);
            return core;
        }

        /// <summary>
        /// 创建一个核心
        /// </summary>
        /// <param name="domain">域</param>
        /// <param name="coreType">添加的核心模块类型</param>
        /// <param name="modules">其他类型模块列表</param>
        /// <param name="datas">初始化参数</param>
        /// <returns>核心模块</returns>
        public static XCore Create(XDomain domain, Type coreType, Type[] modules, object[] datas = null)
        {
            XCore core = new XCore();
            core.Init(domain);
            core.AddModuleFromSystem(coreType, default, default);
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
