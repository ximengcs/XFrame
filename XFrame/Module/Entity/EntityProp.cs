using System;

namespace XFrame.Modules
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class EntityPropAttribute : Attribute
    {
        public int Type { get; }

        public EntityPropAttribute()
        {
            Type = 0;
        }

        public EntityPropAttribute(int type)
        {
            Type = type;
        }
    }
}
