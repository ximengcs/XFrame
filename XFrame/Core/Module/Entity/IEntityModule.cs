using System;
using XFrame.Core;
using XFrame.Modules.Containers;

namespace XFrame.Modules.Entities
{
    public interface IEntityModule : IModule
    {
        void RegisterEntity<T>() where T : class, IEntity;

        T Create<T>(OnDataProviderReady onReady = null) where T : class, IEntity;

        IEntity Create(Type type, OnDataProviderReady onReady = null);

        T Create<T>(IEntity parent, OnDataProviderReady onReady = null) where T : class, IEntity;

        IEntity Create(Type type, IEntity parent, OnDataProviderReady onReady = null);

        T Create<T>(int typeId, OnDataProviderReady onReady = null) where T : class, IEntity;

        IEntity Create(Type baseType, int typeId, OnDataProviderReady onReady = null);

        T Create<T>(IEntity parent, int typeId, OnDataProviderReady onReady = null) where T : class, IEntity;

        IEntity Create(Type baseType, IEntity parent, int typeId, OnDataProviderReady onReady = null);

        void Destroy(IEntity entity);
    }
}
