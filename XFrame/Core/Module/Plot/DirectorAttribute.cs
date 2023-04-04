using System;

namespace XFrame.Modules.Plots
{
    public class DirectorAttribute : Attribute
    {
        public bool Default { get; }

        public DirectorAttribute(bool ifDefault = false)
        {
            Default = ifDefault;
        }
    }
}
