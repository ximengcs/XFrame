using System;
using System.Collections.Generic;

namespace XFrame.Modules
{
    public interface IPoolSystem : IEnumerable<IPool>
    {
        Type RootType { get; }
        IPool Get<T>() where T : IPoolObject;
        IPool Get(Type type);
        void Remove(IPool pool);
    }
}
