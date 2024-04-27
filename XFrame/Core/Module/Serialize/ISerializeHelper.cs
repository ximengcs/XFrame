using System;

namespace XFrame.Modules.Serialize
{
    /// <summary>
    /// Json序列化辅助器
    /// </summary>
    public interface ISerializeHelper
    {
        /// <summary>
        /// 处理文件类型
        /// </summary>
        int HandleType { get; }

        /// <summary>
        /// 初始化生命周期
        /// </summary>
        /// <param name="module"></param>
        void OnInit(ISerializeModule module);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">json文本</param>
        /// <param name="dataType">目标类型</param>
        /// <returns>反序列化到的对象</returns>
        object Deserialize(string json, Type dataType);

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">json本文</param>
        /// <returns>序列化到的对象</returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// 序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>json本文</returns>
        string Serialize<T>(T obj);
    }
}
