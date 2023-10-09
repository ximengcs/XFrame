using System;
using XFrame.Modules.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Tasks;

namespace XFrame.Core
{
    public static class Entry
    {
        #region Inner Filed
        private static bool m_Inited;
        private static bool m_DoStart;
        private static bool m_Runing;
        private static Action m_OnRun;
        private static Stopwatch m_Sw;

        private static XCore m_Base;
        private static XCore m_Core;
        private static XCore m_Custom;

        private static Dictionary<Type, IEntryHandler> m_Handlers;
        #endregion

        #region Event
        public static event Action OnRun
        {
            add
            {
                if (m_Runing)
                {
                    value?.Invoke();
                }
                else
                {
                    m_OnRun += value;
                }
            }
            remove { m_OnRun -= value; }
        }
        #endregion

        #region Interface
        /// <summary>
        /// 初始化核心模块
        /// </summary>
        public static void Init()
        {
            Log.Debug("XFrame", "Launch Framework");
            m_Sw = Stopwatch.StartNew();
            m_Inited = false;
            m_Runing = false;
            m_DoStart = false;
            m_Handlers = new Dictionary<Type, IEntryHandler>();
            m_Base = XCore.Create(typeof(TypeModule));
            m_Core = XCore.Create();
            m_Custom = XCore.Create();

            InnerConfigHandler();
            InenrInitHandler();
            IInitHandler handler = InnerGetHandler<IInitHandler>();
            handler.EnterHandle();

            InnerInit<BaseModuleAttribute>(m_Base);

            if (handler != null)
            {
                handler.BeforeHandle()
                       .AutoDelete()
                       .OnComplete(() =>
                       {
                           InnerInit<CoreModuleAttribute>(m_Core);
                           InnerInit<CommonModuleAttribute>(m_Custom);
                           handler.AfterHandle()
                                  .AutoDelete()
                                  .OnComplete(() =>
                                  {
                                      m_Inited = true;
                                      if (m_DoStart)
                                          Start();
                                  }).Start();
                       }).Start();
            }
            else
            {
                InnerInit<CoreModuleAttribute>(m_Core);
                InnerInit<CommonModuleAttribute>(m_Custom);
                m_Inited = true;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            m_Base.Start();
            if (!m_Inited)
            {
                m_DoStart = true;
                return;
            }

            m_DoStart = false;
            IStartHandler handler = InnerGetHandler<IStartHandler>();
            if (handler != null)
            {
                handler.BeforeHandle()
                       .AutoDelete()
                       .OnComplete(() =>
                       {
                           m_Core.Start();
                           m_Custom.Start();
                           handler.AfterHandle()
                                  .AutoDelete()
                                  .OnComplete(() =>
                                  {
                                      InnerStartRun();
                                  }).Start();
                       }).Start();
            }
            else
            {
                m_Core.Start();
                m_Custom.Start();
                InnerStartRun();
            }
        }

        private static void InnerStartRun()
        {
            m_Sw.Stop();
            Log.Debug("XFrame", $"Lunch spend time {m_Sw.ElapsedMilliseconds} ms");
            m_Sw = null;
            m_Runing = true;
            m_OnRun?.Invoke();
        }

        public static void Trigger(Type type, object data = null)
        {
            m_Base.Trigger(type, data);
            if (m_Runing)
            {
                m_Core.Trigger(type, data);
                m_Custom.Trigger(type, data);
            }
        }

        public static void Trigger<T>(object data = null)
        {
            Trigger(typeof(T), data);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void ShutDown()
        {
            m_Runing = false;
            m_Custom?.Destroy();
            m_Core?.Destroy();
            m_Base?.Destroy();
            m_Custom = null;
            m_Core = null;
            m_Base = null;
            m_OnRun = null;
        }

        /// <summary>
        /// 添加处理器
        /// </summary>
        /// <typeparam name="T">处理器类型</typeparam>
        public static void AddHandler<T>() where T : IEntryHandler
        {
            Type type = typeof(T);
            Type[] handTypes = type.GetInterfaces();
            foreach (Type handType in handTypes)
            {
                if (handType == typeof(IEntryHandler))
                    continue;
                m_Handlers.Add(handType, (IEntryHandler)ModuleUtility.Type.CreateInstance(type));
            }
        }

        /// <summary>
        /// 添加用户定制模块组
        /// </summary>
        /// <typeparam name="T">模块组特性类型</typeparam>
        public static void AddModules<T>() where T : Attribute
        {
            InnerInit<T>(m_Custom);
        }

        /// <summary>
        /// 添加用户定制模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块</returns>
        public static T AddModule<T>(int moduleId = default, object userData = null) where T : IModule
        {
            return (T)InnerAddModule(typeof(T), moduleId, m_Custom, userData);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public static T GetModule<T>(int moduleId = default) where T : IModule
        {
            return (T)InnerGetModule(typeof(T), moduleId);
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static IModuleHelper GetHelper<T>() where T : IModule
        {
            return m_Core.GetHelper<T>();
        }

        /// <summary>
        /// 获取模块主辅助器(第一个注册)
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static T GetMainHelper<T>() where T : IModuleHelper
        {
            return m_Core.GetMainHelper<T>();
        }
        #endregion

        #region Inner Implement
        private static void InnerConfigHandler()
        {
            ITypeModule typeModule = m_Base.GetModule<ITypeModule>();
            TypeSystem typeSys = typeModule.GetOrNew<IConfigHandler>();
            List<Pair<int, IConfigHandler>> handlers = new List<Pair<int, IConfigHandler>>(typeSys.Count);
            foreach (Type type in typeSys)
            {
                IConfigHandler handler = (IConfigHandler)typeModule.CreateInstance(type);
                XOrderAttribute attr = typeModule.GetAttribute<XOrderAttribute>(type);
                if (attr != null)
                {
                    handlers.Add(Pair.Create(attr.Order, handler));
                }
                else
                {
                    handlers.Add(Pair.Create(0, handler));
                }
            }

            handlers.Sort((pair1, pair2) => pair1.Key - pair2.Key);
            foreach (Pair<int, IConfigHandler> handler in handlers)
            {
                handler.Value.OnHandle();
            }
        }

        private static void InenrInitHandler()
        {
            TypeSystem typeSys = ModuleUtility.Type.GetOrNew<IEntryHandler>();
            foreach (Type type in typeSys)
            {
                Type[] handTypes = type.GetInterfaces();
                foreach (Type handType in handTypes)
                {
                    if (handType == typeof(IEntryHandler))
                        continue;
                    m_Handlers.Add(handType, (IEntryHandler)ModuleUtility.Type.CreateInstance(type));
                }
            }

            typeSys = ModuleUtility.Type.GetOrNew<IModuleHandler>();
            foreach (Type type in typeSys)
            {
                IModuleHandler handler = (IModuleHandler)ModuleUtility.Type.CreateInstance(type);
                m_Base.AddHandle(handler.Target, handler);
                m_Core.AddHandle(handler.Target, handler);
                m_Custom.AddHandle(handler.Target, handler);
            }
        }

        private static void InnerInit<T>(XCore target) where T : Attribute
        {
            TypeSystem typeSys = ModuleUtility.Type.GetOrNewWithAttr<T>();
            foreach (Type type in typeSys)
            {
                InnerAddModule(type, default, target, default);
            }
        }

        private static IModule InnerAddModule(Type moduleType, int moduleId, XCore target, object userData)
        {
            IModule module = InnerGetModule(moduleType, moduleId);
            if (module != null)
                return module;

            if (moduleId == default)
            {
                ITypeModule typeModule = m_Base.GetModule<ITypeModule>();
                Attribute[] requires = typeModule.GetAttributes(moduleType, typeof(RequireModuleAttribute));
                if (requires != null && requires.Length > 0)
                {
                    for (int i = 0; i < requires.Length; i++)
                    {
                        RequireModuleAttribute attr = (RequireModuleAttribute)requires[i];
                        InnerAddModule(attr.ModuleType, default, target, userData);
                    }
                }
            }

            return target.Register(moduleType, moduleId, userData);
        }

        private static IModule InnerGetModule(Type moduleType, int moduleId)
        {
            if (m_Base == null)
                return default;

            IModule module;
            module = m_Base.GetModule(moduleType, moduleId);
            if (module == null)
                module = m_Core.GetModule(moduleType, moduleId);
            if (module == null)
                module = m_Custom.GetModule(moduleType, moduleId);
            return module;
        }

        private static T InnerGetHandler<T>()
        {
            if (m_Handlers.TryGetValue(typeof(T), out IEntryHandler handler))
                return (T)handler;
            return default;
        }
        #endregion
    }
}
