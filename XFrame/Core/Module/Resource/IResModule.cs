using System;
using XFrame.Core;
using XFrame.Collections;
using System.Collections.Generic;
using XFrame.Tasks;

namespace XFrame.Modules.Resource
{
    public interface IResModule : IModule
    {
        IResourceHelper Helper { get; }

        XTask Preload(IEnumerable<string> resPaths, Type type);

        XTask Preload(IXEnumerable<string> resPaths, Type type);

        object Load(string resPath, Type type);

        T Load<T>(string resPath);

        ResLoadTask_ LoadAsync(string resPath, Type type);

        ResLoadTask_<T> LoadAsync<T>(string resPath);

        void Unload(object target);

        void UnloadPre(string resPath);

        void UnloadAll();

        void UnloadAllPre();
    }
}
