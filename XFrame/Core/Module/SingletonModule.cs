using XFrame.Modules.Pools;

namespace XFrame.Core
{
    /// <summary>
    /// 单例模块基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonModule<T> : IModule where T : SingletonModule<T>
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public virtual int Id => default;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Inst { get; private set; }

        void IModule.OnInit(object data)
        {
            Inst = (T)this;
            OnInit(data);
        }

        void IModule.OnStart()
        {
            OnStart();
        }

        void IModule.OnUpdate(float escapeTime)
        {
            OnUpdate(escapeTime);
        }

        void IModule.OnDestroy()
        {
            OnDestroy();
            Inst = default;
        }

        protected virtual void OnInit(object data) { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate(float escapeTime) { }
        protected virtual void OnDestroy() { }
    }
}
