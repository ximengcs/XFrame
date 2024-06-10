using System;
using System.Collections.Generic;

namespace XFrame.Core
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        private Dictionary<Type, int> m_UseModules;

        /// <summary>
        /// 是否是默认模块
        /// </summary>
        protected bool IsDefaultModule => Id == 0;

        /// <summary>
        /// 模块Id
        /// </summary>
        public int Id { get; internal set; }

        /// <inheritdoc/>
        public XDomain Domain { get; internal set; }

        void IModule.OnInit(object data, ModuleConfigAction configCallback)
        {
            m_UseModules = new Dictionary<Type, int>();
            configCallback?.Invoke(this);
            OnInit(data);
        }

        /// <summary>
        /// 注册此模块使用的模块类型的模块Id
        /// </summary>
        /// <param name="moduleType">模块类型</param>
        /// <param name="moduleId">模块Id</param>
        public void RegisterUseModule(Type moduleType, int moduleId)
        {
            if (!m_UseModules.ContainsKey(moduleType))
                m_UseModules.Add(moduleType, moduleId);
            else
                m_UseModules[moduleType] = moduleId;
        }

        /// <summary>
        /// 获取使用的模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetUseModule<T>() where T : IModule
        {
            if (!m_UseModules.TryGetValue(typeof(T), out int moduleId))
                moduleId = default;
            return Domain.GetModule<T>(moduleId);
        }

        void IModule.OnStart()
        {
            OnStart();
        }

        void IModule.OnDestroy()
        {
            OnDestroy();
        }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnInit(object data) { }

        /// <summary>
        /// 启动生命周期
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        /// 销魂生命周期
        /// </summary>
        protected virtual void OnDestroy() { }
    }
}
