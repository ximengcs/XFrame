using System;

namespace XFrame.Core
{
    /// <summary>
    /// 更新处理器
    /// </summary>
    public class UpdateHandler : IModuleHandler
    {
        /// <inheritdoc/>
        public Type Target => typeof(IUpdater);

        /// <inheritdoc/>
        public void Handle(IModule module, object data)
        {
            IUpdater updater = module as IUpdater;
            updater.OnUpdate((double)data);
        }
    }
}
