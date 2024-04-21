using System;
using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    /// <summary>
    /// 资源加载任务处理器
    /// </summary>
    public interface IResHandler : IProTaskHandler
    {
        public string AssetPath { get; }

        public Type AssetType { get; }

        void Start();
    }
}
