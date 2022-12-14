
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

        public virtual void OnInit(object data)
        {
            Inst = (T)this;
        }

        public virtual void OnUpdate(float escapeTime)
        {

        }

        public virtual void OnDestroy()
        {

        }

        public void OnCreate()
        {

        }

        public void OnRelease()
        {

        }

        public void OnDestroyFrom()
        {

        }
    }
}
