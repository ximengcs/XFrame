using System;

namespace XFrame.Core
{
    public class UpdateHandler : IModuleHandler
    {
        public Type Target => typeof(IUpdater);

        public void Handle(IModule module, object data)
        {
            IUpdater updater = module as IUpdater;
            updater.OnUpdate((float)data);
        }
    }
}
