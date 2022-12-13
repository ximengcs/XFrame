using System;

namespace XFrame.Modules
{
    public interface IResourceHelper
    {
        void Init(string resPath);
        object Load(string resPath, Type type);
        T Load<T>(string resPath);
        ResLoadTask LoadAsync(string resPath, Type type);
        ResLoadTask<T> LoadAsync<T>(string resPath);
        void Unload(string target);
        void UnloadAll();
    }
}
