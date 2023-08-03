
using XFrame.Modules.Diagnotics;

namespace XFrame.Core
{
    /// <summary>
    /// 单例模块基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonModule<T> : ModuleBase where T : SingletonModule<T>
    {
        /// <summary>
        /// 单例实例
        /// </summary>
        public static T Inst { get; private set; }

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            if (Inst == null)
            {
                Inst = (T)this;
            }
            else
            {
                Log.Debug("XFrame", $"SingletonModule {typeof(T).FullName} add multi instance.");
            }
        }
    }
}
