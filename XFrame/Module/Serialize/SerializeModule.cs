using System;
using XFrame.Core;
using XFrame.Modules.Config;
using XFrame.Modules.XType;

namespace XFrame.Modules.Serialize
{
    /// <summary>
    /// 序列化模块
    /// </summary>
    public class SerializeModule : SingletonModule<SerializeModule>
    {
        private IJsonSerializeHelper m_JsonHelper;

        public override void OnInit(object data)
        {
            base.OnInit(data);
            if (!string.IsNullOrEmpty(XConfig.DefaultJsonSerializer))
            {
                Type type = TypeModule.Inst.GetType(XConfig.DefaultJsonSerializer);
                InnerInit(type);
            }
        }

        #region Interface
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T">目标类型</typeparam>
        /// <param name="json">json本文</param>
        /// <returns>序列化到的对象</returns>
        public T DeserializeJsonToObject<T>(string json)
        {
            return m_JsonHelper.Deserialize<T>(json);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns>json本文</returns>
        public string SerializeObjectToJson(object obj)
        {
            return m_JsonHelper.Serialize(obj);
        }
        #endregion

        private void InnerInit(Type type)
        {
            m_JsonHelper = Activator.CreateInstance(type) as IJsonSerializeHelper;
        }
    }
}
