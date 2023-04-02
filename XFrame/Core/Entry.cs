using System;
using XFrame.Modules.XType;
using System.Collections.Generic;

namespace XFrame.Core
{
    public static class Entry
    {
        #region Inner Filed
        private static bool m_Inited;
        private static bool m_DoStart;
        private static bool m_Runing;

        public static XCore m_Base;
        public static XCore m_Core;
        public static XCore m_Custom;

        private static Dictionary<Type, IEntryHandler> m_Handlers;
        #endregion

        #region Interface
        /// <summary>
        /// 初始化核心模块
        /// </summary>
        public static void Init()
        {
            m_Inited = false;
            m_Runing = false;
            m_DoStart = false;
            m_Handlers = new Dictionary<Type, IEntryHandler>();
            m_Base = XCore.Create();
            m_Core = XCore.Create();
            m_Custom = XCore.Create();

            m_Base.Register<TypeModule>();
            InenrInitHandler();
            IInitHandler handler = InnerGetHandler<IInitHandler>();
            handler.EnterHandle();

            InnerInit<BaseModuleAttribute>(m_Base);

            if (handler != null)
            {
                handler.BeforeHandle()
                       .OnComplete(() =>
                       {
                           InnerInit<CoreModuleAttribute>(m_Core);
                           InnerInit<XModuleAttribute>(m_Custom);
                           handler.AfterHandle()
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
                InnerInit<XModuleAttribute>(m_Custom);
                m_Inited = true;
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
                m_Handlers.Add(handType, (IEntryHandler)Activator.CreateInstance(type));
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
                       .OnComplete(() =>
                       {
                           m_Core.Start();
                           m_Custom.Start();
                           handler.AfterHandle()
                                  .OnComplete(() =>
                                  {
                                      m_Runing = true;
                                  }).Start();
                       }).Start();
            }
            else
            {
                m_Core.Start();
                m_Custom.Start();
                m_Runing = true;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="escapeTime">逃逸时间</param>
        public static void Update(float escapeTime)
        {
            m_Base.Update(escapeTime);
            if (m_Runing)
            {
                m_Core.Update(escapeTime);
                m_Custom.Update(escapeTime);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public static void ShutDown()
        {
            m_Runing = false;
            m_Custom.Destroy();
            m_Core.Destroy();
            m_Base.Destroy();
            m_Custom = null;
            m_Core = null;
            m_Core = null;
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
        public static T AddModule<T>() where T : IModule
        {
            return (T)InnerAddModule(typeof(T), m_Custom);
        }

        /// <summary>
        /// 获取模块
        /// </summary>
        /// <typeparam name="T">模块类型</typeparam>
        /// <returns>模块实例</returns>
        public static T GetModule<T>() where T : IModule
        {
            return (T)InnerGetModule(typeof(T));
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
        private static void InenrInitHandler()
        {
            TypeSystem typeSys = TypeModule.Inst.GetOrNew<IEntryHandler>();
            foreach (Type type in typeSys)
            {
                Type[] handTypes = type.GetInterfaces();
                foreach (Type handType in handTypes)
                {
                    if (handType == typeof(IEntryHandler))
                        continue;
                    m_Handlers.Add(handType, (IEntryHandler)Activator.CreateInstance(type));
                }
            }
        }

        private static void InnerInit<T>(XCore target) where T : Attribute
        {
            TypeSystem typeSys = TypeModule.Inst.GetOrNewWithAttr<T>();
            foreach (Type type in typeSys)
                InnerAddModule(type, target);
        }

        private static IModule InnerAddModule(Type moduleType, XCore target)
        {
            IModule module = InnerGetModule(moduleType);
            if (module != null)
                return module;

            Attribute[] requires = Attribute.GetCustomAttributes(moduleType, typeof(RequireModuleAttribute), true);
            if (requires != null && requires.Length > 0)
            {
                for (int i = 0; i < requires.Length; i++)
                {
                    RequireModuleAttribute attr = (RequireModuleAttribute)requires[i];
                    InnerAddModule(attr.ModuleType, target);
                }
            }

            return target.Register(moduleType);
        }

        private static IModule InnerGetModule(Type moduleType)
        {
            IModule module;
            module = m_Base.GetModule(moduleType);
            if (module == null)
                module = m_Core.GetModule(moduleType);
            if (module == null)
                module = m_Custom.GetModule(moduleType);
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
