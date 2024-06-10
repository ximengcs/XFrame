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

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="ifDefault">是否为默认导演</param>
        public DirectorAttribute(bool ifDefault = false)
        {
            Default = ifDefault;
        }
    }
}
