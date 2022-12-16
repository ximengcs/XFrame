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
        public int Id => default;

        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Inst { get; private set; }

        void IModule.OnInit(object data)
        {
            Inst = (T)this;
            OnInit(data);
        }

        void IModule.OnUpdate(float escapeTime)
        {
            OnUpdate(escapeTime);
        }

        void IModule.OnDestroy()
        {
            OnDestroy();
        }


        void IPoolObject.OnCreate()
        {
            OnCreate();
        }

        void IPoolObject.OnRelease()
        {
            OnRelease();
        }

        void IPoolObject.OnDestroyForever()
        {
            OnDestroyFrom();
        }

        protected virtual void OnInit(object data) { }
        protected virtual void OnUpdate(float escapeTime) { }
        protected virtual void OnDestroy() { }
        protected virtual void OnCreate() { }
        protected virtual void OnRelease() { }
        protected virtual void OnDestroyFrom() { }
    }
}
