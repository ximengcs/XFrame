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

        protected bool IsDefaultModule => Id == 0;

        /// <summary>
        /// 模块Id
        /// </summary>
        public int Id { get; internal set; }

        public XCore Domain { get; internal set; }

        void IModule.OnInit(object data, ModuleConfigAction configCallback)
        {
            m_UseModules = new Dictionary<Type, int>();
            configCallback?.Invoke(this);
            OnInit(data);
        }

        public void RegisterUseModule(Type moduleType, int moduleId)
        {
            if (!m_UseModules.ContainsKey(moduleType))
                m_UseModules.Add(moduleType, moduleId);
            else
                m_UseModules[moduleType] = moduleId;
        }

        public T GetUseModule<T>() where T : IModule
        {
            if (!m_UseModules.TryGetValue(typeof(T), out int moduleId))
                moduleId = default;
            return Entry.GetModule<T>(moduleId);
        }

        void IModule.OnStart()
        {
            OnStart();
        }

        void IModule.OnDestroy()
        {
            OnDestroy();
        }

        protected virtual void OnInit(object data) { }
        protected virtual void OnStart() { }
        protected virtual void OnDestroy() { }
    }
}
