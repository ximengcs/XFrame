using XFrame.Modules.Pools;

namespace XFrame.Modules.Event
{
    internal class DefaultEvent : XEvent, IPoolObject
    {
        public DefaultEvent() : base(default) { }

        public void SetId(int eventId)
        {
            Id = eventId;
        }

        void IPoolObject.OnCreate()
        {

        }

        void IPoolObject.OnRequest()
        {

        }

        void IPoolObject.OnDelete()
        {

        }

        void IPoolObject.OnRelease()
        {
            Id = default;
        }
    }
}
