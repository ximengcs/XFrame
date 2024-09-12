using System;
using System.Collections.Generic;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Reflection;

namespace XFrame.Core.NewXCore
{
    internal partial class XCore2 : ICore
    {
        private XCoreState m_State;
        private Dictionary<Type, ModuleTypeInfo> m_Modules;
        private Dictionary<Type, ModuleHandlerInfo> m_Handlers;

        public XCore2()
        {
            m_State = XCoreState.None;
        }

        public void OnInit()
        {
            if (m_State != XCoreState.None)
                Log.Error(Log.XFrame, $"xcore init error. {m_State}");

            m_State = XCoreState.Initialize;
            m_Modules = new Dictionary<Type, ModuleTypeInfo>();
        }

        public void OnStart()
        {
            if (m_State != XCoreState.Initialize)
                Log.Error(Log.XFrame, $"xcore init error. {m_State}");

            m_State = XCoreState.Awake;
        }

        public void SetHandler(IModuleHandler handler)
        {
            Type handleType = handler.Target;
            List<IModule> modules = new List<IModule>();
            foreach (var entry in m_Modules)
            {
                ModuleTypeInfo typeInfo = entry.Value;
                foreach (var moduleEntry in typeInfo.Modules)
                {
                    IModule module = moduleEntry.Value;
                    Type moduleType = module.GetType();
                    if (handleType.IsAssignableFrom(moduleType))
                    {
                        modules.Add(module);
                    }
                }
            }

            m_Handlers[handleType] = new ModuleHandlerInfo(handler, modules);
        }

        public IModuleHelper AddHelper(Type moduleType, Type helperType)
        {
            ModuleTypeInfo typeInfo = InnerGetInfo(moduleType);
            IModuleHelper helper = (IModuleHelper)m_Domain.TypeModule.CreateInstance(helperType);
            helper.OnInit();
            typeInfo.Helpers.Add(helper);
            return helper;
        }

        public void RemoveHelper(Type moduleType, IModuleHelper helper)
        {
            ModuleTypeInfo typeInfo = InnerGetInfo(moduleType);
            typeInfo.Helpers.Remove(helper);
        }

        public IModule AddModule(Type moduleType, int moduleId, object userData)
        {
            IModule module = null;
            ModuleTypeInfo typeInfo = InnerGetInfo(moduleType);
            switch (m_State)
            {
                case XCoreState.Initialize:
                    module = InnerAddModule(typeInfo, moduleType, moduleId, userData);
                    break;

                case XCoreState.Awake:
                    module = InnerAddModule(typeInfo, moduleType, moduleId, userData);
                    module.OnStart();
                    typeInfo.TriggerHelperOnStart(module);
                    break;

                default:
                    Log.Error(Log.XFrame, $"add module error, because of state error");
                    break;
            }

            return module;
        }

        public void RemoveModule(Type moduleType, int moduleId)
        {
            ModuleTypeInfo typeInfo = InnerGetInfo(moduleType);
            if (typeInfo.Modules.TryGetValue(moduleId, out IModule module))
            {
                typeInfo.TriggerHelperOnDestroy(module);
                typeInfo.RemoveModule(module);
            }

            foreach (var entry in m_Handlers)
            {
                entry.Value.Modules.Remove(module);
            }
        }

        private IModule InnerAddModule(ModuleTypeInfo typeInfo, Type moduleType, int moduleId, object userData)
        {
            IModule module = (IModule)m_Domain.TypeModule.CreateInstance(moduleType);
            typeInfo.TriggerHelperOnCreate(module);
            module.OnInit2(moduleId, m_Domain, userData);
            typeInfo.TriggerHelperOnInit(module);
            typeInfo.AddModule(module);

            foreach (var entry in m_Handlers)
            {
                if (entry.Key.IsAssignableFrom(moduleType))
                {
                    entry.Value.Modules.Add(module);
                }
            }

            return module;
        }

        private ModuleTypeInfo InnerGetInfo(Type moduleType)
        {
            if (!m_Modules.TryGetValue(moduleType, out ModuleTypeInfo info))
            {
                info = ModuleTypeInfo.Create();
                m_Modules.Add(moduleType, info);
            }
            return info;
        }
    }
}
