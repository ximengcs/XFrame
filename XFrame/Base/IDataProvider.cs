using System;

namespace XFrame.Core
{
    public interface IDataProvider : IDisposable
    {
        void SetData<T>(T value) where T : class;
        T GetData<T>() where T : class;
        void SetData<T>(string name, T value) where T : class;
        T GetData<T>(string name) where T : class;
    }
}
