using System;

namespace XFrame.Collections
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XTypeAttribute : Attribute
    {
        public Type Type { get; }

        public XTypeAttribute(Type type)
        {
            Type = type;
        }
    }
}
