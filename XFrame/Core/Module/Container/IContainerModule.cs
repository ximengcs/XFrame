
using System;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    public interface IContainerModule : IModule
    {
        T New<T>(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null) where T : IContainer;

        Container New(bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null);

        IContainer New(Type type, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null);

        IContainer New(Type type, int id, bool updateTrusteeship = true, IContainer master = null, OnDataProviderReady onReady = null);

        void Remove(IContainer container);
    }
}
