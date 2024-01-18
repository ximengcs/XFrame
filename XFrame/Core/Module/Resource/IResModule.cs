using System;
using XFrame.Core;
using System.Collections;
using XFrame.Collections;
using XFrame.Modules.Tasks;

namespace XFrame.Modules.Resource
{
    public interface IResModule : IModule
    {
        IResourceHelper Helper { get; }
        ITask Preload(IEnumerable resPaths, Type type);

        ITask Preload(IXEnumerable<string> resPaths, Type type);

        ITask Preload(string[] resPaths, Type type);

        ITask Preload<T>(IEnumerable resPaths);

        ITask Preload<T>(string[] resPaths);

        ITask Preload<T>(IXEnumerable<string> resPaths);

        ITask Preload(string resPath, Type type);
        ITask Preload<T>(string resPath);

        object Load(string resPath, Type type);

        T Load<T>(string resPath);

        ResLoadTask LoadAsync(string resPath, Type type);

        ResLoadTask<T> LoadAsync<T>(string resPath);

        void Unload(object target);

        void UnloadPre(string resPath);

        void UnloadAll();

        void UnloadAllPre();
    }
}
