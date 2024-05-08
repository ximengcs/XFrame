using XFrame.Modules.Event;
using XFrame.Modules.Pools;

namespace XFrame.Modules.Entities
{
    public class EntityDestroyEvent : XEvent
    {
        private static int s_EventId;
        public static int EventId
        {
            get
            {
                if (s_EventId == 0)
                    s_EventId = typeof(EntityDestroyEvent).GetHashCode();
                return s_EventId;
            }
        }

        public IEntity Entity { get; private set; }

        public static EntityDestroyEvent Create(IEntity entity)
        {
            EntityDestroyEvent e = References.Require<EntityDestroyEvent>();
            e.Entity = entity;
            e.Id = EventId;
            return e;
        }

        protected internal override void OnReleaseFromPool()
        {
            base.OnReleaseFromPool();
            Id = 0;
            Entity = null;
        }
    }
}
