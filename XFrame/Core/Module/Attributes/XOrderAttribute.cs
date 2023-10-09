using System;

namespace XFrame.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class XOrderAttribute : Attribute
    {
        public int Order { get; }

        public XOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
