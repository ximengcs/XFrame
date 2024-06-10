using System;
using XFrame.Modules.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using XFrame.Modules.Diagnotics;
using XFrame.Modules.Pools;
using XFrame.Tasks;

namespace XFrame.Core
{
    /// <summary>
    /// 框架入口
    /// </summary>
    public static class Entry
    {
        #region Inner Filed
        private static bool m_Inited;
        private static bool m_DoStart;
        private static bool m_Runing;
        private static Action m_OnRun;
        private static long m_Time;

        private static int CORE = 1;
        private static int CUSTOM = 2;
        private static XDomain m_Domain;

        private static Dictionary<Type, IEntryHandler> m_Handlers;
        #endregion

        #region Event
        /// <summary>
        /// 开始运行事件
        /// </summary>
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
        /// 当前域
        /// </summary>
        public static XDomain Domain => m_Domain;

        /// <summary>
        /// 初始化核心模块
        /// </summary>
        public static void Init()
        {
            Log.Debug(Log.XFrame, "Launch Framework");
            m_Time = DateTime.Now.Ticks;
            m_Inited = false;
            m_Runing = false;
            m_DoStart = false;
            m_Handlers = new Dictionary<Type, IEntryHandler>();
            m_Domain = new XDomain(3);
            m_Domain.SetTypeModule(typeof(TypeModule));

            InnerConfigHandler();
            InenrInitHandler();
            IInitHandler handler = InnerGetHandler<IInitHandler>();
            handler.EnterHandle();

            InnerInit<BaseModuleAttribute>(m_Domain.Base);
            Log.SetDomain(m_Domain);
            References.SetDomain(m_Domain);
            XTaskHelper.SetDomain(m_Domain);

            if (handler != null)
            {
                handler.BeforeHandle()
                       .OnCompleted(() =>
                       {
                           InnerInit<CoreModuleAttribute>(m_Domain[CORE]);
                           InnerInit<CommonModuleAttribute>(m_Domain[CUSTOM]);
                           handler.AfterHandle()
                                  .OnCompleted(() =>
                                  {
                                      m_Inited = true;
                                      if (m_DoStart)
                                          Start();
                                  }).Coroutine();
                       }).Coroutine();
            }
            else
            {
                InnerInit<CoreModuleAttribute>(m_Domain[CORE]);
                InnerInit<CommonModuleAttribute>(m_Domain[CUSTOM]);
                m_Inited = true;
            }
        }

        /// <summary>
        /// 启动
        /// </summary>
        public static void Start()
        {
            m_Domain.Base.Start();
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
                       .OnCompleted(() =>
                       {
                           InnerStartRun();
                           m_Domain[CORE].Start();
                           m_Domain[CUSTOM].Start();
                           handler.AfterHandle().Coroutine();
                       }).Coroutine();
            }
            else
            {
                InnerStartRun();
                m_Domain[CORE].Start();
                m_Domain[CUSTOM].Start();
            }
        }

        private static void InnerStartRun()
        {
            m_Time = DateTime.Now.Ticks - m_Time;
            Log.Debug(Log.XFrame, $"Lunch spend time {m_Time / TimeSpan.TicksPerMillisecond} ms");
            m_Runing = true;
            m_OnRun?.Invoke();
        }

        /// <summary>
        /// 触发模块处理器 <see cref="IModuleHandler"/>
        /// </summary>
        /// <param name="type">目标类型 <see cref="IModuleHandler.Target"/></param>
        /// <param name="data">参数 <see cref="IModuleHandler.Handle(IModule, object)"/></param>
        public static void Trigger(Type type, object data = null)
        {
            m_Domain.Base.Trigger(type, data);
            if (m_Runing)
            {
                for (int i = 1; i < m_Domain.Count; i++)
                    m_Domain.Trigger(i, type, data);
            }
        }

        /// <summary>
        /// 触发模块处理器 <see cref="IModuleHandler"/>
        /// </summary>
        /// <typeparam name="T">目标类型 <see cref="IModuleHandler.Target"/></typeparam>
        /// <param name="data">参数 <see cref="IModuleHandler.Handle(IModule, object)"/></param>
        public static void Trigger<T>(object data = null)
        {
            Trigger(typeof(T), data);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void ShutDown()
        {
            if (m_Domain != null)
            {
                m_Runing = false;
                m_Domain.Destroy();
                m_Domain = null;
                m_OnRun = null;
            }
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
                m_Handlers.Add(handType, (IEntryHandler)m_Domain.TypeModule.CreateInstance(type));
            }
        }

        /// <summary>
        /// 添加用户定制模块组
        /// </summary>
        /// <typeparam name="T">模块组特性类型</typeparam>
        public static void AddModules<T>() where T : Attribute
        {
            InnerInit<T>(m_Domain[CUSTOM]);
        }

        /// <summary>
        /// 添加用户定制模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块</returns>
        public static T AddModule<T>(int moduleId = default, object userData = null) where T : IModule
        {
            return (T)InnerAddModule(typeof(T), moduleId, m_Domain[CUSTOM], userData);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="module">模块</param>
        public static void RemoveModule(IModule module)
        {
            m_Domain.RemoveModule(module);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        public static void RemoveModule(Type moduleType, int moduleId = default)
        {
            m_Domain.RemoveModule(moduleType, moduleId);
        }

        /// <summary>
        /// 移除模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <param name="moduleId">模块Id</param>
        public static void RemoveModule<T>(int moduleId = default) where T : IModule
        {
            m_Domain.RemoveModule(typeof(T), moduleId);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public static T GetModule<T>(int moduleId = default) where T : IModule
        {
            return (T)m_Domain.GetModule(typeof(T), moduleId);
        }

        /// <summary>
        /// 获取模块辅助器
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static T[] GetHelpers<T>() where T : IModule
        {
            return m_Domain.Base.GetHelpers<T>();
        }

        /// <summary>
        /// 获取模块主辅助器(第一个注册)
        /// </summary>
        /// <typeparam name="T">辅助器类型</typeparam>
        /// <returns>辅助器实例</returns>
        public static T GetMainHelper<T>() where T : IModuleHelper
        {
            return m_Domain.Base.GetMainHelper<T>();
        }
        #endregion

        #region Inner Implement
        private static void InnerConfigHandler()
        {
            ITypeModule typeModule = m_Domain.Base.GetModule<ITypeModule>();
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
            TypeSystem typeSys = m_Domain.TypeModule.GetOrNew<IEntryHandler>();
            foreach (Type type in typeSys)
            {
                Type[] handTypes = type.GetInterfaces();
                foreach (Type handType in handTypes)
                {
                    if (handType == typeof(IEntryHandler))
                        continue;
                    m_Handlers.Add(handType, (IEntryHandler)m_Domain.TypeModule.CreateInstance(type));
                }
            }

            typeSys = m_Domain.TypeModule.GetOrNew<IModuleHandler>();
            foreach (Type type in typeSys)
            {
                IModuleHandler handler = (IModuleHandler)m_Domain.TypeModule.CreateInstance(type);
                m_Domain.AddHandle(handler.Target, handler);
            }
        }

        private static void InnerInit<T>(XCore target) where T : Attribute
        {
            TypeSystem typeSys = m_Domain.Base.GetModule<ITypeModule>().GetOrNewWithAttr<T>();
            foreach (Type type in typeSys)
            {
                InnerAddModule(type, default, target, default);
            }
        }

        private static IModule InnerAddModule(Type moduleType, int moduleId, XCore target, object userData)
        {
            IModule module = m_Domain.GetModule(moduleType, moduleId);
            if (module != null)
                return module;

            if (moduleId == default)
            {
                ITypeModule typeModule = m_Domain.Base.GetModule<ITypeModule>();
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

        private static T InnerGetHandler<T>()
        {
            if (m_Handlers.TryGetValue(typeof(T), out IEntryHandler handler))
                return (T)handler;
            return default;
        }
        #endregion
    }
}
