﻿
namespace XFrame.Modules.Containers
{
    public abstract class ContainerBase
    {
        protected internal virtual void OnInit() { }
        protected internal virtual void OnUpdate(float elapseTime) { }
        protected internal virtual void OnDestroy() { }
        protected internal virtual void OnCreateFromPool() { }
        protected internal virtual void OnDestroyFromPool() { }
        protected internal virtual void OnReleaseFromPool() { }
    }
}
