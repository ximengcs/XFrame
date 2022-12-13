
using XFrame.Modules;

namespace XFrame.Core
{
    public abstract class SingleModule<T> : IModule where T : SingleModule<T>
    {
        public int Id => default;
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
