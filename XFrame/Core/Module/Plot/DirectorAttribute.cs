using System;

namespace XFrame.Modules.Plots
{
    /// <summary>
    /// 导演
    /// </summary>
    public class DirectorAttribute : Attribute
    {
        /// <summary>
        /// 是否为默认
        /// </summary>
        public bool Default { get; }

        public DirectorAttribute(bool ifDefault = false)
        {
            Default = ifDefault;
        }
    }
}
