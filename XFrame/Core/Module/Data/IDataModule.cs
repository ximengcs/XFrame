
using System;
using XFrame.Core;

namespace XFrame.Modules.Datas
{
    public interface IDataModule : IModule
    {
        void Register(Type tableType);
        IDataTable<T> Add<T>(string json, int textType = default) where T : IDataRaw;
        IDataTable<T> Get<T>() where T : IDataRaw;
        IDataTable<T> Get<T>(int tableIndex) where T : IDataRaw;
        T GetOne<T>() where T : IDataRaw;
        T GetOne<T>(int tableIndex) where T : IDataRaw;
        T GetItem<T>(int itemId) where T : IDataRaw;
        T GetItem<T>(int tableIndex, int itemId) where T : IDataRaw;
    }
}
