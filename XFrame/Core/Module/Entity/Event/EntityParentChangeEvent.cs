using XFrame.Modules.Event;

namespace XFrame.Modules.Entities
{
    public class EntityParentChangeEvent : XEvent
    {
        public static int EventId => typeof(EntityParentChangeEvent).GetHashCode();

        public IEntity OldParent { get; }
        public IEntity NewParent { get; }

        public EntityParentChangeEvent(IEntity oldParent, IEntity newParent) : base(EventId)
        {
            OldParent = oldParent;
            NewParent = newParent;
        }
    }
}
