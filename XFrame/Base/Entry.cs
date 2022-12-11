using System;
using System.Collections.Generic;
using XFrame.Collections;
using XFrame.Modules;

namespace XFrame.Core
{
    public static class Entry
    {
        #region Inner Filed
        private static XCollection<IModule> s_Modules;
        private static Dictionary<Type, IModuleHelper> s_MainHelper;
        private static Dictionary<Type, List<IModuleHelper>> s_Helpers;
        #endregion

        #region Life Fun
        public static void Init()
        {
            s_Modules = new XCollection<IModule>();
            s_MainHelper = new Dictionary<Type, IModuleHelper>();
            s_Helpers = new Dictionary<Type, List<IModuleHelper>>();

            InnerAddModule<IDGenerator>();
            InnerAddModule<TimeModule>();
            InnerAddModule<LogModule>();
            InnerAddModule<PoolModule>();
            InnerAddModule<TypeModule>();
            InnerAddModule<FsmModule>();
            InnerAddModule<TaskModule>();
            InnerAddModule<SerializeModule>();
            InnerAddModule<ResModule>();
            InnerAddModule<ArchiveModule>();
            InnerAddModule<DataModule>();
            InnerInitBase();
        }

        public static void Start()
        {
            InnerAddModule<DownloadModule>();
            InnerAddModule<LocalizeModule>();
            InnerAddModule<EntityModule>();
            InnerAddModule<ProcedureModule>();
            InnerInitCore();
        }

        public static void Update(float escapeTime)
        {
            foreach (IModule manager in s_Modules)
                manager.OnUpdate(escapeTime);
        }

        public static void ShutDown()
        {
            var it = s_Modules.GetBackEnumerator();
            while (it.MoveNext())
                it.Current.OnDestroy();
            s_Modules.Clear();
            s_Modules = null;
        }

        public static T Register<T>() where T : IModule
        {
            return InnerAddModule<T>();
        }

        public static T Register<T>(object data) where T : IModule
        {
            return InnerAddModule<T>(data);
        }

        public static T RegisterHelper<T, ModuleT>() where T : IModuleHelper where ModuleT : IModule
        {
            Type mType = typeof(ModuleT);
            if (!s_Helpers.TryGetValue(mType, out List<IModuleHelper> helpers))
            {
                helpers = new List<IModuleHelper>();
                s_Helpers.Add(mType, helpers);
            }
            T helper = Activator.CreateInstance<T>();
            helpers.Add(helper);

            if (!s_MainHelper.ContainsKey(typeof(T)))
                s_MainHelper[typeof(T)] = helper;

            return helper;
        }

        public static T GetModule<T>() where T : IModule
        {
            return InnerGetManager<T>();
        }

        public static IModuleHelper GetHelper<T>() where T : IModule
        {
            if (s_Helpers.TryGetValue(typeof(T), out List<IModuleHelper> helpers))
                return helpers[0];
            return default;
        }

        public static T GetMainHelper<T>() where T : IModuleHelper
        {
            if (s_MainHelper.TryGetValue(typeof(T), out IModuleHelper helper))
                return (T)helper;
            return default;
        }
        #endregion

        #region Inner Implement
        private static void InnerInitBase()
        {
            PoolModule.Inst.Register<IXItem>(32);
        }

        private static void InnerInitCore()
        {

        }

        private static T InnerAddModule<T>(object data = null) where T : IModule
        {
            T module = Activator.CreateInstance<T>();
            module.OnInit(data);
            s_Modules.Add(module);
            return module;
        }

        private static T InnerGetManager<T>() where T : IModule
        {
            return s_Modules.Get<T>();
        }
        #endregion
    }
}
