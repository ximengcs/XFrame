using System;
using XFrame.Core;

namespace XFrame.Modules.Containers
{
    public interface IContainer : IDataProvider
    {
        T Get<T>(int id = default) where T : ICom;
        ICom Get(Type type, int id = default);
        T Add<T>(Action<ICom> comInitComplete = null) where T : ICom;
        T Add<T>(int id, Action<ICom> comInitComplete = null) where T : ICom;
        ICom Add(Type type, Action<ICom> comInitComplete = null);
        ICom Add(Type type, int id = default, Action<ICom> comInitComplete = null);
        T GetOrAdd<T>(Action<ICom> comInitComplete = null) where T : ICom;
        T GetOrAdd<T>(int id = default, Action<ICom> comInitComplete = null) where T : ICom;
        ICom GetOrAdd(Type type, Action<ICom> comInitComplete = null);
        ICom GetOrAdd(Type type, int id = default, Action<ICom> comInitComplete = null);
        void Remove<T>(int id = default) where T : ICom;
        void Remove(Type type, int id = default);
        void Clear();
        void Dispatch(Action<ICom> handle);
    }
}
