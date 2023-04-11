using System;

namespace XFrame.Modules.Containers
{
    public interface IContainer
    {
        T Get<T>(int id = default) where T : ICom;
        ICom Get(Type type, int id = default);
        T Add<T>(int id = default, object userData = null) where T : ICom;
        ICom Add(Type type, int id = default, object userData = null);
        T GetOrAdd<T>(int id = default, object userData = null) where T : ICom;
        ICom GetOrAdd(Type type, int id = default, object userData = null);
        void Remove<T>(int id = default) where T : ICom;
        void Remove(Type type, int id = default);
        void Clear();
        void Dispatch(Action<ICom> com);
    }
}
