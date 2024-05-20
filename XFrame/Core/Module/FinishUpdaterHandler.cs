using System;

namespace XFrame.Core.Module
{
    /// <summary>
    /// 更新处理器
    /// </summary>
    public class FinishUpdateHandler : IModuleHandler
    {
        /// <inheritdoc/>
        public Type Target => typeof(IFinishUpdater);

        /// <inheritdoc/>
        public void Handle(IModule module, object data)
        {
            IFinishUpdater updater = module as IFinishUpdater;
            updater.OnUpdate((double)data);
        }
    }
}
