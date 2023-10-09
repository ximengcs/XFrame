using System;

namespace XFrame.Modules.Reflection
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class XAttribute : Attribute
    {
        public int Id { get; }

        public XAttribute(int id)
        {
            Id = id;
        }
    }
}
