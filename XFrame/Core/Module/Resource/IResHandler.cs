using System;
using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源加载任务处理器
    /// </summary>
    public interface IResHandler : IProTaskHandler
    {
        /// <summary>
        /// 资源路径
        /// </summary>
        public string AssetPath { get; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public Type AssetType { get; }

        /// <summary>
        /// 开始加载
        /// </summary>
        void Start();
    }
}
