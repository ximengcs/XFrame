using System;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.XType;

namespace XFrame.Modules.Serialize
{
    /// <summary>
    /// 序列化模块
    /// </summary>
    [CoreModule]
    public class SerializeModule : SingletonModule<SerializeModule>
    {
        private ISerializeHelper m_Helper;

        protected override void OnInit(object data)
        {
            base.OnInit(data);
            if (!string.IsNullOrEmpty(XConfig.DefaultSerializer))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultSerializer);
                InnerInit(type);
            }
        }

        #region Interface
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="json">json本文</param>
        /// <param name="type">目标类型</param>
        /// <returns>序列化到的对象</returns>
        public object DeserializeToObject(string json, Type type)
        {
            return m_Helper.Deserialize(json, type);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">json本文</param>
        /// <returns>序列化到的对象</returns>
        public T DeserializeToObject<T>(string json)
        {
            return m_Helper.Deserialize<T>(json);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>json本文</returns>
        public string SerializeObjectToRaw(object obj)
        {
            return m_Helper.Serialize(obj);
        }

        #endregion

        private void InnerInit(Type type)
        {
            m_Helper = Activator.CreateInstance(type) as ISerializeHelper;
        }
    }
}
