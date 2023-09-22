
using System;
using XFrame.Core;

namespace XFrame.Modules.Serialize
{
    public interface ISerializeModule : IModule
    {
        object DeserializeToObject(string text, Type type);

        object DeserializeToObject(string text, int textType, Type type);

        T DeserializeToObject<T>(string text);

        T DeserializeToObject<T>(string text, int textType);

        string SerializeObjectToRaw(object obj);

        string SerializeObjectToRaw(object obj, int textType);
    }
}
