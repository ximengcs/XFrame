
namespace XFrame.Core
{
    /// <summary>
    /// 模块基类
    /// </summary>
    public abstract class ModuleBase : IModule
    {
        /// <summary>
        /// 模块Id
        /// </summary>
        public int Id { get; protected set; }

        void IModule.OnInit(object data)
        {
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
        }

        protected virtual void OnInit(object data) { }
        protected virtual void OnStart() { }
        protected virtual void OnUpdate(float escapeTime) { }
        protected virtual void OnDestroy() { }
    }
}
