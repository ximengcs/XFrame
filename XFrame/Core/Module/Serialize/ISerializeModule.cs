
using System;
using XFrame.Core;

namespace XFrame.Modules.Serialize
{
    /// <summary>
    /// 序列化模块
    /// </summary>
    public interface ISerializeModule : IModule
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="text">text本文</param>
        /// <param name="type">目标类型</param>
        /// <returns>序列化到的对象</returns>
        object DeserializeToObject(string text, Type type);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="textType">文本类型</param>
        /// <param name="type">目标类型</param>
        /// <returns>序列化到的对象</returns>
        object DeserializeToObject(string text, int textType, Type type);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="text">text本文</param>
        /// <returns>序列化到的对象</returns>
        T DeserializeToObject<T>(string text);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="text">text本文</param>
        /// <param name="textType">文本类型</param>
        /// <returns>序列化到的对象</returns>
        T DeserializeToObject<T>(string text, int textType);

        /// <summary>
        /// 序列化 
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>json本文</returns>
        string SerializeObjectToRaw(object obj);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <param name="textType">文本类型</param>
        /// <returns>json本文</returns>
        string SerializeObjectToRaw(object obj, int textType);
    }
}
