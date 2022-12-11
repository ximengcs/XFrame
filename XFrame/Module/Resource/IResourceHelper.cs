using System;

namespace XFrame.Modules
{
    public interface IResourceHelper
    {
        XTask Init();
        void LoadAllAsync(Action complete);
        object Load(string dirName, string fileName);
        object Load(params string[] namePart);
        T Load<T>(string dirName, string fileName);
        T Load<T>(params string[] namePart);
        void Unload(string package);
        void UnloadAll();
    }
}
